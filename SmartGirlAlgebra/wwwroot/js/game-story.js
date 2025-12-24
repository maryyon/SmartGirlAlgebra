// Smart Girl Algebra - 2D Game Engine
class AlgebraGame {
    constructor(canvasId) {
        this.canvas = document.getElementById(canvasId);
        this.ctx = this.canvas.getContext('2d');
        this.width = 800;
        this.height = 600;
        this.canvas.width = this.width;
        this.canvas.height = this.height;

        // Player
        this.player = {
            x: 100,
            y: 450,
            width: 40,
            height: 60,
            speed: 5,
            velocityY: 0,
            jumping: false,
            color: '#FF1493',
            direction: 'right'
        };

        // Game state
        this.keys = {};
        this.gravity = 0.8;
        this.jumpPower = -15;
        this.ground = 500;
        this.coins = [];
        this.problems = [];
        this.platforms = [];
        this.score = 0;
        this.level = 1;

        // Animation
        this.animationFrame = 0;
        this.running = false;

        this.setupControls();
        this.generateLevel();
    }

    setupControls() {
        window.addEventListener('keydown', (e) => {
            this.keys[e.key] = true;
            if (e.key === ' ' && !this.player.jumping) {
                this.player.velocityY = this.jumpPower;
                this.player.jumping = true;
            }
        });

        window.addEventListener('keyup', (e) => {
            this.keys[e.key] = false;
        });
    }

    generateLevel() {
        // Create platforms
        this.platforms = [
            { x: 0, y: 500, width: 800, height: 100, color: '#9370DB' },
            { x: 200, y: 400, width: 150, height: 20, color: '#FFD700' },
            { x: 450, y: 300, width: 150, height: 20, color: '#FFD700' },
            { x: 100, y: 250, width: 100, height: 20, color: '#FFD700' },
            { x: 600, y: 350, width: 120, height: 20, color: '#FFD700' }
        ];

        // Create coins
        this.coins = [
            { x: 250, y: 360, radius: 15, collected: false },
            { x: 500, y: 260, radius: 15, collected: false },
            { x: 150, y: 210, radius: 15, collected: false },
            { x: 650, y: 310, radius: 15, collected: false }
        ];

        // Create algebra problems (collectibles) - CHEERLEADING STORY PROBLEMS!
        this.problems = [
            {
                x: 300, y: 350, width: 40, height: 40, collected: false,
                type: 'slider',
                question: '💰 TEAM SHOPPING TRIP!\n\nOMG bestie! Coach gave you $45 to buy pompoms for the squad! 🛍️\n\nEach one costs $7 at the cheer store.\n\nHow many can you buy?',
                min: 0, max: 10, answer: 6
            },
            {
                x: 550, y: 250, width: 40, height: 40, collected: false,
                type: 'target',
                question: '📊 PRACTICE PERFORMANCE!\n\nYou\'re tracking your progress at cheer practice today! 📋\n\nIn the FIRST HALF of practice, you nailed 8 perfect tumbling passes! 💪\n\nBy the END of practice, you had 20 total perfect moves!\n\nHow many perfect tumbling passes did you do in the SECOND HALF?',
                answer: 12,
                options: [10, 12, 14, 16]
            },
            {
                x: 150, y: 210, width: 40, height: 40, collected: false,
                type: 'balance',
                question: '🎯 FUNDRAISER GOAL!\n\nThe squad needs to raise AT LEAST $80 for new uniforms! 💰\n\nYou\'re selling candy bars for $5 each.\n\nHow many do you need to sell to reach your goal?',
                answer: 16
            },
            {
                x: 650, y: 310, width: 40, height: 40, collected: false,
                type: 'drag',
                question: '🏆 COMPETITION SCORING!\n\nYou just competed and the scores are in! 📊\n\nYour team scored 30 points from stunts! 💪\n\nYour TOTAL score was 50 points!\n\nEach dance section is worth 5 points.\n\nHow many dance sections did you complete?',
                answer: 4,
                options: [2, 4, 6, 8]
            }
        ];
    }

