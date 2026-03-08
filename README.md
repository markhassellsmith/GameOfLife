# Conway's Game of Life - Enhanced Edition

	A high-performance Windows Forms implementation of Conway's Game of Life featuring dual cellular visualization modes: generation-based birth colors and dynamic age-based color progression through a 360-color spectrum palette.

![.NET 7](https://img.shields.io/badge/.NET-7.0-blue)
![C#](https://img.shields.io/badge/C%23-11.0-purple)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)

## 🌟 Overview

This project implements Conway's Game of Life with two distinctive visual modes that reveal different aspects of cellular evolution. Switch between generation tracking and lifespan visualization to explore patterns in unique ways.

## ✨ Features

### Visual Innovation - Dual Color Modes

#### 🎨 **Mode 1: Constant Color from Birth**
Cells preserve their birth generation color throughout their lifetime, creating a living "heat map" of cellular age and pattern evolution.

- **360-color gradient** cycling through full RGB spectrum
- **Birth generation tracking**: Each generation assigns a unique color
- **Lifetime preservation**: Cells keep their birth color until death
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

###  Density Variation
Mouse wheel adjusts initial population density; applies to all patterns (destructive of original pattern):
- **Scroll up**: Increase density (more alive cells)
- **Scroll down**: Decrease density (fewer alive cells)
- Works whether simulation is stopped or running (applies to current grid state)
- **Range from 0% to 100%** in 5% increments (default 50%)
- **Density value appears near mouse cursor for real-time feedback while adjusting 
- **Applies to**:
  - Random pattern: Adjusts probability of alive cells at start
  - Blank grid or Any pattern: Randomly kills or alives cells to achieve target density while preserving overall structure
  - Imported RLE patterns: Randomly kills or alives cells to achieve target density
	 
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
- **Mouse Drag**: Left-click paints cells, right-click erases 5×5 area
- **Spacebar Hotkey**: Instant start/stop toggle
- **Menu System**: Organized controls for simulation, speed, and patterns
- **Real-time Statistics**: Live generation count and population display
- **Status Indicator**: Color-coded "Running" (green) / "Stopped" (red) display

## 📋 Requirements

- **.NET 7.0 SDK** or higher
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
- **Import Pattern...** - Load pattern from RLE file
- **Exit** - Close application

#### **Simulation**
- **Start** - Begin the simulation
- **Stop** - Pause the simulation

#### **Speed**
- **Slow** - 1 generation per second (1000ms)
- **Medium** - ~1.8 generations per second (550ms) *[default]*
- **Quick** - 5 generations per second (200ms)

#### **Pattern**
- **Random** - Generate random starting configuration *[default]*

#### **Color Mode Dropdown** (in menu bar)
Select visualization mode:
- **Constant Color from Birth** - Track generation of birth *[default]*
- **Cell Changes Color as Aging** - Visualize lifespan and survival time

### Keyboard Shortcuts
| Key | Action |
|-----|--------|
| `Spacebar` | Toggle Start/Stop |

### Mouse Controls
| Action | Result |
|--------|--------|
| `Left Click + Drag` | Paint cells alive |
| `Right Click + Drag` | Erase 5×5 area |

### Status Bar
- **Color Mode Selector**: Dropdown to switch between birth and aging visualization modes
- **Tick Counter**: Shows current generation number
  - In **Birth mode**: Background cycles through spectrum
  - In **Aging mode**: White background (doesn't interfere with age colors)
- **Population Display**: Real-time count of alive cells
- **Status Indicator**: Shows "Running" (green) or "Stopped" (red)
- **Hint Text**: "Spacebar=Start/Stop"

## 🏗️ Architecture

### Project Structure

#### **Form1.cs** - Core Logic
- **Dual Color Systems**: 
  - `ColorMode.BirthGeneration`: Tracks generation of birth
  - `ColorMode.CellAging`: Tracks generations survived
- **Grid Management**: 150×72 boolean array with double-buffering
- **Simulation Engine**: Neighbor calculation with toroidal wrapping
- **Rendering Pipeline**: Optimized bitmap-based drawing with age-aware invalidation
- **Color Tracking**: 
  - `cellColor[,]`: Per-cell color array
  - `cellAge[,]`: Age tracking array (generations survived)
- **Event Handlers**: UI interaction and timer-driven updates

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
- [ ] **Pattern Library**: Pre-load famous patterns (R-pentomino, gliders, pulsars, Gosper Glider Gun)
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
- **Dual color mode system**:
  - 360-color spectrum generation age visualization
  - Dynamic age-based color progression with lifespan visualization
  - Real-time mode switching with instant grid recoloring
- Optimized rendering pipeline (dirty rectangles + brush caching + age-aware invalidation)
- Enhanced UI with color mode selector, status indicators, and reorganized menus
- RLE pattern import/export support
- Stability detection and auto-stop
- Interactive paint/erase tools
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
3. **Press Spacebar** to start simulation with random pattern
4. **Watch** the colors reveal pattern dynamics:
   - Birth mode: Rainbow trails show generation waves
   - Aging mode: Blue dominance shows exponential decay (most cells die young)
5. **Click and drag** to paint/erase cells manually
6. **Switch modes** anytime to see different perspectives on the same pattern
7. **Adjust speed** via Speed menu
8. **Press Spacebar** again to pause
9. **File → Reset** to generate new random pattern

---

**Enjoy watching life through two colorful lenses! 🌈⏳**

---

## 🧪 Experiment Ideas

Try these to explore the dual color modes:

1. **Create a 2×2 block** in aging mode → Watch it cycle through the rainbow forever
2. **Start a random soup** → Compare how birth vs. aging modes show pattern evolution
3. **Import an R-pentomino** → Observe birth explosion vs. survival patterns
4. **Switch modes mid-simulation** → See how the same pattern looks from different perspectives
5. **Look for "blue deserts"** in aging mode → Regions with constant rebirth (high chaos)
6. **Find "red islands"** in aging mode → Stable patterns with ancient cells

---

*For bugs, feature requests, or contributions, please open an issue on GitHub.*
