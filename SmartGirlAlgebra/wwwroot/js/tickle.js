// The app giggles back, once in a while.
//
// Fires on roughly one button tap in twenty-five, and never twice inside a
// minute. A tickle line, a beat, then Grandma's tagline - always the pair, and
// always in the same voice.
//
// What it must never interrupt:
//   - an open microphone (KT reads a word back; a giggle would be heard as his
//     answer and scored as one)
//   - the reading voice, mid-sentence
//   - the reading controls themselves - tapping a word to hear it is a request
//     for that word, not an invitation to be funny
window.sgaTickle = (function () {
  var lines = [];
  var taglines = [];
  var rate = 1;

  // How often the signature is breathed rather than said.
  var WHISPER_IN = 3;

  // The clips are normalised to -14 LUFS and pushed through a gain node, so
  // this sits them just under the reading rather than compensating for a quiet
  // recording. Tone is untouched - level only.
  var VOLUME = 0.85;

  var ONE_IN = 25;
  var COOLDOWN_MS = 60000;

  var last = -Infinity;
  var armed = false;

  // text -> recorded clip. Her real voice where we have it; the browser's
  // synthetic one only where we don't, so a missing clip is a downgrade rather
  // than a silence.
  var clips = {};
  var playing = null;

  // Synthetic speech is capped at volume 1 by the browser. A recorded clip is
  // not: routing it through a gain node lets it go genuinely louder, which is
  // the only real headroom this app has.
  var audioCtx = null;
  var boosted = new WeakSet();

  function boost(el, gain) {
    try {
      var Ctx = window.AudioContext || window.webkitAudioContext;
      if (!Ctx) return;

      if (!audioCtx) audioCtx = new Ctx();
      if (audioCtx.state === 'suspended') audioCtx.resume();

      // An element can only be connected to the graph once, ever.
      if (boosted.has(el)) return;
      boosted.add(el);

      var src = audioCtx.createMediaElementSource(el);
      var g = audioCtx.createGain();
      g.gain.value = gain;

      // Stop the boost clipping into distortion on a loud clip.
      var comp = audioCtx.createDynamicsCompressor();
      comp.threshold.value = -8;
      comp.ratio.value = 6;

      src.connect(g);
      g.connect(comp);
      comp.connect(audioCtx.destination);
    } catch (e) { /* no Web Audio here; the element still plays normally */ }
  }

  function loadClips() {
    try {
      fetch('/audio/tickle/manifest.json', { cache: 'no-cache' })
        .then(function (r) { return r.ok ? r.json() : {}; })
        .then(function (m) { clips = m || {}; })
        .catch(function () { clips = {}; });
    } catch (e) { clips = {}; }
  }

  function muted() {
    try { return localStorage.getItem('sgaSilly') === 'off'; }
    catch (e) { return false; }
  }

  function setMuted(off) {
    try { localStorage.setItem('sgaSilly', off ? 'off' : 'on'); }
    catch (e) { /* private browsing - lasts the session */ }
  }

  function busy() {
    try {
      if (playing) return true;
      if (window.sgaListen && window.sgaListen.active && window.sgaListen.active()) return true;
      if (window.sgaSpeech && window.sgaSpeech.busy && window.sgaSpeech.busy()) return true;
    } catch (e) { }
    return false;
  }

  function configure(cfg) {
    lines = (cfg && cfg.lines) || [];
    taglines = (cfg && cfg.taglines) || [];
    rate = (cfg && cfg.rate) || 1;
    loadClips();
    arm();
  }

  function arm() {
    if (armed) return;
    armed = true;
    // Capture phase, so it still counts even if the app stops the event.
    document.addEventListener('click', onTap, true);
  }

  function onTap(e) {
    if (!lines.length || muted()) return;

    var el = e.target && e.target.closest ? e.target.closest('button, a') : null;
    if (!el) return;

    // Reading controls and the whiteboard are working tools, not punchlines.
    if (el.closest('.sga-read, .sga-tap, .sga-scratch, .sga-silly')) return;

    if (busy()) return;

    var now = (window.performance && performance.now) ? performance.now() : 0;
    if (now - last < COOLDOWN_MS) return;
    if (Math.floor(Math.random() * ONE_IN) !== 0) return;

    last = now;
    fire();
  }

  // English mostly, Spanish and Patwa now and then - but only if the device
  // actually has a voice for it. An English voice reading Spanish is worse than
  // plain English, and worse still for Patwa.
  function pickTagline() {
    var usable = taglines.filter(function (tl) {
      if (!tl.lang) return true;
      // Plain English is always safe; anything else needs a real voice for it.
      if (tl.lang.toLowerCase() === 'en-us' && !tl.strict) return true;
      try { return window.sgaSpeech.hasVoice(tl.lang, tl.strict); } catch (e) { return false; }
    });

    if (!usable.length) return null;

    var total = 0;
    for (var i = 0; i < usable.length; i++) total += (usable[i].weight || 1);

    var roll = Math.random() * total;
    for (var j = 0; j < usable.length; j++) {
      roll -= (usable[j].weight || 1);
      if (roll <= 0) return usable[j];
    }

    return usable[0];
  }

  function fire() {
    if (!lines.length) return;

    var line = lines[Math.floor(Math.random() * lines.length)];
    var items = [{ text: line, rate: rate, volume: VOLUME, pause: 750 }];

    var tl = pickTagline();

    if (tl) {
      var whisper = Math.floor(Math.random() * WHISPER_IN) === 0;

      items.push({
        text: tl.text,
        lang: tl.lang || 'en-US',
        strict: !!tl.strict,
        rate: whisper ? Math.max(0.7, rate - 0.2) : Math.max(0.75, rate - 0.12),
        volume: whisper ? VOLUME * 0.42 : VOLUME,
        pitch: whisper ? 0.92 : 1.05,
        pause: 0
      });
    }

    play(items, 0);
  }

  // One item at a time: her recording if we have it, the synthetic voice if not.
  function play(items, i) {
    if (i >= items.length) { playing = null; return; }

    var it = items[i];
    var file = clips[it.text];

    if (!file) {
      // No recording of this line - say it, then carry on down the queue.
      try {
        window.sgaSpeech.sequence([it], null);
        var wait = Math.max(1600, it.text.length * 200 / (it.rate || 1)) + (it.pause || 0);
        playing = window.setTimeout(function () { play(items, i + 1); }, wait);
      } catch (e) { playing = null; }
      return;
    }

    var a = new Audio('/audio/tickle/' + file);
    a.volume = (it.volume === undefined) ? 1 : it.volume;

    // Push it through the gain node so her voice actually carries.
    boost(a, 2.6);

    var moved = false;
    function go() {
      if (moved) return;
      moved = true;
      playing = window.setTimeout(function () { play(items, i + 1); }, it.pause || 0);
    }

    a.onended = go;
    a.onerror = go;

    playing = a;

    var p = a.play();
    if (p && p.catch) p.catch(go);
  }

  function hush() {
    if (!playing) return;

    try {
      if (typeof playing === 'number') window.clearTimeout(playing);
      else { playing.pause(); playing.currentTime = 0; }
    } catch (e) { }

    playing = null;
  }

  return {
    configure: configure,
    hush: hush,
    muted: muted,
    setMuted: setMuted,
    fire: fire        // so the mute toggle can play a sample when switched on
  };
})();
