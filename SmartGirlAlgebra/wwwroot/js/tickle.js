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
  var tagline = '';
  var rate = 1;

  var ONE_IN = 25;
  var COOLDOWN_MS = 60000;

  var last = -Infinity;
  var armed = false;

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
      if (window.sgaListen && window.sgaListen.active && window.sgaListen.active()) return true;
      if (window.sgaSpeech && window.sgaSpeech.busy && window.sgaSpeech.busy()) return true;
    } catch (e) { }
    return false;
  }

  function configure(cfg) {
    lines = (cfg && cfg.lines) || [];
    tagline = (cfg && cfg.tagline) || '';
    rate = (cfg && cfg.rate) || 1;
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

  function fire() {
    if (!lines.length) return;

    var line = lines[Math.floor(Math.random() * lines.length)];
    var items = [{ text: line, rate: rate, pause: 750 }];

    // The tagline always follows, a touch warmer and slower.
    if (tagline) items.push({ text: tagline, rate: Math.max(0.75, rate - 0.12), pause: 0 });

    try { window.sgaSpeech.sequence(items, null); } catch (e) { }
  }

  return {
    configure: configure,
    muted: muted,
    setMuted: setMuted,
    fire: fire        // so the mute toggle can play a sample when switched on
  };
})();
