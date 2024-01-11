using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yumikou.Wpf.Controls.Helpers;

namespace System.Windows.Controls
{
    internal static class ScrollContentPresenterExtension
    {
        static internal double ValidateInputOffset(double offset, string parameterName)
        {
            if (MathHelper.IsNaN(offset))
            {
                //throw new ArgumentOutOfRangeException(parameterName, SR.Get(SRID.ScrollViewer_CannotBeNaN, parameterName));
                throw new ArgumentOutOfRangeException(parameterName, $"the {parameterName} of ScrollViewer can not be NaN");
            }
            return Math.Max(0.0, offset);
        }

        static internal double CoerceOffset(double offset, double extent, double viewport)
        {
            if (offset > extent - viewport) { offset = extent - viewport; }
            if (offset < 0) { offset = 0; }
            return offset;
        }

        internal static double ComputeScrollOffsetWithMinimalScroll(
            double topView,
            double bottomView,
            double topChild,
            double bottomChild)
        {
            bool alignTop = false;
            bool alignBottom = false;
            return ComputeScrollOffsetWithMinimalScroll(topView, bottomView, topChild, bottomChild, ref alignTop, ref alignBottom);
        }

        internal static double ComputeScrollOffsetWithMinimalScroll(
            double topView,
            double bottomView,
            double topChild,
            double bottomChild,
            ref bool alignTop,
            ref bool alignBottom)
        {
            // # CHILD POSITION       CHILD SIZE      SCROLL      REMEDY
            // 1 Above viewport       <= viewport     Down        Align top edge of child & viewport
            // 2 Above viewport       > viewport      Down        Align bottom edge of child & viewport
            // 3 Below viewport       <= viewport     Up          Align bottom edge of child & viewport
            // 4 Below viewport       > viewport      Up          Align top edge of child & viewport
            // 5 Entirely within viewport             NA          No scroll.
            // 6 Spanning viewport                    NA          No scroll.
            //
            // Note: "Above viewport" = childTop above viewportTop, childBottom above viewportBottom
            //       "Below viewport" = childTop below viewportTop, childBottom below viewportBottom
            // These child thus may overlap with the viewport, but will scroll the same direction/

            bool fAbove = MathHelper.LessThan(topChild, topView) && MathHelper.LessThan(bottomChild, bottomView);
            bool fBelow = MathHelper.GreaterThan(bottomChild, bottomView) && MathHelper.GreaterThan(topChild, topView);
            bool fLarger = (bottomChild - topChild) > (bottomView - topView);

            // Handle Cases:  1 & 4 above
            if ((fAbove && !fLarger)
               || (fBelow && fLarger)
               || alignTop)
            {
                alignTop = true;
                return topChild;
            }

            // Handle Cases: 2 & 3 above
            else if (fAbove || fBelow || alignBottom)
            {
                alignBottom = true;
                return (bottomChild - (bottomView - topView));
            }

            // Handle cases: 5 & 6 above.
            return topView;
        }
    }
}
