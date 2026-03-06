# Conway's Game of Life - Enhanced Edition

A high-performance Windows Forms implementation of Conway's Game of Life featuring unique cellular age visualization through a 360-color spectrum palette.

![.NET 7](https://img.shields.io/badge/.NET-7.0-blue)
![C#](https://img.shields.io/badge/C%23-11.0-purple)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)

## 🌟 Overview

This project implements Conway's Game of Life with a distinctive visual twist: each cell retains the color of the generation in which it was born, creating a mesmerizing rainbow visualization that reveals the age, complexity, and evolution patterns of the cellular automaton in real-time.

## ✨ Features

### Visual Innovation
- **🎨 Spectrum Age Coloring**: Cells preserve their birth generation color throughout their lifetime
  - 360-color gradient cycling through full RGB spectrum
  - Older cells display colors from past generations
  - New cells show the current generation color
  - Creates a living "heat map" of cellular age and activity
  - Instantly reveals pattern types:
    - **Still lifes** appear as solid single colors (ancient survivors)
    - **Oscillators** display alternating color bands
    - **Gliders/spaceships** leave colorful trails across the grid
    - **Methuselahs** create explosive rainbow patterns
    - **Chaotic regions** show multi-colored complexity

### Core Simulation
- **150×72 cell grid** (10,800 cells total)
- Classic Conway's rules (B3/S23): Birth on 3 neighbors, Survival on 2-3 neighbors
- Toroidal wrapping (edges wrap to opposite sides)
- Automatic stability detection (stops after 5 generations of unchanged population)

### Performance Optimizations
- **Dirty Rectangle Rendering**: Only redraws cells that changed state
- **Brush Caching**: Reuses color brushes to minimize memory allocations
- **Buffer.BlockCopy**: High-speed array operations for state management
- **Single Bitmap Rendering**: All cells drawn to one bitmap vs. 10,800+ individual controls
- Runs smoothly at ~5 generations/second on a 10,800 cell grid

### Interactive Controls
- **Mouse Click**: Toggle individual cell states
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

````````

3. **Build and Run**:
   - Press `F5` to build and start debugging
   - Or press `Ctrl+F5` to run without debugging

### Build from Command Line

## 🎮 Usage

### Menu Controls

#### **File**
- **Reset** - Clear grid and generate new random pattern
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

### Keyboard Shortcuts
| Key | Action |
|-----|--------|
| `Spacebar` | Toggle Start/Stop |

### Mouse Controls
| Action | Result |
|--------|--------|
| `Left Click` on cell | Toggle cell alive/dead |

### Status Bar
- **Tick Counter**: Shows current generation number with background color cycling through spectrum
- **Population Display**: Real-time count of alive cells
- **Status Indicator**: Shows "Running" (green) or "Stopped" (red)
- **Hint Text**: "Spacebar=Start/Stop"

## 🏗️ Architecture

### Project Structure

#### **Form1.cs** - Core Logic
- **Grid Management**: 150×72 boolean array with double-buffering
- **Simulation Engine**: Neighbor calculation with toroidal wrapping
- **Rendering Pipeline**: Optimized bitmap-based drawing
- **Color Tracking**: Per-cell color array preserving birth generation
- **Event Handlers**: UI interaction and timer-driven updates

#### **ColorPalettes.cs**
- **Spectrum360**: 360-color RGB gradient array
  - Cycles through red → yellow → green → cyan → blue → magenta → red
  - Each generation uses next color in sequence (wraps at 360)

#### **Game Rules Implementation**

## 🔧 Technical Highlights

### Color-Coded Cell Age System

The unique color visualization works by:
1. Each generation increments a color index (0-359)
2. Newly born cells get assigned the current generation's color
3. Cells retain their birth color until death
4. Color is stored in a parallel 2D array matching the cell grid

**Result**: Visual representation of pattern complexity and cellular age distribution.

### Efficient Dirty Rendering

### Stability Detection

Automatically stops simulation when population count remains unchanged for 5 consecutive generations:

**Note**: This is population-based, not pattern-based, so won't catch all stable states (see Limitations).

## ⚠️ Known Limitations

- **Auto-stop detection**: Only detects population stability, not pattern stability
  - Will **not** auto-stop for oscillators with period > 1 (e.g., blinkers, toads)
  - Will **not** auto-stop for patterns with constant population but changing positions (e.g., gliders)
- **Grid size**: Fixed at 150×72 cells (compile-time constant)
- **Color palette**: Limited to 360 colors, repeats after 360 generations
- **Pattern library**: Currently only supports random initialization
- **No save/load**: Cannot persist interesting patterns

## 🚧 Future Enhancements

### Planned Features
- [ ] **Pattern Library**: Pre-load famous patterns (R-pentomino, gliders, pulsars, Gosper Glider Gun)
- [ ] **Save/Load Configurations**: Export/import pattern files
- [ ] **Statistics Panel**: 
  - Birth/death rates per generation
  - Population density percentage
  - Peak population reached
  - Average cell lifespan
- [ ] **Advanced Detection**: Pattern-based stability with state history
- [ ] **Export Options**: Save as GIF animation or video
- [ ] **Configurable Grid**: Runtime grid size adjustment
- [ ] **Additional Palettes**: Monochrome, heat map, custom color schemes
- [ ] **Drawing Tools**: Brush modes for easier pattern creation
- [ ] **Undo/Redo**: Step backward/forward through generations

## 🎨 Color Visualization Examples

### What the Colors Reveal:

| Pattern Type | Color Pattern | Meaning |
|--------------|---------------|---------|
| **Still Life** (Block, Beehive) | Solid ancient color | Cells from generation 0, never died |
| **Period-2 Oscillator** (Blinker, Toad) | Two alternating colors | Born and reborn on alternating ticks |
| **Glider** | Rainbow trail | Leaves history of colors as it moves |
| **Methuselah** (R-pentomino) | Explosive rainbow | High birth activity across many generations |
| **Stable Region** | Few solid colors | Low activity, ancient survivors |
| **Chaotic Region** | Multi-colored mess | High turnover, many generations mixed |

## 📜 Credits


### Enhancements in This Fork
- 360-color spectrum age visualization system
- Optimized rendering pipeline (dirty rectangles + brush caching)
- Enhanced UI with status indicators and reorganized menus
- Stability detection and auto-stop
- Interactive cell toggling
- Comprehensive documentation

### Conway's Game of Life
Created by mathematician **John Horton Conway** in 1970.

## 📄 License

[ MIT ]

## 👤 Most Recent Author

[Mark H. Smith]  :
- <https://github.com/markhassellsmith>


## Contributors

Quentin MOREL :

- @Im-Rises
- <https://github.com/Im-Rises>





---

## 🎯 Quick Start Guide

1. **Launch** the application
2. **Press Spacebar** to start simulation with random pattern
3. **Watch** the rainbow colors reveal pattern evolution
4. **Click cells** to manually toggle states
5. **Adjust speed** via Speed menu
6. **Press Spacebar** again to pause
7. **File → Reset** to generate new random pattern

---

**Enjoy watching the rainbow of life evolve! 🌈**

---

*For bugs, feature requests, or contributions, please open an issue on GitHub.*
