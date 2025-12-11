
---

## Unity version & settings

- **Recommended Unity:** **Unity 2021.3 LTS** or **Unity 2022.3 LTS** (long-term support)  
- **Scripting Runtime:** .NET 4.x Equivalent  
- **Target platforms:** Android (primary), iOS (optional), Windows Editor for testing  
- **Compression:** Use Android IL2CPP for release builds on mobile

> Note: Use the same LTS version across the team to avoid project upgrade conflicts.

---

## Dependencies / Packages

- Unity built-in packages (Audio, Microphone API)  
- `TextMeshPro` (Unity Package Manager)  
- (Optional) `Cinemachine` for camera smoothing  
- (Optional) `UniTask` or other async libs if used in breath processing pipelines

If using external DSP/ML libraries (example: RNNoise, TensorFlow Lite), list and install them per game README.

---

## How to run (developer / local)

1. Clone repository:
2. Open the specific Unity project (e.g. `BalloonRescue`) in Unity Hub with the recommended Unity version.
3. Open the scene `Scenes/Main.unity`.
4. In Editor, press **Play** to test. Use the Microphone Test Tool in `common/BreathProcessing` to simulate inhale/exhale events.

### Running on Android
- Switch Build Target to Android (File → Build Settings → Android)
- Ensure microphone permission is enabled in Player Settings (Microphone / RECORD_AUDIO)
- Build & Run to a connected device

---

## Breath input integration

Each game uses a common breath-processing module located in `common/BreathProcessing`. Key points:

- **Mic Capture**: reads microphone samples (recommended 16 kHz, 16-bit PCM).
- **Preprocessing**: DC removal, bandpass for respiration envelope, envelope extraction.
- **Action mapping**:
- `Inhale` detection → returns `BREATH_INHALE` event
- `Exhale` detection → returns `BREATH_EXHALE` event
- `Hold` detection → returns `BREATH_HOLD` event
- The mapping script exposes UnityEvents or C# events used by game controllers.

> The included basic breath detector is **for prototype/demonstration** only. For research-grade detection, replace with your trained model or validated DSP pipeline.

---

## Data capture & labeling

- Each gameplay session can be logged (timestamped) to CSV / JSON:
- Do **not** store personally identifying information (PII).
- Obtain informed consent before collecting real child data — follow the `docs/data_privacy_and_ethics.md` guidance.

---

## Assets (what is included / placeholder)

Each game contains **placeholder assets** and an `Assets/README` listing which original art/audio to substitute:

- Character sprites / animations (Idle, Charge, Action)
- Environment backgrounds and obstacles
- Particle effects (sparkles, bubbles, fire)
- UI icons (stars, badges, hearts)
- Sound effects:
- `Inhale_Sound.wav` (soft whoosh)
- `Exhale_Sound.wav` (burst/whoosh)
- `Hold_Sound.wav` (tone)
- reward chime, pop, splash, fire roar

> Replace placeholders with final art and licensed audio for production.

---

## Controls & Gameplay mapping (overview)

- **In Editor (no mic):** use keyboard shortcuts to simulate:
- `I` = Inhale
- `E` = Exhale
- `H` = Hold
- **On Device (mic):** BreathProcessing module triggers the same events — games subscribe and react.

---

## Privacy & Ethics (short)

- This software is a **research prototype** and **not a medical device**.
- Obtain parental consent before any recording. Follow local IRB / ethics requirements.
- Anonymize and encrypt all logged data. Follow GDPR/Local data protection rules if applicable.

---

## Contributing

1. Fork repo → create branch `feature/<name>`  
2. Add code / assets / tests in the appropriate project folder  
3. Open a Pull Request with description and testing steps  
4. Ensure Unity project settings are not modified broadly (document changes)

---

## Known limitations & TODO

- Current breath detector is a prototype (DSP thresholds). Plan to integrate ML model (on-device or cloud) for higher accuracy.
- Camera-based chest motion analysis is experimental — consider clinical validation.
- Accessibility UI (large fonts / audio instructions) needs polishing.

---

## License

Specify your license (e.g., MIT): update `LICENSE` file in repo root.

---

## Contacts / Team

Project NASAM — Breathing Games team

- Maryam Alaa Labib Husein — 6maryam611@gmail.com  
- Aya Hamed Abd Alaziz Eldouski — ayahamed101112@gmail.com  
- Ahmed Abdalrahim Eisa — aeisa7123@gmail.com

---

## Useful docs (in `docs/`)

- `docs/design_gameplay.md` — per-game flow & rules  
- `docs/data_privacy_and_ethics.md` — consent forms & anonymization tips  
- `docs/asset_list.md` — exact art & audio required per game

---

**Start here:** Open Unity Hub → Add `BalloonRescue` project → Open Scene `Main` → Play.

---

