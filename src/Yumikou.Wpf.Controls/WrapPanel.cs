using MS.Internal;
using System.ComponentModel;

using System.Diagnostics;
using System.Reflection;
using System.Windows.Threading;

using System.Windows.Media;
using System;
using Yumikou.Wpf.Controls.Helpers;
using System.Windows.Controls;
using System.Windows;
using System.Data;

namespace Yumikou.Wpf.Controls
{
    /// <summary>
    /// WrapPanel is used to place child UIElements at sequential positions from left to the right
    /// and then "wrap" the lines of children from top to the bottom.
    /// 
    /// All children get the layout partition of size ItemWidth x ItemHeight.
    /// 
    /// Use HorizontalSpacing and VerticalSpacing set children spacing
    /// </summary>
    public class WrapPanel : Panel
    {
        //-------------------------------------------------------------------
        //
        //  Constructors
        //
        //-------------------------------------------------------------------

        #region Constructors
        static WrapPanel()
        {
        }

        /// <summary>
        ///     Default constructor
        /// </summary>
        public WrapPanel() : base()
        {
            _orientation = Orientation.Horizontal;
        }

        #endregion

        //-------------------------------------------------------------------
        //
        //  Public Methods
        //
        //-------------------------------------------------------------------

        #region Public Methods


        #endregion

        //-------------------------------------------------------------------
        //
        //  Public Properties + Dependency Properties's
        //
        //-------------------------------------------------------------------

        #region Public Properties

        private static bool IsWidthHeightValid(object value)
        {
            double v = (double)value;
            return (MathHelper.IsNaN(v)) || (v >= 0.0d && !Double.IsPositiveInfinity(v));
        }

        /// <summary>
        /// DependencyProperty for <see cref="ItemWidth" /> property.
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
                DependencyProperty.Register(
                        "ItemWidth",
                        typeof(double),
                        typeof(WrapPanel),
                        new FrameworkPropertyMetadata(
                                Double.NaN,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsWidthHeightValid));

        /// <summary>
        /// The ItemWidth and ItemHeight properties specify the size of all items in the WrapPanel. 
        /// Note that children of 
        /// WrapPanel may have their own Width/Height properties set - the ItemWidth/ItemHeight 
        /// specifies the size of "layout partition" reserved by WrapPanel for the child.
        /// If this property is not set (or set to "Auto" in markup or Double.NaN in code) - the size of layout
        /// partition is equal to DesiredSize of the child element.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }


        /// <summary>
        /// DependencyProperty for <see cref="ItemHeight" /> property.
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
                DependencyProperty.Register(
                        "ItemHeight",
                        typeof(double),
                        typeof(WrapPanel),
                        new FrameworkPropertyMetadata(
                                Double.NaN,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsWidthHeightValid));


        /// <summary>
        /// The ItemWidth and ItemHeight properties specify the size of all items in the WrapPanel. 
        /// Note that children of 
        /// WrapPanel may have their own Width/Height properties set - the ItemWidth/ItemHeight 
        /// specifies the size of "layout partition" reserved by WrapPanel for the child.
        /// If this property is not set (or set to "Auto" in markup or Double.NaN in code) - the size of layout
        /// partition is equal to DesiredSize of the child element.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        private static bool IsSpacingValid(object value)
        {
            double v = (double)value;
            return (MathHelper.IsNaN(v)) || (!Double.IsNegativeInfinity(v) && !Double.IsPositiveInfinity(v));
        }

        public static readonly DependencyProperty HorizontalSpacingProperty =
                DependencyProperty.Register(
                        "HorizontalSpacing",
                        typeof(double),
                        typeof(WrapPanel),
                        new FrameworkPropertyMetadata(
                                Double.NaN,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsSpacingValid));

        [TypeConverter(typeof(LengthConverter))]
        public double HorizontalSpacing
        {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set { SetValue(HorizontalSpacingProperty, value); }
        }

        public static readonly DependencyProperty VerticalSpacingProperty =
                DependencyProperty.Register(
                        "VerticalSpacing",
                        typeof(double),
                        typeof(WrapPanel),
                        new FrameworkPropertyMetadata(
                                Double.NaN,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsSpacingValid));

