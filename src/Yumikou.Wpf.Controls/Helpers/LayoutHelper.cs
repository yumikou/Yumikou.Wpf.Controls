using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Yumikou.Wpf.Controls.Helpers
{
    internal static class LayoutHelper
    {
        public static double RoundLayoutValue(double value, double dpiScale)
        {
            double newValue;

            // If DPI == 1, don't use DPI-aware rounding.
            if (!MathHelper.AreClose(dpiScale, 1.0))
            {
                newValue = Math.Round(value * dpiScale) / dpiScale;
                // If rounding produces a value unacceptable to layout (NaN, Infinity or MaxValue), use the original value.
                if (double.IsNaN(newValue) ||
                    double.IsInfinity(newValue) ||
                    MathHelper.AreClose(newValue, double.MaxValue))
                {
                    newValue = value;
                }
            }
            else
            {
                newValue = Math.Round(value);
            }

            return newValue;
        }

        public static DpiScale GetDpi(Visual visual)
        {
#if NET462_OR_GREATER || NETCOREAPP
            return VisualTreeHelper.GetDpi(visual);
#else
            var source = PresentationSource.FromVisual(visual);

            if (source?.CompositionTarget == null)
            {
                return new DpiScale(1.0, 1.0);
            }

            var device = source.CompositionTarget.TransformToDevice;
            return new DpiScale(device.M11, device.M22); 
#endif
        }
    }

#if NET462_OR_GREATER || NETCOREAPP
#else
    /// <summary>Stores DPI information from which a <see cref="T:System.Windows.Media.Visual" /> or <see cref="T:System.Windows.UIElement" /> is rendered.</summary>
    internal struct DpiScale
    {
        /// <summary>Gets the DPI scale on the X axis.</summary>
        /// <returns>The DPI scale for the X axis.</returns>
        public double DpiScaleX { get; private set; }

        /// <summary>Gets the DPI scale on the Yaxis.</summary>
        /// <returns>The DPI scale for the Y axis.</returns>
        public double DpiScaleY { get; private set; }

        /// <summary>Get or sets the PixelsPerDip at which the text should be rendered.</summary>
        /// <returns>The current <see cref="P:System.Windows.DpiScale.PixelsPerDip" /> value.</returns>
        public double PixelsPerDip
        {
            get
            {
                return DpiScaleY;
            }
        }

        /// <summary>Gets the DPI along X axis.</summary>
        /// <returns>The DPI along the X axis.</returns>
        public double PixelsPerInchX
        {
            get
            {
                return 96.0 * DpiScaleX;
            }
        }

        /// <summary>Gets the DPI along Y axis.</summary>
        /// <returns>The DPI along the Y axis.</returns>
        public double PixelsPerInchY
        {
            get
            {
                return 96.0 * DpiScaleY;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.DpiScale" /> structure.</summary>
        /// <param name="dpiScaleX">The DPI scale on the X axis.</param>
        /// <param name="dpiScaleY">The DPI scale on the Y axis. </param>
        public DpiScale(double dpiScaleX, double dpiScaleY)
        {
            DpiScaleX = dpiScaleX;
            DpiScaleY = dpiScaleY;
        }
    }
#endif
}
