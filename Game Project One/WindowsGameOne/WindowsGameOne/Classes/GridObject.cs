using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WindowsGameOne.BaseClasses;

namespace WindowsGameOne.Classes
{
    public class GridObject : VisualObject
    {
        public float LineThickness;
        public Vector2 GridSize;
        public Vector2 CellSize;
        public Color GridColor;

        public GridObject()
        {
            ZIndex = (float)RenderingLayer.RenderingLayerMax;
            //GridWidth = 15;
            //GridHeight = 15;
            //CellWidth = 64;
            //CellHeight = 64;
            GridColor = Color.Red;
            LineThickness = 1;
        }

        public override void Draw()
        {

            Sprite.Begin(SpriteSortMode.Deferred,
                         BlendState.AlphaBlend,
                         SamplerState.LinearClamp,
                         DepthStencilState.Default,
                         RasterizerState.CullNone,
                         null,
                         CameraObject.Camera.GetCameraTransform());
            for (int i = 0; i < GridSize.Y + 1; i++)
                DrawLine(new Vector2(0, CellSize.Y * i), new Vector2(CellSize.X * GridSize.X, CellSize.Y * i));
            for (int j = 0; j < GridSize.X + 1; j++)
                DrawLine(new Vector2(CellSize.X * j, 0), new Vector2(CellSize.X * j, CellSize.Y * GridSize.Y));
            Sprite.End();
        }

        void DrawLine(Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            Sprite.Draw(Texture, point1, null, GridColor, angle, Vector2.Zero, new Vector2(length, LineThickness), SpriteEffects.None, 1);
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
