using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Yumikou.Wpf.Controls.Helpers
{
    internal class MathHelper
    {
        // smallest such that 1.0+DoubleEpsilon != 1.0
        internal static readonly double DoubleEpsilon = 2.2204460492503131e-016;

        private const float FloatEpsilon = 1.192092896e-07F;

        /// <summary>
        /// AreClose - Returns whether or not two doubles are "close".  That is, whether or 
        /// not they are within epsilon of each other.
        /// </summary> 
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        public static bool AreClose(double value1, double value2)
        {
            //in case they are Infinities (then epsilon check does not work)
            if (value1 == value2) return true;
            double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * DoubleEpsilon;
            double delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        /// <summary>
        /// AreClose - Returns whether or not two doubles are "close".  That is, whether or
        /// not they are within epsilon of each other.
        /// </summary>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        /// <param name="eps"> The fixed epsilon value used to compare.</param>
        public static bool AreClose(double value1, double value2, double eps)
        {
            //in case they are Infinities (then epsilon check does not work)
            if (value1 == value2) return true;
            double delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        /// <summary>
        /// AreClose - Returns whether or not two floats are "close".  That is, whether or 
        /// not they are within epsilon of each other.
        /// </summary> 
        /// <param name="value1"> The first float to compare. </param>
        /// <param name="value2"> The second float to compare. </param>
        public static bool AreClose(float value1, float value2)
        {
            //in case they are Infinities (then epsilon check does not work)
            if (value1 == value2) return true;
            float eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0f) * FloatEpsilon;
            float delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        /// <summary>
        /// LessThan - Returns whether or not the first double is less than the second double.
        /// That is, whether or not the first is strictly less than *and* not within epsilon of
        /// the other number.
        /// </summary>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        public static bool LessThan(double value1, double value2)
        {
            return (value1 < value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// LessThan - Returns whether or not the first float is less than the second float.
        /// That is, whether or not the first is strictly less than *and* not within epsilon of
        /// the other number.
        /// </summary>
        /// <param name="value1"> The first single float to compare. </param>
        /// <param name="value2"> The second single float to compare. </param>
        public static bool LessThan(float value1, float value2)
        {
            return (value1 < value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// GreaterThan - Returns whether or not the first double is greater than the second double.
        /// That is, whether or not the first is strictly greater than *and* not within epsilon of
        /// the other number.
        /// </summary>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        public static bool GreaterThan(double value1, double value2)
        {
            return (value1 > value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// GreaterThan - Returns whether or not the first float is greater than the second float.
        /// That is, whether or not the first is strictly greater than *and* not within epsilon of
        /// the other number.
        /// </summary>
        /// <param name="value1"> The first float to compare. </param>
        /// <param name="value2"> The second float to compare. </param>
        public static bool GreaterThan(float value1, float value2)
        {
            return (value1 > value2) && !AreClose(value1, value2);
        }

        /// <summary>
        /// LessThanOrClose - Returns whether or not the first double is less than or close to
        /// the second double.  That is, whether or not the first is strictly less than or within
        /// epsilon of the other number.
        /// </summary>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        public static bool LessThanOrClose(double value1, double value2)
        {
            return (value1 < value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// LessThanOrClose - Returns whether or not the first float is less than or close to
        /// the second float.  That is, whether or not the first is strictly less than or within
        /// epsilon of the other number.
        /// </summary>
        /// <param name="value1"> The first float to compare. </param>
        /// <param name="value2"> The second float to compare. </param>
        public static bool LessThanOrClose(float value1, float value2)
        {
            return (value1 < value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// GreaterThanOrClose - Returns whether or not the first double is greater than or close to
        /// the second double.  That is, whether or not the first is strictly greater than or within
        /// epsilon of the other number.
        /// </summary>
        /// <param name="value1"> The first double to compare. </param>
        /// <param name="value2"> The second double to compare. </param>
        public static bool GreaterThanOrClose(double value1, double value2)
        {
            return (value1 > value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// GreaterThanOrClose - Returns whether or not the first float is greater than or close to
        /// the second float.  That is, whether or not the first is strictly greater than or within
        /// epsilon of the other number.
        /// </summary>
        /// <param name="value1"> The first float to compare. </param>
        /// <param name="value2"> The second float to compare. </param>
        public static bool GreaterThanOrClose(float value1, float value2)
        {
            return (value1 > value2) || AreClose(value1, value2);
        }

        /// <summary>
        /// IsOne - Returns whether or not the double is "close" to 1.  Same as AreClose(double, 1),
        /// but this is faster.
        /// </summary>
        /// <param name="value"> The double to compare to 1. </param>
        public static bool IsOne(double value)
        {
            return Math.Abs(value - 1.0) < 10.0 * DoubleEpsilon;
        }

        /// <summary>
        /// IsOne - Returns whether or not the float is "close" to 1.  Same as AreClose(float, 1),
        /// but this is faster.
        /// </summary>
        /// <param name="value"> The float to compare to 1. </param>
        public static bool IsOne(float value)
        {
            return Math.Abs(value - 1.0f) < 10.0f * FloatEpsilon;
        }

        /// <summary>
        /// IsZero - Returns whether or not the double is "close" to 0.  Same as AreClose(double, 0),
        /// but this is faster.
        /// </summary>
        /// <param name="value"> The double to compare to 0. </param>
        public static bool IsZero(double value)
        {
            return Math.Abs(value) < 10.0 * DoubleEpsilon;
        }

        /// <summary>
        /// IsZero - Returns whether or not the float is "close" to 0.  Same as AreClose(float, 0),
        /// but this is faster.
        /// </summary>
        /// <param name="value"> The float to compare to 0. </param>
        public static bool IsZero(float value)
        {
            return Math.Abs(value) < 10.0f * FloatEpsilon;
        }

        // The Point, Size, Rect and Matrix class have moved to WinCorLib.  However, we provide
        // internal AreClose methods for our own use here.

        /// <summary>
        /// Compares two points for fuzzy equality.  This function
        /// helps compensate for the fact that double values can 
        /// acquire error when operated upon
        /// </summary>
        /// <param name='point1'>The first point to compare</param>
        /// <param name='point2'>The second point to compare</param>
        /// <returns>Whether or not the two points are equal</returns>
        public static bool AreClose(Point point1, Point point2)
        {
            return AreClose(point1.X, point2.X) && AreClose(point1.Y, point2.Y);
        }

        /// <summary>
        /// Compares two Size instances for fuzzy equality.  This function
        /// helps compensate for the fact that double values can 
        /// acquire error when operated upon
        /// </summary>
        /// <param name='size1'>The first size to compare</param>
        /// <param name='size2'>The second size to compare</param>
        /// <returns>Whether or not the two Size instances are equal</returns>
        public static bool AreClose(Size size1, Size size2)
        {
            return AreClose(size1.Width, size2.Width) &&
                   AreClose(size1.Height, size2.Height);
        }

        /// <summary>
        /// Compares two Vector instances for fuzzy equality.  This function
        /// helps compensate for the fact that double values can 
        /// acquire error when operated upon
        /// </summary>
        /// <param name='vector1'>The first Vector to compare</param>
        /// <param name='vector2'>The second Vector to compare</param>
        /// <returns>Whether or not the two Vector instances are equal</returns>
        public static bool AreClose(System.Windows.Vector vector1, System.Windows.Vector vector2)
        {
            return AreClose(vector1.X, vector2.X) &&
                   AreClose(vector1.Y, vector2.Y);
        }

        /// <summary>
        /// Compares two rectangles for fuzzy equality.  This function
        /// helps compensate for the fact that double values can 
        /// acquire error when operated upon
        /// </summary>
        /// <param name='rect1'>The first rectangle to compare</param>
        /// <param name='rect2'>The second rectangle to compare</param>
        /// <returns>Whether or not the two rectangles are equal</returns>
        public static bool AreClose(Rect rect1, Rect rect2)
        {
            // If they're both empty, don't bother with the double logic.
            if (rect1.IsEmpty)
            {
                return rect2.IsEmpty;
            }

            // At this point, rect1 isn't empty, so the first thing we can test is
            // rect2.IsEmpty, followed by property-wise compares.

            return (!rect2.IsEmpty) &&
                AreClose(rect1.X, rect2.X) &&
                AreClose(rect1.Y, rect2.Y) &&
                AreClose(rect1.Height, rect2.Height) &&
                AreClose(rect1.Width, rect2.Width);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsBetweenZeroAndOne(double val)
        {
            return (GreaterThanOrClose(val, 0) && LessThanOrClose(val, 1));
        }

#if !PBTCOMPILER

        [StructLayout(LayoutKind.Explicit)]
        private struct NanUnion
        {
            [FieldOffset(0)] internal double DoubleValue;
            [FieldOffset(0)] internal UInt64 UintValue;
        }

        // The standard CLR double.IsNaN() function is approximately 100 times slower than our own wrapper,
        // so please make sure to use DoubleUtil.IsNaN() in performance sensitive code.
        // PS item that tracks the CLR improvement is DevDiv Schedule : 26916.
        // IEEE 754 : If the argument is any value in the range 0x7ff0000000000001L through 0x7fffffffffffffffL 
        // or in the range 0xfff0000000000001L through 0xffffffffffffffffL, the result will be NaN.         
        public static bool IsNaN(double value)
        {
            NanUnion t = new NanUnion();
            t.DoubleValue = value;

            UInt64 exp = t.UintValue & 0xfff0000000000000;
            UInt64 man = t.UintValue & 0x000fffffffffffff;

            return (exp == 0x7ff0000000000000 || exp == 0xfff0000000000000) && (man != 0);
        }
#endif

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static double Clamp(double val, double min, double max)
        {
            if (min > max)
            {
                ThrowCannotBeGreaterThanException(min, max);
            }

            if (val < min)
            {
                return min;
            }
            else if (val > max)
            {
                return max;
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static decimal Clamp(decimal val, decimal min, decimal max)
        {
            if (min > max)
            {
                ThrowCannotBeGreaterThanException(min, max);
            }

            if (val < min)
            {
                return min;
            }
            else if (val > max)
            {
                return max;
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(int val, int min, int max)
        {
            if (min > max)
            {
                ThrowCannotBeGreaterThanException(min, max);
            }

            if (val < min)
            {
                return min;
            }
            else if (val > max)
            {
                return max;
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// Converts an angle in degrees to radians.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        public static double Deg2Rad(double angle)
        {
            return angle * (Math.PI / 180d);
        }

        /// <summary>
        /// Converts an angle in gradians to radians.
        /// </summary>
        /// <param name="angle">The angle in gradians.</param>
        /// <returns>The angle in radians.</returns>
        public static double Grad2Rad(double angle)
        {
            return angle * (Math.PI / 200d);
        }

        /// <summary>
        /// Converts an angle in turns to radians.
        /// </summary>
        /// <param name="angle">The angle in turns.</param>
        /// <returns>The angle in radians.</returns>
        public static double Turn2Rad(double angle)
        {
            return angle * 2 * Math.PI;
        }

        private static void ThrowCannotBeGreaterThanException<T>(T min, T max)
        {
            throw new ArgumentException($"{min} cannot be greater than {max}.");
        }
    }
}
