# Unity Game Developer Case Study - Deterministic Roulette

## Objective
Create a **3D** single-player Unity prototype of a **Deterministic Roulette** game, focusing on immersive roulette wheel animations, robust data tracking, and standard Roulette rules. The game should allow players to **manually choose** the next winning number for testing or controlled gameplay scenarios.

---

## Features

### 1. Roulette Wheel & Animations
- **Deterministic Outcome Selection**: Players can select the next winning number via UI. If not selected, the result is random.
- **Wheel Spinning Animation**: Smooth 3D wheel spin with realistic ball drop.
- **Visual Cues**: Clear indication of winning numbers and colors (red/black/green).

### 2. Player Statistics
- **Win/Loss Tracking**: Track spins, total wins, and overall profit/loss.
- **Historical Record**: Display statistics in the UI or a dedicated menu.

### 3. Full Roulette Rules & Mechanics
- **Betting & Chip System**: Place bets using different chip denominations.
- **Bet Types**: Support Inside Bets (Straight, Split, etc.) and Outside Bets (Red/Black, Even/Odd, etc.).
- **Bonus: Zero/Double-Zero (Optional)**: Toggle between European (single zero) or American (double zero) rules.
- **Spin & Payout**: Calculate winnings and update chips after each spin.
- **Multi-Round Flow**: Retain win/loss counters between spins.

### 4. *Bonus: Save & Load Game State
- **Auto-Save (Optional)**: Save game state (bets, chips, etc.) on exit.
- **Resume (Optional)**: Continue from the saved state on reopening.
- **Persistent Data (Optional)**: Retain stats and preferences across sessions.

### 5. Creatives
- **3D Assets**: Licensed or custom models for the table, chips, and environment.
- **UI**: Polished interface for betting, statistics, and deterministic selection.
- **Sound Effects**: Audio for wheel spin, ball drop, wins, and losses.
- **VFX**: Celebratory effects (e.g., confetti) for wins.

---

## Evaluation Criteria
1. **Gameplay & Mechanics**:  
   - Realistic wheel spin and accurate roulette logic.  
   - Functional deterministic outcome selection.  
   - Intuitive bet placement.  

2. **Persistence & Statistics**:  
   - Track and display win/loss stats.  
   - *(Optional)* Save/load game state.  

3. **Code Quality**:  
   - Adherence to OOP/SOLID principles.  
   - Modular, reusable components.  

4. **Creativity & Polish**:  
   - Engaging visuals, sounds, and effects.  
   - Cohesive 3D assets.  

5. **Version Control**:  
   - Clear Git commits and structured branching.  

6. **Documentation**:  
   - Code comments and README detailing architecture, setup, and usage.  

---

## Technologies
- **Engine**: Unity (2022.3.x)  
- **Language**: C#  
- **Version Control**: Git  

---

## Additional Notes
- **Restrictions**: No third-party plugins/SDKs (except Unity UI frameworks).  
- **Assets**: Textures, audio, and 3D models may be sourced online (e.g., Unity Asset Store).  

---

## README Requirements
Include a **README.md** with:  
- **Controls/Instructions**: How to play, place bets, and use deterministic mode.  
- **Known Issues/Future Plans**: Areas for improvement.  
- **Demo Link/Video**: Showcase the prototype.  
- **OOP Principles**: Explain applied principles (e.g., Encapsulation, SOLID).  
