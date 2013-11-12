using Microsoft.Xna.Framework;
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
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XnaControls.Controls;
using XnaGameObjects.Classes;

namespace WPFXSample.Windows
{

    public class TilesetChangedEventArgs : EventArgs
    {
        public Tileset OldSet { get; set; }
        public Tileset NewSet { get; set; }
    }

    public class DialogOkEventArgs : EventArgs
    {
        public Tileset Set { get; set; }
        public Boolean Cancel { get; set; }
    }

    /// <summary>
    /// Interaction logic for TilesetPropertiesWindow.xaml
    /// </summary>
    public partial class TilesetPropertiesWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region TilesetPropertiesWindow Events
        public event EventHandler<TilesetChangedEventArgs> TilesetChanged;

        public void OnTilesetChanged(TilesetChangedEventArgs e)
        {
            EventHandler<TilesetChangedEventArgs> handler = TilesetChanged;
            if (handler != null) { handler(this, e); }
        }

        public void RaiseTilesetChangedEvent(Tileset oldset)
        {
            OnTilesetChanged(new TilesetChangedEventArgs() { OldSet = oldset, NewSet = Tileset });
        }

        public event EventHandler<DialogOkEventArgs> DialogOk;

        public void OnDialogOk(DialogOkEventArgs e)
        {
            EventHandler<DialogOkEventArgs> handler = DialogOk;
            if (handler != null) { handler(this, e); }
        }

        public void RaiseDialogOkEvent(Boolean cancel = false)
        {
            OnDialogOk(new DialogOkEventArgs()
            {
                Set = Tileset,
                Cancel = cancel
            });
        }
        #endregion TilesetPropertiesWindow Events

        #region TilesetPropertiesWindow Properties
        private Tileset _Tileset;
        public Tileset Tileset
        {
            get { return _Tileset; }
            set
            {
                Tileset old = _Tileset;
                _Tileset = value;
                NotifyPropertyChanged("Tileset");
                RaiseTilesetChangedEvent(old);
            }
        }

        private int _TileCountX = 1;
        public int TileCountX
        {
            get { return _TileCountX; }
            set { _TileCountX = value; NotifyPropertyChanged("TileCountX"); }
        }

        private int _TileCountY = 1;
        public int TileCountY
        {
            get { return _TileCountY; }
            set { _TileCountY = value; NotifyPropertyChanged("TileCountY"); }
        }

        private int _TileSizeX = 32;
        public int TileSizeX
        {
            get { return _TileSizeX; }
            set { _TileSizeX = value; NotifyPropertyChanged("TileSizeX"); }
        }

        private int _TileSizeY = 32;
        public int TileSizeY
        {
            get { return _TileSizeY; }
            set { _TileSizeY = value; NotifyPropertyChanged("TileSizeY"); }
        }
        #endregion TilesetPropertiesWindow Properties

        public TilesetPropertiesWindow()
        {
            DataContext = this;
            InitializeComponent();
            TilesetChanged += TilesetPropertiesWindow_TilesetChanged;
        }

        void TilesetPropertiesWindow_TilesetChanged(object sender, TilesetChangedEventArgs e)
        {
            //if (e.OldSet != null)
            //    e.OldSet.TileSizeChanged -= Tileset_TileSizeChanged;

            //Tileset.TileSizeChanged += Tileset_TileSizeChanged;

            ResizeWindow();
        }

        void ResizeWindow()
        {
            if (Tileset.Texture != null)
            {
                XControl.Width = Tileset.Texture.Width;
                XControl.Height = Tileset.Texture.Height;
            }
            Measure(new Size(XControl.ActualWidth, XControl.ActualHeight));
        }

        void Tileset_TileSizeChanged(object sender, TileSizeChangedEventArgs e)
        {
        }

        private void XnaControl_Draw(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                XnaControl Sender = sender as XnaControl;
                Sender.GraphicsDevice.Clear(Color.CornflowerBlue);
                Tileset.Draw();
            }
        }

        private void TextCountX_LostFocus(object sender, RoutedEventArgs e)
        {
            NumberBox t = sender as NumberBox;

            if (Tileset != null)
                Tileset.TileCount = new Vector2(t.Value, Tileset.TileCount.Y);
        }

        private void TextCountY_LostFocus(object sender, RoutedEventArgs e)
        {
            NumberBox t = sender as NumberBox;

            if (Tileset != null)
                Tileset.TileCount = new Vector2(Tileset.TileCount.X, t.Value);
        }

        private void TextSizeX_LostFocus(object sender, RoutedEventArgs e)
        {
            NumberBox t = sender as NumberBox;

            if (Tileset != null)
                Tileset.TileSize = new Vector2(Tileset.TileSize.X, t.Value);
        }

        private void TextSizeY_LostFocus(object sender, RoutedEventArgs e)
        {
            NumberBox t = sender as NumberBox;

            if (Tileset != null)
                Tileset.TileSize = new Vector2(t.Value, Tileset.TileSize.Y);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseDialogOkEvent();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseDialogOkEvent(true);
            this.Close();
        }
    }
}
