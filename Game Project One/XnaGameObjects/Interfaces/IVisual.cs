using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGameObjects.Interfaces
{
    /// <summary>
    /// This Interface is implemented by all Drawable GameObject classes
    /// </summary>
    public interface IVisualElement
    {

        float ZIndex
        {
            get;
        }

        SpriteBatch Sprite
        {
            get;
        }

        Texture2D Texture
        {
            get;
        }

        Rectangle? SourceRectangle
        {
            get;
        }

        void Draw();
    }
}
