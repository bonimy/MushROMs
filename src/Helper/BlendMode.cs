// <copyright file="BlendMode.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

/*
Blend modes provided were taken from the blend options in Adobe Photoshop. A reference to where each algorithm was obtained is provided in the method that implements them.

Refer to https://en.wikipedia.org/wiki/Blend_modes for information on all of the blend algorithms.
*/

namespace Helper
{
    /// <summary>
    /// Specifies the color blending style for two <see cref="ColorF"/> color values.
    /// </summary>
    public enum BlendMode
    {
        /// <summary>
        /// Performs an alpha compositing blend.
        /// </summary>
        Alpha,

        /// <summary>
        /// Performs a grayscale blend.
        /// </summary>
        Grayscale,

        /// <summary>
        /// Multiplies the numbers for each pixel of the top layer with the corresponding pixel for the bottom layer. The result is a darker picture.
        /// </summary>
        Multiply,

        /// <summary>
        /// The values of the pixels in the two layers are inverted, multiplied, and then inverted again. This yields the opposite effect to <see cref="Multiply"/>. The result is a brighter picture.
        /// </summary>
        Screen,

        /// <summary>
        /// Combines <see cref="Multiply"/> and <see cref="Screen"/>.
        /// </summary>
        Overlay,

        /// <summary>
        /// Combines <see cref="Multiply"/> and <see cref="Screen"/> blend modes. Equivalent to <see cref="Overlay"/>, but with the bottom and top images swapped.
        /// </summary>
        HardLight,

        /// <summary>
        /// A softer version of <see cref="HardLight"/>.
        /// </summary>
        SoftLight,

        /// <summary>
        /// Divides the bottom layer by the inverted top layer.
        /// </summary>
        ColorDodge,

        /// <summary>
        /// Sums the values in the two layers.
        /// </summary>
        LinearDodge,

        /// <summary>
        /// Divides the inverted bottom layer by the top layer, and then inverts the result.
        /// </summary>
        ColorBurn,

        /// <summary>
        /// Sums the value in the two layers and subtracts 1. This is the same as inverting each layer, adding them together (as in <see cref="LinearDodge"/>), and then inverting the result.
        /// </summary>
        LinearBurn,

        /// <summary>
        /// Combines <see cref="ColorDodge"/> and <see cref="ColorBurn"/> (rescaled so that neutral colors become middle gray).
        /// </summary>
        VividLight,

        /// <summary>
        /// Combines <see cref="LinearDodge"/> and <see cref="LinearBurn"/> (rescaled so that neutral colors become middle gray).
        /// </summary>
        LinearLight,

        /// <summary>
        /// Subtracts the bottom layer from the top layer or the other way round, to always get a positive value.
        /// </summary>
        Difference,

        /// <summary>
        /// Creates a pixel that retains the smallest components of the foreground and background pixels.
        /// </summary>
        Darken,

        /// <summary>
        /// Selects the maximum of each component from the foreground and background pixels.
        /// </summary>
        Lighten,

        /// <summary>
        /// Selects the color with the lowest luma channel.
        /// </summary>
        DarkerColor,

        /// <summary>
        /// Selects the color with the highest luma channel.
        /// </summary>
        LighterColor,

        /// <summary>
        /// Replaces the hue of the top layer with the hue of the bottom layer.
        /// </summary>
        Hue,

        /// <summary>
        /// Replaces the saturation of the top layer with the hue of the bottom layer.
        /// </summary>
        Saturation,

        /// <summary>
        /// Replaces the luminosity of the top layer with the hue of the bottom layer.
        /// </summary>
        Luminosity,

        /// <summary>
        /// Divides the top layer by the bottom layer.
        /// </summary>
        Divide
    }
}
