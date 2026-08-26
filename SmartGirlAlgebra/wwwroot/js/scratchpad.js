// Scratch pad: finger-drawing surface plus an HSV colour wheel.
// Canvas work lives here rather than in C# — the interop chatter of a
// per-pointer-move round trip would make drawing feel laggy.

(function () {
  const WHEEL = 168;

  function hsvToRgb(h, s, v) {
    const c = v * s;
    const x = c * (1 - Math.abs(((h / 60) % 2) - 1));
    const m = v - c;
    let r = 0, g = 0, b = 0;
    if (h < 60) { r = c; g = x; }
    else if (h < 120) { r = x; g = c; }
    else if (h < 180) { g = c; b = x; }
    else if (h < 240) { g = x; b = c; }
    else if (h < 300) { r = x; b = c; }
    else { r = c; b = x; }
    return {
      r: Math.round((r + m) * 255),
      g: Math.round((g + m) * 255),
      b: Math.round((b + m) * 255)
    };
  }

  function hsvToCss(h, s, v) {
    const { r, g, b } = hsvToRgb(h, s, v);
    return `rgb(${r}, ${g}, ${b})`;
  }

  // Per-canvas state, so more than one pad on a page stays independent.
  const pads = new WeakMap();

  function initPad(canvas) {
    if (!canvas || pads.has(canvas)) return;

    const state = { hue: 210, sat: 0.85, bright: 0.9, drawing: false, last: null };
    state.color = hsvToCss(state.hue, state.sat, state.bright);
    pads.set(canvas, state);

    const ctx = canvas.getContext("2d");
    const resize = () => {
      const rect = canvas.getBoundingClientRect();
      if (rect.width === 0) return;
      const dpr = window.devicePixelRatio || 1;
      // Preserve what's already drawn across an orientation change.
      const snapshot = canvas.width > 0 ? canvas.toDataURL() : null;
      canvas.width = rect.width * dpr;
      canvas.height = rect.height * dpr;
      ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
      ctx.lineCap = "round";
      ctx.lineJoin = "round";
      ctx.lineWidth = 5;   // a fingertip, not a stylus, on a now much bigger board
      if (snapshot) {
        const img = new Image();
        img.onload = () => ctx.drawImage(img, 0, 0, rect.width, rect.height);
        img.src = snapshot;
      }
    };
    resize();
    window.addEventListener("resize", resize);

    const at = (e) => {
      const rect = canvas.getBoundingClientRect();
      return { x: e.clientX - rect.left, y: e.clientY - rect.top };
    };

    canvas.addEventListener("pointerdown", (e) => {
      e.preventDefault();
      state.drawing = true;
      state.last = at(e);
      canvas.setPointerCapture(e.pointerId);
    });

    canvas.addEventListener("pointermove", (e) => {
      if (!state.drawing || !state.last) return;
      const p = at(e);
      ctx.strokeStyle = state.color;
      ctx.beginPath();
      ctx.moveTo(state.last.x, state.last.y);
      ctx.lineTo(p.x, p.y);
      ctx.stroke();
      state.last = p;
    });

    const end = () => { state.drawing = false; state.last = null; };
    canvas.addEventListener("pointerup", end);
    canvas.addEventListener("pointerleave", end);
    canvas.addEventListener("pointercancel", end);
  }

  function clearPad(canvas) {
    const ctx = canvas && canvas.getContext("2d");
    if (!ctx) return;
    ctx.save();
    ctx.setTransform(1, 0, 0, 1, 0, 0);
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.restore();
  }

  function paintWheel(wheel, bright) {
    const ctx = wheel.getContext("2d");
    const dpr = window.devicePixelRatio || 1;
    const size = WHEEL * dpr;
    wheel.width = size;
    wheel.height = size;

    const img = ctx.createImageData(size, size);
    const radius = size / 2;
    for (let y = 0; y < size; y++) {
      for (let x = 0; x < size; x++) {
        const dx = x - radius;
        const dy = y - radius;
        const dist = Math.sqrt(dx * dx + dy * dy);
        const i = (y * size + x) * 4;
        if (dist <= radius) {
          let h = (Math.atan2(dy, dx) * 180) / Math.PI + 90;
          if (h < 0) h += 360;
          const { r, g, b } = hsvToRgb(h, Math.min(dist / radius, 1), bright);
          img.data[i] = r;
          img.data[i + 1] = g;
          img.data[i + 2] = b;
          img.data[i + 3] = dist > radius - dpr ? 255 * (radius - dist) : 255;
        } else {
          img.data[i + 3] = 0;
        }
      }
    }
    ctx.putImageData(img, 0, 0);
  }

  // Wires the wheel, the selector dot, the swatch and the brightness slider
  // to the pad's colour. Everything updates in place — no interop per move.
  function initWheel(canvas, wheel, dot, swatch, slider) {
    const state = pads.get(canvas);
    if (!state || !wheel) return;

    const paintDot = () => {
      const rad = (state.hue - 90) * (Math.PI / 180);
      const reach = state.sat * (WHEEL / 2 - 4);
      dot.style.left = `${WHEEL / 2 + Math.cos(rad) * reach}px`;
      dot.style.top = `${WHEEL / 2 + Math.sin(rad) * reach}px`;
      dot.style.backgroundColor = state.color;
      if (swatch) swatch.style.backgroundColor = state.color;
      if (slider) {
        slider.style.background =
          `linear-gradient(to right, #000, ${hsvToCss(state.hue, state.sat, 1)})`;
      }
    };

    const pick = (e) => {
      const rect = wheel.getBoundingClientRect();
      const dx = e.clientX - rect.left - WHEEL / 2;
      const dy = e.clientY - rect.top - WHEEL / 2;
      const dist = Math.min(Math.sqrt(dx * dx + dy * dy), WHEEL / 2 - 4);
      let h = (Math.atan2(dy, dx) * 180) / Math.PI + 90;
      if (h < 0) h += 360;
      state.hue = h;
      state.sat = dist / (WHEEL / 2 - 4);
      state.color = hsvToCss(state.hue, state.sat, state.bright);
      paintDot();
    };

    paintWheel(wheel, state.bright);
    paintDot();

    wheel.addEventListener("pointerdown", (e) => {
      e.preventDefault();
      wheel.setPointerCapture(e.pointerId);
      pick(e);
    });
    wheel.addEventListener("pointermove", (e) => {
      if (e.buttons !== 1) return;
      pick(e);
    });

    if (slider) {
      slider.value = state.bright;
      slider.addEventListener("input", () => {
        state.bright = Number(slider.value);
        state.color = hsvToCss(state.hue, state.sat, state.bright);
        paintWheel(wheel, state.bright);
        paintDot();
      });
    }
  }

  window.sgaScratch = { initPad, clearPad, initWheel };
})();
