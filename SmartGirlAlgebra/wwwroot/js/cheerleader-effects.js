// HYPER-ANIMATED CHEERLEADING EFFECTS!

// Create sparkle trail on mouse move
document.addEventListener('mousemove', function(e) {
    if (Math.random() > 0.7) { // Only create sparkles 30% of the time
        createSparkle(e.clientX, e.clientY);
    }
});

function createSparkle(x, y) {
    const sparkle = document.createElement('div');
    sparkle.className = 'sparkle-trail';
    sparkle.style.left = x + 'px';
    sparkle.style.top = y + 'px';
    
    // Random color from cheerleading palette
    const colors = ['#FF1493', '#FFD700', '#00FFFF', '#9370DB', '#FF10F0', '#39FF14'];
    sparkle.style.background = `radial-gradient(circle, ${colors[Math.floor(Math.random() * colors.length)]}, transparent)`;
    
    document.body.appendChild(sparkle);
    
    setTimeout(() => {
        sparkle.remove();
    }, 2000);
}

// Create floating pom-poms - smaller megaphones and actual pompom images
function createFloatingPomPoms() {
    const types = ['megaphone', 'pompom', 'megaphone', 'pompom', 'megaphone', 'pompom'];

    for (let i = 0; i < 15; i++) {
        setTimeout(() => {
            const pomPom = document.createElement('div');
            const type = types[Math.floor(Math.random() * types.length)];

            pomPom.style.position = 'fixed';
            pomPom.style.left = Math.random() * 100 + '%';
            pomPom.style.top = Math.random() * 100 + '%';
            pomPom.style.zIndex = '1';
            pomPom.style.pointerEvents = 'none';

            if (type === 'pompom') {
                const size = (Math.random() * 30 + 40) + 'px'; // 40-70px (original size)
                pomPom.style.width = size;
                pomPom.style.height = size;
                pomPom.style.backgroundImage = 'url(/images/pompoms.png)';
                pomPom.style.backgroundSize = 'contain';
                pomPom.style.backgroundRepeat = 'no-repeat';
                pomPom.style.backgroundPosition = 'center';
                pomPom.className = 'blinking-pompom';
            } else {
                pomPom.textContent = '📣';
                pomPom.style.fontSize = (Math.random() * 0.5 + 1) + 'em'; // 1-1.5em (smaller megaphones)
                pomPom.className = 'pom-pom';
            }

            document.body.appendChild(pomPom);

            // Remove after animation
            setTimeout(() => {
                pomPom.remove();
            }, 10000);
        }, i * 500);
    }
}

// Create confetti explosion
function createConfetti(x, y) {
    const colors = ['#FF1493', '#FFD700', '#00FFFF', '#9370DB', '#FF10F0', '#39FF14', '#FF69B4'];
    const types = ['megaphone', 'pompom', 'dot'];

    for (let i = 0; i < 30; i++) {
        const confetti = document.createElement('div');
        const type = types[Math.floor(Math.random() * types.length)];

        if (type === 'pompom') {
            confetti.style.width = '20px';  // Original pompom size
            confetti.style.height = '20px';
            confetti.style.backgroundImage = 'url(/images/pompoms.png)';
            confetti.style.backgroundSize = 'contain';
            confetti.style.backgroundRepeat = 'no-repeat';
            confetti.style.backgroundPosition = 'center';
            confetti.className = 'blinking-pompom';
        } else if (type === 'megaphone') {
            confetti.textContent = '📣';
            confetti.className = 'pom-pom';
            confetti.style.fontSize = '0.5em'; // Smaller megaphones
        } else {
            confetti.style.width = '10px';
            confetti.style.height = '10px';
            confetti.style.background = colors[Math.floor(Math.random() * colors.length)];
        }
        
        confetti.style.position = 'fixed';
        confetti.style.left = x + 'px';
        confetti.style.top = y + 'px';
        confetti.style.pointerEvents = 'none';
        confetti.style.zIndex = '9999';
        
        const angle = (Math.PI * 2 * i) / 30;
        const velocity = Math.random() * 5 + 5;
        const vx = Math.cos(angle) * velocity;
        const vy = Math.sin(angle) * velocity;
        
        document.body.appendChild(confetti);
        
        animateConfetti(confetti, vx, vy);
    }
}

function animateConfetti(element, vx, vy) {
    let x = parseFloat(element.style.left);
    let y = parseFloat(element.style.top);
    let opacity = 1;
    let rotation = 0;
    
    function update() {
        x += vx;
        y += vy;
        vy += 0.5; // gravity
        opacity -= 0.02;
        rotation += 10;
        
        element.style.left = x + 'px';
        element.style.top = y + 'px';
        element.style.opacity = opacity;
        element.style.transform = `rotate(${rotation}deg)`;
        
        if (opacity > 0) {
            requestAnimationFrame(update);
        } else {
            element.remove();
        }
    }
    
    update();
}

// Add click effects to buttons
document.addEventListener('click', function(e) {
    if (e.target.tagName === 'BUTTON') {
        createConfetti(e.clientX, e.clientY);
    }
});

// Initialize floating pom-poms on page load
window.addEventListener('load', function() {
    createFloatingPomPoms();
    
    // Recreate pom-poms every 15 seconds
    setInterval(createFloatingPomPoms, 15000);
});

// Add rainbow trail to cursor
let lastX = 0;
let lastY = 0;

document.addEventListener('mousemove', function(e) {
    const dx = e.clientX - lastX;
    const dy = e.clientY - lastY;
    const distance = Math.sqrt(dx * dx + dy * dy);
    
    if (distance > 10) {
        createRainbowTrail(e.clientX, e.clientY);
        lastX = e.clientX;
        lastY = e.clientY;
    }
});

function createRainbowTrail(x, y) {
    const trail = document.createElement('div');
    trail.style.position = 'fixed';
    trail.style.left = x + 'px';
    trail.style.top = y + 'px';
    trail.style.width = '5px';
    trail.style.height = '5px';
    trail.style.borderRadius = '50%';
    trail.style.pointerEvents = 'none';
    trail.style.zIndex = '1';
    
    const colors = ['#FF1493', '#FFD700', '#00FFFF', '#9370DB', '#FF10F0'];
    trail.style.background = colors[Math.floor(Math.random() * colors.length)];
    trail.style.boxShadow = `0 0 10px ${trail.style.background}`;
    
    document.body.appendChild(trail);
    
    setTimeout(() => {
        trail.style.transition = 'all 0.5s';
        trail.style.opacity = '0';
        trail.style.transform = 'scale(0)';
        setTimeout(() => trail.remove(), 500);
    }, 50);
}

