// <copyright file="MathHelper.cs" company="Public Domain">
//     Copyright (c) 2017 Nelson Garcia.
// </copyright>

using System;
using static System.Math;

namespace Helper
{
    public static class MathHelper
    {
        public const float DefaultTolerance = 1e-7f;

        public static bool NearlyEquals(float left, float right)
        {
            return NearlyEquals(left, right, DefaultTolerance);
        }

        public static bool NearlyEquals(float left, float right, float tolerance)
        {
            return Abs(left - right) <= tolerance;
        }

        public static float SnapToLimit(float value, float limit, float tolerance)
        {
            return NearlyEquals(value, limit, tolerance) ? limit : value;
        }

        public static float Clamp(float value, float min, float max)
        {
            return Clamp(value, min, max, 0);
        }

        public static float Clamp(float value, float min, float max, float tolerance)
        {
            // NaN cannot be clamped to a range.
            if (Single.IsNaN(value))
            {
                return Single.NaN;
            }

            // If max and min are equal within the desired precision, clamp to the midpoint.
            if (NearlyEquals(max, min, tolerance))
            {
                return (max + min) / 2;
            }

            // If max is less than min, then we have a null range.
            if (max < min)
            {
                return Single.NaN;
            }

            // Clamp left
            if (NearlyEquals(value, min, tolerance) || value < min)
            {
                return min;
            }

            // Clamp right
            if (NearlyEquals(value, max, tolerance) || value > max)
            {
                return max;
            }

            // Return original value
            return value;
        }

        public static float Max(float value1, float value2, params float[] values)
        {
            var max = Math.Max(value1, value2);
            if (values != null)
            {
                foreach (var value in values)
                {
                    max = Math.Max(max, value);
                }
            }

            return max;
        }

        public static float Max(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(
                    nameof(values),
                    SR.ErrorEmptyOrNullArray(nameof(values)));
            }

            if (values.Length == 0)
            {
                throw new ArgumentException(
                    SR.ErrorEmptyOrNullArray(nameof(values)),
                    nameof(values));
            }

            var max = values[0];
            foreach (var value in values)
            {
                if (max < value)
                {
                    max = value;
                }
            }

            return max;
        }

        public static float Min(float value1, float value2, params float[] values)
        {
            var min = Math.Min(value1, value2);
            if (values != null)
            {
                foreach (var value in values)
                {
                    min = Math.Min(min, value);
                }
            }

            return min;
        }

        public static float Min(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(
                    nameof(values),
                    SR.ErrorEmptyOrNullArray(nameof(values)));
            }

            if (values.Length == 0)
            {
                throw new ArgumentException(
                    SR.ErrorEmptyOrNullArray(nameof(values)),
                    nameof(values));
            }

            var min = values[0];
            foreach (var value in values)
            {
                if (min > value)
                {
                    min = value;
                }
            }

            return min;
        }
    }
}
