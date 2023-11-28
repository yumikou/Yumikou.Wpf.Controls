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
    /// use <see cref="BoxShadows" /> set shadow
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
        protected override Geometry? GetLayoutClip(Size layoutSlotSize) // Bug fixed: Before net46, the UseLayoutRound=true would cause ClipToBounds=false to fail
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
            if (boxShadows is not null && boxShadows.Count > 0)
            {
                CornerRadius cornerRadius = CornerRadius;
                Thickness borders = BorderThickness;
                Rect borderOuterRect = new Rect(this.RenderSize);
#if NET46_OR_GREATER || NETCOREAPP
                if (this.UseLayoutRounding)
                {
                    DpiScale dpi = LayoutHelper.GetDpi(this);
                    borders = new Thickness(LayoutHelper.RoundLayoutValue(borders.Left, dpi.DpiScaleX), LayoutHelper.RoundLayoutValue(borders.Top, dpi.DpiScaleY),
                       LayoutHelper.RoundLayoutValue(borders.Right, dpi.DpiScaleX), LayoutHelper.RoundLayoutValue(borders.Bottom, dpi.DpiScaleY));
                }
#endif
                Rect borderInnerRect = DrawingHelper.DeflateRect(borderOuterRect, borders);
                Radii borderOuterRadii = new Radii(cornerRadius, borders, true);
                Radii borderInnerRadii = new Radii(cornerRadius, borders, false);
                StreamGeometry? borderOuterGeometry = null;
                StreamGeometry? borderInnerGeometry = null;
                foreach (BoxShadow boxShadow in boxShadows)
                {
                    if (boxShadow.Brush is not null 
                        && ((boxShadow.IsInset && !MathHelper.IsZero(borderInnerRect.Width) && !MathHelper.IsZero(borderInnerRect.Height)) 
                            || (!boxShadow.IsInset && !MathHelper.IsZero(borderOuterRect.Width) && !MathHelper.IsZero(borderOuterRect.Height))))
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
                        if (boxShadow.IsInset)
                        {
                            if (borderInnerGeometry is null)
                            {
                                borderInnerGeometry = new StreamGeometry();
                                using (StreamGeometryContext ctx = borderInnerGeometry.Open())
                                {
                                    DrawingHelper.GenerateRoundedRectangleGeometry(ctx, borderInnerRect, borderInnerRadii);
                                }
                                borderInnerGeometry.Freeze();
                            }

                            Radii insetRadii = new Radii(borderInnerRadii, spreadBorders, false, 1);
                            Rect insetRect = DrawingHelper.DeflateRect(borderInnerRect, spreadBorders, boxShadow.OffsetX, boxShadow.OffsetY);
                            StreamGeometry insetGeometry = new StreamGeometry();
                            using (StreamGeometryContext ctx = insetGeometry.Open())
                            {
                                DrawingHelper.GenerateRoundedRectangleGeometry(ctx, insetRect, insetRadii);
                            }
                            insetGeometry.Freeze();

                            //if (!MathHelper.IsZero(boxShadow.BlurRadius)) //TODO: inset blur effect, may need to customize the effect
                            //{

                            //}
                            //else
                            //{
                            PathGeometry shadowGeometry = CombinedGeometry.Combine(borderInnerGeometry, insetGeometry, GeometryCombineMode.Exclude, null);
                            dc.DrawGeometry(boxShadow.Brush, null, shadowGeometry);
                            //}
                        }
                        else
                        {
                            if (borderOuterGeometry is null)
                            {
                                borderOuterGeometry = new StreamGeometry();
                                using (StreamGeometryContext ctx = borderOuterGeometry.Open())
                                {
                                    DrawingHelper.GenerateRoundedRectangleGeometry(ctx, borderOuterRect, borderOuterRadii);
                                }
                                borderOuterGeometry.Freeze();
                            }

                            Radii outerRadii = new Radii(borderOuterRadii, spreadBorders, true, 1);
                            Rect outerRect = DrawingHelper.InflateRect(borderOuterRect, spreadBorders, boxShadow.OffsetX, boxShadow.OffsetY);
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
                                PathGeometry shadowGeometry = CombinedGeometry.Combine(outerShadowRectGeometry, borderOuterGeometry, GeometryCombineMode.Exclude, null);
                                dc.DrawGeometry(outerShadowVb, null, shadowGeometry);
                            }
                            else
                            {
                                PathGeometry shadowGeometry = CombinedGeometry.Combine(outerGeometry, borderOuterGeometry, GeometryCombineMode.Exclude, null);
                                dc.DrawGeometry(boxShadow.Brush, null, shadowGeometry);
                            }
                        }
                    }
                }
            }
        }

        

        

    }
}