    update() {
        // Player movement
        if (this.keys['ArrowLeft'] || this.keys['a']) {
            this.player.x -= this.player.speed;
            this.player.direction = 'left';
        }
        if (this.keys['ArrowRight'] || this.keys['d']) {
            this.player.x += this.player.speed;
            this.player.direction = 'right';
        }

        // Apply gravity
        this.player.velocityY += this.gravity;
        this.player.y += this.player.velocityY;

        // Ground collision
        if (this.player.y + this.player.height >= this.ground) {
            this.player.y = this.ground - this.player.height;
            this.player.velocityY = 0;
            this.player.jumping = false;
        }

        // Platform collision
        this.platforms.forEach(platform => {
            if (this.checkPlatformCollision(this.player, platform)) {
                this.player.y = platform.y - this.player.height;
                this.player.velocityY = 0;
                this.player.jumping = false;
            }
        });

        // Coin collection
        this.coins.forEach(coin => {
            if (!coin.collected && this.checkCircleCollision(this.player, coin)) {
                coin.collected = true;
                this.score += 10;
                this.playSound('coin');
            }
        });

        // Problem collection
        this.problems.forEach(problem => {
            if (!problem.collected && this.checkCollision(this.player, problem)) {
                problem.collected = true;
                this.triggerProblem(problem);
            }
        });

        // Keep player in bounds
        this.player.x = Math.max(0, Math.min(this.width - this.player.width, this.player.x));

        this.animationFrame++;
    }

    checkCollision(rect1, rect2) {
        return rect1.x < rect2.x + rect2.width &&
               rect1.x + rect1.width > rect2.x &&
               rect1.y < rect2.y + rect2.height &&
               rect1.y + rect1.height > rect2.y;
    }



    checkPlatformCollision(player, platform) {
        return player.x < platform.x + platform.width &&
               player.x + player.width > platform.x &&
               player.y + player.height >= platform.y &&
               player.y + player.height <= platform.y + 20 &&
               player.velocityY >= 0;
    }

    checkCircleCollision(rect, circle) {
        const closestX = Math.max(rect.x, Math.min(circle.x, rect.x + rect.width));
        const closestY = Math.max(rect.y, Math.min(circle.y, rect.y + rect.height));
        const distanceX = circle.x - closestX;
        const distanceY = circle.y - closestY;
        return (distanceX * distanceX + distanceY * distanceY) < (circle.radius * circle.radius);
    }

    draw() {
        // Clear canvas
        this.ctx.fillStyle = '#E6F3FF';
        this.ctx.fillRect(0, 0, this.width, this.height);

        // Draw platforms
        this.platforms.forEach(platform => {
            this.ctx.fillStyle = platform.color;
            this.ctx.fillRect(platform.x, platform.y, platform.width, platform.height);
            this.ctx.strokeStyle = '#fff';
            this.ctx.lineWidth = 2;
            this.ctx.strokeRect(platform.x, platform.y, platform.width, platform.height);
        });

        // Draw coins
        this.coins.forEach(coin => {
            if (!coin.collected) {
                const pulse = Math.sin(this.animationFrame * 0.1) * 3;
                this.ctx.fillStyle = '#FFD700';
                this.ctx.beginPath();
                this.ctx.arc(coin.x, coin.y, coin.radius + pulse, 0, Math.PI * 2);
                this.ctx.fill();
                this.ctx.strokeStyle = '#FFA500';
                this.ctx.lineWidth = 3;
                this.ctx.stroke();

                // Draw $ symbol
                this.ctx.fillStyle = '#FFA500';
                this.ctx.font = 'bold 16px Arial';
                this.ctx.textAlign = 'center';
                this.ctx.textBaseline = 'middle';
                this.ctx.fillText('$', coin.x, coin.y);
            }
        });

        // Draw problems
        this.problems.forEach(problem => {
            if (!problem.collected) {
                const bounce = Math.abs(Math.sin(this.animationFrame * 0.05)) * 10;
                this.ctx.fillStyle = '#FF69B4';
                this.ctx.fillRect(problem.x, problem.y - bounce, problem.width, problem.height);
                this.ctx.strokeStyle = '#FF1493';
                this.ctx.lineWidth = 3;
                this.ctx.strokeRect(problem.x, problem.y - bounce, problem.width, problem.height);

                // Draw ? symbol
                this.ctx.fillStyle = '#fff';
                this.ctx.font = 'bold 24px Arial';
                this.ctx.textAlign = 'center';
                this.ctx.textBaseline = 'middle';
                this.ctx.fillText('?', problem.x + problem.width/2, problem.y - bounce + problem.height/2);
            }
        });

        // Draw player
        this.drawPlayer();

        // Draw HUD
        this.drawHUD();
    }

