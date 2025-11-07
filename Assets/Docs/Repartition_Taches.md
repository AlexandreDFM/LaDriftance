# Répartition des tâches — Game Jam (3 personnes)

## 🎮 Mode de jeu pour la jam : Local Split-Screen

**Concept adapté** : 1 joueur contrôle le fuyant (drift), 1 joueur contrôle Marvin (voitures de police + événements), écran splitté horizontal ou vertical.

**Durée estimée** : 48-72h (game jam standard)

---

## 👥 Répartition des rôles

### 🔧 Personne 1 : Programmeur Gameplay / Véhicules
**Responsabilité principale** : Mécanique de conduite, physique véhicule, split-screen

#### Tâches prioritaires (Jour 1)
- [ ] Setup Input System pour 2 joueurs (Player1 = Fuyant, Player2 = Marvin)
  - Configurer `InputSystem_Actions.inputactions` avec 2 device schemes
  - Player1 : WASD + Space (handbrake) + Shift (boost)
  - Player2 : Arrow keys + actions pour contrôler police/événements
- [ ] Créer `PlayerCarController.cs` avec drift basique
  - Rigidbody + WheelColliders (ou arcade controller simplifié)
  - Drift detection (angle forward vs velocity)
  - Boost simple (force additionnelle)
- [ ] Implémenter split-screen avec Cinemachine
  - 2 Virtual Cameras (une pour chaque joueur)
  - Viewport horizontal (50/50) ou vertical
  - Camera follow pour Player1, vue god-mode ou fixe pour Player2

#### Tâches secondaires (Jour 2)
- [ ] `PoliceCarController.cs` - contrôle direct par Player2
  - 1-2 voitures que Player2 peut prendre le contrôle
  - Switcher entre les voitures (Tab key)
  - Collisions et poursuite manuelle
- [ ] Système de collision et dégâts basique
  - Health pour véhicules
  - Respawn si coincé
- [ ] Debug et tuning physique

#### Jour 3 (polish)
- [ ] Ajustements gameplay basés sur playtest
- [ ] Fix bugs critiques

---

### 🎨 Personne 2 : Level Design / Intégration Assets
**Responsabilité principale** : Création de la map, placement props, ambiance visuelle

#### Tâches prioritaires (Jour 1)
- [ ] Setup scène `GameJam_Fairground`
  - Importer assets Synty (`PolygonHorrorCarnival`, `PolygonStreetRacer`, `PolygonPumpkins`, `PolygonHalloweenMasks`)
  - Créer terrain de base avec routes principales
  - Placer 2-3 manèges iconiques (grande roue, carrousel)
- [ ] Définir zones clés
  - Zone de départ (spawn Player1)
  - 2-3 checkpoints
  - Zone d'extraction (objectif final)
  - Points de spawn police pour Player2
- [ ] Éclairage de base
  - Lumières néon/volumétriques
  - Fog pour ambiance
  - Post-processing URP (bloom, color grading sombre)

#### Tâches secondaires (Jour 2)
- [ ] Props et obstacles
  - Stands, barrières, raccourcis
  - Objets destructibles (optionnel)
  - Zones de drift optimales
- [ ] Waypoints et références pour police
  - Marqueurs pour navigation Player2
- [ ] Effets visuels
  - Particules (fumée, feux, étincelles)
  - Décals pour routes usées

#### Jour 3 (polish)
- [ ] Polish visuel (props additionnels, détails)
- [ ] Optimisation performances (culling, LOD si nécessaire)
- [ ] Build lighting final

---

### 🎵 Personne 3 : Audio / UI / Marvin System
**Responsabilité principale** : Son, UI, système de Marvin et événements

#### Tâches prioritaires (Jour 1)
- [ ] Setup AudioMixer
  - Groups : Music, SFX, Voice, UI
  - Snapshots : Normal, Alert, Chase
- [ ] UI de base
  - HUD Player1 : vitesse, jauge boost, timer
  - HUD Player2 : contrôles Marvin disponibles, cooldowns
  - Message "Extraction atteinte" / "Police gagne"
- [ ] Intégrer musique d'ambiance
  - Track d'ambiance dark carnival (libre de droits ou créée)
  - Intensité qui monte pendant poursuite

#### Tâches secondaires (Jour 2)
- [ ] `MarvinController.cs` - système d'événements pour Player2
  - 3-4 actions activables par Player2 :
    - Boost vitesse police (cooldown 30s)
    - Fermer raccourci (cooldown 45s)
    - Activer sirène/lumières distraction (cooldown 20s)
    - Message vocal via Speaker (cooldown 15s)
  - UI montrant cooldowns
- [ ] Speakers placement + audio spatial
  - 4-6 Speakers dans la map
  - Player2 peut activer Speaker proche pour jouer ligne Marvin
  - 3-5 lignes vocales (TTS ou enregistrées)
- [ ] SFX essentiels
  - Moteur, drift, frein
  - Sirènes police
  - Collisions
  - UI clicks

#### Jour 3 (polish)
- [ ] Voix Marvin finales (si temps : enregistrement acteur)
- [ ] Menu principal simple (Start Game, Quit)
- [ ] Écran de victoire/défaite
- [ ] Balance audio final

---

## 📅 Timeline suggérée (72h jam)

### Jour 1 (0-24h) — Foundation
**Objectif** : Prototype jouable avec véhicules et split-screen fonctionnel

- **H0-8** : Setup projet, import assets, scène de base, input system
- **H8-16** : PlayerCar contrôlable + split-screen camera
- **H16-24** : Map layout basique, UI minimale, audio setup

