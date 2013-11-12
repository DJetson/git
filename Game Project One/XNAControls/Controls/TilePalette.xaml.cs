using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for TilePalette.xaml
    /// </summary>
    public partial class TilePalette : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region TilePalette Events
        public static readonly RoutedEvent SelectedTileChangedEvent = EventManager.RegisterRoutedEvent("SelectedTileChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TilePalette));

        public event RoutedEventHandler SelectedTileChanged
        {
            add { AddHandler(SelectedTileChangedEvent, value); }
            remove { RemoveHandler(SelectedTileChangedEvent, value); }
        }

        void RaiseSelectedTileChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(TilePalette.SelectedTileChangedEvent,this);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent ActiveToolTypeChangedEvent = EventManager.RegisterRoutedEvent("ActiveToolTypeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TilePalette));

        public event RoutedEventHandler ActiveToolTypeChanged
        {
            add { AddHandler(ActiveToolTypeChangedEvent, value); }
            remove { RemoveHandler(ActiveToolTypeChangedEvent, value); }
        }

        void RaiseActiveToolTypeChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(TilePalette.ActiveToolTypeChangedEvent, this);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent ZoomInClickedEvent = EventManager.RegisterRoutedEvent("ZoomInClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TilePalette));

        public event RoutedEventHandler ZoomInClicked
        {
            add { AddHandler(ZoomInClickedEvent, value); }
            remove { RemoveHandler(ZoomInClickedEvent, value); }
        }

        void RaiseZoomInClickedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(TilePalette.ZoomInClickedEvent);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent ZoomOutClickedEvent = EventManager.RegisterRoutedEvent("ZoomOutClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TilePalette));

        public event RoutedEventHandler ZoomOutClicked
        {
            add { AddHandler(ZoomOutClickedEvent, value); }
            remove { RemoveHandler(ZoomOutClickedEvent, value); }
        }

        void RaiseZoomOutClickedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(TilePalette.ZoomOutClickedEvent);
            RaiseEvent(newEventArgs);
        }
        #endregion TilePalette Events

        #region TilePalette Properties
        private UITile _Selected;
        public UITile Selected
        {
            get { return _Selected; }
            set { _Selected = value; NotifyPropertyChanged("Selected"); }
        }

        private ToolType _ActiveToolType = ToolType.Pencil;
        public ToolType ActiveToolType
        {
            get { return _ActiveToolType; }
            set { _ActiveToolType = value; NotifyPropertyChanged("ActiveToolType"); }
        }

        #endregion TilePalette Properties

        private Boolean IsMouseLeftButtonDown = false;
        private Point MouseDownPosition;
        private TileControl ClickedTile;

        public TilePalette()
        {
            InitializeComponent();
        }

        private void TileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseSelectedTileChangedEvent();
        }

        private void LevelToolBar_ActiveToolTypeChanged(object sender, RoutedEventArgs e)
        {
            LevelToolBar ToolBar = sender as LevelToolBar;
            ActiveToolType = ToolBar.ActiveToolType;
            RaiseActiveToolTypeChangedEvent();
        }

        private void LevelToolBar_ZoomInClicked(object sender, RoutedEventArgs e)
        {
            RaiseZoomInClickedEvent();
        }

        private void LevelToolBar_ZoomOutClicked(object sender, RoutedEventArgs e)
        {
            RaiseZoomOutClickedEvent();
        }

        private void TileList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClickedTile = sender as TileControl;

            if (ClickedTile == null)
                return;

            IsMouseLeftButtonDown = true;
            MouseDownPosition = e.GetPosition(this);
        }

        private void TileList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseLeftButtonDown = false;
            ClickedTile = null;
        }

        private void TileList_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseLeftButtonDown)
            {
                UITile SelectedTile = (sender as TileControl).DataContext as UITile;

                if (Math.Abs(MouseDownPosition.X - e.GetPosition(this).X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(MouseDownPosition.Y - e.GetPosition(this).Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DragDrop.DoDragDrop(sender as TileControl, SelectedTile, DragDropEffects.Copy);
                }
            }
        }


    }
}
