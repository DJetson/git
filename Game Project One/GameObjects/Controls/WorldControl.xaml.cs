using GameObjects.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameObjects.Controls
{
    /// <summary>
    /// Interaction logic for WorldControl.xaml
    /// </summary>
    public partial class WorldControl : UserControl
    {
        public WorldControl()
        {
            InitializeComponent();
            DataContextChanged += WorldControl_DataContextChanged;
        }

        void WorldControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is WorldObject)
            {
                (DataContext as WorldObject).Control = this;
            }
        }

        private void UserControl_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            WorldObject World = ((sender as UserControl).DataContext as WorldObject);

            World.ProcessInput(sender, e);

            //foreach (IReceivesInput Item in World.InputList)
            //    Item.OnKeyDown(sender, e);
        }

        private void UserControl_PreviewKeyUp_1(object sender, KeyEventArgs e)
        {
            WorldObject World = ((sender as UserControl).DataContext as WorldObject);

            //foreach (IReceivesInput Item in World.InputList)
            //    Item.OnKeyUp(sender, e);
        }

        //private void UserControl_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    Focus();
        //}

        //private void UserControl_LostFocus_1(object sender, System.Windows.RoutedEventArgs e)
        //{
        //}

        private void UserControl_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
        {
            //Focus();
        }
    }
}