**Milestone Jour 1** : 2 joueurs peuvent se déplacer en split-screen sur une map simple

---

### Jour 2 (24-48h) — Core Gameplay
**Objectif** : Mécanique de jeu complète (poursuite, Marvin, objectifs)

- **H24-32** : Police contrôlable par Player2, système Marvin événements
- **H32-40** : Objectifs (extraction), conditions victoire/défaite, props map
- **H40-48** : SFX, voix Marvin, polish mécanique

**Milestone Jour 2** : Jeu jouable de bout en bout avec win/lose conditions

---

### Jour 3 (48-72h) — Polish & Playtesting
**Objectif** : Balance, polish, bug fixes

- **H48-60** : Playtesting intensif, ajustements gameplay
- **H60-68** : Polish audio/visuel, menu, build final
- **H68-72** : Buffer pour bugs critiques et préparation soumission

**Milestone Jour 3** : Build stable prêt à soumettre

---

## 🎯 Features prioritaires (Must-Have pour jam)

### Core (obligatoire)
- [x] PlayerCar drift fonctionnel
- [x] Split-screen 2 joueurs
- [x] Police contrôlable par Player2
- [x] Map jouable avec extraction
- [x] Win/lose conditions
- [x] UI basique (HUD, menus)

### Important (nice-to-have)
- [ ] Système Marvin avec 3 événements
- [ ] 3-5 lignes vocales Marvin
- [ ] Boost drift pour Player1
- [ ] Ambiance audio complète
- [ ] Post-processing URP

### Optionnel (si temps restant)
- [ ] Checkpoints intermédiaires
- [ ] Véhicule health/dégâts
- [ ] Raccourcis destructibles
- [ ] Score/stats fin de partie
- [ ] IA police backup (si Player2 veut aide)

---

## 🔄 Communication et workflow

### Daily syncs
- **Matin** (15 min) : check-in, objectifs du jour
- **Soir** (15 min) : démo progrès, ajustements pour lendemain

### Git workflow
- Branch `master` = stable
- Chacun travaille sur sa feature branch
- Merge via pull requests ou direct (si petite équipe, direct OK)
- Commit souvent, push régulièrement

### Build tests
- Build test toutes les 6-8h pour vérifier intégration
- Player2 (Level Designer) responsable de maintenir scène principale propre

---

## 🛠️ Setup technique initial (à faire ensemble, H0-2)

### Unity Project Settings
- [x] URP configuré (`URPProjectSettings.asset` déjà présent)
- [ ] Input System package installé et `InputSystem_Actions.inputactions` setup
- [ ] Cinemachine package installé
- [ ] Post-processing activé

### Folders structure
```
Assets/
  ├── Docs/              (déjà créé)
  ├── Scenes/
  │   └── GameJam_Fairground.unity
  ├── Scripts/
  │   ├── Player/
  │   │   ├── PlayerCarController.cs
  │   │   └── DriftDetection.cs
  │   ├── Police/
  │   │   └── PoliceCarController.cs
  │   ├── Marvin/
  │   │   ├── MarvinController.cs
  │   │   └── Speaker.cs
  │   └── UI/
  │       └── GameUI.cs
  ├── Prefabs/
  │   ├── PlayerCar.prefab
  │   ├── PoliceCar.prefab
  │   └── Speaker.prefab
  ├── Audio/
  │   ├── Music/
  │   ├── SFX/
  │   └── Marvin/
  └── Materials/
```

### Assets Synty à utiliser
- `PolygonStreetRacer/` : véhicules (base PlayerCar & PoliceCar)
- `PolygonHorrorCarnival/` : manèges, stands, layout
- `PolygonPumpkins/` + `PolygonHalloweenMasks/` : déco et ambiance

---

## 📝 Notes importantes

### Split-Screen spécifique
- **Layout recommandé** : horizontal (top = Player1 fuyant, bottom = Player2 Marvin vue tactique)
- **Camera Player1** : third-person follow derrière voiture
- **Camera Player2** : vue isométrique/RTS-style pour voir la map et placer événements, OU première personne dans voiture de police avec switch

### Marvin simplifié pour jam
Au lieu d'IA vocale complexe, Player2 **EST** Marvin :
- Contrôle direct des voitures de police (1-2 véhicules)
- Peut activer 3-4 événements via UI (buttons avec cooldowns)
- Peut déclencher lignes vocales manuellement via Speakers

### Scope réaliste
- **1 map** unique (pas de multi-niveaux)
- **1 mode de jeu** : Player1 fuit vers extraction, Player2 tente de l'arrêter
- **Timer** : 3-5 minutes par partie
- **Win conditions** :
  - Player1 gagne si extraction atteinte
  - Player2 gagne si temps écoulé OU Player1 immobilisé 10s

---

## ✅ Checklist pré-soumission (Jour 3)

- [ ] Build Windows et/ou macOS fonctionnel
- [ ] Contrôles expliqués (écran de démarrage ou README)
- [ ] Pas de bugs bloquants
- [ ] Audio volumes équilibrés
- [ ] Performance stable (>30 FPS)
- [ ] Fichier README.txt avec :
  - Contrôles Player1 et Player2
  - Objectif du jeu
  - Crédits (noms de l'équipe + assets Synty)

---

## 🎊 Bon courage pour la jam !

Cette répartition est flexible — ajustez selon les compétences de chacun. L'important est de **communiquer souvent** et de **playtester tôt** pour itérer rapidement.

**Rappel** : mieux vaut un jeu simple et fun qu'un jeu complexe et buggé. Priorisez les features core et ajoutez le reste si le temps le permet !
