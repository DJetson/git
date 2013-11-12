using System;
using System.Collections.Generic;
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
    /// Interaction logic for BrushMap.xaml
    /// </summary>
    public partial class BrushMap : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion
        //private readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(LevelGridControl), typeof(BrushMap));
        //public LevelGridControl Content
        //{
        //    get { return (LevelGridControl)GetValue(ContentProperty); }
        //    set { SetValue(ContentProperty, value); }
        //}

        //private readonly DependencyProperty BrushHeightProperty = DependencyProperty.Register("BrushHeight", typeof(int), typeof(BrushMap));
        //public int BrushHeight
        //{
        //    get { return (int)GetValue(BrushHeightProperty); }
        //    set { SetValue(BrushHeightProperty, value); }
        //}

        //private readonly DependencyProperty BrushWidthProperty = DependencyProperty.Register("BrushWidth", typeof(int), typeof(BrushMap));
        //public int BrushWidth
        //{
        //    get { return (int)GetValue(BrushWidthProperty); }
        //    set { SetValue(BrushWidthProperty, value); }
        //}

        private readonly DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(double), typeof(BrushMap), new PropertyMetadata(1.0));
        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }

        private readonly DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(double), typeof(BrushMap), new PropertyMetadata(1.0));
        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }


        private UILayer _BrushTiles = new UILayer();
        public UILayer BrushTiles
        {
            get { return _BrushTiles; }
            set { _BrushTiles = value; NotifyPropertyChanged("BrushTiles"); }
        }

        private readonly DependencyProperty BrushSizeProperty = DependencyProperty.Register("BrushSize", typeof(Size), typeof(BrushMap));
        public Size BrushSize
        {
            get { return (Size)GetValue(BrushSizeProperty); }
            set { SetValue(BrushSizeProperty, value); }
        }

        private readonly DependencyProperty TileSizeProperty = DependencyProperty.Register("TileSize", typeof(Size), typeof(BrushMap));
        public Size TileSize
        {
            get { return (Size)GetValue(TileSizeProperty); }
            set { SetValue(TileSizeProperty, value); }
        }

        //private readonly DependencyProperty TileWidthProperty = DependencyProperty.Register("TileWidth", typeof(int), typeof(BrushMap));
        //public int TileWidth
        //{
        //    get { return (int)GetValue(TileWidthProperty); }
        //    set { SetValue(TileWidthProperty, value); }
        //}

        //private readonly DependencyProperty TileHeightProperty = DependencyProperty.Register("TileHeight", typeof(int), typeof(BrushMap));
        //public int TileHeight
        //{
        //    get { return (int)GetValue(TileHeightProperty); }
        //    set { SetValue(TileHeightProperty, value); }
        //}

        private readonly DependencyProperty GridColorProperty = DependencyProperty.Register("GridColor", typeof(Color), typeof(BrushMap));
        public Color GridColor
        {
            get { return (Color)GetValue(GridColorProperty); }
            set { SetValue(GridColorProperty, value); }
        }

        private readonly DependencyProperty GridOpacityProperty = DependencyProperty.Register("GridOpacity", typeof(double), typeof(BrushMap));
        public double GridOpacity
        {
            get { return (double)GetValue(GridOpacityProperty); }
            set { SetValue(GridOpacityProperty, value); }
        }

        private readonly DependencyProperty GridLineThicknessProperty = DependencyProperty.Register("GridLineThickness", typeof(double), typeof(BrushMap));
        public double GridLineThickness
        {
            get { return (double)GetValue(GridLineThicknessProperty); }
            set { SetValue(GridLineThicknessProperty, value); }
        }

        //private GridInfo _BrushMapGrid;

        public BrushMap()
        {
            InitializeComponent();
        }

        private void LevelGridControl_Drop(object sender, DragEventArgs e)
        {
            UITile t = e.Data.GetData(typeof(UITile)) as UITile;

            if (t == null)
                return;
            UITile NewTile = t.Copy;

            Point p = e.GetPosition(this);
            Point CurrentGridLocation = new Point((int)(p.X / TileSize.Width), (int)(p.Y / TileSize.Height));
            Point GridPosition = new Point(CurrentGridLocation.X * TileSize.Width, CurrentGridLocation.Y * TileSize.Height);
            NewTile.Position = GridPosition;

            BrushTiles.AddTile(NewTile);
//            DrawTile(t, p);
        }

        private void DrawTile(UITile SelectedTile, Point Position)
        {
            UITile ExistingTile = BrushTiles.IsCellOccupied(Position);
            if (ExistingTile != null)
            {
                BrushTiles.RemoveTile(BrushTiles.GetCollocatedTileControl(Position));
            }
            UITile NewTile = SelectedTile.Clone() as UITile;
            NewTile.TileSize = SelectedTile.TileSize;
            System.Windows.Point p = Position;
            System.Windows.Point Pos = new System.Windows.Point((int)(((int)(p.X / (TileSize.Width))) * (TileSize.Width)), (int)(((int)(p.Y / (TileSize.Height))) * (TileSize.Height)));
            NewTile.Position = Pos;
            TileControl t = new TileControl() { DataContext = NewTile };
            //Canvas.SetLeft(t, NewTile.Position.X);
            //Canvas.SetTop(t, NewTile.Position.Y);
            BrushTiles.AddTile(t);
        }

        private void BrushControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (IsMouseLeftButtonDown)
            //    return;
            //IsMouseLeftButtonDown = true;

            //if (SelectedTile == null)
            //    return;

            //System.Windows.Point p = e.GetPosition(LevelControl);
            //System.Windows.Point CurrentGridLocation = new System.Windows.Point((int)(p.X / TilePalette.TileWidth), (int)(p.Y / TilePalette.TileHeight));
            //System.Windows.Point GridPosition = new System.Windows.Point(CurrentGridLocation.X * TilePalette.TileWidth, CurrentGridLocation.Y * TilePalette.TileHeight);

            //if (ActiveToolType == ToolType.Pencil)
            //    DrawTile(p);
            //else if (ActiveToolType == ToolType.Eraser)
            //    EraseTile(GridPosition);

            //LastGridLocation = new System.Windows.Point((int)(p.X / TilePalette.TileWidth), (int)(p.Y / TilePalette.TileHeight));
        }
    }
}
