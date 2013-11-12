using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFXSample.Windows;
using XnaControls.Classes;
using XnaControls.Controls;
using XnaGameObjects.Classes;
using XnaGameObjects.Managers;

namespace WPFXSample
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        private ObjectManager ObjectManager;
        private LoadManager LoadManager;
        private TileManager TileManager;
        private CameraObject Camera;
        private ContentBuilder contentBuilder;
        private ContentManager contentManager;
        private Level TestLevel;
        private LayerInfo ActiveLayer;
        private Boolean IsMouseLeftButtonDown = false;
        private Boolean IsMouseRightButtonDown = false;
        private System.Windows.Point LastGridLocation;

        private Size _NewBrushSize = new Size(2,2);
        public Size NewBrushSize
        {
            get { return _NewBrushSize; }
            set { _NewBrushSize = value; NotifyPropertyChanged("NewBrushSize"); }
        }

        private UIBrush _ActiveBrush;
        public UIBrush ActiveBrush
        {
            get { return _ActiveBrush; }
            set { _ActiveBrush = value; NotifyPropertyChanged("ActiveBrush"); }
        }

        private UILevel _Level;
        public UILevel Level
        {
            get { return _Level; }
            set { _Level = value; NotifyPropertyChanged("Level"); }
        }

        private Vector2 _MousePosition = Vector2.Zero;
        public Vector2 MousePosition
        {
            get { return _MousePosition; }
            set { _MousePosition = value; NotifyPropertyChanged("MousePosition"); }
        }

        private UITileSet _TilePalette;
        public UITileSet TilePalette
        {
            get { return _TilePalette; }
            set { _TilePalette = value; NotifyPropertyChanged("TilePalette"); }
        }

        private UITile _SelectedTile;
        public UITile SelectedTile
        {
            get { return _SelectedTile; }
            set { _SelectedTile = value; NotifyPropertyChanged("SelectedTile"); }
        }

        private ToolType _ActiveToolType;
        public ToolType ActiveToolType
        {
            get { return _ActiveToolType; }
            set { _ActiveToolType = value; NotifyPropertyChanged("ActiveToolType"); }
        }

        private GridInfo _GridInfo;
        public GridInfo GridInfo
        {
            get { return _GridInfo; }
            set { _GridInfo = value; NotifyPropertyChanged("GridInfo"); }
        }

        private ObservableCollection<TileControl> _LevelItems = new ObservableCollection<TileControl>();
        public ObservableCollection<TileControl> LevelItems
        {
            get { return _LevelItems; }
            set { _LevelItems = value; NotifyPropertyChanged("LevelItems"); }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            TestLevel = new Level();
        }

        #region Loading
        /// <summary>
        /// Handler for the MainWindow_Loaded event. Occurs after the window has loaded but before it is first displayed
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">Additional arguments</param>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Xna.Framework.Input.Mouse.WindowHandle = new WindowInteropHelper(this).Handle;
            InitializeContentPipeline();
            InitializeManagers();
            InitializeLevel();
            InitializeGrid();
            LoadDefaultTiles();
        }

        /// <summary>
        /// Initializes an empty level with the default size and tile size and sets the scale to 1
        /// </summary>
        private void InitializeLevel()
        {
            Level = new UILevel()
            {
                Scale = 1.0,
                LevelSize = new Size(Properties.Settings.Default.GridWidth, Properties.Settings.Default.GridHeight),
                TileSize = new Size(Properties.Settings.Default.TileWidth, Properties.Settings.Default.TileHeight)
            };
        }

        /// <summary>
        /// Initializes the grid overlay with the appropriate default values from the settings table
        /// </summary>
        private void InitializeGrid()
        {
            GridInfo = new GridInfo()
            {
                GridColor = Properties.Settings.Default.GridColor,
                LineThickness = Properties.Settings.Default.GridLineThickness,
                Opacity = Properties.Settings.Default.GridOpacity,
                LevelSize = new Size(Properties.Settings.Default.GridWidth, Properties.Settings.Default.GridHeight),
                TileSize = new Size(Properties.Settings.Default.TileWidth, Properties.Settings.Default.TileHeight)
            };
        }

        /// <summary>
        /// Initializes the various managers used by the level editor
        /// </summary>
        private void InitializeManagers()
        {
            ObjectManager = new ObjectManager();
            LoadManager = new LoadManager(contentManager, GraphicsDeviceService.Instance.GraphicsDevice);
            LoadManager.LoadFromXML("C:/DirtWare/Game Project One/WPFXSample/Files/Settings.xml");
            ObjectManager.GetLoadedObjects();
            TileManager = new TileManager();
        }

        /// <summary>
        /// Initializes the XNA Content pipeline
        /// </summary>
        private void InitializeContentPipeline()
        {
            contentBuilder = new ContentBuilder();
            ServiceContainer s = new ServiceContainer();

            GraphicsDeviceService graphicsDeviceService = GraphicsDeviceService.AddRef(new WindowInteropHelper(this).Handle);

            s.AddService<IGraphicsDeviceService>(graphicsDeviceService);
            contentManager = new ContentManager(s, contentBuilder.OutputDirectory);
        }
        #endregion Loading

        //void xControl_XNAMouseMove(object sender, XnaControls.Controls.XNAMouseMoveEventArgs e)
        //{
        //    System.Windows.Point p = (new System.Windows.Point((int)e.Position.X, (int)e.Position.Y));
        //    MousePosition = e.Position - Camera.Position;
        //}


        /// <summary>
        /// Adds the specified resource to the Content Manager
        /// </summary>
        /// <param name="Path">The location of the content file</param>
        /// <param name="Name">The resource name</param>
        private Boolean AddContent(String Path, String Name)
        {
            contentBuilder.Add(Path, Name);
            string buildError = contentBuilder.Build();

            if (String.IsNullOrEmpty(buildError) == false)
            {
                MessageBox.Show(buildError, "Error");
                return false;
            }

            return true;
        }

        #region Tileset Functions
        //KEEP THIS
        private void LoadDefaultTiles()
        {
            String Path = Properties.Settings.Default.TileSetPath;
            String Key = Properties.Settings.Default.TileSetKey;

            if (AddContent(Path, Key))
            {
                Texture2D texture = contentManager.Load<Texture2D>(Key);

                Tileset Tileset = new Tileset()
                {
                    ResourceName = Key.Split('.')[0],
                    ResourceFilePath = Path,
                    Position = Vector2.Zero,
                    Rotation = 0,
                    Size = new Vector2(texture.Width, texture.Height),
                    Sprite = new SpriteBatch(GraphicsDeviceService.Instance.GraphicsDevice),
                    Texture = texture,
                    TileCount = new Vector2((float)Properties.Settings.Default.TileSetSizeX, (float)Properties.Settings.Default.TileSetSizeY),
                    TileSize = new Vector2((float)Properties.Settings.Default.TileWidth, (float)Properties.Settings.Default.TileHeight)
                };

                Tileset.LoadResources(contentManager, GraphicsDeviceService.Instance.GraphicsDevice);
                SetActiveTileSet(Tileset);
                //t.DialogOk += TileProperties_DialogOk;
                //t.ShowDialog();
            }
        }

        private void LoadTileset(String Path, String Key)
        {
            if (AddContent(Path,Key))
            {
                Texture2D texture = contentManager.Load<Texture2D>(Key);
                TilesetPropertiesWindow t = new TilesetPropertiesWindow();

                t.Tileset = new Tileset()
                {
                    ResourceName = Key.Split('.')[0],
                    ResourceFilePath = Path,
                    Position = Vector2.Zero,
                    Rotation = 0,
                    Size = new Vector2(texture.Width, texture.Height),
                    Sprite = new SpriteBatch(GraphicsDeviceService.Instance.GraphicsDevice),
                    Texture = texture,
                    TileCount = new Vector2(1, 1),
                    TileSize = new Vector2(texture.Width, texture.Height),
                };
                t.Tileset.LoadResources(contentManager, GraphicsDeviceService.Instance.GraphicsDevice);
                t.DialogOk += TileProperties_DialogOk;
                t.ShowDialog();
            }
        }

        private void LoadTileset(List<String> Paths, List<String> Keys)
        {
            Cursor = Cursors.Wait;

            foreach (String s in Paths)
            {
                LoadTileset(s, Keys[Paths.IndexOf(s)]);
            }
            Cursor = Cursors.Arrow;
        }
        #endregion Tileset Functions

        

        #region Brush/Tile Selection
        /// <summary>
        /// Event handler for the Tile Palette control. This occurs whenever the selected tile changes.
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">Additional arguments</param>
        private void TilePaletteControl_SelectedTileChanged(object sender, RoutedEventArgs e)
        {
            TilePalette t = sender as TilePalette;
            SelectedTile = t.Selected;
        }

        #endregion Brush/Tile Selection

        #region MouseHandlers
        /// <summary>
        /// Event handler for the Level Objects control. Occurs whenever the Left Mouse button is pressed
        /// </summary>
        /// <param name="sender">The object which raised the event</param>
        /// <param name="e">Additional arguments</param>
        private void LevelObjects_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseLeftButtonDown)
                return;
            IsMouseLeftButtonDown = true;

            if (SelectedTile == null)
                return;

            System.Windows.Point p = e.GetPosition(LevelControl);
            System.Windows.Point CurrentGridLocation = new System.Windows.Point((int)(p.X / TilePalette.TileWidth), (int)(p.Y / TilePalette.TileHeight));
            System.Windows.Point GridPosition = new System.Windows.Point(CurrentGridLocation.X * TilePalette.TileWidth, CurrentGridLocation.Y * TilePalette.TileHeight);

            if (ActiveToolType == ToolType.Pencil)
                DrawTile(p);
            else if (ActiveToolType == ToolType.Eraser)
                EraseTile(GridPosition);

            LastGridLocation = new System.Windows.Point((int)(p.X / TilePalette.TileWidth), (int)(p.Y / TilePalette.TileHeight));
        }

        /// <summary>
        /// Event handler for the LevelObjects control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LevelControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseLeftButtonDown = false;
        }

        private void LevelControl_MouseMove(object sender, MouseEventArgs e)
        {
            ///TODO: This is broken at the moment. For some reason this event is getting fired all over the place. Like dozens of times
            ///      in rapid succession.
            if (IsMouseLeftButtonDown == false || e.Handled == true)
                return;

            System.Windows.Point p = e.GetPosition(LevelControl);
            System.Windows.Point CurrentGridLocation = new System.Windows.Point((int)(p.X / TilePalette.TileWidth), (int)(p.Y / TilePalette.TileHeight));
            System.Windows.Point GridPosition = new System.Windows.Point(CurrentGridLocation.X * TilePalette.TileWidth, CurrentGridLocation.Y * TilePalette.TileHeight);
            if (LastGridLocation.X == CurrentGridLocation.X && LastGridLocation.Y == CurrentGridLocation.Y)
                return;


            if (ActiveToolType == ToolType.Pencil)
                DrawTile(e.GetPosition(LevelControl));
            //else if(ActiveToolType == ToolType.Brush)
            ///TODO: Write Brush functionality
            else if (ActiveToolType == ToolType.Eraser)
                EraseTile(GridPosition);
            else
                return;
            e.Handled = true;
        }
        #endregion MouseHandlers

        #region Drawing
        private void DrawTile(System.Windows.Point Position)
        {
            UITile ExistingTile = Level.ActiveLayer.IsCellOccupied(Position);
            if (ExistingTile != null)
            {
                Level.ActiveLayer.RemoveTile(Level.ActiveLayer.GetCollocatedTileControl(Position));
            }
            UITile NewTile = SelectedTile.Clone() as UITile;
            NewTile.TileSize = SelectedTile.TileSize;
            System.Windows.Point p = Position;
            System.Windows.Point Pos = new System.Windows.Point((int)(((int)(p.X / (TilePalette.TileWidth))) * (TilePalette.TileWidth)), (int)(((int)(p.Y / (TilePalette.TileHeight))) * (TilePalette.TileHeight)));
            NewTile.Position = Pos;
            TileControl t = new TileControl() { DataContext = NewTile };
            Canvas.SetLeft(t, NewTile.Position.X);
            Canvas.SetTop(t, NewTile.Position.Y);
            Level.ActiveLayer.AddTile(t);
        }

        private void EraseTile(System.Windows.Point Position)
        {
            Level.ActiveLayer.RemoveTile(Position);
        }
        #endregion Drawing

        #region LayerNavigation
        private void LayerNavigator_VisibleLayersChanged(object sender, RoutedEventArgs e)
        {
            LayerNavigator Sender = sender as LayerNavigator;
        }

        private void TilePaletteControl_ActiveToolTypeChanged(object sender, RoutedEventArgs e)
        {
            TilePalette t = sender as TilePalette;
            ActiveToolType = t.ActiveToolType;
            //Set active brush here.
        }
        #endregion LayerNavigation

        #region Zooming
        private void TilePaletteControl_ZoomInClicked(object sender, RoutedEventArgs e)
        {
            Level.Scale *= 1.15;
        }

        private void TilePaletteControl_ZoomOutClicked(object sender, RoutedEventArgs e)
        {
            Level.Scale *= 0.85;
        }
        #endregion Zooming

        #region DialogHandlers
        void fileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            OpenFileDialog Sender = sender as OpenFileDialog;

            if (Sender != null && Sender.FileNames != null)
                LoadTileset(new List<String>(Sender.FileNames), new List<String>(Sender.SafeFileNames));
        }

        private void NewLevelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LevelPropertiesWindow LevelProperties = new LevelPropertiesWindow()
            {
                LevelWidth = (int)Properties.Settings.Default.GridWidth,
                LevelHeight = (int)Properties.Settings.Default.GridHeight,
                TileWidth = (int)Properties.Settings.Default.TileWidth,
                TileHeight = (int)Properties.Settings.Default.TileHeight
            };

            //Attach to OnOk event
            LevelProperties.LevelPropertiesChanged += LevelProperties_LevelPropertiesChanged;
            LevelProperties.ShowDialog();
        }

        void LevelProperties_LevelPropertiesChanged(object sender, LevelPropertiesChangedEventArgs e)
        {
            if (e.Cancel)
                return;
            this.Level = new UILevel(new Size(e.TileWidth, e.TileHeight), new Size(e.LevelWidth, e.LevelHeight));
            Level.ActiveLayer = Level.Layers[0];
            Level.Scale = 1.0;
            Level.BackgroundColor = Properties.Settings.Default.LevelBackgroundColor;

            this.GridInfo.LevelSize = new Size(e.LevelWidth, e.LevelHeight);
            this.GridInfo.TileSize = new Size(e.TileWidth, e.TileHeight);
        }

        /// <summary>
        /// Event handler for the File Menu. Occurs when the Load Tileset menu item is clicked
        /// </summary>
        /// <param name="sender">The object which raised the event</param>
        /// <param name="e">Additional arguments</param>
        private void LoadTilesetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = System.IO.Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = System.IO.Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Tileset";

            fileDialog.Filter = "PNG Files(*.png)|*.png;|" +
                                "BMP Files (*.bmp)|*.bmp|" +
                                "JPEG Files (*.jpg)|*.jpg|" +
                                "All Files (*.*)|*.*";

            fileDialog.FileOk += fileDialog_FileOk;

            fileDialog.ShowDialog();
        }

        /// <summary>
        /// Event handler for the Tile Properties Dialog. This occurs when the tile properties dialog is closed
        /// </summary>
        /// <param name="sender">The object which raised the event</param>
        /// <param name="e">Additional arguments</param>
        void TileProperties_DialogOk(object sender, DialogOkEventArgs e)
        {
            //If the cancel button was pressed then return.
            if (e.Cancel)
                return;
            SetActiveTileSet(e.Set);
        }

        public void SetActiveTileSet(Tileset Tiles)
        {
            //
            BitmapImage img = new BitmapImage(new Uri(Tiles.ResourceFilePath));
            TilePalette = new UITileSet()
            {
                Image = img,
                TileCountX = (int)Tiles.TileCount.X,
                TileCountY = (int)Tiles.TileCount.Y,
                TileHeight = Tiles.TileSize.Y,
                TileWidth = Tiles.TileSize.X,
            };

            TilePalette.GenerateId();
            TilePalette.GenerateTiles();

            Tiles.Id = TilePalette.Id;

            TileManager.AddTileset(TilePalette.Id, Tiles);
            Level = Level ?? new UILevel();
            Level.TileSize = new Size(Tiles.TileSize.X, Tiles.TileSize.Y);
            Level.ActiveLayer = Level.Layers[0];
            Level.Scale = 1.0;

            GridInfo = new GridInfo()
            {
                LevelSize = Level.LevelSize,
                TileSize = Level.TileSize,
                GridColor = Properties.Settings.Default.GridColor,
                Opacity = Properties.Settings.Default.GridOpacity,
                LineThickness = Properties.Settings.Default.GridLineThickness
            };
        }
        #endregion DialogHandlers

    }
}
