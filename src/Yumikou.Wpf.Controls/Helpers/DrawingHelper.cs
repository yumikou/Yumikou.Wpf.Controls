using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yumikou.Wpf.Controls.Helpers
{
    internal static class DrawingHelper
    {
        /// <summary>
        ///     Generates a StreamGeometry.
        /// </summary>
        /// <param name="ctx">An already opened StreamGeometryContext.</param>
        /// <param name="rect">Rectangle for geomentry conversion.</param>
        /// <param name="radii">Corner radii.</param>
        /// <returns>Result geometry.</returns>
        public static void GenerateRoundedRectangleGeometry(StreamGeometryContext ctx, Rect rect, Radii radii)
        {
            //
            //  compute the coordinates of the key points
            //

            Point topLeft = new Point(radii.LeftTop, 0);
            Point topRight = new Point(rect.Width - radii.RightTop, 0);
            Point rightTop = new Point(rect.Width, radii.TopRight);
            Point rightBottom = new Point(rect.Width, rect.Height - radii.BottomRight);
            Point bottomRight = new Point(rect.Width - radii.RightBottom, rect.Height);
            Point bottomLeft = new Point(radii.LeftBottom, rect.Height);
            Point leftBottom = new Point(0, rect.Height - radii.BottomLeft);
            Point leftTop = new Point(0, radii.TopLeft);

            //
            //  check keypoints for overlap and resolve by partitioning radii according to
            //  the percentage of each one.  
            //

            //  top edge is handled here
            if (topLeft.X > topRight.X)
            {
                double v = (radii.LeftTop) / (radii.LeftTop + radii.RightTop) * rect.Width;
                topLeft.X = v;
                topRight.X = v;
            }

            //  right edge
            if (rightTop.Y > rightBottom.Y)
            {
                double v = (radii.TopRight) / (radii.TopRight + radii.BottomRight) * rect.Height;
                rightTop.Y = v;
                rightBottom.Y = v;
            }

            //  bottom edge
            if (bottomRight.X < bottomLeft.X)
            {
                double v = (radii.LeftBottom) / (radii.LeftBottom + radii.RightBottom) * rect.Width;
                bottomRight.X = v;
                bottomLeft.X = v;
            }

            // left edge
            if (leftBottom.Y < leftTop.Y)
            {
                double v = (radii.TopLeft) / (radii.TopLeft + radii.BottomLeft) * rect.Height;
                leftBottom.Y = v;
                leftTop.Y = v;
            }

            //
            //  add on offsets
            //

            Vector offset = new Vector(rect.TopLeft.X, rect.TopLeft.Y);
            topLeft += offset;
            topRight += offset;
            rightTop += offset;
            rightBottom += offset;
            bottomRight += offset;
            bottomLeft += offset;
            leftBottom += offset;
            leftTop += offset;

            //
            //  create the border geometry
            //
            ctx.BeginFigure(topLeft, true /* is filled */, true /* is closed */);

            // Top line
            ctx.LineTo(topRight, true /* is stroked */, false /* is smooth join */);

            // Upper-right corner
            double radiusX = rect.TopRight.X - topRight.X;
            double radiusY = rightTop.Y - rect.TopRight.Y;
            if (!MathHelper.IsZero(radiusX)
                || !MathHelper.IsZero(radiusY))
            {
                ctx.ArcTo(rightTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Right line
            ctx.LineTo(rightBottom, true /* is stroked */, false /* is smooth join */);

            // Lower-right corner
            radiusX = rect.BottomRight.X - bottomRight.X;
            radiusY = rect.BottomRight.Y - rightBottom.Y;
            if (!MathHelper.IsZero(radiusX)
                || !MathHelper.IsZero(radiusY))
            {
                ctx.ArcTo(bottomRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Bottom line
            ctx.LineTo(bottomLeft, true /* is stroked */, false /* is smooth join */);

            // Lower-left corner
            radiusX = bottomLeft.X - rect.BottomLeft.X;
            radiusY = rect.BottomLeft.Y - leftBottom.Y;
            if (!MathHelper.IsZero(radiusX)
                || !MathHelper.IsZero(radiusY))
            {
                ctx.ArcTo(leftBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Left line
            ctx.LineTo(leftTop, true /* is stroked */, false /* is smooth join */);

            // Upper-left corner
            radiusX = topLeft.X - rect.TopLeft.X;
            radiusY = leftTop.Y - rect.TopLeft.Y;
            if (!MathHelper.IsZero(radiusX)
                || !MathHelper.IsZero(radiusY))
            {
                ctx.ArcTo(topLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }
        }

        /// <summary>
        /// 通过厚度膨胀Rect
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="thick"></param>
        /// <returns></returns>
        public static Rect InflateRect(Rect rt, Thickness thick, double offsetX = 0, double offsetY = 0)
        {
            return new Rect(rt.Left - thick.Left + offsetX,
                            rt.Top - thick.Top + offsetY,
                            Math.Max(0.0, rt.Width + thick.Left + thick.Right),
                            Math.Max(0.0, rt.Height + thick.Top + thick.Bottom));
        }

        /// <summary>
        /// 通过厚度缩小Rect
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="thick"></param>
        /// <returns></returns>
        public static Rect DeflateRect(Rect rt, Thickness thick, double offsetX = 0, double offsetY = 0)
        {
            return new Rect(rt.Left + thick.Left + offsetX,
                            rt.Top + thick.Top + offsetY,
                            Math.Max(0.0, rt.Width - thick.Left - thick.Right),
                            Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
        }
    }

    internal struct Radii
    {
        public Radii(CornerRadius radii, Thickness borders, bool outer, double changePercent = 0.5)
        {
            double left = changePercent * borders.Left;
            double top = changePercent * borders.Top;
            double right = changePercent * borders.Right;
            double bottom = changePercent * borders.Bottom;

            if (outer)
            {
                if (MathHelper.IsZero(radii.TopLeft))
                {
                    LeftTop = TopLeft = 0.0;
                }
                else
                {
                    LeftTop = radii.TopLeft + left;
                    TopLeft = radii.TopLeft + top;
                }
                if (MathHelper.IsZero(radii.TopRight))
                {
                    TopRight = RightTop = 0.0;
                }
                else
                {
                    TopRight = radii.TopRight + top;
                    RightTop = radii.TopRight + right;
                }
                if (MathHelper.IsZero(radii.BottomRight))
                {
                    RightBottom = BottomRight = 0.0;
                }
                else
                {
                    RightBottom = radii.BottomRight + right;
                    BottomRight = radii.BottomRight + bottom;
                }
                if (MathHelper.IsZero(radii.BottomLeft))
                {
                    BottomLeft = LeftBottom = 0.0;
                }
                else
                {
                    BottomLeft = radii.BottomLeft + bottom;
                    LeftBottom = radii.BottomLeft + left;
                }
            }
            else
            {
                LeftTop = Math.Max(0.0, radii.TopLeft - left);
                TopLeft = Math.Max(0.0, radii.TopLeft - top);
                TopRight = Math.Max(0.0, radii.TopRight - top);
                RightTop = Math.Max(0.0, radii.TopRight - right);
                RightBottom = Math.Max(0.0, radii.BottomRight - right);
                BottomRight = Math.Max(0.0, radii.BottomRight - bottom);
                BottomLeft = Math.Max(0.0, radii.BottomLeft - bottom);
                LeftBottom = Math.Max(0.0, radii.BottomLeft - left);
            }
        }

        public Radii(Radii radii, Thickness borders, bool outer, double changePercent = 0.5)
        {
            double left = changePercent * borders.Left;
            double top = changePercent * borders.Top;
            double right = changePercent * borders.Right;
            double bottom = changePercent * borders.Bottom;

            if (outer)
            {
                if (MathHelper.IsZero(radii.TopLeft))
                {
                    LeftTop = TopLeft = 0.0;
                }
                else
                {
                    LeftTop = radii.LeftTop + left;
                    TopLeft = radii.TopLeft + top;
                }
                if (MathHelper.IsZero(radii.TopRight))
                {
                    TopRight = RightTop = 0.0;
                }
                else
                {
                    TopRight = radii.TopRight + top;
                    RightTop = radii.RightTop + right;
                }
                if (MathHelper.IsZero(radii.BottomRight))
                {
                    RightBottom = BottomRight = 0.0;
                }
                else
                {
                    RightBottom = radii.RightBottom + right;
                    BottomRight = radii.BottomRight + bottom;
                }
                if (MathHelper.IsZero(radii.BottomLeft))
                {
                    BottomLeft = LeftBottom = 0.0;
                }
                else
                {
                    BottomLeft = radii.BottomLeft + bottom;
                    LeftBottom = radii.LeftBottom + left;
                }
            }
            else
            {
                LeftTop = Math.Max(0.0, radii.LeftTop - left);
                TopLeft = Math.Max(0.0, radii.TopLeft - top);
                TopRight = Math.Max(0.0, radii.TopRight - top);
                RightTop = Math.Max(0.0, radii.RightTop - right);
                RightBottom = Math.Max(0.0, radii.RightBottom - right);
                BottomRight = Math.Max(0.0, radii.BottomRight - bottom);
                BottomLeft = Math.Max(0.0, radii.BottomLeft - bottom);
                LeftBottom = Math.Max(0.0, radii.LeftBottom - left);
            }
        }

        public double LeftTop;
        public double TopLeft;
        public double TopRight;
        public double RightTop;
        public double RightBottom;
        public double BottomRight;
        public double BottomLeft;
        public double LeftBottom;
    }
}
