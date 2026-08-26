// Listening for one word from a five-year-old.
//
// The microphone is opened only when he presses the button, and closed the
// moment a word comes back or the few seconds are up — it is never left open.
// In Chrome and Edge the audio goes to a cloud service for recognition; if the
// browser can't do it, or permission is refused, the caller is told plainly and
// falls back to simply saying the word for him.
window.sgaListen = (function () {
  var rec = null;
  var ref = null;
  var settled = false;
  var timer = null;

  function supported() {
    return !!(window.SpeechRecognition || window.webkitSpeechRecognition);
  }

  function clearTimer() {
    if (timer) { window.clearTimeout(timer); timer = null; }
  }

  function stop() {
    clearTimer();
    if (rec) {
      try { rec.abort(); } catch (e) { }
      rec = null;
    }
  }

  function finish(heard, problem) {
    if (settled) return;
    settled = true;
    clearTimer();

    if (rec) {
      try { rec.stop(); } catch (e) { }
      rec = null;
    }

    if (ref) {
      try { ref.invokeMethodAsync('OnHeard', heard || '', problem || ''); } catch (e) { }
    }
  }

  function start(dotNetRef, seconds) {
    stop();
    ref = dotNetRef;
    settled = false;

    var Ctor = window.SpeechRecognition || window.webkitSpeechRecognition;
    if (!Ctor) { finish('', 'unsupported'); return; }

    try { rec = new Ctor(); }
    catch (e) { finish('', 'unsupported'); return; }

    rec.lang = 'en-US';
    rec.interimResults = false;
    rec.continuous = false;
    // A child's voice is not a news reader's. Take every guess the engine has
    // and let the caller decide whether any of them is the word.
    rec.maxAlternatives = 6;

    rec.onresult = function (e) {
      var heard = [];
      for (var i = 0; i < e.results.length; i++) {
        var r = e.results[i];
        for (var j = 0; j < r.length; j++) {
          if (r[j] && r[j].transcript) heard.push(r[j].transcript);
        }
      }
      finish(heard.join('|'), '');
    };

    rec.onerror = function (e) { finish('', (e && e.error) || 'error'); };
    rec.onend = function () { finish('', 'quiet'); };

    try { rec.start(); }
    catch (e) { finish('', 'error'); return; }

    // He gets a few seconds, then we help rather than leaving him waiting.
    timer = window.setTimeout(function () {
      try { rec.stop(); } catch (e) { finish('', 'quiet'); }
    }, (seconds || 6) * 1000);
  }

  // True while the microphone is open, so nothing else may speak.
  function active() { return !!rec && !settled; }

  return { start: start, stop: stop, supported: supported, active: active };
})();
