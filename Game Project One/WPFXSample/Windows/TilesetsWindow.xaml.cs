using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using System.Windows.Interop;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XnaControls.Classes;
using XnaGameObjects.Classes;
using XnaGameObjects.Managers;

namespace WPFXSample.Windows
{
    /// <summary>
    /// Interaction logic for TilesetsWindow.xaml
    /// </summary>
    public partial class TilesetsWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        //private Dictionary<String, SpriteBatch> _Sprites = new Dictionary<string, SpriteBatch>();
        //public Dictionary<String, SpriteBatch> Sprites
        //{
        //    get { return _Sprites; }
        //}
        private Tile _Selected;
        public Tile Selected
        {
            get { return _Selected; }
            set { _Selected = value; NotifyPropertyChanged("Selected"); }
        }
            
        private Microsoft.Xna.Framework.Rectangle WindowBounds;

        public TilesetsWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeSprites();
            TileManager.TilesetsChanged += TileManager_TilesetsChanged;
            Loaded += TilesetsWindow_Loaded;
        }

        void TilesetsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GraphicsDeviceService.Instance.DeviceCreated += Instance_DeviceCreated;
            Microsoft.Xna.Framework.Input.Mouse.WindowHandle = new WindowInteropHelper(this).Handle;
        }

        void Instance_DeviceCreated(object sender, EventArgs e)
        {
            GraphicsDeviceService service = sender as GraphicsDeviceService;
            service.GraphicsDevice.Clear(Color.WhiteSmoke);
        }

        void TileManager_TilesetsChanged(object sender, TilesetsChangedEventArgs e)
        {
            //if(e.Operation == ManagerOperation.Added)
            //    _Sprites.Add(e.Key, new SpriteBatch(GraphicsDeviceService.Instance.GraphicsDevice));

            //else if(e.Operation == ManagerOperation.Removed)
            //    _Sprites.Remove(e.Key);

            CalculateWindowSize();
        }

        private void CalculateWindowSize()
        {
            if (TileManager.Sets == null)
                return;

            WindowBounds = new Microsoft.Xna.Framework.Rectangle(0,0,0,0);
            foreach (Tileset t in TileManager.Sets.Values)
            {
                float Left = t.Texture.Bounds.Left;
                float Top = t.Texture.Bounds.Top;
                float Right = t.Texture.Bounds.Right;
                float Bottom = t.Texture.Bounds.Bottom;

                if (Left < WindowBounds.Left)
                    WindowBounds.Location = new Microsoft.Xna.Framework.Point((int)Left,WindowBounds.Location.Y);
                if (Top < WindowBounds.Top)
                    WindowBounds.Location = new Microsoft.Xna.Framework.Point(WindowBounds.Location.X, (int)Top);
                if (Right > WindowBounds.Right)
                    WindowBounds.Width = (int)Right - WindowBounds.Left;
                if (Bottom > WindowBounds.Bottom)
                    WindowBounds.Height = (int)Bottom - WindowBounds.Top;
            }

            xControl.Width = WindowBounds.Width;
            xControl.Height = WindowBounds.Height;
        }

        private void InitializeSprites()
        {
        }

        private void XnaControl_Draw(object sender, RoutedEventArgs e)
        {
            //foreach (KeyValuePair<String,SpriteBatch> s in Sprites)
            foreach(Tileset t in TileManager.Sets.Values)
            {
                //String key = s.Key;
                //SpriteBatch val = s.Value as SpriteBatch;
                //Microsoft.Xna.Framework.Rectangle dest = TileManager.Sets[key].Texture.Bounds;
                t.Draw();
                //t.Sprite.Begin();
                //t..Draw(TileManager.Sets[key].Texture, dest, Color.CornflowerBlue);
                //val.End();
            }
        }

        private void XnaControl_XNAMouseMove(object sender, XnaControls.Controls.XNAMouseMoveEventArgs e)
        {
            
        }

        private void xControl_XNAMouseLeftButtonDown(object sender, XnaControls.Controls.XNAMouseButtonEventArgs e)
        {
            Vector2 MousePosition = e.Position + CameraObject.Camera.Position;
            
        }
    }
}
