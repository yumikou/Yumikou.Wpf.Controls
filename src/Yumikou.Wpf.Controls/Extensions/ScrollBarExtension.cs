using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace System.Windows.Controls.Primitives
{
    internal static class ScrollBarExtension
    {
        internal static bool IsValidOrientation(object o)
        {
            Orientation value = (Orientation)o;
            return value == Orientation.Horizontal
                || value == Orientation.Vertical;
        }
    }
}
