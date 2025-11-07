# Game Design Document — La Driftrance

## Objectif du document
Fournir un guide technique et de design clair pour implémenter un prototype jouable et extensible du jeu "La Driftance".

## Contrainte de prototype
- **Plateforme** : Unity (URP). Utiliser les assets Synty déjà présents.
- **Livrable MVP** : une scène jouable (Prototype_Fairground) avec PlayerCar contrôlable en drift, 2 Police AI véhicules, Marvin jouant des lignes via Speakers.

---

## 1) Résumé du gameplay
- **Type** : jeu de drift arcade-horreur solo (éventuellement co-op/multijoueur ultérieurement).
- **But** : atteindre une extraction en évitant/semant des voitures de police contrôlées par des loup-garous ou Marvin.

---

## 2) Mécaniques détaillées

### Contrôles
- Accélérer, freiner, braquage, frein à main (drift), boost
- Bindings via `InputSystem_Actions.inputactions`

### Drift detection
- Mesurer l'angle entre le vector forward du véhicule et sa vélocité projetée
- Si angle > seuil ET vitesse > seuil → état Drift actif et charge jauge Boost

### Boost
- Consommation instantanée d'une réserve → force appliquée en avant
- Applique un bref cooldown

### Collisions
- Gestion via Rigidbody + WheelColliders
- Collisions élevées déclenchent camera shake et dégâts au véhicule (durabilité optionnelle)

---

## 3) IA et comportements

### États PoliceCar
- **Patrouille** : waypoints
- **Alerte** : investigation autour du dernier point de détection
- **Poursuite** : tracking vers position du joueur
- **Immobilisé** : si dégâts critiques
- **Recherche** : perte de trace

### Transition
- Détection par distance et angle de vision (cone) + bruit (événements)
- Marvin peut forcer état Alerte/Poursuite globalement

### Werewolf behaviour
- Si PoliceCar immobilisée → spawn d'un Werewolf qui chasse à pied (melee)
- Werewolf a cooldown d'attaque et animation de saut

---

## 4) Marvin — System Design

### MarvinController (singleton)
API publique :
- `Marvin.Speak(lineId)` : joue la ligne sur le Speaker le plus proche ou un Speaker ciblé ; déclenche effets secondaires (sirènes, boost IA aggression)
- `Marvin.TriggerEvent(eventId)` : ex. CloseGate(zoneId), OpenShortcut(shortcutId), PowerOutage(duration)
- `Marvin.SetAggression(level, duration)` : influence param IA (speedMultiplier, detectionRadius)

### Speakers
- Objets 3D avec AudioSource (spatialized) et tag `Speaker`
- Possibilité d'avoir snapshots audio et ducking

---

## 5) Données et paramètres

### PlayerCar parameters
- mass, engineForce, brakeForce, maxSteerAngle
- gripForward, gripSideways
- driftAngleThreshold, boostAmount, boostCooldown

### Police parameters
- patrolSpeed, pursuitSpeed
- detectionRadius, detectionAngle
- aggressionMultiplier

---

## 6) UI/UX

### HUD
- Speedometer
- BoostGauge
- Minimap radar indiquant police (icônes)
- Objective marker
- Vehicle health (optionnel)

### Messages contextuels
- "Vous êtes repéré"
- "Extraction dans Xs"
- "Checkpoint ouvert"
- Toasts courts pour événements Marvin

---

## 7) Audio & VO

### Mixer groups
- Music, SFX, Voice, UI
- Snapshots : Normal, Alert, Panic

### Marvin
- 10+ lignes vocales
- Spatialization via speakers
- Ducking music on announcements

---

## 8) Art & Level Design

### Assets Synty
- `PolygonHorrorCarnival` : layout fête foraine
- `PolygonStreetRacer` : véhicules et props de rue
- `PolygonPumpkins` & `PolygonHalloweenMasks` : ambiance et décorations

### Level composition
- Routes principales
- Zones latérales (shortcuts)
- Manèges destructibles
- Obstacles dynamiques

---

## 9) Tests et critères d'acceptation (MVP)

- Player drift feels responsive and boost activates when charged
- Police AI engages pursuit and can be influenced by Marvin lines
- Marvin plays audio on Speakers and triggers at least one event (ex : close gate) per run
- Scene runs at >=30 FPS on target machine with medium settings

---

## 10) Roadmap & estimations

- **Prototype (MVP)** : 1–2 semaines — playable scene, basic AI, Marvin voice integration
- **Alpha** : 6–8 semaines — multiple maps, tuning, additional AI states, upgrades
- **Beta** : 3 mois+ — balancing, content, QA, potential multiplayer

---

## Edge cases à gérer

- Voiture coincée → respawn ou self‑righting après timeout
- IA bloquée → fallback waypoint/teleport reposition
- Trop de sources audio → ducking & limite concurrent voices
- Tunnelling collisions à haute vitesse → continuous collision detection
- Perf sur bas de gamme → LOD, culling, limiter particules