    drawPlayer() {
        const bob = Math.sin(this.animationFrame * 0.2) * 3;

        // Body
        this.ctx.fillStyle = this.player.color;
        this.ctx.fillRect(this.player.x, this.player.y + bob, this.player.width, this.player.height);

        // Head
        this.ctx.fillStyle = '#FFB6C1';
        this.ctx.beginPath();
        this.ctx.arc(this.player.x + this.player.width/2, this.player.y + bob - 10, 15, 0, Math.PI * 2);
        this.ctx.fill();

        // Eyes
        this.ctx.fillStyle = '#000';
        const eyeOffset = this.player.direction === 'right' ? 3 : -3;
        this.ctx.fillRect(this.player.x + this.player.width/2 - 5 + eyeOffset, this.player.y + bob - 13, 3, 3);
        this.ctx.fillRect(this.player.x + this.player.width/2 + 2 + eyeOffset, this.player.y + bob - 13, 3, 3);

        // Smile
        this.ctx.strokeStyle = '#000';
        this.ctx.lineWidth = 2;
        this.ctx.beginPath();
        this.ctx.arc(this.player.x + this.player.width/2, this.player.y + bob - 8, 8, 0.2, Math.PI - 0.2);
        this.ctx.stroke();

        // Pom-poms
        if (Math.floor(this.animationFrame / 10) % 2 === 0) {
            this.ctx.fillStyle = '#FFD700';
            this.ctx.beginPath();
            this.ctx.arc(this.player.x - 5, this.player.y + bob + 20, 8, 0, Math.PI * 2);
            this.ctx.fill();
            this.ctx.beginPath();
            this.ctx.arc(this.player.x + this.player.width + 5, this.player.y + bob + 20, 8, 0, Math.PI * 2);
            this.ctx.fill();
        }
    }

    drawHUD() {
        // Score
        this.ctx.fillStyle = '#333';
        this.ctx.font = 'bold 24px Arial';
        this.ctx.textAlign = 'left';
        this.ctx.fillText(`💎 Score: ${this.score}`, 10, 30);

        // Level
        this.ctx.fillText(`⭐ Level: ${this.level}`, 10, 60);

        // Controls hint
        this.ctx.font = '16px Arial';
        this.ctx.fillStyle = '#666';
        this.ctx.fillText('Arrow Keys / WASD to move, SPACE to jump', 10, this.height - 10);
    }

    triggerProblem(problem) {
        // Call back to Blazor with problem type and data
        console.log('triggerProblem called!', problem);
        console.log('window.blazorGame exists?', !!window.blazorGame);

        if (window.blazorGame) {
            console.log('Calling ShowProblem...');
            window.blazorGame.invokeMethodAsync('ShowProblem',
                problem.type,
                problem.question,
                problem.answer,
                problem.min || 0,
                problem.max || 10,
                JSON.stringify(problem.options || [])
            ).then(() => {
                console.log('ShowProblem completed successfully!');
            }).catch(err => {
                console.error('ShowProblem failed:', err);
            });
        } else {
            console.error('window.blazorGame is not set! Cannot show problem.');
        }
    }

    playSound(type) {
        // Simple sound feedback
        console.log(`Sound: ${type}`);
    }

    start() {
        this.running = true;
        this.gameLoop();
    }

    stop() {
        this.running = false;
    }

    gameLoop() {
        if (!this.running) return;

        this.update();
        this.draw();

        requestAnimationFrame(() => this.gameLoop());
    }
}

// Global game instance
let gameInstance = null;

window.initGame = (canvasId) => {
    gameInstance = new AlgebraGame(canvasId);
    gameInstance.start();
    return true;
};

window.stopGame = () => {
    if (gameInstance) {
        gameInstance.stop();
    }
};

window.addScore = (points) => {
    if (gameInstance) {
        gameInstance.score += points;
    }
};