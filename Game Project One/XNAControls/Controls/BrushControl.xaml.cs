using System;
using System.Collections.Generic;
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
    /// Interaction logic for BrushControl.xaml
    /// </summary>
    public partial class BrushControl : UserControl
    {
        private static readonly DependencyProperty BrushLayerProperty = DependencyProperty.Register("BrushLayer", typeof(UILayer), typeof(BrushControl));
        public UILayer BrushLayer
        {
            get { return (UILayer)GetValue(BrushLayerProperty); }
            set { SetValue(BrushLayerProperty, value); }
        }

        private static readonly DependencyProperty BrushTileWidthProperty = DependencyProperty.Register("BrushTileWidth", typeof(double), typeof(BrushControl));
        public double BrushTileWidth
        {
            get { return (double)GetValue(BrushTileWidthProperty); }
            set { SetValue(BrushTileWidthProperty, value); }
        }

        private static readonly DependencyProperty BrushTileHeightProperty = DependencyProperty.Register("BrushTileHeight", typeof(double), typeof(BrushControl));
        public double BrushTileHeight
        {
            get { return (double)GetValue(BrushTileHeightProperty); }
            set { SetValue(BrushTileHeightProperty, value); }
        }


        private static readonly DependencyProperty BrushTileColumnsProperty = DependencyProperty.Register("BrushTileColumns", typeof(double), typeof(BrushControl));
        public double BrushTileColumns
        {
            get { return (double)GetValue(BrushTileColumnsProperty); }
            set { SetValue(BrushTileColumnsProperty, value); }
        }

        private static readonly DependencyProperty BrushTileRowsProperty = DependencyProperty.Register("BrushTileRows", typeof(double), typeof(BrushControl));
        public double BrushTileRows
        {
            get { return (double)GetValue(BrushTileRowsProperty); }
            set { SetValue(BrushTileRowsProperty, value); }
        }

        public BrushControl()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
