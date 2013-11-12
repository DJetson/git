using GPOne.Interfaces;
using GPOne.Objects;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace GPOne.Controls
{
    /// <summary>
    /// Interaction logic for WorldControl.xaml
    /// </summary>
    public partial class WorldControl : UserControl
    {
        public WorldControl()
        {
            InitializeComponent();
        }

        private void UserControl_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            WorldObject World = ((sender as UserControl).DataContext as WorldObject);

            World.ProcessInput(sender, e);

            foreach (IReceivesInput Item in World.InputList)
                Item.OnKeyDown(sender, e);
        }

        private void UserControl_PreviewKeyUp_1(object sender, KeyEventArgs e)
        {
            WorldObject World = ((sender as UserControl).DataContext as WorldObject);

            foreach (IReceivesInput Item in World.InputList)
                Item.OnKeyUp(sender, e);
        }

        private void UserControl_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            Focus();
        }

        private void UserControl_LostFocus_1(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void UserControl_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Focus();
        }
    }
}
