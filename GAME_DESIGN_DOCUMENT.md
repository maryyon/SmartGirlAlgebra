# 🎮 ALGEBRA QUEST - Game Design Document

## Executive Summary

**Title:** Algebra Quest: The Mathematical Adventure  
**Genre:** 3D Educational Adventure/Puzzle Platformer  
**Platform:** PC, Web (Unity WebGL), Mobile (future)  
**Target Audience:** 7th-8th grade students (ages 12-14)  
**Core Hook:** Explore a magical 3D world where solving algebra unlocks treasures, powers, and new areas

---

## 1. Game Vision

### High Concept
*"Zelda meets Khan Academy"* - A 3D adventure game where algebra is the key to everything. Students explore vibrant worlds, battle math monsters, solve environmental puzzles, and compete with friends - all while mastering 7th-8th grade algebra without realizing they're learning.

### Design Pillars
1. **Exploration First** - Curiosity drives learning
2. **Math as Power** - Solving problems = gaining abilities
3. **No Punishment** - Mistakes are learning opportunities
4. **Social & Competitive** - Leaderboards, co-op, challenges
5. **Progression Visible** - Always see yourself improving

---

## 2. Story & Setting

### The World: Mathoria
A magical floating island broken into 7 regions, each representing an algebra topic. The evil **Chaos Lord** has scattered the **Equations of Power** across the land, causing mathematical disorder.

### Player Role
You are a **Math Apprentice** chosen to restore balance by collecting the Equations of Power and mastering algebra to defeat the Chaos Lord.

### Narrative Arc
- **Act 1:** Tutorial Island → Learn basics, get first power
- **Act 2:** Explore 5 regions → Master different algebra topics
- **Act 3:** Unlock Final Fortress → Face Chaos Lord in epic math battle
- **Endgame:** Infinite challenges, leaderboards, multiplayer

---

## 3. Core Gameplay Loop

```
EXPLORE → ENCOUNTER → SOLVE → REWARD → PROGRESS
    ↑                                        ↓
    ←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←←
```

### Detailed Loop
1. **Explore** the 3D world (walk, run, jump, climb, fly)
2. **Encounter** challenges (locked doors, NPCs, puzzles, enemies)
3. **Solve** algebra problems to overcome challenges
4. **Reward** with coins, gems, XP, items, story progression
5. **Progress** to new areas, unlock abilities, level up

---

## 4. Game Mechanics

### 4.1 Movement & Exploration
- **WASD** - Move character
- **Space** - Jump (double jump unlockable)
- **Shift** - Sprint (stamina-based)
- **E** - Interact with objects/NPCs
- **Tab** - Open inventory/map
- **Mouse** - Look around (3rd person camera)

**Unlockable Movement:**
- Level 5: Double Jump
- Level 10: Glide
- Level 15: Wall Climb
- Level 20: Teleport

### 4.2 Challenge Types

#### A. Environmental Puzzles
**Example:** Bridge Repair
- Broken bridge blocks path
- Equation appears: `2x + 5 = 13`
- Solve correctly → Bridge materializes
- Cross to new area

#### B. NPC Quests
**Example:** Merchant's Dilemma
- NPC: "I need 3 more apples than twice what I have. I need 11 total. How many do I have?"
- Build equation: `2x + 3 = 11`
- Solve → Get reward (coins, item)

#### C. Combat Encounters
**Example:** Math Monster Battle
- Enemy appears with HP bar
- Each correct answer = damage dealt
- Wrong answer = small HP loss (not game over!)
- Defeat enemy → XP + loot

#### D. Treasure Hunts
**Example:** Hidden Chest
- Clue: "The treasure is at coordinates where x + 3 = 7"
- Solve → Coordinates revealed
- Navigate to location → Open chest

#### E. Timed Challenges (Optional)
**Example:** Speed Run
- Solve 5 equations in 2 minutes
- Leaderboard ranking
- Bonus rewards for top times

### 4.3 Problem Presentation

**Interactive UI Overlay:**
```
┌─────────────────────────────────────┐
│  🧙‍♂️ Wizard's Challenge            │
│                                     │
│  Solve: 3x - 7 = 14                │
│                                     │
│  [Input: ____]                     │
│                                     │
│  [💡 Hint] [✓ Submit] [❌ Skip]    │
│                                     │
│  Streak: 🔥 5  |  XP: +50          │
└─────────────────────────────────────┘
```

