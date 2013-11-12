using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaGameObjects.Classes
{
    public class Level : VisualObject
    {
        private int TileCountX;
        private int TileCountY;
        private int TileWidth;
        private int TileHeight;

        public List<VisualObject> UndefinedLayer = new List<VisualObject>();
        public List<VisualObject> PopupForegroundLayer = new List<VisualObject>();
        public List<VisualObject> PopupBackgroundLayer = new List<VisualObject>();
        public List<VisualObject> UIForegroundLayer = new List<VisualObject>();
        public List<VisualObject> UIBackgroundLayer = new List<VisualObject>();
        public List<VisualObject> ViewEffectsLayer = new List<VisualObject>();
        public List<VisualObject> PlayerLayer = new List<VisualObject>();
        public List<VisualObject> ViewForegroundLayer = new List<VisualObject>();
        public List<VisualObject> ViewBackgroundLayer = new List<VisualObject>();
        public List<VisualObject> RenderingLayerMax = new List<VisualObject>();

        public List<List<VisualObject>> LevelObjects = new List<List<VisualObject>>();

        private VisualObject[][][] LevelTiles;

        public Level()
        {
            LevelObjects = new List<List<VisualObject>>() { UndefinedLayer, PopupForegroundLayer, PopupBackgroundLayer,
                                                            UIForegroundLayer, UIBackgroundLayer, ViewEffectsLayer,
                                                            PlayerLayer, ViewForegroundLayer, ViewBackgroundLayer, 
                                                            RenderingLayerMax};
        }

        public override void Draw()
        {
            foreach (List<VisualObject> Layer in LevelObjects)
            {
                if (Layer.Count == 0)
                    continue;

                foreach (VisualObject v in Layer)
                {
                    if(v.IsLoaded)
                        v.Draw();
                }
            }
        }

        public void AddObject(VisualObject NewObject)
        {
            if(LevelObjects[NewObject.GetLayer()].Contains(NewObject) == false)
                LevelObjects[NewObject.GetLayer()].Add(NewObject);
        }

        //public Level(int tileCountX, int tileCountY, int tileWidth, int tileHeight)
        //{

        //    TileCountX = tileCountX;
        //    TileCountY = tileCountY;
        //    TileWidth = tileWidth;
        //    TileHeight = tileHeight;

        //    LevelTiles = LevelTiles ?? new VisualObject[Enum.GetNames(typeof(RenderingLayer)).Count()][][];
        //    for (int k = 0; k < Enum.GetNames(typeof(RenderingLayer)).Count(); k++)
        //    {
        //        LevelTiles[k] = LevelTiles[k] ?? new VisualObject[TileCountY][];
        //        for (int j = 0; j < TileCountY; j++)
        //        {
        //            LevelTiles[k][j] = LevelTiles[k][j] ?? new VisualObject[TileCountX];
        //            for (int i = 0; i < TileCountX; i++)
        //            {
        //                LevelTiles[k][j][i] = LevelTiles[k][j][i] ?? new Tile();
        //            }
        //        }

        //    }
        //}

        //public override void Draw()
        //{
        //    return;
        //    if (LevelTiles == null)
        //        return;
        //    for (int k = Enum.GetNames(typeof(RenderingLayer)).Count() - 1; k >= 0; k--)
        //    {
        //        if (LevelTiles[k] == null)
        //            continue;
        //        for (int j = 0; j < TileCountY; j++)
        //        {
        //            if (LevelTiles[k][j] == null)
        //                continue;
        //            for (int i = 0; i < TileCountX; i++)
        //            {
        //                if (LevelTiles[k][j][i] == null || LevelTiles[k][j][i].IsLoaded == false)
        //                    continue;
        //                LevelTiles[k][j][i].Draw();
        //            }
        //        }
        //    }
        //}

        //public void AddTile(Tile t, int x, int y, RenderingLayer layer)
        //{
        //    LevelTiles[(int)layer][y][x] = t;
        //}
    }
}
