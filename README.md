# CyberArena
## 🚧 Work in Progress

This project is still under development. Features are being added incrementally, and there may be breaking changes as the codebase matures.


# 🕹️ Isometric Dungeon Crawler

A Unity-based dungeon crawler with an isometric view, likely set in a cyberpunk-inspired world.  
This project is a gameplay-focused prototype combining procedural generation, modular systems, and stylized visuals.  

---

## ✨ Features

- 🔄 **Procedural Level Generation**  
  Dynamically creates dungeon floors using modular templates and custom logic.

- 🔧 **Highly Modular Weapon System** *(WIP)*  
  Weapons are built from interchangeable parts and behaviors for customization and experimentation.

- 🐍 **Swarming Enemies** *(Planned - Using Unity Entity Framework)*  
  Implementing efficient enemy AI that can swarm and adapt to the player’s movement, leveraging Unity’s Entity Framework for high performance and scalability.

## 🧱 Architecture

- Modular, maintainable codebase focused on reusability.
- Heavy use of **ScriptableObjects** for configuring base data like:
  - Level templates
  - Weapons & modules
  - Enemy stats & behaviors
- Clear separation of logic (e.g., model-view-controller approach) for flexibility and scalability.

## 🧩 Design Patterns

- **Builder Pattern**  
  Used for room creation in the dungeon generation. The **RoomBuilder** class allows for flexible and dynamic room construction, ensuring rooms can be customized and generated with ease, whether they need specific features (doors, traps, enemies) or are simply empty rooms.

- **Factory Pattern**  
  Utilized for creating and configuring weapons and enemies dynamically, enabling easier addition of new types without altering existing code.

- **Observer Pattern**  
  Applied for weapon and player state updates, where the system reacts to changes in player stats or weapon mods, keeping the system decoupled and flexible.

---
