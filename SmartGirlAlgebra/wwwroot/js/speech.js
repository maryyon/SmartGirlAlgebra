// Reading the instructions out loud, one highlighted word at a time.
//
// Chrome and Edge fire a `boundary` event per word, which is exact. iOS Safari
// never fires it, and a five-year-old on an iPad is the whole audience here, so
// when no boundary arrives we pace the highlight ourselves off word length.
// Slightly out of step beats no highlight at all.
window.sgaSpeech = (function () {
  var ref = null;
  var timer = null;
  var active = false;

  function clearTimer() {
    if (timer) { window.clearTimeout(timer); timer = null; }
  }

  function finish() {
    clearTimer();
    active = false;
    if (ref) {
      try { ref.invokeMethodAsync('OnDone'); } catch (e) { /* component already gone */ }
    }
  }

  function stop() {
    try { window.speechSynthesis.cancel(); } catch (e) { }
    finish();
  }

  // A warm, clear voice in the language asked for. Whatever is first otherwise —
  // the reading matters more than the accent.
  function pickVoice(lang, strict) {
    var tag = (lang || 'en').toLowerCase().replace('_', '-');
    // Strict means the region matters: "en-JM" must be an actual Jamaican voice,
    // not any English one, or Patwa comes out in an American accent.
    var want = strict ? tag : tag.slice(0, 2);

    var all = [];
    try { all = window.speechSynthesis.getVoices() || []; } catch (e) { return null; }

    var matching = all.filter(function (v) {
      return (v.lang || '').toLowerCase().replace('_', '-').indexOf(want) === 0;
    });

    if (!matching.length) return null;

    var nice = matching.filter(function (v) {
      return /samantha|karen|moira|aria|jenny|zira|paulina|monica|helena|female|natural/i.test(v.name || '');
    });

    return nice.length ? nice[0] : matching[0];
  }

  // Whether this device can say anything in that language at all. A Spanish
  // line read by an English voice is worse than not saying it.
  function hasVoice(lang, strict) { return !!pickVoice(lang, strict); }

  function speak(text, starts, dotNetRef, rate) {
    stop();
    ref = dotNetRef;

    if (!('speechSynthesis' in window)) {
      if (ref) { try { ref.invokeMethodAsync('OnUnsupported'); } catch (e) { } }
      return;
    }

    active = true;
    var u = new SpeechSynthesisUtterance(text);
    u.rate = rate;
    u.pitch = 1.05;

    var v = pickVoice();
    if (v) { u.voice = v; u.lang = v.lang; }

    var gotBoundary = false;

    u.onboundary = function (e) {
      if (!active) return;
      if (e.name && e.name !== 'word') return;
      gotBoundary = true;
      clearTimer();

      var idx = 0;
      for (var i = 0; i < starts.length; i++) {
        if (starts[i] <= e.charIndex) { idx = i; } else { break; }
      }
      try { ref.invokeMethodAsync('OnWord', idx); } catch (err) { }
    };

    u.onend = function () { finish(); };
    u.onerror = function () { finish(); };

    window.speechSynthesis.speak(u);

    // Give the real boundary events a moment to show up before falling back.
    window.setTimeout(function () {
      if (!active || gotBoundary) return;

      var per = [];
      for (var k = 0; k < starts.length; k++) {
        var end = (k + 1 < starts.length) ? starts[k + 1] : text.length;
        per.push(Math.max(230, (end - starts[k]) * 78 / rate));
      }

      var i = 0;
      (function tick() {
        if (!active || i >= starts.length) return;
        try { ref.invokeMethodAsync('OnWord', i); } catch (e) { return; }
        var wait = per[i];
        i++;
        timer = window.setTimeout(tick, wait);
      })();
    }, 420);
  }

  // Voices load asynchronously on most browsers; touching the list early makes
  // sure one is available by the time a child presses the button.
  if ('speechSynthesis' in window) {
    try {
      window.speechSynthesis.getVoices();
      window.speechSynthesis.onvoiceschanged = function () { };
    } catch (e) { }
  }

  // One word, said slowly, for a reader who is stuck on it. Kept separate from
  // speak() so a tapped word never disturbs a read-along in progress.
  function say(word, rate, dotNetRef) {
    try { window.speechSynthesis.cancel(); } catch (e) { }

    if (!('speechSynthesis' in window)) {
      if (dotNetRef) { try { dotNetRef.invokeMethodAsync('OnSaid'); } catch (e) { } }
      return;
    }

    var settled = false;
    function done() {
      if (settled) return;
      settled = true;
      if (dotNetRef) { try { dotNetRef.invokeMethodAsync('OnSaid'); } catch (e) { } }
    }

    var u = new SpeechSynthesisUtterance(word);
    u.rate = rate;
    u.pitch = 1.02;

    var v = pickVoice();
    if (v) { u.voice = v; u.lang = v.lang; }

    u.onend = done;
    u.onerror = done;

    window.speechSynthesis.speak(u);

    // Some mobile browsers fire neither event. Release the highlight anyway.
    window.setTimeout(done, Math.max(1400, word.length * 240 / rate));
  }

  // A short run of utterances at different speeds, with pauses between - used to
  // say a sentence, slow down, and then repeat the word he missed.
  function sequence(items, dotNetRef) {
    try { window.speechSynthesis.cancel(); } catch (e) { }

    var i = 0;

    function next() {
      if (i >= items.length) {
        if (dotNetRef) {
          try { dotNetRef.invokeMethodAsync('OnSequenceDone'); } catch (e) { }
        }
        return;
      }

      var it = items[i++];
      var moved = false;

      function go() {
        if (moved) return;
        moved = true;
        window.setTimeout(next, it.pause || 0);
      }

      var u = new SpeechSynthesisUtterance(it.text);
      u.rate = it.rate;
      u.pitch = (it.pitch === undefined) ? 1.05 : it.pitch;
      // A real hush: the API has a volume, so a whisper is an actual whisper.
      u.volume = (it.volume === undefined) ? 1 : it.volume;

      var v = pickVoice(it.lang, it.strict);
      if (v) { u.voice = v; u.lang = v.lang; }
      else if (it.lang) { u.lang = it.lang; }

      u.onend = go;
      u.onerror = go;

      window.speechSynthesis.speak(u);

      // Safety net for browsers that fire neither event.
      window.setTimeout(go, Math.max(1600, it.text.length * 200 / it.rate));
    }

    next();
  }

  function busy() {
    try { return window.speechSynthesis.speaking || window.speechSynthesis.pending; }
    catch (e) { return false; }
  }

  return {
    speak: speak,
    busy: busy,
    hasVoice: hasVoice,
    say: say,
    sequence: sequence,
    stop: stop,
    supported: function () { return 'speechSynthesis' in window; }
  };
})();
