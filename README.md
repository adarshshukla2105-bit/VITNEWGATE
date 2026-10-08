# VIT Entrance Gate - Unity 3D Project

A complete 3D Unity project featuring a grand campus entrance gate with two architectural stone pillars, an upper surface/roof spanning and covering both pillars, an interactive double-wing iron gate, an asphalt road with markings, sidewalks, a security cabin, street lamps, and smooth camera controls.

---

## 🚀 How to Open and Run in Unity

### Method 1: Open in Unity Hub
1. Open **Unity Hub**.
2. Click **Add** -> **Add project from disk**.
3. Select this folder:  
   `/Users/adarshshukla/Desktop/VITNEWGATE`
4. Choose your installed Unity Editor version (Unity 2021.3 LTS, 2022.3 LTS, 2023.x, or Unity 6).
5. Click **Open**.

---

## ⚡ 1-Click Automated Scene Generation

Once the project is open in Unity:

1. In the top Unity menu bar, click:  
   **`VIT Gate`** -> **`Generate Gate & Road Scene (1-Click)`**  
   *(or open `VIT Gate` -> `Open Gate Builder Window...`)*
2. The entire 3D scene will instantly construct itself in the Hierarchy:
   - Asphalt Road with center dashed lines and shoulder lines.
   - Left and Right Stone Pillars (Base, Shaft, Capital).
   - Upper Covering Architrave Beam & Roof Overhang.
   - Dual-wing wrought iron gate with pivots.
   - Security Cabin & Pillar Lanterns.
   - Directional Sun Light & Optimized Main Camera.
3. Press the **Play ▶** button at the top of Unity!

---

## 🎮 Play Mode Controls

| Action | Controls |
| :--- | :--- |
| **Open / Close Gate** | Press **`[Space]`** or **`[E]`** |
| **Free-Fly Camera (Look)** | Hold **`Right Mouse Button`** and move mouse |
| **Free-Fly Camera (Move)** | **`W / A / S / D`** |
| **Move Camera Up / Down** | **`E`** (Up) / **`Q`** (Down) |
| **Camera Speed Boost** | Hold **`Left Shift`** |

---

## 📐 Scene Hierarchy & Dimensions (For Manual Creation / Viva)

If you need to explain or recreate the objects manually using Unity 3D Primitives:

### 1. Ground & Road (`Environment`)
- **Grass Ground**: 3D Object -> Cube  
  - Position: `(0, -0.25, 0)` | Scale: `(80, 0.5, 80)`  
  - Material: `Grass_Ground.mat`
- **Asphalt Road**: 3D Object -> Cube  
  - Position: `(0, 0.05, 0)` | Scale: `(8, 0.1, 60)`  
  - Material: `Asphalt_Road.mat`
- **Sidewalk Left**: 3D Object -> Cube  
  - Position: `(-5.25, 0.11, 0)` | Scale: `(2.5, 0.22, 60)`
- **Sidewalk Right**: 3D Object -> Cube  
  - Position: `(5.25, 0.11, 0)` | Scale: `(2.5, 0.22, 60)`

### 2. Pillars (`Gate_Structure`)
- **Pillar Distance**: `9.0m` center-to-center
- **Left Pillar**:
  - Base Pedestal: Position `(-4.5, 0.5, 0)` | Scale `(2.0, 1.0, 2.0)`
  - Column Shaft: Position `(-4.5, 3.5, 0)` | Scale `(1.4, 5.0, 1.4)`
  - Top Capital: Position `(-4.5, 6.2, 0)` | Scale `(1.9, 0.4, 1.9)`
  - Material: `Stone_Pillar.mat`
- **Right Pillar**:
  - Base Pedestal: Position `(4.5, 0.5, 0)` | Scale `(2.0, 1.0, 2.0)`
  - Column Shaft: Position `(4.5, 3.5, 0)` | Scale `(1.4, 5.0, 1.4)`
  - Top Capital: Position `(4.5, 6.2, 0)` | Scale `(1.9, 0.4, 1.9)`
  - Material: `Stone_Pillar.mat`

### 3. Upper Surface Covering The Pillars
- **Main Architrave Beam** (Spans between & rests on top of both pillars):  
  - Position: `(0, 6.8, 0)` | Scale: `(11.4, 0.8, 1.8)`  
  - Material: `Upper_Roof.mat`
- **Roof Overhang Slab** (Extends past pillars on both sides for weather shelter):  
  - Position: `(0, 7.3, 0)` | Scale: `(12.2, 0.35, 2.6)`  
  - Material: `Stone_Pillar.mat`
- **Decorative Parapet / Crown**:  
  - Position: `(0, 7.6, 0)` | Scale: `(10.5, 0.25, 2.0)`
- **Entrance Signboard**:  
  - Position: `(0, 6.8, 0.95)` | Scale: `(7.5, 0.6, 0.08)`  
  - Text: `"★ VELLORE INSTITUTE OF TECHNOLOGY ★ MAIN ENTRANCE GATE"`

### 4. Interactive Gate Wings (`Gate_Mechanism`)
- **Left Wing Pivot**: Position `(-3.8, 0, 0)`
- **Right Wing Pivot**: Position `(3.8, 0, 0)`
- Contains rectangular steel frame and vertical iron bars (`Iron_Gate.mat`)
- Component: `GateController.cs` (interpolates rotation from 0° to ±95° on Spacebar).

---

## 📁 File Structure

```
VITNEWGATE/
├── Assets/
│   ├── Editor/
│   │   └── VITGateBuilderEditor.cs     # Editor Menu and 1-Click Builder Window
│   ├── Materials/
│   │   ├── Asphalt_Road.mat            # Dark road asphalt
│   │   ├── Grass_Ground.mat            # Green terrain
│   │   ├── Iron_Gate.mat               # Metallic wrought iron
│   │   ├── Road_YellowLine.mat         # Yellow lane markings
│   │   ├── Stone_Pillar.mat            # Sandstone / Granite pillars
│   │   └── Upper_Roof.mat              # Terracotta architectural trim
│   ├── Scenes/
│   │   └── CampusGateScene.unity       # Ready-to-open scene
│   └── Scripts/
│       ├── FreeFlyCamera.cs            # Fly & inspect camera controller
│       ├── GateController.cs           # Interactive gate open/close script
│       └── VITGateSceneBuilder.cs      # Core procedural 3D scene generator
└── ProjectSettings/
    ├── EditorBuildSettings.asset
    └── ProjectVersion.txt
```
