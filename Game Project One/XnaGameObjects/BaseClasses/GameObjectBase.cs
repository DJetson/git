using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Xml.Serialization;

namespace XnaGameObjects.BaseClasses
{
    public class GameObjectBase
    {
        protected Boolean _IsLoaded;
        public virtual Boolean IsLoaded
        {
            get { return _IsLoaded; }
        }

        public GameObjectBase()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        protected virtual void LoadContent()
        {
        }

        public virtual void LoadResources(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _IsLoaded = true;
        }

        public virtual void SerializeXML(StreamWriter file)
        {
            if (file == null)
                throw new Exception("SerializeXML failed: The Filestream was null.");

            XmlSerializer writer = new XmlSerializer(typeof(GameObjectBase));
            writer.Serialize(file, this);
        }
    }
}
