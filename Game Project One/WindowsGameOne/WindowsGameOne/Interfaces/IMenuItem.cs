using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGameOne.Classes;

namespace WindowsGameOne.Interfaces
{
    public interface IMenuItem
    {
        Boolean IsSelected { get; set; }
        Guid MenuId { get; set; }
        UIMenu Parent { get; set; }
        Boolean IsEnabled { get; set; }
        Boolean IsVisible { get; set; }

        void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice);
        void Update(GameTime gameTime);
        void Draw();

    }
}
