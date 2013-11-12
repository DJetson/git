using GPOne.BaseClasses;
using GPOne.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GPOne.Controls
{
    /// <summary>
    /// Interaction logic for SensoryInfo.xaml
    /// </summary>
    public partial class SensoryInfo : GameObjectBase, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        //private Vector _Forward;
        public Vector Forward
        {
            get
            {
                if (DataContext != null && DataContext is IPositionInfo)
                {
                    IPositionInfo Target = DataContext as IPositionInfo;
                    return new Vector(Target.Position.Current.X + 100, Target.Position.Current.Y + 100); 
                }
                else
                    return new Vector(0, 0);

            }
        }

        public Vector StartPoint
        {
            get
            {
                if (DataContext != null && DataContext is CharacterBase)
                {
                    CharacterBase Target = DataContext as CharacterBase;
                    return new Vector(Target.CenterX, Target.CenterY);
                }
                return new Vector(0, 0);
            }
        }

        public SensoryInfo()
        {
            InitializeComponent();

            DataContextChanged += SensoryInfo_DataContextChanged;
        }

        void SensoryInfo_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null && DataContext is IPositionInfo)
            {
                IPositionInfo Target = DataContext as IPositionInfo;

                Target.Position.PropertyChanged += Position_PropertyChanged;
                Target.Velocity.PropertyChanged += Position_PropertyChanged;
            }
        }

        void Position_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Forward");
            NotifyPropertyChanged("StartPoint");
        }
    }
}
