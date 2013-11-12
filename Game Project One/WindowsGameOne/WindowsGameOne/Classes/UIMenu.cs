using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGameOne.Interfaces;

namespace WindowsGameOne.Classes
{
    public class UIMenu : UIFrame
    {
        public List<IMenuItem> MenuItems;
        public IMenuItem SelectedItem;

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            base.LoadResources(Content, GraphicsDevice);

            foreach (IMenuItem item in MenuItems)
            {
                item.LoadResources(Content, GraphicsDevice);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HasFocus == false)
                return;

            foreach (IMenuItem item in MenuItems)
            {
                item.Update(gameTime);
            }

        }

        public override void Draw()
        {
            base.Draw();


        }


    }
}
