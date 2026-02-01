# Shadows of Doubt - Game Classes Reference

## Core Game Systems

### Player (inherits Human)
The player character. Access via `Player.Instance`

**Key Fields:**
- `fpsMode` (bool) - First person mode active
- `fps` (FirstPersonController) - FPS controller
- `charController` (CharacterController) - Unity character controller
- `cam` (CameraController) - Camera controller
- `transitionActive` (bool) - In a transition animation
- `autoTravelActive` (bool) - Auto-travel active
- `answeringPhone` (Telephone) - Current phone being used
- `crouchedTransition` (float) - Crouch state
- `gasLevel` (float) - Gas exposure level
- `hurt` (float) - Hurt indicator
- `bed` (Interactable) - Last used bed

**Key Methods:**
- `EnablePlayerMovement(bool)` - Enable/disable movement
- `EnablePlayerMouseLook(bool)` - Enable/disable mouse look
- `UpdateMovementPhysics()` - Update physics
- `TransformPlayerController()` - Start transition animation

---

### Human (inherits Actor)
Base class for all humans (Player, Citizen, Police)

**Identity Fields:**
- `humanID` (int) - Unique ID
- `citizenName`, `firstName`, `surName` (string) - Name parts
- `gender` (Human.Gender) - male/female/nonBinary
- `birthday` (string) - Birth date
- `bloodType` (Human.BloodType) - A/B/AB/O positive/negative
- `fingerprintLoop` (int) - Fingerprint pattern

**Location Fields:**
- `home` (NewAddress) - Home address
- `residence` (ResidenceController) - Residence controller
- `job` (Occupation) - Job/occupation
- `currentRoom` (NewRoom) - Current room
- `currentGameLocation` (NewGameLocation) - Current location

**Stats Fields:**
- `health`, `maxHealth` (float) - Health
- `nourishment` (float) - Hunger (0-1)
- `energy` (float) - Tiredness (0-1)
- `hygiene` (float) - Cleanliness (0-1)
- `breathRecoveryRate` (float) - Stamina recovery

**Personality Fields:**
- `humility`, `emotionality`, `extraversion` (float) - Personality traits
- `agreeableness`, `conscientiousness`, `creativity` (float)
- `characterTraits` (List<Human.Trait>) - Character traits
- `sexuality`, `homosexuality` (float) - Attraction

**State Fields:**
- `isAsleep`, `isUnconscious`, `isDead` (bool) - States
- `isArrested` (bool) - Arrested state
- `partner` (Citizen) - Romantic partner
- `paramour` (Citizen) - Affair partner

**Key Methods:**
- `Heal(float)` - Heal health
- `RecieveDamage(float, Actor)` - Take damage
- `SetDeath(CauseOfDeath, bool, Human)` - Kill human
- `WakeUp()` - Wake from sleep
- `Sleep(Interactable)` - Go to sleep

---

### GameplayController
Manages overall gameplay state. Access via `GameplayController.Instance`

**Player Stats:**
- `money` (int) - Player's money
- `lockPicks` (int) - Lockpick count
- `socialCredit` (int) - Social credit score
- `socialCreditPerks` (List) - Active perks

**Discovery:**
- `evidenceDictionary` (Dictionary<string, Evidence>) - All discovered evidence
- `factList` (List<Fact>) - All discovered facts
- `acquiredPasscodes` (List<Passcode>) - Known passcodes
- `acquiredNumbers` (List<PhoneNumber>) - Known phone numbers

**World State:**
- `enforcers` (List<Human>) - Police/enforcers
- `crimeScenes` (HashSet<NewGameLocation>) - Active crime scenes
- `hospitalBeds` (List<Interactable>) - Hospital beds
- `activeGadgets` (List<Interactable>) - Deployed gadgets

**Key Methods:**
- `AddMoney(int, bool, string)` - Add/remove money
- `AddOrMergePasscodeData(Passcode, bool)` - Add passcode
- `AddOrMergePhoneNumberData(int, bool, List<Human>, string, bool)` - Add phone number
- `SetPlayerKnowsPassword(NewAddress)` - Mark password known

---

### SessionData
Manages session state, time, weather. Access via `SessionData.Instance`