**Features:**
- Clean, non-intrusive UI
- Pause game world during problem
- Show work area for calculations
- Progressive hints (3 levels)
- Skip option (costs coins, no XP)

---

## 5. Progression Systems

### 5.1 Character Leveling
- **XP Sources:** Solving problems, completing quests, finding secrets
- **Level Benefits:** 
  - New abilities (movement, powers)
  - Stat increases (speed, stamina)
  - Cosmetic unlocks

### 5.2 Skill Trees
**Three Branches:**
1. **Explorer** - Movement abilities, map reveals
2. **Scholar** - Hint bonuses, XP multipliers
3. **Champion** - Combat bonuses, leaderboard perks

### 5.3 Currency System
- **Gold Coins** - Earned from solving problems
- **Gems** - Rare drops from hard challenges
- **Star Fragments** - Collectibles hidden in world

**Spending:**
- Cosmetics (outfits, accessories, pets)
- Hints and skips
- Fast travel unlocks
- Temporary power-ups

### 5.4 Achievement System
- **First Steps** - Solve first equation
- **Streak Master** - 10 correct in a row
- **Explorer** - Visit all 7 regions
- **Perfectionist** - Complete area with 100%
- **Speed Demon** - Top 10 on any leaderboard
- **Treasure Hunter** - Find all hidden chests
- **Math Wizard** - Reach max level

---

## 6. World Design

### Region Map
```
                    🏔️ Peak of Polynomials
                         ↑
    🌲 Forest ← 🏰 Hub → ⛰️ Mountain
       ↓                    ↓
    🌊 Ocean ← 🏜️ Desert → 🔥 Volcano
                ↓
            👑 Final Fortress
```

### 6.1 Tutorial Island (Hub)
**Theme:** Peaceful village  
**Algebra Topic:** Basic equations (one-step)  
**Challenges:** 5 tutorial quests  
**Unlocks:** Access to first 3 regions

### 6.2 Forest of Variables
**Theme:** Enchanted forest, glowing trees  
**Algebra Topic:** Multi-step equations, variables  
**Challenges:** 
- Tree puzzles (balance equations)
- Animal NPCs with word problems
- Hidden treasure coordinates
**Boss:** The Equation Ent

### 6.3 Mountain of Exponents
**Theme:** Rocky peaks, floating platforms  
**Algebra Topic:** Exponents, powers, roots  
**Challenges:**
- Climbing puzzles (solve to create handholds)
- Wind currents (calculate trajectories)
- Crystal caves (pattern recognition)
**Boss:** The Power Golem

### 6.4 Ocean of Inequalities
**Theme:** Underwater realm, coral reefs  
**Algebra Topic:** Inequalities, graphing basics  
**Challenges:**
- Current navigation (inequality ranges)
- Submarine controls (coordinate plane)
- Treasure diving (depth calculations)
**Boss:** The Inequality Kraken

### 6.5 Desert of Systems
**Theme:** Ancient ruins, sandstorms  
**Algebra Topic:** Systems of equations  
**Challenges:**
- Door mechanisms (2 equations, 2 unknowns)
- Oasis finding (intersection points)
- Pyramid puzzles (substitution method)
**Boss:** The Sphinx of Systems

### 6.6 Volcano of Polynomials
**Theme:** Lava flows, obsidian platforms  
**Algebra Topic:** Polynomials, factoring  
**Challenges:**
- Lava bridge timing (factor to find safe times)
- Rock formations (polynomial patterns)
- Heat shield puzzles (simplify expressions)
**Boss:** The Polynomial Phoenix

### 6.7 Peak of Quadratics
**Theme:** Snowy summit, aurora lights  
**Algebra Topic:** Quadratic equations  
**Challenges:**
- Parabolic jumps (trajectory calculations)
- Ice slide puzzles (quadratic formula)
- Crystal alignment (completing the square)
**Boss:** The Quadratic Dragon

### 6.8 Final Fortress
**Theme:** Dark castle, chaos energy  
**Final Boss:** The Chaos Lord  
**Challenge:** Mixed algebra topics, adaptive difficulty  
**Reward:** Game completion, infinite mode unlock

---

## 7. Multiplayer & Social

### 7.1 Leaderboards
- **Global Rankings** - Total XP, level, achievements
- **Regional Rankings** - Per-topic mastery
- **Speed Runs** - Fastest completion times
- **Weekly Challenges** - Rotating special events

