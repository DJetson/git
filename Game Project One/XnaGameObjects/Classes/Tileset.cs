using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using XnaGameObjects.BaseClasses;

namespace XnaGameObjects.Classes
{

    public class TileSizeChangedEventArgs : EventArgs
    {
        public Vector2 OldSize { get; set; }
        public Vector2 NewSize { get; set; }
    }

    public class TileCountChangedEventArgs : EventArgs
    {
        public Vector2 OldCount { get; set; }
        public Vector2 NewCount { get; set; }
    }

    public class Tileset : VisualObject
    {
        #region Tileset Properties
        private Guid _Id;
        public Guid Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private String _ResourceFilePath;
        public String ResourceFilePath
        {
            get { return _ResourceFilePath; }
            set { _ResourceFilePath = value; }
        }

        private String _ResourceName;
        public String ResourceName
        {
            get { return _ResourceName; }
            set { _ResourceName = value; }
        }

        private Texture2D _Texture;
        public Texture2D Texture
        {
            get { return _Texture; }
            set { _Texture = value; }
        }

        private Vector2 _TileSize;
        public Vector2 TileSize
        {
            get { return _TileSize; }
            set
            {
                Vector2 OldSize = _TileSize;
                _TileSize = value;
                RaiseTileSizeChangedEvent(OldSize);
            }
        }

        private Vector2 _TileCount = new Vector2(1,1);
        public Vector2 TileCount
        {
            get { return _TileCount; }
            set
            {
                Vector2 oldCount = _TileCount;
                _TileCount = value;
                RaiseTileCountChangedEvent(oldCount);
            }
        }

        private List<Tile> _Tiles;
        public List<Tile> Tiles
        {
            get { return _Tiles; }
            set { _Tiles = value; }
        }

        public override SpriteBatch Sprite
        {
            get { return _Sprite; }
            set { _Sprite = value; }
        }

        public override Vector2 Size
        {
            get { if (Texture == null) return Vector2.Zero; return new Vector2(Texture.Width, Texture.Height); }
        }

        private GridObject Grid;
        private Boolean IsGridVisible = true;
        #endregion Tileset Properties

        #region Tileset Events

        /// <summary>
        /// This event is raised whenever the TileSize property is changed
        /// </summary>
        public event EventHandler<TileSizeChangedEventArgs> TileSizeChanged;
        public void OnTileSizeChanged(TileSizeChangedEventArgs e)
        {
            EventHandler<TileSizeChangedEventArgs> handler = TileSizeChanged;
            if (handler != null) { handler(this, e); }
        }

        /// <summary>
        /// Raises a TileSizeChanged Event
        /// </summary>
        /// <param name="oldSize">The TileSize value prior to this change</param>
        public void RaiseTileSizeChangedEvent(Vector2 oldSize)
        {
            OnTileSizeChanged(new TileSizeChangedEventArgs() { OldSize = oldSize, NewSize = TileSize });
        }

        /// <summary>
        /// This event is raised whenver the TileCount property is changed
        /// </summary>
        public event EventHandler<TileCountChangedEventArgs> TileCountChanged;
        public void OnTileCountChanged(TileCountChangedEventArgs e)
        {
            EventHandler<TileCountChangedEventArgs> handler = TileCountChanged;
            if (handler != null) { handler(this, e); }
        }

        /// <summary>
        /// Raises a TileCountChanged Event
        /// </summary>
        /// <param name="oldCount">The TileCount value prior to this change</param>
        public void RaiseTileCountChangedEvent(Vector2 oldCount)
        {
            OnTileCountChanged(new TileCountChangedEventArgs() { OldCount = oldCount, NewCount = TileCount });
        }

        #endregion Tileset Events

        public Tileset()
        {
            ZIndex = (float)RenderingLayer.ViewBackground;
            TileSizeChanged += Tileset_TileSizeChanged;
            TileCountChanged += Tileset_TileCountChanged;
        }

        void Tileset_TileCountChanged(object sender, TileCountChangedEventArgs e)
        {
            //Grid.CellSize = new Vector2(Size.X / TileCount.X, Size.Y / TileCount.Y);
            ConfigureGrid();
        }

        void Tileset_TileSizeChanged(object sender, TileSizeChangedEventArgs e)
        {
            ConfigureGrid();
        }

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            if (String.IsNullOrEmpty(ResourceName) == false)
                _Texture = _Texture ?? Content.Load<Texture2D>(ResourceName);

            _Sprite = new SpriteBatch(GraphicsDevice);
            InitializeGrid(Content, GraphicsDevice);
        }

        private void ConfigureGrid()
        {
            Grid = Grid ?? new GridObject();
            Grid.Position = Vector2.Zero;
            Grid.CellSize = TileSize;
            Grid.GridSize = TileCount;
            Grid.GridColor = Color.Red;
            Grid.LineThickness = 1.0f;
            //{
            //    Position = Vector2.Zero,
            //    CellSize = TileSize,
            //    GridSize = new Vector2((int)(Size.X / TileSize.X)),
            //    GridColor = Color.Green,
            //    LineThickness = 2.0f,
            //};


        }

        private void InitializeGrid(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ConfigureGrid();
            Grid.LoadResources(Content, GraphicsDevice);
            Grid.IgnoreCamera = true;
        }

        public override void Draw()
        {
            Sprite.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            Sprite.Draw(Texture, new Rectangle(0, 0, (int)Size.X, (int)Size.Y), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, ZIndex);
            Sprite.End();
            if (Grid.IsLoaded)
                Grid.Draw();
        }
        //public static List<Tileset> Load(XDocument XML)

    }
}
