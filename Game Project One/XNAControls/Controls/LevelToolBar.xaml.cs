using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace XnaControls.Controls
{
    public enum ToolType { Eraser, Pencil, Brush };

    /// <summary>
    /// Interaction logic for LevelToolBar.xaml
    /// </summary>
    public partial class LevelToolBar : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region RoutedEvents
        public static readonly RoutedEvent ActiveToolTypeChangedEvent = EventManager.RegisterRoutedEvent("ActiveToolTypeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LevelToolBar));

        public event RoutedEventHandler ActiveToolTypeChanged
        {
            add { AddHandler(ActiveToolTypeChangedEvent, value); }
            remove { RemoveHandler(ActiveToolTypeChangedEvent, value); }
        }

        void RaiseActiveToolTypeChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(LevelToolBar.ActiveToolTypeChangedEvent);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent ZoomInClickedEvent = EventManager.RegisterRoutedEvent("ZoomInClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LevelToolBar));

        public event RoutedEventHandler ZoomInClicked
        {
            add { AddHandler(ZoomInClickedEvent, value); }
            remove { RemoveHandler(ZoomInClickedEvent, value); }
        }

        void RaiseZoomInClickedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(LevelToolBar.ZoomInClickedEvent);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent ZoomOutClickedEvent = EventManager.RegisterRoutedEvent("ZoomOutClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LevelToolBar));

        public event RoutedEventHandler ZoomOutClicked
        {
            add { AddHandler(ZoomOutClickedEvent, value); }
            remove { RemoveHandler(ZoomOutClickedEvent, value); }
        }

        void RaiseZoomOutClickedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(LevelToolBar.ZoomOutClickedEvent);
            RaiseEvent(newEventArgs);
        }
        #endregion RoutedEvents

        #region LevelToolBar Dependency Properties

        private static readonly DependencyProperty ActiveToolTypeProperty = DependencyProperty.Register("ActiveToolType", typeof(ToolType), typeof(LevelToolBar));
        public ToolType ActiveToolType
        {
            get { return (ToolType)GetValue(ActiveToolTypeProperty); }
            set { SetValue(ActiveToolTypeProperty, value); }
        }

        //private Boolean _IsEraserActive;
        //public Boolean IsEraserActive
        //{
        //    get { return _IsEraserActive; }
        //    set { _IsEraserActive = value; NotifyPropertyChanged("IsEraserActive"); }
        //}

        //private Boolean _IsPencilActive;
        //public Boolean IsPencilActive
        //{
        //    get { return _IsPencilActive; }
        //    set { _IsPencilActive = value; NotifyPropertyChanged("IsPencilActive"); }
        //}

        //private Boolean _IsBrushActive;
        //public Boolean IsBrushActive
        //{
        //    get { return _IsBrushActive; }
        //    set { _IsBrushActive = value; NotifyPropertyChanged("IsBrushActive"); }
        //}

        //private ToolType _ToolType;
        //public ToolType ToolType
        //{
        //    get { return _ToolType; }
        //    set { _ToolType = value; NotifyPropertyChanged("ToolType"); }
        //}

        #endregion LevelToolBar Dependency Properties

        public LevelToolBar()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void BrushToggle_Checked(object sender, RoutedEventArgs e)
        {
            ActiveToolType = ToolType.Brush;
            RaiseActiveToolTypeChangedEvent();
        }

        private void PencilToggle_Checked(object sender, RoutedEventArgs e)
        {
            ActiveToolType= ToolType.Pencil;
            RaiseActiveToolTypeChangedEvent();
        }

        private void EraserToggler_Checked(object sender, RoutedEventArgs e)
        {
            ActiveToolType = ToolType.Eraser;
            RaiseActiveToolTypeChangedEvent();
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseZoomInClickedEvent();
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseZoomOutClickedEvent();
        }
    }
}