**Time:**
- `gameTime` (float) - Current time (0-24)
- `day` (int) - Current day number
- `play` (bool) - Game is unpaused

**Weather:**
- `isRaining` (bool) - Currently raining
- `currentWeather` - Weather state

**Key Methods:**
- `PauseGame(bool, bool, bool)` - Pause game
- `ResumeGame()` - Resume game

---

### MurderController
Handles murder generation and state. Access via `MurderController.Instance`

**Fields:**
- `activeMurder` (Murder) - Current active murder
- `murders` (List<Murder>) - All murders

**Murder Class Fields:**
- `murderer` (Human) - The killer
- `victim` (Human) - The victim
- `state` (MurderState) - Current state
- `murderWeapon` (Interactable) - Murder weapon
- `killLocation` - Murder location
- `preset` (MurderPreset) - Murder configuration
- `mo` (MurderMO) - Modus operandi

**Murder States:**
- `none`, `acquireEquipment`, `research`, `waitForLocation`
- `travellingTo`, `executing`, `post`, `escaping`
- `unsolved`, `solved`

---

### Citizen (inherits Human)
NPC citizens in the city

**Additional Fields:**
- `ai` (NewAIController) - AI controller
- `currentAction` (NewAIAction) - Current AI action
- `routine` - Daily routine

---

### Interactable
Base class for all interactive objects

**Identity:**
- `id` (int) - Unique world ID
- `name` (string) - Object name
- `preset` (InteractablePreset) - Type preset

**Location:**
- `node` (NewNode) - Current node
- `wPos`, `wEuler` (Vector3) - World position/rotation
- `furnitureParent` (FurnitureLocation) - Parent furniture

**State:**
- `locked` (bool) - Is locked
- `val` (float) - Monetary value
- `inInventory` (Human) - Carried by
- `belongsTo` (Human) - Owner

**Evidence:**
- `evidence` (Evidence) - Associated evidence
- `passcode` (Passcode) - Associated passcode

**Key Methods:**
- `OnInteraction(InteractionAction, Actor, bool)` - Handle interaction
- `PickUpTarget(Human, bool)` - Pick up object
- `OnCompleteLockpick()` - Lockpick completed
- `ForcePhysicsActive()` - Enable physics

---

### Case
Represents a detective case

**Fields:**
- `caseID` (int) - Unique ID
- `name` (string) - Case name
- `isActive`, `isSolved` (bool) - State
- `elements` (List<CaseElement>) - Evidence on board
- `resolves` (List<ResolveQuestion>) - Objectives
- `reward` (int) - Base reward

**Key Methods:**
- `OnCaseSolved()` - When case is solved
- `SetActive(bool)` - Activate/deactivate

---

### Evidence
Represents a piece of evidence

**Fields:**
- `evID` (string) - Unique ID
- `evName` (string) - Display name
- `discovered` (bool) - Has been discovered
- `preset` (EvidencePreset) - Type preset

**Subtypes:**
- `EvidenceCitizen`, `EvidenceWitness` - Person evidence
- `EvidenceFingerprint`, `EvidenceDNA`, `EvidenceBlood` - Forensic
- `EvidenceLocation`, `EvidenceBuilding` - Location
- `EvidenceKey`, `EvidenceTelephone` - Items

---

### NewAIController
Controls NPC AI behavior

**Fields:**
- `human` (Human) - Controlled human
- `currentGoal` (NewAIGoal) - Current goal
- `currentAction` (NewAIAction) - Current action
- `alertness` (float) - Alert level
- `investigating` (bool) - Investigating something

---

## Enums

### Human.Gender
- `male = 0`
- `female = 1`
- `nonBinary = 2`

### Human.BloodType
- `oPos = 0`, `oNeg = 1`
- `aPos = 2`, `aNeg = 3`
- `bPos = 4`, `bNeg = 5`
- `abPos = 6`, `abNeg = 7`

### Human.Death.CauseOfDeath
- `gunshot`, `bludgeoning`, `stabbing`
- `strangulation`, `poison`, `other`

### MurderController.MurderState
- `none = 0`, `acquireEquipment = 1`
- `research = 2`, `waitForLocation = 3`
- `travellingTo = 4`, `executing = 5`
- `post = 6`, `escaping = 7`
- `unsolved = 8`, `solved = 9`
