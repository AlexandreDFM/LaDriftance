# La Driftrance — Fuite à la Fête Foraine

## 🎯 Pitch complet

**Un jeu de drift arcade‑horreur fait avec Unity** : driftez à travers une fête foraine corrompue pour échapper à des loup‑garous aux commandes de voitures de police, ou à Marvin — l'animateur fou — qui pirate les véhicules et harangue les passants via les haut‑parleurs.

### Contexte narratif
La fête foraine de Hollowridge, autrefois joyeuse, est devenue la scène d'une nuit cauchemardesque. Des loup‑garous ont pris le contrôle des voitures de patrouille et écument les allées ; Marvin, l'ex‑animateur, a piraté les systèmes sonores et de sécurité pour saboter les fuyards. Le joueur incarne un pilote coincé dans la foire : **objectif — atteindre l'extraction avant d'être arrêté ou dévoré**.

### Modes de jeu
- **Mode Loup‑garou** : des poursuivants humains/monstres dans des voitures de police vous traquent et peuvent sortir pour chasser à pied.
- **Mode Marvin** : Marvin contrôle à distance plusieurs voitures de police et déclenche événements via les hauts‑parleurs (sirènes, messages, embuscades).

### Direction artistique et ambiance
- **Style** : assets Synty low‑poly stylisé (neon, volumes sombres, haut contraste). Utiliser URP pour bloom, fog et chromatic aberration.
- **Ambiance sonore** : synthwave/dark carnival, sirènes, hurlements, voix de Marvin.

### Mécaniques de gameplay
- **Drift centré** : commandes réactives (accélérer, freiner, contre‑braquage, handbrake) avec détection de drift et jauge de boost.
- **Boost** : récompensé pour drift prolongé (courte accélération de fuite).
- **Navigation & raccourcis** : manèges, rampes, raccourcis risqués pour semer les poursuivants.
- **IA poursuivante** : véhicules policiers suivant des waypoints / NavMesh + état poursuite/recherche/immobilisé. Les loup‑garous peuvent quitter le véhicule.
- **Marvin (système d'événements)** : orchestre vagues d'IA, active barricades, joue des lignes vocales via `Speaker` et modifie l'agressivité.

### Objectifs et progression
- **Objectif par run** : atteindre la zone d'extraction avant d'être neutralisé.
- **Progression** : déblocage de véhicules, améliorations (moteur, suspension, pneus drift), skins, nouvelles cartes.
- **Modes supplémentaires** : Score Attack, défis quotidiens.

### Systèmes techniques
- **Rendu** : URP (le projet contient `URPProjectSettings.asset`)
- **Input** : Unity Input System (`Assets/InputSystem_Actions.inputactions`)
- **Caméra** : Cinemachine Virtual Camera (follow + dynamic shake)
- **Véhicules** : Rigidbody + WheelColliders
- **IA** : NavMesh / waypoint steering + behavior states
- **Audio** : AudioMixer snapshots, haut‑parleurs spatialisés pour Marvin

### Assets Synty présents
- `Assets/PolygonStreetRacer/` : véhicules, routes, barrières
- `Assets/PolygonHorrorCarnival/` : manèges, stands, lampadaires
- `Assets/PolygonPumpkins/`, `Assets/PolygonHalloweenMasks/` : props d'ambiance

---

## 📚 Documentation détaillée

Pour plus d'informations techniques et de design, consulter les documents dans `Assets/Docs/` :

- **[GDD.md](Assets/Docs/GDD.md)** — Game Design Document complet (mécaniques, IA, données, UI/UX, tests)
- **[Dialogues.md](Assets/Docs/Dialogues.md)** — Lignes vocales de Marvin et messages HUD
- **[Prototypage.md](Assets/Docs/Prototypage.md)** — Plan détaillé pour créer le prototype Unity

