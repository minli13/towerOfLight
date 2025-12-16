# Tower of Light -  Report

## Challenges & Fixes

### 1. Accessing Scripts and GameObjects Across Scenes
- **Challenge:** Many scripts needed to reference objects or components in other scenes, such as UI elements or lights, but direct references were lost when switching scenes
- **Fix:** Implemented strategies such as:
  - Using `FindObjectOfType<T>()` or `Find()` at scene start to locate required components dynamically  
  - Passing references through manager objects that persist across scenes using `DontDestroyOnLoad`  
  - Ensuring all necessary objects were present in the active scene before attempting to access them  

### 2. Screen and GameObject Sizing
- **Challenge:** UI elements and game objects were misaligned or blurry across different resolutions. The scale of objects didn’t match the camera view in some scenes.  
- **Fix:** 
  - Adjusted the **Canvas Scaler** to “Scale With Screen Size” with a fixed reference resolution  
  - Made sure UI RectTransforms were fully stretched or properly anchored  
  - Verified all game objects had consistent local scale `(1,1,1)`  
  - Used TextMeshPro for clearer UI text  

### 3. Handling Logic Between Multiple Objects
- **Challenge:** Coordinating interactions between many objects (doors, lights, toggles, player interactions) was complex 
- **Fix:** 
  - Introduced a **Scene/Game Manager** to handle state and interactions  
  - Used clear naming conventions and organized scripts to reduce confusion  
  - Broke down logic into smaller, single-purpose functions for easier debugging  

### 4. Dark/Blackout Environment
- **Challenge:** Needed the scene to feel dark while still allowing the player to navigate
- **Fix:**  
  - Added a semi-transparent black UI panel overlay for base darkness  
  - Implemented a `FlickerDarkness` script to randomly adjust alpha  

---

## Future Improvements
- Expand the storyline into Chapter 2 and more
- Implement saving progress
- Add more interactive objects and puzzles  
- Introduce sound and lighting cues tied to story progression
