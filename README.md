# Conway's Game of Life - Enhanced Edition

A high-performance Windows Forms implementation of Conway's Game of Life featuring dual cellular visualization modes, intelligent pattern tiling, clipboard-based editing with transforms, and an intuitive creative workflow for exploring cellular automata.

![.NET 10](https://img.shields.io/badge/.NET-10.0-blue)
![C#](https://img.shields.io/badge/C%23-14.0-purple)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)

## 🌟 Overview

This project implements Conway's Game of Life with two distinctive visual modes that reveal different aspects of cellular evolution. Switch between generation tracking and lifespan visualization to explore patterns in unique ways. Features both full-grid tiling for symmetry and precision copy/paste editing with preview and transform operations.

**📚 For Educators**: See [EDUCATIONAL.md](EDUCATIONAL.md) for classroom activities, code quality learning points, extensibility exercises, and mathematical exploration topics.

## ✨ Features

### Visual Innovation - Dual Color Modes

#### 🎨 **Mode 1: Constant Color from Birth**
Cells preserve their birth generation color throughout their lifetime, creating a living "heat map" of cellular age and pattern evolution.

- **360-color gradient** cycling through full RGB spectrum
- **Birth generation tracking**: Each generation assigns a unique color
- **Lifetime preservation**: Cells keep their birth color until death
- **Complementary foreground colors**: Tick counter text color automatically adjusts to complement the background for maximum readability
- **Pattern revelation**:
  - **Still lifes** appear as solid single colors (ancient survivors)
  - **Oscillators** display alternating color bands
  - **Gliders/spaceships** leave colorful trails across the grid
  - **Methuselahs** create explosive rainbow patterns
  - **Chaotic regions** show multi-colored complexity

#### ⏳ **Mode 2: Cell Changes Color as Aging**
Cells dynamically change color based on how many generations they survive, visualizing lifespan distribution in real-time.

- **Age-based progression**: Cells start blue and shift through the spectrum as they age
- **3x acceleration factor**: Colors change 3 steps per generation for visible progression
- **Lifespan visualization**:
  - **Blue cells** (age 0-10): Newborns and young cells - most common
  - **Cyan/Green cells** (age 10-30): Middle-aged survivors
  - **Yellow/Orange cells** (age 30-50): Rare long-lived cells
  - **Red cells** (age 50+): Ancient elders in stable patterns
- **Continuous cycling**: Colors wrap around after ~120 generations, creating rainbow cycles for stable patterns
- **Perfect for observing**:
  - **Mortality rates**: Visual representation of cell lifespan distribution
  - **Stable patterns**: 2×2 blocks cycle through rainbow continuously
  - **Exponential decay**: Most cells die young (blue dominates), few reach old age

### 🎯 **Pattern Tiling System** - NEW!

Create complex designs effortlessly with intelligent region tiling:

#### **Ctrl+Drag Selection** (Windows-standard interaction)
- **Hold Ctrl** over grid → Cursor changes to crosshair + hint appears
- **Ctrl+Left-drag** → Blue dashed rectangle shows selection
- **Live dimensions** displayed above selection (e.g., "12×8 cells")
- **Press Enter** → Pattern tiles across entire grid
- **Press Escape** → Cancel selection

#### **T-Key Selection Mode** (Persistent mode for multiple selections)
- **Press T** → Enter selection mode (mode indicator turns cyan)
- **Click-drag** → Create selection rectangles
- **Press Enter** → Apply tiling
- **Press Escape** or **Press T again** → Exit selection mode

#### **How Tiling Works**
1. **Select any rectangular region** (pattern + spacing you want)
2. **Selection captures**:
   - Cell states (alive/dead)
   - Cell colors (preserves visual appearance)
   - Empty spacing around pattern
3. **Grid clears** and tiles pattern from (0,0) using modulo wrapping
4. **Result**: Seamless repetition across entire grid

#### **Creative Workflow**
```
Draw small pattern → Ctrl+Drag to select → Enter → Ctrl+R to run!
```

**Perfect for**:
- Creating symmetric initial conditions
- Testing how local patterns interact globally
- Generating beautiful geometric structures
- Quick experimentation with spacing variations

### ✂️ **Copy/Paste/Transform System** - NEW!

Precise pattern manipulation with preview-before-place workflow:

#### **Copy Operation (Ctrl+C)**
- **Select a region** using T-key mode or Ctrl+Drag
- **Press Ctrl+C** → Pattern copied to clipboard
- **Selection remains visible** (can copy multiple times)
- **Captures**: Cell states + colors (preserves visual appearance)

#### **Paste Preview Mode (Ctrl+V)**
- **Press Ctrl+V** → Enters paste preview mode
- **Mode indicator turns light green**: "Paste mode"
- **Pattern appears as overlay** following your mouse cursor
- **Transform operations available** (H, V, R, Shift+R) - see below
- **Click to place** → Pattern pastes at clicked location
- **Press Esc** → Cancel and exit paste mode

#### **Transform Operations (Work ONLY in Paste Mode)**
These operations modify the clipboard pattern **before** placement:

| Key | Operation | Description |
|-----|-----------|-------------|
| **H** | Flip Horizontal | Mirror pattern left-to-right |
| **V** | Flip Vertical | Mirror pattern top-to-bottom |
| **R** | Rotate 90° CW | Clockwise quarter turn |
| **Shift+R** | Rotate 90° CCW | Counter-clockwise quarter turn |

**Additional transform** (from Edit → Transform menu):
- **Rotate 180°** - Half turn (equivalent to H+V or R+R)

#### **Paste Mode Workflow**
```
1. Select region → 2. Ctrl+C (copy) → 3. Ctrl+V (enter paste mode)
4. Transform (H/V/R) [optional] → 5. Click to place → 6. Done!
   Or press Esc to cancel
```

#### **Key Distinction: Tiling vs. Copy/Paste**

| Feature | **Tiling (Enter)** | **Copy/Paste (Ctrl+C/V)** |
|---------|-------------------|---------------------------|
| **Action** | Repeats pattern across **entire grid** | Places pattern **once** at clicked location |
| **Workflow** | Select → Enter | Select → Ctrl+C → Ctrl+V → Click |
| **Grid cleared?** | ✅ Yes (destructive) | ❌ No (additive) |
| **Transform support** | ❌ No | ✅ Yes (H/V/R/Shift+R) |
| **Preview** | ❌ No | ✅ Yes (paste mode overlay) |
| **Multiple placements** | ❌ Single application | ✅ Can paste multiple times (Ctrl+V again) |
| **Use case** | Create symmetric full-grid patterns | Precise single-placement editing |

**Perfect for**:
- **Tiling**: Wallpaper patterns, test periodic behaviors
- **Copy/Paste**: Building complex structures, experimenting with orientations, level editing

###  Density Variation
Ctrl+Mouse wheel adjusts pattern density by adding or removing cells:
- **Ctrl+Scroll up**: Add ~5% more cells randomly (increases density)
- **Ctrl+Scroll down**: Remove ~5% of cells randomly (decreases density)
- Works whether simulation is stopped or running (modifies current pattern)
- **Adds/removes approximately 5% of the current cell count** per scroll (minimum 1 cell)
- **Density value appears near mouse cursor for real-time feedback while adjusting**
- **Plain mouse wheel** (without Ctrl) scrolls the grid when it doesn't fit the window
- **Non-destructive**: Preserves existing pattern structure while adding/removing cells
- **Applies to all pattern types**:
  - Preset shapes (Glider, Pulsar, etc.): Adds/removes cells while keeping the original pattern intact
  - Manual drawings: Fine-tune your creations by adding or removing individual cells
  - Tiled patterns: Adjust density of the tiled result
  - Imported RLE patterns: Modify density after import
	 
### Core Simulation
- **150×72 cell grid** (10,800 cells total)
- Classic Conway's rules (B3/S23): Birth on 3 neighbors, Survival on 2-3 neighbors
- Toroidal wrapping (edges wrap to opposite sides)
- Automatic stability detection (stops after 5 generations of unchanged population)

### Performance Optimizations
- **Dirty Rectangle Rendering**: Only redraws cells that changed state OR aged (in aging mode)
- **Brush Caching**: Reuses color brushes to minimize memory allocations
- **Buffer.BlockCopy**: High-speed array operations for state management
- **Single Bitmap Rendering**: All cells drawn to one bitmap vs. 10,800+ individual controls
- **Smart invalidation**: Aging cells trigger redraws even when position unchanged
- Runs smoothly at ~5 generations/second on a 10,800 cell grid

### Interactive Controls
- **Color Mode Selector**: Dropdown to switch between birth and aging visualization modes
- **Mouse Drawing**: 
  - **Left-click + drag** paints cells alive (single cell precision)
  - **Right-click + drag** erases 5×5 area (rounded eraser cursor shown)
- **Mouse Wheel**: 
  - **Ctrl+Wheel** adjusts pattern density by adding/removing cells (preserves existing pattern)
  - **Plain Wheel** scrolls the grid viewport when grid is larger than window
- **Pattern Tiling**:
  - **Ctrl+Drag** for quick one-off selections
  - **T key** for persistent selection mode
  - **Enter** to apply tiling
  - **Escape** to cancel selection
- **Keyboard Shortcuts**: 
  - **Ctrl+R** = Run/Start
  - **Ctrl+S** = Stop
  - **Spacebar** = Toggle (when grid focused)
  - **T** = Toggle tiling selection mode
- **Menu System**: Organized controls for simulation, speed, patterns, and tiling
- **Real-time Statistics**: Live generation count and population display
- **Status Indicator**: Color-coded "Running" (green) / "Stopped" (red) display
- **Mode Indicator**: Shows current interaction mode (Drawing / Selection)

## 📋 Requirements

- **.NET 10.0 SDK** or higher
- **Windows** operating system (WinForms dependency)
- **Visual Studio 2022** or later (recommended for development)
- **~1600×900 screen resolution** or higher (optimized for 1728×972 window)

## 🚀 Installation

### From Source

1. **Clone the repository**:
   ```bash
   git clone https://github.com/markhassellsmith/GameOfLife.git
   cd GameOfLife
   ```

2. **Open in Visual Studio**:
   - Launch `GameOfLife.sln`

3. **Build and Run**:
   - Press `F5` to build and start debugging
   - Or press `Ctrl+F5` to run without debugging

### Build from Command Line
```bash
dotnet build
dotnet run
```

## 🎮 Usage

### Menu Controls

#### **File**
- **Reset** - Clear grid and generate new random pattern
- **Clear the Grid** - Remove all cells (blank slate for manual drawing)
- **Export Pattern...** - Save current grid as RLE file
- **Import Pattern...** - Load pattern from RLE file with preview
- **Exit (Alt+F4)** - Close application

#### **Edit**
- **Copy (Ctrl+C)** - Copy selected region to clipboard
- **Paste (Ctrl+V)** - Enter paste preview mode with clipboard pattern
- **Transform** - Modify clipboard pattern (only works in paste mode):
  - **Flip Horizontal (H)** - Mirror pattern left-to-right
  - **Flip Vertical (V)** - Mirror pattern top-to-bottom
  - **Rotate 90° CW (R)** - Clockwise quarter turn
  - **Rotate 90° CCW (Shift+R)** - Counter-clockwise quarter turn
  - **Rotate 180°** - Half turn

#### **Simulation**
- **Start (Ctrl+R)** - Begin the simulation [Left-hand shortcut!]
- **Stop (Ctrl+S)** - Pause the simulation [Left-hand shortcut!]

#### **Speed**
- **Slow** - 1 generation per second (1000ms)
- **Medium** - ~1.8 generations per second (550ms) *[default]*
- **Quick** - 5 generations per second (200ms)

#### **Pattern**
- **Random** - Generate random starting configuration *[default]*
- **Load Preset...**
  - **Shapes...** - Classic Game of Life patterns
	- Glider - Simple period-4 spaceship
	- Lightweight Spaceship (LWSS) - Period-4 spaceship
	- Middleweight Spaceship (MWSS) - Period-4 spaceship
	- Heavyweight Spaceship (HWSS) - Period-4 spaceship
	- Blinker - Period-2 oscillator
	- Toad - Period-2 oscillator
	- Beacon - Period-2 oscillator
	- Pulsar - Period-3 oscillator
	- Pentadecathlon - Period-15 oscillator
	- *──────────*
	- R-Pentomino - Famous methuselah (~1100 generations)
	- Acorn - Methuselah (~5200 generations)
	- Diehard - Methuselah (dies after 130 generations)
	- Pi Heptomino - Methuselah (~173 generations)
	- *──────────*
	- Gosper Glider Gun - Period-30 glider generator
	- Eater 1 - Stable glider consumer
  - **Tilings...** - Full-grid geometric patterns
	- Chessboard (5×5) - Alternating blocks with perfect symmetry
	- Ladder Brick - Dynamic brick pattern with oscillators
	- Diagonal Stripes (width 5) - 45° bands spawning spaceships
	- Thin Diagonal Stripes (width 1-2) - **NEW!** Narrow diagonals creating long-lived glider fields with symmetric evolution
	- Double Diagonal - Cross-hatch diamond lattice
	- Concentric Rectangles - Radial pulsing waves
	- Pulsar Grid (period-3) - **NEW!** Synchronized field of pulsars creating beautiful oscillating patterns
- **Tile Selection (Ctrl+Drag or T)** - Enter/exit manual tiling mode

#### **Color Mode Dropdown** (in menu bar)
Select visualization mode:
- **Constant Color from Birth** - Track generation of birth *[default]*
- **Cell Changes Color as Aging** - Visualize lifespan and survival time

### Keyboard Shortcuts
| Key | Action | Always Available? |
|-----|--------|-------------------|
| **Ctrl+R** | **Run/Start** simulation | ✅ Yes (menu shortcut) |
| **Ctrl+S** | **Stop** simulation | ✅ Yes (menu shortcut) |
| `Spacebar` | Toggle Start/Stop | When grid focused |
| **Ctrl+C** | **Copy** selected region to clipboard | ✅ Yes (menu shortcut) |
| **Ctrl+V** | **Paste** - Enter paste preview mode | ✅ Yes (menu shortcut) |
| **H** | Flip Horizontal | ⚠️ **Only in paste mode** |
| **V** | Flip Vertical | ⚠️ **Only in paste mode** |
| **R** | Rotate 90° CW | ⚠️ **Only in paste mode** |
| **Shift+R** | Rotate 90° CCW | ⚠️ **Only in paste mode** |
| **T** | Toggle Tiling Selection mode | ✅ Yes |
| **Ctrl+Drag** | Quick tiling selection | While holding Ctrl |
| **Enter** | Apply tiling (when selection active) | ✅ Yes |
| **Escape** | Cancel selection / Exit tiling or paste mode | ✅ Yes |
| `Alt+F4` | Exit application | ✅ Yes (menu shortcut) |

**Left-Hand Friendly**: Ctrl+R, Ctrl+S, Ctrl+C, and Ctrl+V work regardless of which control has focus!

**Transform Keys**: H, V, R, and Shift+R only work when in paste preview mode (after pressing Ctrl+V). They have no effect in other modes.

**Tip**: Press **Alt** to reveal menu access keys (Alt+F for File, Alt+E for Edit, Alt+S for Simulation, Alt+E for Speed, Alt+P for Pattern)

### Mouse Controls
| Action | Result | Visual Feedback |
|--------|--------|-----------------|
| `Left Click + Drag` | Paint cells alive | Single cell precision |
| `Right Click + Drag` | Erase 5×5 area | Rounded eraser cursor |
| `Ctrl + Mouse Wheel Up` | Increase density +5% | Overlay shows "Density: X%" |
| `Ctrl + Mouse Wheel Down` | Decrease density -5% | Overlay shows "Density: X%" |
| `Mouse Wheel Up/Down` | Scroll grid vertically | Standard scrolling (when grid larger than window) |
| **Hold Ctrl** | Show selection cursor | Crosshair + cyan hint in mode indicator |
| **Ctrl + Left Drag** | Select region for tiling | Blue dashed rectangle + dimensions |

### Visual Feedback
- **Mode Indicator** (in menu bar):
  - Green "Mode: Drawing" = Normal state
  - Cyan "Ctrl+Drag to Select" = Ctrl key held
  - Sky blue "Mode: Select Region (Ctrl)" = Ctrl+drag active
  - Sky blue "Mode: Select Region (T)" = T-key mode active
  - Light green "Paste mode" = Paste preview active (Ctrl+V pressed)
- **Selection Rectangle**: Blue dashed line with dimensions overlay
- **Paste Overlay**: Pattern preview following mouse cursor in paste mode
- **Density Overlay**: Yellow tooltip near cursor (auto-hides after 5 seconds)
- **Eraser Cursor**: Black rounded rectangle outline (5×5 cells)

### Status Bar
- **Color Mode Selector**: Dropdown to switch between birth and aging visualization modes
- **Mode Indicator**: Shows interaction state (Drawing / Ctrl+Drag to Select / Select Region)
- **Tick Counter**: Shows current generation number
  - In **Birth mode**: Background cycles through spectrum
  - In **Aging mode**: White background (doesn't interfere with age colors)
- **Population Display**: Real-time count of alive cells
- **Status Indicator**: Shows "Running" (green) or "Stopped" (red)

## 🏗️ Architecture

### Project Structure

#### **Form1.cs** - Core Logic
- **Dual Color Systems**: 
  - `ColorMode.BirthGeneration`: Tracks generation of birth
  - `ColorMode.CellAging`: Tracks generations survived
- **Tiling System**:
  - `InteractionMode.Drawing`: Normal paint/erase mode
  - `InteractionMode.TilingSelection`: Selection mode (T-key or Ctrl+drag)
  - Smart pattern capture (states + colors)
  - Modulo-based seamless tiling algorithm
- **Copy/Paste System**:
  - Clipboard storage for selected patterns
  - Paste preview mode with real-time transform operations
  - Transform functions: Flip horizontal/vertical, rotate 90°/180°
  - Non-destructive single-placement editing
- **Grid Management**: 150×72 boolean array with double-buffering
- **Simulation Engine**: Neighbor calculation with toroidal wrapping
- **Rendering Pipeline**: Optimized bitmap-based drawing with age-aware invalidation
- **Color Tracking**: 
  - `cellColor[,]`: Per-cell color array
  - `cellAge[,]`: Age tracking array (generations survived)
- **Event Handlers**: UI interaction and timer-driven updates
- **Keyboard Handling**: 
  - Form-level `KeyPreview = true` for reliable key capture
  - Menu shortcuts (Ctrl+R, Ctrl+S, Ctrl+C, Ctrl+V) work regardless of focus
  - Mode-aware key handling (H/V/R only work in paste mode)

#### **ColorPalettes.cs**
- **Spectrum360**: 360-color RGB gradient array
  - Cycles through red → yellow → green → cyan → blue → magenta → red
  - **Birth mode**: Each generation uses next color in sequence (wraps at 360)
  - **Aging mode**: Cell age × 3 determines color index (3x acceleration factor)

#### **Game Rules Implementation**
```csharp
// Conway's B3/S23 Rules
if (alive && (neighbors < 2 || neighbors > 3)) 
	dies();       // Underpopulation or Overpopulation
if (alive && (neighbors == 2 || neighbors == 3)) 
	survives();   // Survival (ages in aging mode)
if (dead && neighbors == 3) 
	birth();      // Reproduction (gets current color or age 0)
```

## 🔧 Technical Highlights

### Pattern Tiling Algorithm

**Smart Capture**:
```csharp
// Convert selection rectangle to cell coordinates
int startCellX = selectionRect.X / cellSpacing;
int startCellY = selectionRect.Y / cellSpacing;
int endCellX = (selectionRect.X + selectionRect.Width) / cellSpacing;
int endCellY = (selectionRect.Y + selectionRect.Height) / cellSpacing;

// Capture tile unit (no +1 - endCell is already one past)
int patternWidth = endCellX - startCellX;
int patternHeight = endCellY - startCellY;

// Preserve both states AND colors
for (int i = 0; i < patternWidth; i++) {
	for (int j = 0; j < patternHeight; j++) {
		pattern[i, j] = squaresState[startCellX + i, startCellY + j];
		patternColors[i, j] = cellColor[startCellX + i, startCellY + j];
	}
}
```

**Seamless Tiling with Modulo**:
```csharp
// Tile from (0,0) using modulo wrapping
for (int i = 0; i < squarePerLine; i++) {
	for (int j = 0; j < squarePerColumn; j++) {
		int patternI = i % patternWidth;  // Repeat horizontally
		int patternJ = j % patternHeight; // Repeat vertically
		squaresState[i, j] = pattern[patternI, patternJ];
		cellColor[i, j] = patternColors[patternI, patternJ];
	}
}
```

**Result**: Perfect seamless repetition with preserved visual appearance!

### Dual Color System Architecture

**Constant Color from Birth** works by:
1. Each generation increments a global color index (0-359)
2. Newly born cells get assigned the current generation's color
3. Cells retain their birth color until death
4. `cellColor[i,j]` stores birth color throughout lifetime

**Cell Changes Color as Aging** works by:
1. Newly born cells start at age 0 (blue, color index 240)
2. Each generation survived increments `cellAge[i,j]`
3. Display color = `(240 + cellAge × 3) % 360`
4. 3x multiplier (`cellAgingColorFactor`) makes color changes visible within typical lifespans
5. Colors wrap after ~120 generations, creating continuous cycles for stable patterns
6. Cells update color **every tick** they survive, triggering dirty rendering

**Result**: Two complementary views of pattern behavior:
- **Birth mode**: Reveals pattern formation history and generation waves
- **Aging mode**: Reveals lifespan distribution and exponential mortality

### Efficient Dirty Rendering
```csharp
// Only redraw changed cells
if (cellStateChanged || cellAged) {  // cellAged true in aging mode
	changedCells.Add((i, j));
}
RenderGridDirty(changedCells);  // Redraw only changed cells
```

### Stability Detection

Automatically stops simulation when population count remains unchanged for 5 consecutive generations:
```csharp
if (AliveCount == PreviousAliveCount) {
	stableConsecutiveCount++;
	if (stableConsecutiveCount >= 5) stopSimulation();
}
```

**Note**: This is population-based, not pattern-based, so won't catch all stable states (see Limitations).

## ⚠️ Known Limitations

- **Auto-stop detection**: Only detects population stability, not pattern stability
  - Will **not** auto-stop for oscillators with period > 1 (e.g., blinkers, toads)
  - Will **not** auto-stop for patterns with constant population but changing positions (e.g., gliders)
- **Grid size**: Fixed at 150×72 cells (compile-time constant)
- **Color palette**: Limited to 360 colors
  - **Birth mode**: Repeats after 360 generations
  - **Aging mode**: Wraps after ~120 generations (360 ÷ 3)
- **Aging acceleration**: Fixed at 3x (compile-time constant `cellAgingColorFactor`)
- **Performance in aging mode**: All surviving cells redraw every tick (vs. only changed cells in birth mode)

## 🚧 Future Enhancements

### Planned Features
- [x] ~~**Pattern Library**~~ **COMPLETED** - Now includes 10 preset shapes and 5 tiling patterns
- [ ] **Statistics Panel**:
  - Birth/death rates per generation
  - Population density percentage
  - Peak population reached
  - Average cell lifespan (especially interesting in aging mode!)
  - Age distribution histogram
- [ ] **Advanced Detection**: Pattern-based stability with state history
- [ ] **Export Options**: Save as GIF animation or video
- [ ] **Configurable Grid**: Runtime grid size adjustment
- [ ] **Additional Palettes**: Monochrome, heat map, custom color schemes
- [ ] **Undo/Redo**: Step backward/forward through generations
- [ ] **Configurable aging factor**: Runtime adjustment of color change speed (`cellAgingColorFactor`)
- [ ] **Hybrid mode**: Combine both color systems (hue = birth, brightness = age)

## 🎨 Color Visualization Examples

### Constant Color from Birth Mode

| Pattern Type | Color Pattern | Meaning |
|--------------|---------------|---------|
| **Still Life** (Block, Beehive) | Solid ancient color | Cells from generation 0, never died |
| **Period-2 Oscillator** (Blinker, Toad) | Two alternating colors | Born and reborn on alternating ticks |
| **Glider** | Rainbow trail | Leaves history of colors as it moves |
| **Methuselah** (R-pentomino) | Explosive rainbow | High birth activity across many generations |
| **Stable Region** | Few solid colors | Low activity, ancient survivors |
| **Chaotic Region** | Multi-colored mess | High turnover, many generations mixed |

### Cell Changes Color as Aging Mode

| Pattern Type | Color Pattern | Meaning |
|--------------|---------------|---------|
| **Still Life** (Block, Beehive) | Continuously cycling rainbow | Cells age indefinitely, wrapping through spectrum every ~120 generations |
| **Random soup** | Dominated by blue/cyan | Exponential decay: most cells die young (age 1-10) |
| **Methuselah** (R-pentomino) | Blue explosions with rare greens | Massive birth activity, few survivors reach middle age |
| **Glider** | Uniform color moving | All cells same age, advancing together |
| **Stable + Chaos mix** | Red/orange islands in blue sea | Stable patterns show aged cells, chaos shows constant rebirth |
| **2×2 Block** | Smoothly cycling through all colors | Perfect example: survives forever, cycles through spectrum |

### Comparing Modes on Same Pattern

| Pattern | Birth Mode Shows | Aging Mode Shows |
|---------|------------------|------------------|
| **R-pentomino** | When each region formed | Which cells are long-lived vs. transient |
| **Gosper Glider Gun** | When each glider was born | Age of stationary parts vs. young gliders |
| **Blinkers** | Two-color alternation | Young cells (constantly age 0-1) |
| **Random soup** | Formation history | Mortality rate distribution |

## 📜 Credits

### Original Source
Based on the Conway's Game of Life implementation by **Quentin MOREL**  
Original repository: [Im-Rises/GameOfLife](https://github.com/Im-Rises/GameOfLife)

### Enhancements in This Fork
- **Pattern Tiling System**:
  - Windows-standard Ctrl+Drag interaction for quick selections
  - T-key persistent selection mode
  - Smart pattern capture (states + colors + spacing)
  - Seamless modulo-based tiling algorithm
  - Visual feedback (crosshair cursor, mode indicator, selection dimensions)
- **Copy/Paste/Transform System**:
  - Clipboard-based pattern editing with Ctrl+C / Ctrl+V
  - Preview-before-place workflow (paste mode overlay)
  - Real-time transform operations (flip horizontal/vertical, rotate 90°/180°)
  - Non-destructive single-placement editing
  - Mode-aware keyboard handling (transforms only work in paste mode)
- **Left-Hand Keyboard Shortcuts**:
  - Ctrl+R = Run/Start (always available via menu shortcut)
  - Ctrl+S = Stop (always available via menu shortcut)
  - Ctrl+C = Copy, Ctrl+V = Paste (always available)
  - Reliable focus handling with `KeyPreview = true`
  - T key for tiling mode toggle
- **Dual color mode system**:
  - 360-color spectrum generation age visualization
  - Dynamic age-based color progression with lifespan visualization
  - Real-time mode switching with instant grid recoloring
- **Pattern Library**:
  - 10 preset shapes (spaceships, oscillators, methuselahs, guns)
  - 7 mathematically interesting tiling patterns (including NEW: Pulsar Grid and Thin Diagonal Stripes)
  - Organized menu hierarchy (Shapes... and Tilings... submenus)
- **Visual Feedback Enhancements**:
  - Mode indicator with state-based coloring (green/cyan/sky blue)
  - Rounded eraser cursor preview
  - Selection rectangle with dimensions overlay
  - Density adjustment overlay (auto-hides after 5 seconds)
- Optimized rendering pipeline (dirty rectangles + brush caching + age-aware invalidation)
- Enhanced UI with color mode selector, status indicators, and reorganized menus
- RLE pattern import/export support with preview dialog
- Stability detection and auto-stop
- Interactive paint/erase tools with mouse wheel density control
- Comprehensive documentation with statistical analysis

### Conway's Game of Life
Created by mathematician **John Horton Conway** in 1970.

## 📄 License

[ MIT ]

## 👤 Most Recent Author

[Mark H. Smith]:
- <https://github.com/markhassellsmith>

## Contributors

Quentin MOREL:
- @Im-Rises
- <https://github.com/Im-Rises>

---

## 🎯 Quick Start Guide

1. **Launch** the application
2. **Select color mode** from dropdown in menu bar:
   - "Constant Color from Birth" - See pattern formation history *[default]*
   - "Cell Changes Color as Aging" - See lifespan distribution
3. **Press Ctrl+R** to start simulation with random pattern (or press Spacebar)
4. **Watch** the colors reveal pattern dynamics:
   - Birth mode: Rainbow trails show generation waves
   - Aging mode: Blue dominance shows exponential decay (most cells die young)
5. **Try the tiling feature**:
   - Draw a small pattern (3-5 cells)
   - **Hold Ctrl** (cursor becomes crosshair)
   - **Drag** to select pattern + spacing around it
   - **Press Enter** to tile it across the grid
   - **Press Ctrl+R** to watch it evolve!
6. **Click and drag** to paint/erase cells manually
7. **Switch modes** anytime to see different perspectives on the same pattern
8. **Adjust speed** via Speed menu
9. **Press Ctrl+S** to pause
10. **File → Reset** to generate new random pattern

---

**Enjoy watching life through two colorful lenses! 🌈⏳**

---

## 🧪 Experiment Ideas

Try these to explore the dual color modes and tiling:

### Classic Patterns
1. **Create a 2×2 block** in aging mode → Watch it cycle through the rainbow forever
2. **Start a random soup** → Compare how birth vs. aging modes show pattern evolution
3. **Import an R-pentomino** → Observe birth explosion vs. survival patterns
4. **Switch modes mid-simulation** → See how the same pattern looks from different perspectives
5. **Look for "blue deserts"** in aging mode → Regions with constant rebirth (high chaos)
6. **Find "red islands"** in aging mode → Stable patterns with ancient cells

### Tiling Experiments
7. **Draw a glider** → Ctrl+Drag select with spacing → Tile → Watch gliders interact!
8. **Create a 3×3 block with 2-cell spacing** → Tile → Observe emergent behavior
9. **Load Chessboard tiling** in birth mode → See ancient solid blocks vs. random soup chaos
10. **Load Ladder Brick tiling** in aging mode → Watch oscillators stay blue while stable parts age to red
11. **Load Diagonal Stripes** in birth mode → See rainbow trails where gliders travel along diagonals
12. **Draw an oscillator** (blinker/toad) → Select with tight spacing → Tile → Beautiful synchronization!
13. **Freehand doodle** a pattern → Ctrl+Drag → See what emerges from your art!
14. **Single alive cell** → Select 10×10 region around it → Tile → Uniform starting condition
15. **Combine patterns**: Tile a pattern, then use T-mode to select and tile a different region again!
10. **Load Double Diagonal** in aging mode → Find the ancient red intersections vs. young blue edges
11. **Load Concentric Rectangles** → Observe radial wave propagation in birth mode vs. mortality distribution in aging mode
12. **Try all 5 tilings** at different densities → See how density affects pattern evolution and longevity

### Density Experiments (Mouse Wheel)
13. **Mouse wheel a random soup** down to 10% density → Watch sparse evolution with long-lived structures
14. **Mouse wheel any tiling pattern** up to 90% density → Observe overcrowding and rapid die-off
15. **Adjust density mid-simulation** → See how population changes affect stable patterns

---

*For bugs, feature requests, or contributions, please open an issue on GitHub.*
