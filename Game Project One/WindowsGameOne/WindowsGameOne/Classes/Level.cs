using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WindowsGameOne.Extensions;

namespace WindowsGameOne.Classes
{
    public class Level
    {
        private Vector2 _Size;
        public Vector2 Size
        {
            get { return new Vector2(_Grid.CellSize.X * _Grid.GridSize.X, _Grid.CellSize.Y * _Grid.GridSize.Y); }
            //get { return _Size; }
            //set { _Size = value; }
        }

        private GridObject _Grid;
        public GridObject Grid
        {
            get { return _Grid; }
            set { _Grid = value; }
        }

        private Boolean _IsGridVisible;
        public Boolean IsGridVisible
        {
            get { return _IsGridVisible; }
            set { _IsGridVisible = value; }
        }


        public static Level Load(XDocument XML)
        {
            List<Level> NewLevel = new List<Level>();

            NewLevel = (from e in XML.Descendants("Level")
                        where (string)e.Attribute("Type") == ("Level")
                        select new Level()
                          {
                              //Size = new Vector2(e.Element("Size").AttributeValueNull("X").ConvertToFloat(),
                              //                   e.Element("Size").AttributeValueNull("Y").ConvertToFloat()),
                              IsGridVisible = e.AttributeValueNull("IsGridVisible").ConvertToBoolean(),
                              Grid = GridObject.Load(e)[0]
                          } as Level).ToList<Level>();

            return NewLevel[0];
        }

        public void Draw()
        {
            Grid.Draw();
        }

        internal void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            Grid.LoadResources(Content, GraphicsDevice);
        }
    }
}
