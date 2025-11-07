# Dialogues & UX — Marvin

## But
Fournir ~12 lignes vocales de Marvin en français avec variantes, plus messages HUD/UX courts prêts à être pluggés dans `MarvinController`.

---

## Instructions d'implémentation

- Chaque ligne a un `lineId` et un `priority` (0=low, 1=medium, 2=high)
- `Marvin.Speak(lineId)` joue sur Speaker ciblé
- Prévoir pitch/pan/randomization léger pour variation (pitch ±5%)

---

## Lignes vocales (français) — suggestions

1. **[M001][priority=2]** "Hé là ! On joue à chat ? Attrapez‑le !"
   *// taunt d'alerte*

2. **[M002][priority=2]** "Personne ne sort avant la fermeture. Sauf toi... si tu survies."
   *// menace*

3. **[M003][priority=1]** "Coupez les lumières... et que le spectacle commence !"
   *// déclenche power outage*

4. **[M004][priority=2]** "Les portes sont verrouillées, prenez un autre chemin, idiot."
   *// lock path*

5. **[M005][priority=1]** "Il est derrière la grande roue... dépêche‑toi !"
   *// hint/location*

6. **[M006][priority=1]** "La sirène est pour vous, c'est votre quiproquo. Dégagez !"
   *// taunt + siren*

7. **[M007][priority=0]** "Que la fête continue... pour eux."
   *// ambience low*

8. **[M008][priority=2]** "Extraction en approche ? Non, c'est ma scène — pas la tienne."
   *// taunt if near extraction*

9. **[M009][priority=1]** "Plus vite, plus fort — ou vous finissez en prix de foire."
   *// encourage IA speedup*

10. **[M010][priority=2]** "Regardez ce numéro : voitures folles en direct !"
    *// event announce*

11. **[M011][priority=1]** "Bien joué... pour l'instant. Mais la nuit n'est pas finie."
    *// on player close call*

12. **[M012][priority=2]** "Cessez de respirer trop fort — ça attire l'attention."
    *// scare line*

---

## Variantes et UX

- Pour chaque `lineId`, prévoir 2 variantes distinctes (pitch, words) pour éviter répétition
- Lors d'une ligne priority=2, ducker audio music et augmenter siren SFX
- Afficher HUD toast court (ex : "Marvin : 'Attrapez‑le !'")

---

## HUD/UX messages (courts) — mapping

- **HUD_Alert_1** : "Vous êtes repéré !"
- **HUD_Alert_2** : "Speaker activé : Marvin détecté"
- **HUD_Event_Lock** : "Un passage a été verrouillé"
- **HUD_Event_Open** : "Un raccourci s'est ouvert"
- **HUD_Extraction** : "Extraction : Xs"

---

## Fichier de localisation (exemple simplifié)

```json
{
  "M001": "Hé là ! On joue à chat ? Attrapez‑le !",
  "HUD_Alert_1": "Vous êtes repéré !",
  "HUD_Event_Lock": "Un passage a été verrouillé"
}
```

---

## Notes sur voix

- **Direction** : voix de type animateur forain, grinçante, joueuse et menaçante
- **Option** : enregistrer un acteur vocal ou générer via TTS de haute qualité (ne pas utiliser voix synthétiques de mauvaise qualité en release)
