using System.Windows;
using System.Windows.Controls;

namespace AoBSigmaker.Controls
{
    /// <summary>
    /// A wrap panel which can apply a margin to each child item.
    /// </summary>
    public class ItemMarginStackPanel : StackPanel
    {
        /// <summary>
        /// ItemMargin static DP.
        /// </summary>
        public static readonly DependencyProperty s_itemMarginProperty =
            DependencyProperty.Register(
            "ItemMargin",
            typeof(Thickness),
            typeof(ItemMarginStackPanel),
            new FrameworkPropertyMetadata(
                new Thickness(),
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The margin that will be applied to each Item in the wrap panel.
        /// </summary>
        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(s_itemMarginProperty);
            set => SetValue(s_itemMarginProperty, value);
        }

        /// <summary>
        /// Overridden. Sets item margins before calling base implementation.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            RefreshItemMargin();

            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// Overridden. Sets item margins before calling base implementation.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            RefreshItemMargin();

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// Refresh the child item margins.
        /// </summary>
        private void RefreshItemMargin()
        {
            UIElementCollection children = InternalChildren;
            for (int i = 0, count = children.Count; i < count; i++)
            {
                FrameworkElement? ele = children[i] as FrameworkElement;
                if (ele is { })
                {
                    ele.Margin = ItemMargin;
                }
            }
        }
    }
}