        [TypeConverter(typeof(LengthConverter))]
        public double VerticalSpacing
        {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set { SetValue(VerticalSpacingProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="Orientation" /> property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
                StackPanel.OrientationProperty.AddOwner(
                        typeof(WrapPanel),
                        new FrameworkPropertyMetadata(
                                Orientation.Horizontal,
                                FrameworkPropertyMetadataOptions.AffectsMeasure,
                                new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Specifies dimension of children positioning in absence of wrapping.
        /// Wrapping occurs in orthogonal direction. For example, if Orientation is Horizontal, 
        /// the items try to form horizontal rows first and if needed are wrapped and form vertical stack of rows.
        /// If Orientation is Vertical, items first positioned in a vertical column, and if there is
        /// not enough space - wrapping creates additional columns in horizontal dimension.
        /// </summary>
        public Orientation Orientation
        {
            get { return _orientation; }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// <see cref="PropertyMetadata.PropertyChangedCallback"/>
        /// </summary>
        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WrapPanel p = (WrapPanel)d;
            p._orientation = (Orientation)e.NewValue;
        }

        private Orientation _orientation;

        #endregion

        //-------------------------------------------------------------------
        //
        //  Protected Methods
        //
        //-------------------------------------------------------------------

        #region Protected Methods

        private struct UVSize
        {
            internal UVSize(Orientation orientation, double width, double height)
            {
                U = V = 0d;
                _orientation = orientation;
                Width = width;
                Height = height;
            }

            internal UVSize(Orientation orientation)
            {
                U = V = 0d;
                _orientation = orientation;
            }

            internal double U;
            internal double V;
            private Orientation _orientation;

            internal double Width
            {
                get { return (_orientation == Orientation.Horizontal ? U : V); }
                set { if (_orientation == Orientation.Horizontal) U = value; else V = value; }
            }
            internal double Height
            {
                get { return (_orientation == Orientation.Horizontal ? V : U); }
                set { if (_orientation == Orientation.Horizontal) V = value; else U = value; }
            }
        }


        /// <summary>
        /// <see cref="FrameworkElement.MeasureOverride"/>
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            UVSize curLineSize = new UVSize(Orientation);
            UVSize panelSize = new UVSize(Orientation);
            UVSize uvConstraint = new UVSize(Orientation, constraint.Width, constraint.Height);
            double uSpacing = Orientation == Orientation.Horizontal ? HorizontalSpacing : VerticalSpacing;
            double vSpacing = Orientation == Orientation.Horizontal ? VerticalSpacing : HorizontalSpacing;
            bool uSpacingSet = !MathHelper.IsNaN(uSpacing);
            bool vSpacingSet = !MathHelper.IsNaN(vSpacing);

            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            bool itemWidthSet = !MathHelper.IsNaN(itemWidth);
            bool itemHeightSet = !MathHelper.IsNaN(itemHeight);

            Size childConstraint = new Size(
                (itemWidthSet ? itemWidth : constraint.Width),
                (itemHeightSet ? itemHeight : constraint.Height));

            UIElementCollection children = InternalChildren;

            bool isFlushLineCache = true;
            for (int i = 0, count = children.Count; i < count; i++)
            {
                UIElement child = children[i] as UIElement;
                if (child == null) continue;

                //Flow passes its own constrint to children
                child.Measure(childConstraint);

                //this is the size of the child in UV space
                UVSize sz = new UVSize(
                    Orientation,
                    (itemWidthSet ? itemWidth : child.DesiredSize.Width),
                    (itemHeightSet ? itemHeight : child.DesiredSize.Height));

                if (i > 0 && uSpacingSet && !isFlushLineCache)
                {
                    if (MathHelper.GreaterThan(curLineSize.U + uSpacing, uvConstraint.U)) //需要换行
                    {
                        panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                        panelSize.V += curLineSize.V;
                        if (vSpacingSet)
                        {
                            panelSize.V += vSpacing;
                        }
                        curLineSize = new UVSize(Orientation); //通过uspace开的新行，不考虑额外换行的问题，即使uSpace大于限定宽度，也不处理，且换新一行时，上一个uspace不需要再计算
                        isFlushLineCache = true;
                    }
                    else
                    {
                        curLineSize.U += uSpacing;
                        curLineSize.V = Math.Max(0d, curLineSize.V);
                    }
                }

                if (MathHelper.GreaterThan(curLineSize.U + sz.U, uvConstraint.U)) //need to switch to another line
                {
                    if (!isFlushLineCache)
                    {
                        panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                        panelSize.V += curLineSize.V;
                        if (vSpacingSet)
                        {
                            panelSize.V += vSpacing;
                        }
                    }

                    curLineSize = sz;
                    isFlushLineCache = false;

                    if (MathHelper.GreaterThan(sz.U, uvConstraint.U)) //the element is wider then the constrint - give it a separate line                    
                    {
                        panelSize.U = Math.Max(sz.U, panelSize.U);
                        panelSize.V += sz.V;
                        if (vSpacingSet && i < (count - 1)) //如果当前已经是最后一个数据，则不需要再添加vspace
                        {
                            panelSize.V += vSpacing;
                        }
                        isFlushLineCache = true;
                        curLineSize = new UVSize(Orientation);
                    }
                }
                else //continue to accumulate a line
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                    isFlushLineCache = false;
                }
            }

            //the last line size, if any should be added
            panelSize.U = Math.Max(curLineSize.U, panelSize.U);
            panelSize.V += curLineSize.V;

            //go from UV space to W/H space
            return new Size(panelSize.Width, panelSize.Height);

        }

        /// <summary>
        /// <see cref="FrameworkElement.ArrangeOverride"/>
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double uSpacing = Orientation == Orientation.Horizontal ? HorizontalSpacing : VerticalSpacing;
            double vSpacing = Orientation == Orientation.Horizontal ? VerticalSpacing : HorizontalSpacing;
            bool uSpacingSet = !MathHelper.IsNaN(uSpacing);
            bool vSpacingSet = !MathHelper.IsNaN(vSpacing);

            int firstInLine = 0;
            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            double accumulatedV = 0;
            double itemU = (Orientation == Orientation.Horizontal ? itemWidth : itemHeight);
            UVSize curLineSize = new UVSize(Orientation);
            UVSize uvFinalSize = new UVSize(Orientation, finalSize.Width, finalSize.Height);
            bool itemWidthSet = !MathHelper.IsNaN(itemWidth);
            bool itemHeightSet = !MathHelper.IsNaN(itemHeight);
            bool useItemU = (Orientation == Orientation.Horizontal ? itemWidthSet : itemHeightSet);

            UIElementCollection children = InternalChildren;
            bool isFlushLineCache = true;
            for (int i = 0, count = children.Count; i < count; i++)
            {
                UIElement child = children[i] as UIElement;
                if (child == null) continue;

                UVSize sz = new UVSize(
                    Orientation,
                    (itemWidthSet ? itemWidth : child.DesiredSize.Width),
                    (itemHeightSet ? itemHeight : child.DesiredSize.Height));

                if (i > 0 && uSpacingSet && !isFlushLineCache)
                {
                    if (MathHelper.GreaterThan(curLineSize.U + uSpacing, uvFinalSize.U)) //需要换行
                    {
                        arrangeLine(accumulatedV, curLineSize.V, firstInLine, i, useItemU, itemU, uSpacingSet, uSpacing); //绘制上一行
                        accumulatedV += curLineSize.V;
                        if (vSpacingSet)
                        {
                            accumulatedV += vSpacing;
                        }
                        curLineSize = new UVSize(Orientation); //通过uspace开的新行，不考虑额外换行的问题，即使uSpace大于限定宽度，也不处理，且换新一行时，上一个uspace不需要再计算
                        firstInLine = i;
                        isFlushLineCache = true;
                    }
                    else
                    {
                        curLineSize.U += uSpacing;
                        curLineSize.V = Math.Max(0d, curLineSize.V);
                    }
                }

                if (MathHelper.GreaterThan(curLineSize.U + sz.U, uvFinalSize.U)) //need to switch to another line
                {
                    if (!isFlushLineCache)
                    {
                        arrangeLine(accumulatedV, curLineSize.V, firstInLine, i, useItemU, itemU, uSpacingSet, uSpacing); //绘制上一行

                        accumulatedV += curLineSize.V;
                        if (vSpacingSet) //只要缓冲行里有数据
                        {
                            accumulatedV += vSpacing;
                        }
                        firstInLine = i;
                    }
                    curLineSize = sz;
                    isFlushLineCache = false;

                    if (MathHelper.GreaterThan(sz.U, uvFinalSize.U)) //the element is wider then the constraint - give it a separate line                    
                    {
                        //switch to next line which only contain one element
                        arrangeLine(accumulatedV, sz.V, firstInLine, ++firstInLine, useItemU, itemU, uSpacingSet, uSpacing); //绘制当前数据

                        accumulatedV += sz.V;
                        if (vSpacingSet && i < (count - 1)) //如果当前已经是最后一个数据，则不需要再添加vspace
                        {
                            accumulatedV += vSpacing;
                        }
                        curLineSize = new UVSize(Orientation);
                        isFlushLineCache = true;
                    }
                }
                else //continue to accumulate a line
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                    isFlushLineCache = false;
                }
            }

            //arrange the last line, if any
            if (firstInLine < children.Count)
            {
                arrangeLine(accumulatedV, curLineSize.V, firstInLine, children.Count, useItemU, itemU, uSpacingSet, uSpacing);
            }

            return finalSize;

        }

        private void arrangeLine(double v, double lineV, int start, int end, bool useItemU, double itemU, bool useSpacingU, double spacingU)
        {
            double u = 0;
            bool isHorizontal = (Orientation == Orientation.Horizontal);

            UIElementCollection children = InternalChildren;
            for (int i = start; i < end; i++)
            {
                UIElement child = children[i] as UIElement;
                if (child != null)
                {
                    UVSize childSize = new UVSize(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);
                    double layoutSlotU = (useItemU ? itemU : childSize.U);
                    child.Arrange(new Rect(
                        (isHorizontal ? u : v),
                        (isHorizontal ? v : u),
                        (isHorizontal ? layoutSlotU : lineV),
                        (isHorizontal ? lineV : layoutSlotU)));
                    u += layoutSlotU;
                    if (useSpacingU && i < (end - 1))
                    {
                        u += spacingU;
                    }
                }
            }
        }

        #endregion Protected Methods
    }
}