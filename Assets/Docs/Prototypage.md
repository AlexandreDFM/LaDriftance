# Plan de prototypage — La Driftance

## Objectif
Créer une scène prototype jouable dans Unity avec PlayerCar, drift, Police AI et Marvin système vocal.

---

## Étapes concrètes

### 1. Scene setup
- Créer une scène `Prototype_Fairground`
- Importer packs Synty (`PolygonHorrorCarnival`, `PolygonStreetRacer`, `PolygonPumpkins`, `PolygonHalloweenMasks`)
- Placer terrain, allée principale, 1-2 manèges

### 2. Player vehicle
- Instancier `PlayerCar` (ex. from PolygonStreetRacer)
- Ajouter Rigidbody/WheelCollider
- Attacher `PlayerController` script
- Lier `InputSystem_Actions`

### 3. Camera
- Ajouter Cinemachine VirtualCamera
- Composer to follow player
- Add camera shake on collisions

### 4. Police AI
- Créer 2 PoliceCar prefabs with simple waypoint path
- Pursuit transition (if player within X meters, switch to pursuit steering)

### 5. Marvin & Speakers
- Créer `MarvinController` that holds audio clips
- List of `Speaker` gameobjects placed around map
- Implement simple API: `Marvin.Speak(lineId)` → plays clip on nearest active speaker and triggers AI aggression boost

### 6. Drift & Boost
- Implement wheel friction tuning
- Drift detection (angle between forward and velocity) that charges a boost meter
- On boost, apply forward force

### 7. Simple HUD
- Speed
- Boost gauge
- Minimap radar icons for police

### 8. QA loop
- Playtest for feel & tuning

---

## Mapping des assets Synty

- **`Assets/PolygonStreetRacer/`** → véhicules, routes, barrières, pneus, effets de véhicule  
  *Utilisé pour PlayerCar et PoliceCar prefabs*

- **`Assets/PolygonHorrorCarnival/`** → manèges, stands, lampadaires, décor de foire  
  *Utilisé pour level geometry*

- **`Assets/PolygonPumpkins/`** & **`Assets/PolygonHalloweenMasks/`** → props d'ambiance, textures de fête d'Halloween, masques accrochés, décors de stand

- **`Assets/InputSystem_Actions.inputactions`** → utiliser pour bindings

---

## Tests rapides à effectuer après prototypage

### Jouabilité
- Drift feeling
- Collision handling
- Pursuit behavior sur 3 runs

### Fonctionnel
- Marvin triggers speakers
- Police IA change état

### Performance
- Profiler check sur Mac (scènes heavy with particles)
- Reduce if <30 fps

---

## Propositions de contenu additionnel et priorisation

### MVP (2–4 semaines prototype)
- 1 map
- PlayerCar
- 2 Police AI
- Marvin with 3 voice lines
- Basic drift + boost
- HUD

### v1 (2–3 mois)
- Multiple maps
- Vehicle upgrades
- Unlockables
- More AI behaviors
- Daily challenges

### Futur
- Multiplayer chase
- AI directors for dynamic intensity
- Procedural events

---

## Plan de son et voix (Marvin)

- Enregistrer 10 lines de Marvin (taunts, instructions, menaces)
- Prévoir variations selon distance/état
- Placer haut‑parleurs comme objets 3D
- Marvin utilise spatialized audio pour immersion
- Mixer snapshots : calm → alert → panic (ducking musique + sirènes)

---

## Livrables concrets

- Prototype Unity scene `Prototype_Fairground`
- `PlayerController.cs`
- Simple `PoliceAI.cs`
- `MarvinController.cs`
- README succinct dans `Assets/Docs/` expliquant comment lancer la scène et bindings Input
