﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Yumikou.Wpf.Controls
{
    /// <summary>
    /// TODO: 内阴影IsInset
    /// </summary>
    public class BoxShadow : Freezable
    {
        //public bool IsInset { get; set; } = false;

        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        public double Spread
        {
            get { return (double)GetValue(SpreadProperty); }
            set { SetValue(SpreadProperty, value); }
        }

        /// <summary>
        /// 模糊半径
        /// </summary>
        public double Blur
        {
            get { return (double)GetValue(BlurProperty); }
            set { SetValue(BlurProperty, value); }
        }

        public KernelType BlurKernelType
        {
            get { return (KernelType)GetValue(BlurKernelTypeProperty); }
            set { SetValue(BlurKernelTypeProperty, value); }
        }

        public RenderingBias BlurRenderingBias
        {
            get { return (RenderingBias)GetValue(BlurRenderingBiasProperty); }
            set { SetValue(BlurRenderingBiasProperty, value); }
        }

        public Brush? Brush
        {
            get { return (Brush?)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="OffsetX" /> property.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty
            = DependencyProperty.Register("OffsetX", typeof(double), typeof(BoxShadow),
                                          new PropertyMetadata(0.0));

        /// <summary>
        /// DependencyProperty for <see cref="OffsetY" /> property.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty
            = DependencyProperty.Register("OffsetY", typeof(double), typeof(BoxShadow),
                                          new PropertyMetadata(0.0));

        /// <summary>
        /// DependencyProperty for <see cref="Spread" /> property.
        /// </summary>
        public static readonly DependencyProperty SpreadProperty
            = DependencyProperty.Register("Spread", typeof(double), typeof(BoxShadow),
                                          new PropertyMetadata(0.0));

        /// <summary>
        /// DependencyProperty for <see cref="Blur" /> property.
        /// </summary>
        public static readonly DependencyProperty BlurProperty
            = DependencyProperty.Register("Blur", typeof(double), typeof(BoxShadow),
                                          new PropertyMetadata(0.0));

        /// <summary>
        /// DependencyProperty for <see cref="BlurKernelType" /> property.
        /// </summary>
        public static readonly DependencyProperty BlurKernelTypeProperty
            = DependencyProperty.Register("BlurKernelType", typeof(KernelType), typeof(BoxShadow),
                                          new PropertyMetadata(KernelType.Gaussian));

        /// <summary>
        /// DependencyProperty for <see cref="BlurRenderingBias" /> property.
        /// </summary>
        public static readonly DependencyProperty BlurRenderingBiasProperty
            = DependencyProperty.Register("BlurRenderingBias", typeof(RenderingBias), typeof(BoxShadow),
                                          new PropertyMetadata(RenderingBias.Performance));

        /// <summary>
        /// DependencyProperty for <see cref="Brush" /> property.
        /// </summary>
        public static readonly DependencyProperty BrushProperty
            = DependencyProperty.Register("Brush", typeof(Brush), typeof(BoxShadow),
                                          new FrameworkPropertyMetadata(null, OnBrushChanged));

        private static void OnBrushChanged(DependencyObject obj,
                        DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is Brush brush)
            {
                brush.Freeze();
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BoxShadow();
        }
    }
}
