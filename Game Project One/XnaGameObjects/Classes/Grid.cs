using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XnaGameObjects.Classes
{

    public class GridSizeChangedEventArgs : EventArgs
    {
        public Vector2 CellSize { get; set; }
        public Vector2 GridSize { get; set; }
    }


    public class GridObject : VisualObject
    {

        #region Grid Events
        public event EventHandler<GridSizeChangedEventArgs> GridSizeChanged;
        public void OnGridSizeChanged(GridSizeChangedEventArgs e)
        {
            EventHandler<GridSizeChangedEventArgs> handler = GridSizeChanged;
            if (handler != null) { handler(this, new GridSizeChangedEventArgs() { CellSize = this.CellSize, GridSize = this.GridSize }); }
        }
        #endregion Grid Events

        #region Grid Fields

        private Boolean _IgnoreCamera = false;
        public Boolean IgnoreCamera
        {
            get { return _IgnoreCamera; }
            set { _IgnoreCamera = value; }
        }

        private Vector2 _CellSize = new Vector2(32, 32);
        public Vector2 CellSize { get { return _CellSize; } set { _CellSize = value; } }

        private Vector2 _GridSize = new Vector2(10, 10);
        public Vector2 GridSize { get { return _GridSize; } set { _GridSize = value; } }

        private float _LineThickness = 1;
        public float LineThickness { get { return _LineThickness; } set { _LineThickness = value; } }

        private Color _GridColor = Color.Red;
        public Color GridColor { get { return _GridColor; } set { _GridColor = value; } }

        public override Rectangle? SourceRectangle
        {
            get
            {
                if (CellSize == null || GridSize == null)
                    return new Rectangle(0, 0, 0, 0);
                else
                    return new Rectangle?(new Rectangle(0, 0, (int)(CellSize.X * GridSize.X), (int)(CellSize.Y * GridSize.Y)));
            }
        }

        public override Vector2 Size
        {
            get
            {
                if (CellSize == null || GridSize == null)
                    return Vector2.Zero;
                else
                    return new Vector2(CellSize.X * GridSize.X, CellSize.Y * GridSize.Y);
            }
        }

        #endregion Grid Fields



        public GridObject()
        {
            ZIndex = (float)RenderingLayer.ViewForeground;
        }

        #region Rendering
        public override void Draw()
        {
            if (IgnoreCamera == false)
                Sprite.Begin(SpriteSortMode.BackToFront,
                             BlendState.AlphaBlend,
                             SamplerState.LinearClamp,
                             DepthStencilState.Default,
                             RasterizerState.CullNone,
                             null,
                             CameraObject.Camera.GetCameraTransform());
            else
                Sprite.Begin();
            for (int i = 0; i < GridSize.Y + 1; i++)
                DrawLine(new Vector2(0, CellSize.Y * i), new Vector2(CellSize.X * GridSize.X, CellSize.Y * i));
            for (int j = 0; j < GridSize.X + 1; j++)
                DrawLine(new Vector2(CellSize.X * j, 0), new Vector2(CellSize.X * j, CellSize.Y * GridSize.Y));
            Sprite.End();
        }

        private void DrawLine(Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            Sprite.Draw(Texture, point1, null, GridColor, angle, Vector2.Zero, new Vector2(length, LineThickness), SpriteEffects.None, ZIndex);
        }
        #endregion Rendering

        /// <summary>
        /// Get the ID of the cell at the specified position
        /// </summary>
        /// <param name="Position">The position to check</param>
        /// <returns>The Id of the cell at the specified position</returns>
        public int GetCellAtPosition(Vector2 position)
        {
            Vector2 LocalPosition = position - this.Position;

            if (GridSize.X == 0 || GridSize.Y == 0 || CellSize.X == 0 || CellSize.Y == 0)
                throw new Exception("GetCellAtPosition failed. The GridSize and CellSize must be non-zero.");

            Vector2 GridLocation = new Vector2((int)LocalPosition.X / CellSize.X, (int)LocalPosition.Y / CellSize.Y);
            return (int)((GridLocation.Y * GridSize.X) + GridLocation.X);
        }

        /// <summary>
        /// Get the coordinates of the cell at the specified position
        /// </summary>
        /// <param name="Position">The position to check</param>
        /// <returns>The coordinates of the cell at the given position</returns>
        public Vector2 GetGridCoordsAtPosition(Vector2 position)
        {
            Vector2 LocalPosition = position - this.Position;

            if (GridSize.X == 0 || GridSize.Y == 0 || CellSize.X == 0 || CellSize.Y == 0)
                throw new Exception("GetGridCoordsAtPosition failed. The GridSize and CellSize must be non-zero.");
            
            return new Vector2((int)LocalPosition.X / CellSize.X, (int)LocalPosition.Y / CellSize.Y);
        }

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            if (_IsLoaded)
                return;

            _Sprite = new SpriteBatch(GraphicsDevice);

            _Texture = new Texture2D(GraphicsDevice, 1, 1);
            _Texture.SetData(new Color[] { Color.White });

            _IsLoaded = true;
        }

        public static List<GridObject> Load(XDocument XML)
        {
            List<GridObject> Grids = new List<GridObject>();

            Grids = (from e in XML.Descendants("Object")
                     where (string)e.Attribute("Type") == ("GridObject")
                     select new GridObject()
                     {
                         GridSize = new Vector2((float)e.Element("GridSize").Attribute("Width"),
                                                (float)e.Element("GridSize").Attribute("Height")),
                         CellSize = new Vector2((float)e.Element("CellSize").Attribute("Width"),
                                                (float)e.Element("CellSize").Attribute("Height")),
                         GridColor = new Color((int)(e.Element("GridLines").Element("Color").Attribute("R")),
                                               (int)(e.Element("GridLines").Element("Color").Attribute("G")),
                                               (int)(e.Element("GridLines").Element("Color").Attribute("B")),
                                               (int)(e.Element("GridLines").Element("Color").Attribute("A"))),
                         LineThickness = (float)e.Element("GridLines").Attribute("LineThickness")
                     } as GridObject).ToList<GridObject>();
            return Grids;
        }

        public static List<GridObject> Load(XElement Root)
        {
            List<GridObject> Grid = new List<GridObject>();

            Grid = (from e in Root.Descendants("Grid")
                    select new GridObject()
                    {
                        GridSize = new Vector2((float)e.Element("GridSize").Attribute("Width"),
                                               (float)e.Element("GridSize").Attribute("Height")),
                        CellSize = new Vector2((float)e.Element("CellSize").Attribute("Width"),
                                               (float)e.Element("CellSize").Attribute("Height")),
                        GridColor = new Color((int)(e.Element("GridLines").Element("Color").Attribute("R")),
                                              (int)(e.Element("GridLines").Element("Color").Attribute("G")),
                                              (int)(e.Element("GridLines").Element("Color").Attribute("B")),
                                              (int)(e.Element("GridLines").Element("Color").Attribute("A"))),
                        LineThickness = (float)e.Element("GridLines").Attribute("LineThickness")
                    } as GridObject).ToList<GridObject>();

            return Grid;
        }
    }
}
