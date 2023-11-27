using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Yumikou.Wpf.Controls.Helpers;

namespace Yumikou.Wpf.Controls
{
    /// <summary>
    /// 使用 <see cref="BoxShadows" /> 设置阴影
    /// </summary>
    public class Border : System.Windows.Controls.Border
    {
        public BoxShadows? BoxShadows
        {
            get { return (BoxShadows?)GetValue(BoxShadowsProperty); }
            set { SetValue(BoxShadowsProperty, value); }
        }


        public Border()
        {
            BoxShadows = new BoxShadows();
        }

#if NET46_OR_GREATER || NETCOREAPP
#else
        protected override Geometry? GetLayoutClip(Size layoutSlotSize) // bug fixed: net46以下, 设置UseLayoutRound=true时，会导致ClipToBounds=false失效
        {
            return this.ClipToBounds ? base.GetLayoutClip(layoutSlotSize) : null;
        }
#endif

        /// <summary>
        /// DependencyProperty for <see cref="BoxShadows" /> property.
        /// </summary>
        public static readonly DependencyProperty BoxShadowsProperty
            = DependencyProperty.Register("BoxShadows", typeof(BoxShadows), typeof(Border),
                                          new FrameworkPropertyMetadata(
                                                null, FrameworkPropertyMetadataOptions.AffectsRender
                                              ));

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            BoxShadows? boxShadows = BoxShadows;
            if (boxShadows != null && boxShadows.Count > 0)
            {
                CornerRadius cornerRadius = CornerRadius;
                Thickness borders = BorderThickness;
                Rect innerRect = new Rect(this.RenderSize); //阴影的内层Rect
#if NET46_OR_GREATER || NETCOREAPP
                if (this.UseLayoutRounding)
                {
                    DpiScale dpi = LayoutHelper.GetDpi(this);
                    borders = new Thickness(LayoutHelper.RoundLayoutValue(borders.Left, dpi.DpiScaleX), LayoutHelper.RoundLayoutValue(borders.Top, dpi.DpiScaleY),
                       LayoutHelper.RoundLayoutValue(borders.Right, dpi.DpiScaleX), LayoutHelper.RoundLayoutValue(borders.Bottom, dpi.DpiScaleY));
                }
#endif
                Radii innerRadii = new Radii(cornerRadius, borders, true); //阴影的内层圆角半径
                StreamGeometry innerGeometry = new StreamGeometry();
                using (StreamGeometryContext ctx = innerGeometry.Open())
                {
                    if (!MathHelper.IsZero(innerRect.Width) && !MathHelper.IsZero(innerRect.Height))
                    {
                        DrawingHelper.GenerateRoundedRectangleGeometry(ctx, innerRect, innerRadii);
                    }
                }
                innerGeometry.Freeze();

                foreach (BoxShadow boxShadow in boxShadows)
                {
                    if (boxShadow.Brush != null)
                    {
                        Thickness spreadBorders = new Thickness(boxShadow.SpreadRadius);
#if NET46_OR_GREATER || NETCOREAPP
                        if (this.UseLayoutRounding)
                        {
                            DpiScale dpi = LayoutHelper.GetDpi(this);
                            spreadBorders = new Thickness(LayoutHelper.RoundLayoutValue(spreadBorders.Left, dpi.DpiScaleX), LayoutHelper.RoundLayoutValue(spreadBorders.Top, dpi.DpiScaleY),
                                LayoutHelper.RoundLayoutValue(spreadBorders.Right, dpi.DpiScaleX), LayoutHelper.RoundLayoutValue(spreadBorders.Bottom, dpi.DpiScaleY));
                        }
#endif
                        Radii outerRadii = new Radii(innerRadii, spreadBorders, true, 1);
                        Rect outerRect = DrawingHelper.InflateRect(innerRect, spreadBorders, boxShadow.OffsetX, boxShadow.OffsetY);

                        StreamGeometry outerGeometry = new StreamGeometry();
                        using (StreamGeometryContext ctx = outerGeometry.Open())
                        {
                            DrawingHelper.GenerateRoundedRectangleGeometry(ctx, outerRect, outerRadii);
                        }
                        outerGeometry.Freeze();

                        if (!MathHelper.IsZero(boxShadow.BlurRadius))
                        {
                            DrawingVisual outerShadowDv = new DrawingVisual();
                            using (var outerShadowDvContext = outerShadowDv.RenderOpen())
                            {
                                outerShadowDvContext.DrawGeometry(boxShadow.Brush, null, outerGeometry);
                            }
                            outerShadowDv.Effect = new BlurEffect()
                            {
                                Radius = boxShadow.BlurRadius,
                                KernelType = boxShadow.BlurKernelType,
                                RenderingBias = boxShadow.BlurRenderingBias
                            };
                            VisualBrush outerShadowVb = new VisualBrush();
                            outerShadowVb.Visual = outerShadowDv;
                            Rect outerShadowRect = DrawingHelper.InflateRect(outerRect, new Thickness(boxShadow.BlurRadius));
                            RectangleGeometry outerShadowRectGeometry = new RectangleGeometry(outerShadowRect);
                            PathGeometry shadowGeometry = CombinedGeometry.Combine(outerShadowRectGeometry, innerGeometry, GeometryCombineMode.Exclude, null);
                            dc.DrawGeometry(outerShadowVb, null, shadowGeometry);
                        }
                        else //当模糊度为0时，直接绘制
                        {
                            PathGeometry shadowGeometry = CombinedGeometry.Combine(outerGeometry, innerGeometry, GeometryCombineMode.Exclude, null);
                            dc.DrawGeometry(boxShadow.Brush, null, shadowGeometry);
                        }
                    }
                }
            }
        }

        

        

    }
}
