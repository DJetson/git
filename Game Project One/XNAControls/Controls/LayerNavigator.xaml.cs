using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XnaControls.Classes;

namespace XnaControls.Controls
{



    /// <summary>
    /// Interaction logic for LayerNavigator.xaml
    /// </summary>
    public partial class LayerNavigator : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region LayerNavigator RoutedEvents

        #region ActiveLayerChanged Event
        public static readonly RoutedEvent ActiveLayerChangedEvent = EventManager.RegisterRoutedEvent("ActiveLayerChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LayerNavigator));

        public event RoutedEventHandler ActiveLayerChanged
        {
            add { AddHandler(ActiveLayerChangedEvent, value); }
            remove { RemoveHandler(ActiveLayerChangedEvent, value); }
        }

        private void RaiseActiveLayerChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(LayerNavigator.ActiveLayerChangedEvent);
            RaiseEvent(newEventArgs);
        }
        #endregion ActiveLayerChanged Event

        #region VisibleLayersChanged Event
        public static readonly RoutedEvent VisibleLayersChangedEvent = EventManager.RegisterRoutedEvent("VisibleLayersChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LayerNavigator));

        public event RoutedEventHandler VisibleLayersChanged
        {
            add { AddHandler(VisibleLayersChangedEvent, value); }
            remove { RemoveHandler(VisibleLayersChangedEvent, value); }
        }

        private void RaiseVisibleLayersChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(LayerNavigator.VisibleLayersChangedEvent);
            RaiseEvent(newEventArgs);
        }
        #endregion VisibleLayersChanged Event

        #endregion LayerNavigator RoutedEvents

        #region LayerNavigator Properties
        //private ObservableCollection<LayerInfo> _LayerCollection = new ObservableCollection<LayerInfo>();
        //public ObservableCollection<LayerInfo> LayerCollection
        //{
        //    get { return _LayerCollection; }
        //    set { _LayerCollection = value; NotifyPropertyChanged("LayerCollection"); }
        //}

        //private LayerInfo _ActiveLayer;
        //public LayerInfo ActiveLayer
        //{
        //    get { return _ActiveLayer; }
        //    set { _ActiveLayer = value; NotifyPropertyChanged("ActiveLayer"); }
        //}
        #endregion LayerNavigator Properties

        public LayerNavigator()
        {
            InitializeComponent();
            this.DataContextChanged += LayerNavigator_DataContextChanged;
        }

        private void LayerNavigator_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is UILevel == false)
                return;

            InitializeLayerNavigator();
        }

        private void InitializeLayerNavigator()
        {
            UILevel Level = DataContext as UILevel;

            Level.ActiveLayer = Level.Layers[0];
            Level.ActiveLayer.LayerInfo.IsActive = true;
        }

        private void IsActiveButton_Checked(object sender, RoutedEventArgs e)
        {
            UILevel Level = DataContext as UILevel;
            Level.ActiveLayer.LayerInfo.IsActive = false;
            UILayer NewActiveLayer = (sender as ToggleButton).DataContext as UILayer;
            Level.ActiveLayer = NewActiveLayer;
        }
    }
}