### 7.2 Co-op Mode (Future)
- **2-4 Players** - Explore together
- **Shared Challenges** - Solve collaboratively
- **Trade System** - Exchange items
- **Team Battles** - Compete against other teams

### 7.3 PvP Arena (Future)
- **Math Duels** - Race to solve problems
- **Battle Royale** - Last player standing
- **Tournament Mode** - Bracketed competition

---

## 8. Art Style & Audio

### Visual Style
**Inspiration:** Zelda: Breath of the Wild meets Fortnite
- **Colorful & Vibrant** - Not realistic, stylized
- **Readable** - Clear visual hierarchy
- **Magical** - Glowing effects, particle systems
- **Optimized** - Runs on mid-range hardware

### Character Design
- **Customizable Avatar** - Choose gender, skin tone, hair, outfit
- **Unlockable Cosmetics** - Hats, capes, pets, emotes
- **Expressive Animations** - Victory dances, emotes

### Audio
- **Music:** Upbeat, adventurous, region-specific themes
- **SFX:** Satisfying feedback (coins, level up, correct answer)
- **Voice:** Optional NPC voice acting (text-to-speech acceptable)

---

## 9. Technical Architecture

### Engine: Unity 3D
**Version:** Unity 2022 LTS or newer  
**Language:** C# (reuse existing algebra logic!)  
**Rendering:** Universal Render Pipeline (URP)

### Core Systems
1. **Player Controller** - Movement, camera, input
2. **Challenge Manager** - Problem generation, validation
3. **Progression System** - XP, levels, unlocks
4. **Save System** - Cloud save (PlayFab or similar)
5. **UI System** - Menus, HUD, problem overlay
6. **World Manager** - Region loading, streaming
7. **Multiplayer** - Photon or Mirror (future)

### Data Structure
```csharp
// Reuse your existing models!
public class AlgebraChallenge
{
    public AlgebraTopic Topic;
    public int Difficulty;
    public string Question;
    public string Answer;
    public List<string> Hints;
    public int XPReward;
    public int CoinReward;
}
```

---

## 10. Monetization (Optional)

### Free-to-Play Model
- **Base Game:** 100% free, all content accessible
- **Cosmetics:** Optional purchases (outfits, pets, emotes)
- **Battle Pass:** Seasonal rewards track
- **No Pay-to-Win:** Cannot buy XP, levels, or advantages

### Premium Version (Alternative)
- **One-Time Purchase:** $9.99
- **All Content Included**
- **No Ads**
- **Exclusive Cosmetics**

---

## 11. Development Roadmap

### Phase 1: Prototype (4-6 weeks)
- [ ] Unity project setup
- [ ] Basic 3D world (one small area)
- [ ] Player movement (WASD, jump, camera)
- [ ] One challenge type (environmental puzzle)
- [ ] Problem UI overlay
- [ ] Integrate algebra logic from existing code
- [ ] Basic reward system (coins, XP)

### Phase 2: Vertical Slice (8-12 weeks)
- [ ] Complete one full region (Forest)
- [ ] All challenge types implemented
- [ ] Character progression system
- [ ] Save/load functionality
- [ ] Polish and juice (particles, sounds)
- [ ] Playtest with students

### Phase 3: Content Expansion (12-16 weeks)
- [ ] Build remaining 6 regions
- [ ] Boss battles
- [ ] Achievement system
- [ ] Leaderboards
- [ ] More cosmetics and unlockables

### Phase 4: Polish & Launch (4-8 weeks)
- [ ] Bug fixes
- [ ] Performance optimization
- [ ] Tutorial refinement
- [ ] Marketing materials
- [ ] Soft launch (beta testing)
- [ ] Full release

### Phase 5: Post-Launch (Ongoing)
- [ ] Multiplayer features
- [ ] Seasonal events
- [ ] New regions/content
- [ ] Community feedback integration

---

## 12. Success Metrics

### Educational Goals
- ✅ Students spend 30+ minutes per session
- ✅ 80%+ problem accuracy after practice
- ✅ Voluntary return rate >60%
- ✅ Self-reported confidence increase

### Engagement Goals
- ✅ 10,000+ players in first 3 months
- ✅ Average session length: 25+ minutes
- ✅ Daily active users: 30%+ of total
- ✅ Completion rate: 40%+ finish Act 1

---

**Next Steps:** Review this GDD, then we'll start building the Unity prototype! 🚀

