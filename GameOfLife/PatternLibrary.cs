using System.Collections.Generic;

namespace GameOfLife
{
    /// <summary>
    /// Library of built-in Game of Life patterns in RLE format.
    /// </summary>
    public static class PatternLibrary
    {
        /// <summary>
        /// Gets all available built-in patterns.
        /// </summary>
        public static Dictionary<string, string> GetAllPatterns()
        {
            return new Dictionary<string, string>
            {
                { "Glider", Glider },
                { "Lightweight Spaceship (LWSS)", LightweightSpaceship },
                { "Blinker", Blinker },
                { "Toad", Toad },
                { "Beacon", Beacon },
                { "Pulsar", Pulsar },
                { "Gosper Glider Gun", GosperGliderGun },
                { "R-Pentomino", RPentomino },
                { "Acorn", Acorn },
                { "Diehard", Diehard }
            };
        }

        // Small patterns (oscillators and spaceships)

        /// <summary>
        /// The smallest, most common, and first-discovered spaceship.
        /// Moves diagonally with period 4. Size: 3x3.
        /// </summary>
        public static readonly string Glider = @"#N Glider
#C The smallest, most common, and first-discovered spaceship.
#C Diagonal movement with period 4.
x = 3, y = 3, rule = B3/S23
bob$2bo$3o!";

        /// <summary>
        /// A small spaceship that travels orthogonally.
        /// Moves horizontally with period 4. Size: 5x4.
        /// </summary>
        public static readonly string LightweightSpaceship = @"#N Lightweight Spaceship
#C A small spaceship that travels orthogonally.
#C Moves with period 4.
x = 5, y = 4, rule = B3/S23
b3o$o3bo$4bo$o2bo!";

        /// <summary>
        /// The smallest and most common oscillator.
        /// Period 2. Size: 3x1.
        /// </summary>
        public static readonly string Blinker = @"#N Blinker
#C The smallest and most common oscillator.
#C Period 2.
x = 3, y = 1, rule = B3/S23
3o!";

        /// <summary>
        /// A period 2 oscillator.
        /// Size: 4x2.
        /// </summary>
        public static readonly string Toad = @"#N Toad
#C A period 2 oscillator.
x = 4, y = 2, rule = B3/S23
b3o$3o!";

        /// <summary>
        /// A period 2 oscillator composed of two blocks.
        /// Size: 4x4.
        /// </summary>
        public static readonly string Beacon = @"#N Beacon
#C A period 2 oscillator.
x = 4, y = 4, rule = B3/S23
2o$2o$2b2o$2b2o!";

        /// <summary>
        /// A large period 3 oscillator with high symmetry.
        /// One of the most recognizable patterns. Size: 13x13.
        /// </summary>
        public static readonly string Pulsar = @"#N Pulsar
#C A large period 3 oscillator.
x = 13, y = 13, rule = B3/S23
2b3o3b3o2b2$o4bobo4bo$o4bobo4bo$o4bobo4bo$2b3o3b3o2b2$2b3o3b3o2b$o4bobo
4bo$o4bobo4bo$o4bobo4bo2$2b3o3b3o!";

        // Methuselahs (long-lived patterns)

        /// <summary>
        /// A methuselah that stabilizes after 1,103 generations.
        /// Creates a diverse pattern evolution from a small seed. Size: 3x3.
        /// </summary>
        public static readonly string RPentomino = @"#N R-pentomino
#C A methuselah that stabilizes after 1103 generations.
#C Creates a diverse pattern evolution.
x = 3, y = 3, rule = B3/S23
b2o$2o$bo!";

        /// <summary>
        /// A methuselah that stabilizes after 5,206 generations.
        /// Creates 633 cells including 13 escaped gliders. Size: 7x3.
        /// </summary>
        public static readonly string Acorn = @"#N Acorn
#C A methuselah that stabilizes after 5206 generations.
#C Creates 633 cells including 13 escaped gliders.
x = 7, y = 3, rule = B3/S23
bo5b$3bo3b$2o2b3o!";

        /// <summary>
        /// A methuselah that vanishes completely after 130 generations.
        /// One of the few patterns that dies completely. Size: 8x3.
        /// </summary>
        public static readonly string Diehard = @"#N Diehard
#C A methuselah that vanishes after 130 generations.
#C One of the few patterns that dies completely.
x = 8, y = 3, rule = B3/S23
6b2o$2o6b$bo3b3o!";

        // Complex patterns

        /// <summary>
        /// The first discovered gun (infinite growth pattern).
        /// Produces a new glider every 30 generations. Size: 36x9.
        /// </summary>
        public static readonly string GosperGliderGun = @"#N Gosper Glider Gun
#C The first discovered gun (infinite growth pattern).
#C Produces a new glider every 30 generations.
x = 36, y = 9, rule = B3/S23
24bo11b$22bobo11b$12b2o6b2o12b2o$11bo3bo4b2o12b2o$2o8bo5bo3b2o14b$2o8b
o3bob2o4bobo11b$10bo5bo7bo11b$11bo3bo20b$12b2o!";
    }
}
