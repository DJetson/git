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

namespace XnaControls.Controls
{
    /// <summary>
    /// Interaction logic for LevelGridControl.xaml
    /// </summary>
    public partial class LevelGridControl : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion

        #region Dependency Properties
        /// <summary>
        /// Level Size measured in Tiles
        /// </summary>
        private static readonly DependencyProperty LevelSizeProperty = DependencyProperty.Register("LevelSize", typeof(Size), typeof(LevelGridControl), new PropertyMetadata(LevelSizeChanged));
        public Size LevelSize
        {
            get { return (Size)GetValue(LevelSizeProperty); }
            set { SetValue(LevelSizeProperty, value); }
        }

        private static void LevelSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelGridControl GridControl = d as LevelGridControl;
            RecalculateGrid(GridControl);
        }


        /// <summary>
        /// The width and height of a single tile
        /// </summary>
        private static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register("TileSize", typeof(Size), typeof(LevelGridControl),new PropertyMetadata(TileSizeChanged));
        public Size TileSize
        {
            get { return (Size)GetValue(TileSizeProperty); }
            set { SetValue(TileSizeProperty, value); }
        }

        private static void TileSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelGridControl GridControl = d as LevelGridControl;
            RecalculateGrid(GridControl);
        }

        /// <summary>
        /// The color of the gridlines
        /// </summary>
        private static readonly DependencyProperty GridColorProperty = DependencyProperty.Register("GridColor", typeof(Color), typeof(LevelGridControl), new PropertyMetadata(GridColorChanged));
        public Color GridColor
        {
            get { return (Color)GetValue(GridColorProperty); }
            set { SetValue(GridColorProperty, value); }
        }

        private static void GridColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelGridControl GridControl = d as LevelGridControl;
            RecalculateGrid(GridControl);
        }

        /// <summary>
        /// The opacity of the Grid
        /// </summary>
        private static readonly DependencyProperty GridOpacityProperty = DependencyProperty.Register("GridOpacity", typeof(float), typeof(LevelGridControl), new PropertyMetadata(GridOpacityChanged));
        public float GridOpacity
        {
            get { return (float)GetValue(GridOpacityProperty); }
            set { SetValue(GridOpacityProperty, value); }
        }

        private static void GridOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelGridControl GridControl = d as LevelGridControl;
            RecalculateGrid(GridControl);
        }

        /// <summary>
        /// The thickness of the individual lines in the grid
        /// </summary>
        private static readonly DependencyProperty GridLineThicknessProperty = DependencyProperty.Register("GridLineThickness", typeof(double), typeof(LevelGridControl), new PropertyMetadata(GridLineThicknessChanged));
        public double GridLineThickness
        {
            get { return (double)GetValue(GridLineThicknessProperty); }
            set { SetValue(GridLineThicknessProperty, value); }
        }

        private static void GridLineThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelGridControl GridControl = d as LevelGridControl;
            RecalculateGrid(GridControl);
        }
        #endregion Dependency Properties

        #region Properties
        private ObservableCollection<Line> _GridLines;
        public ObservableCollection<Line> GridLines
        {
            get { return _GridLines; }
            set { _GridLines = value; NotifyPropertyChanged("GridLines"); }
        }

        private double _LevelWidth;
        public double LevelWidth
        {
            get { return _LevelWidth; }
            set { _LevelWidth = value; NotifyPropertyChanged("LevelWidth"); }
        }

        private double _LevelHeight;
        public double LevelHeight
        {
            get { return _LevelHeight; }
            set { _LevelHeight = value; NotifyPropertyChanged("LevelHeight"); }
        }

        #endregion Properties

        public LevelGridControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// Generates grid lines based on the LevelSize and TileSize values
        /// </summary>
        private static void RecalculateGrid(LevelGridControl GridControl)
        {
            GridControl.GridLines = GridControl.GridLines ?? new ObservableCollection<Line>();
            GridControl.GridLines.Clear();


            double VerticalLineLength = GridControl.LevelSize.Height * GridControl.TileSize.Height;
            double HorizontalLineLength = GridControl.LevelSize.Width * GridControl.TileSize.Width;

            GridControl.LevelWidth = HorizontalLineLength;
            GridControl.LevelHeight = VerticalLineLength;

            ///Nothing to calculate, so just return
            if (HorizontalLineLength == 0 || VerticalLineLength == 0)
                return;

            Color LineColor = new Color()
            {
                ScA = GridControl.GridOpacity,
                ScR = GridControl.GridColor.ScR,
                ScG = GridControl.GridColor.ScG,
                ScB = GridControl.GridColor.ScB,
            };

            ///Generate Vertical Lines
            for (int x = 0; x <= GridControl.LevelSize.Width * GridControl.TileSize.Width; x += (int)GridControl.TileSize.Width)
            {
                Line NewLine = new Line()
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = VerticalLineLength,
                    Stroke = new SolidColorBrush(LineColor),
                    Fill = new SolidColorBrush(LineColor),
                    StrokeThickness = GridControl.GridLineThickness,
                };

                GridControl.GridLines.Add(NewLine);
            }

            ///Generate Horizontal Lines
            for (int y = 0; y <= GridControl.LevelSize.Height * GridControl.TileSize.Height; y += (int)GridControl.TileSize.Height)
            {
                Line NewLine = new Line()
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = HorizontalLineLength,
                    Y2 = y,
                    Stroke = new SolidColorBrush(LineColor),
                    Fill = new SolidColorBrush(LineColor),
                    StrokeThickness = GridControl.GridLineThickness,
                };

                GridControl.GridLines.Add(NewLine);
            }
        }
    }
}
