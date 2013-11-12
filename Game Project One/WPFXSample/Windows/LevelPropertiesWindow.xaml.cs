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
using System.Windows.Shapes;

namespace WPFXSample.Windows
{
    public class LevelPropertiesChangedEventArgs : EventArgs
    {
        public int LevelWidth { get; set; }
        public int LevelHeight { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public Boolean Cancel { get; set; }
    }

    /// <summary>
    /// Interaction logic for LevelPropertiesWindow.xaml
    /// </summary>
    public partial class LevelPropertiesWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region LevelPropertiesWindow Events
        public event EventHandler<LevelPropertiesChangedEventArgs> LevelPropertiesChanged;

        public void OnLevelPropertiesChanged(LevelPropertiesChangedEventArgs e)
        {
            EventHandler<LevelPropertiesChangedEventArgs> handler = LevelPropertiesChanged;
            if (handler != null) { handler(this, e); }
        }

        public void RaiseLevelPropertiesChangedEvent(Boolean cancel = false)
        {
            OnLevelPropertiesChanged(new LevelPropertiesChangedEventArgs()
            {
                LevelHeight = this.LevelHeight,
                LevelWidth = this.LevelWidth,
                TileHeight = this.TileHeight,
                TileWidth = this.TileWidth,
                Cancel = cancel
            });
        }
        #endregion LevelPropertiesWindow Events

        #region LevelPropertiesWindow Properties
        private int _LevelWidth;
        public int LevelWidth
        {
            get { return _LevelWidth; }
            set { _LevelWidth = value; NotifyPropertyChanged("LevelWidth"); }
        }

        private int _LevelHeight;
        public int LevelHeight
        {
            get { return _LevelHeight; }
            set { _LevelHeight = value; NotifyPropertyChanged("LevelHeight"); }
        }

        private int _TileWidth;
        public int TileWidth
        {
            get { return _TileWidth; }
            set { _TileWidth = value; NotifyPropertyChanged("TileWidth"); }
        }

        private int _TileHeight;
        public int TileHeight
        {
            get { return _TileHeight; }
            set { _TileHeight = value; NotifyPropertyChanged("TileHeight"); }
        }
        #endregion LevelPropertiesWindow Properties

        public LevelPropertiesWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseLevelPropertiesChangedEvent();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseLevelPropertiesChangedEvent(true);
            Close();
        }
    }
}
