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
    /// Interaction logic for ProjectileControl.xaml
    /// </summary>
    public partial class ProjectileControl : GameControl
    {
        public ProjectileControl()
        {
            DataContextChanged += ProjectileControl_DataContextChanged;
            InitializeComponent();
        }

        void ProjectileControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (DataContext as ProjectileObject).Control = this;
        }
    }
}
