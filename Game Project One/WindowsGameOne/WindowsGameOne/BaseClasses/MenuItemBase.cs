using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGameOne.Classes;

namespace WindowsGameOne.BaseClasses
{
    public abstract class MenuItemBase : UIFrame
    {
        /// <summary>
        /// Whether this item is currently selected
        /// </summary>
        public Boolean IsSelected { get; set; }
        public Guid MenuId { get; set; }
        public Boolean IsEnabled { get; set; }
        public Boolean IsVisible { get; set; }

        //public abstract void Draw();
        //public abstract void Update(GameTime gameTime);
        //public abstract void Activated(object Sender);
    }
}
