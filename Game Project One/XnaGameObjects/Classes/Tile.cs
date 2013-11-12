using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaGameObjects.Managers;

namespace XnaGameObjects.Classes
{
    public class Tile : VisualObject
    {
        public Guid TilesetKey;
        public int TileId;
        public Rectangle SourceRect;
        public Rectangle DestinationRect;
        public SpriteBatch TileSprite;
        public Color Color;

        public override void Draw()
        {
            TileSprite.Begin();
            TileSprite.Draw(TileManager.Sets[TilesetKey].Texture, DestinationRect, SourceRect, Color, Rotation, Vector2.Zero, SpriteEffects.None, ZIndex);
            TileSprite.End();
        }

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            if (IsLoaded)
                return;

            int TileCountX = (int)TileManager.Sets[TilesetKey].TileCount.X;
            int TileCountY = (int)TileManager.Sets[TilesetKey].TileCount.Y;
            int TileWidth = (int)TileManager.Sets[TilesetKey].TileSize.X;
            int TileHeight = (int)TileManager.Sets[TilesetKey].TileSize.Y;

            if (TileSprite == null)
                TileSprite = new SpriteBatch(GraphicsDevice);
    
            SourceRect = new Rectangle(TileId % TileCountX * TileWidth,
                                       TileId / TileCountX * TileHeight,
                                       TileWidth,
                                       TileHeight);

            Color = Color.White;
            _IsLoaded = true;
            //base.LoadResources(Content, GraphicsDevice);
        }
    }
}
