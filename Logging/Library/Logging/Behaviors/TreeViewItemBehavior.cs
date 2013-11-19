using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Logging.Behaviors
{
    /// <summary>
    /// TreeViewItemBehavior is used to specify if TreeViewItems will be brought into view when they are selected.
    /// </summary>
	public static class TreeViewItemBehavior
	{
        /// <summary>
        /// Expose and associate this behavior with TreeViewItem
        /// </summary>
		public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty = DependencyProperty.RegisterAttached("IsBroughtIntoViewWhenSelected", typeof(Boolean), typeof(TreeViewItemBehavior), new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));

        /// <summary>
        /// Gets a value indicating whether the item will be brought into view when selected
        /// </summary>
        /// <param name="treeViewItem">The targeted TreeViewItem</param>
        /// <returns>Whether the item will be brought into view when selected</returns>
		public static Boolean GetIsBroughtIntoViewWhenSelected(TreeViewItem treeViewItem)
		{
			return (Boolean)treeViewItem.GetValue(IsBroughtIntoViewWhenSelectedProperty);
		}

        /// <summary>
        /// Sets a value indicating whether the item will be brought into view when selected
        /// </summary>
        /// <param name="treeViewItem">The targeted TreeViewItem</param>
        /// <param name="value">Whether the item will be brought into view when selected</param>
		public static void SetIsBroughtIntoViewWhenSelected(TreeViewItem treeViewItem, Boolean value)
		{
			treeViewItem.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
		}

        /// <summary>
        /// Assigns or unassigns the behavior to the TreeViewItem when the IsBroughtIntoViewWhenSelected property is changed 
        /// </summary>
        /// <param name="d">The targeted TreeViewItem</param>
        /// <param name="e">Additional arguments related to this event</param>
		static void OnIsBroughtIntoViewWhenSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeViewItem item = d as TreeViewItem;
			if (item == null)
				return;

			if (!(e.NewValue is Boolean))
				return;

			if ((Boolean)e.NewValue)
				item.Selected += OnTreeViewItemSelected;
			else
				item.Selected -= OnTreeViewItemSelected;
		}

        /// <summary>
        /// Responsible for bringing the TreeViewItem into view when it is selected
        /// </summary>
        /// <param name="sender">The TreeViewItem which has been selected</param>
        /// <param name="e">Additional arguments associated with this event</param>
		static void OnTreeViewItemSelected(object sender, RoutedEventArgs e)
		{
			if(object.ReferenceEquals(sender, e.OriginalSource))
				return;

			TreeViewItem item = e.OriginalSource as TreeViewItem;
			if(item != null)
				item.BringIntoView();

		}
	}
}
