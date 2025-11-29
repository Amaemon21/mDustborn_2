# 🚀 DUSTBORN — Post-Apocalyptic FPS Shooter  
**Personal Project • Unity • Zenject • Addressables • MVVM (R3) • Google Sheets Pipeline**

Dustborn is a post-apocalyptic FPS shooter with survival and RPG elements.  
The project focuses on deep systemic gameplay: loot, inventory, crafting, quests, multiple weapon types, and advanced enemy AI.

> **This project serves as a showcase of clean architecture, modular systems, reactive UI, DI patterns, content pipelines, and production-level gameplay systems.**

---

## 📌 Main Features

### 🎮 Gameplay
- Post-apocalyptic FPS / Survival experience  
- Advanced loot system  
- Grid-based inventory  
- Crafting  
- Quest system (conditions, triggers, rewards)  
- Multiple weapon types (guns, melee, modified weapons)  
- AI enemies with patrol, detection, aggression, and state-driven behavior  

---

## 🧱 Project Architecture

### **GameEntryPoint**
Central initialization point that bootstraps all services, installers, systems, and scene composition.

### **Zenject**
Used for:
- Dependency Injection  
- Modular architecture  
- Testability  
- Runtime scene composition  
- Swappable services (Inventory, Loot, AI, Persistence)

### **Addressables**
Handles memory-safe loading of:
- Sprites  
- Models  
- FX  
- Configs and data  

### **MVVM + R3 (Reactive Architecture)**
- ViewModel holds the state  
- UI updates through R3 observables  
- No direct references between View and Model  
- Fully reactive, safe, and scalable UI bindings  

---

## 📦 Content Pipeline  
### **Google Sheets → JSON → ScriptableObject → Runtime**

A fully automated content workflow:

1. **Google Sheets**  
   Designers edit item data:  
   ID, Name, Description, Type, Rarity, Stack size, IconName.

2. **Export to JSON**  
   Google Sheets are converted into `.json` files automatically.

3. **ScriptableObject Generation**  
   Unity importer creates SO items from JSON.  
   **Important:** ScriptableObjects contain **only primitive fields**:  
   - `string`  
   - `int`
   - 'enum'
   
   This makes SO lightweight and memory-efficient.

4. **Sprites are NOT stored in SO**  
   Only the **icon name** is stored:
   ```json
   "IconName": "medkit_01"

Gameplay Systems
Inventory
 - Grid layout
 - Stack limits
 - Drag & Drop logic
 - Fully reactive UI (MVVM + R3)

Loot System
 - Loot tables
 - Item rarities
 - Dynamic enemy/container drops
 - Easily extendable through config data

Weapons
 - Modular weapon system
 - Fire types, recoil, spread, sway
 - Attachment and upgrade support

AI Enemies
 - Patrol logic
 - Player detection
 - Aggression and chase
 - Sound-based reaction
 - Behaviour StateMachine

Quests
 - Conditions and triggers
 - Rewards (items, XP, scripted actions)
 - Reactive quest journal UI

 Technologies Used
  - Zenject
  - Addressables
  - MVVM + R3
  - Google Sheets → JSON → ScriptableObject pipeline
  - ScriptableObject factories

Links
 - Telegram Dev Log: http://t.me/dustborn21
 - GitHub Repository: https://github.com/Amaemon21/mDustborn_2

Project Status
Active Development — new systems, AI, weapons and content are added frequently.
