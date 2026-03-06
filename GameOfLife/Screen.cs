using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace WinFormsFractal
{
    /// <summary>
    /// This is the ScreenStructures class that sets up pixels and ColorStructs.
    /// </summary>
    public class ScreenStructures
    {
        /// <summary>
        /// This is the ColorStruct class containing three integers for red, green, blue.
        /// </summary>
        public struct ColorStruct
        {
            /// <summary>
            /// The red color component (0-255).
            /// </summary>
            public int red;

            /// <summary>
            /// The green color component (0-255).
            /// </summary>
            public int green;

            /// <summary>
            /// The blue color component (0-255).
            /// </summary>
            public int blue;
        }

        /// <summary>
        /// Represents a pixel with x, y coordinates and a color value.
        /// </summary>
        public struct Pixel
        {
            /// <summary>
            /// The x-coordinate of the pixel.
            /// </summary>
            public int x;

            /// <summary>
            /// The y-coordinate of the pixel.
            /// </summary>
            public int y;

            /// <summary>
            /// The color of the pixel.
            /// </summary>
            public ColorStruct pixcolor;
        }
    }
}
