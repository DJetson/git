using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WindowsGameOne.BaseClasses;
using WindowsGameOne.Interfaces;

namespace WindowsGameOne.Classes
{

    /// <summary>
    /// This is used to define the initial ZIndex for game objects so that they are rendered in the proper order.
    /// InGame Background objects are rendered first, then InGame Foreground objects, InGame Effects, UIObjects
    /// Popups, and finally anything for which no ZIndex is specified will be rendered. This is to ensure
    /// that improperly initialized objects don't go unnoticed.
    /// </summary>
    public enum RenderingLayer
    {
        Undefined,
        PopupLayerForeground,
        PopupLayerBackground,
        UILayerForeground,
        UILayerBackground,
        ViewEffectsLayer,
        PlayerLayer,
        ViewForeground,
        ViewBackground,
        RenderingLayerMax
    };

    [Serializable]
    [XmlInclude(typeof(VisualObject))]
    public class VisualObject : GameObjectBase, IVisualElement
    {
        protected float _ZIndex = (float)RenderingLayer.Undefined;
        public virtual float ZIndex
        {
            get { return _ZIndex; }
            set { _ZIndex = (float)value / (float)RenderingLayer.RenderingLayerMax; }
        }

        [NonSerialized]
        protected SpriteBatch _Sprite;
        public virtual SpriteBatch Sprite
        {
            get { return _Sprite; }
        }

        protected String _ResourceName;
        public virtual String ResourceName
        {
            get { return _ResourceName; }
            set { }
        }

        [NonSerialized]
        protected Texture2D _Texture;
        public virtual Texture2D Texture
        {
            get { return _Texture; }
        }

        [NonSerialized]
        protected Rectangle? _SourceRectangle;
        public virtual Rectangle? SourceRectangle
        {
            get { return _SourceRectangle; }
        }

        protected Vector2 _Position;
        public virtual Vector2 Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        protected Vector2 _Size;
        public virtual Vector2 Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        public virtual void Draw()
        {
            throw new NotImplementedException();
        }

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            if (_IsLoaded)
                return;

            _Sprite = new SpriteBatch(GraphicsDevice);
            _Texture = Content.Load<Texture2D>(_ResourceName);

            _IsLoaded = true;
        }

        public override void SerializeXML(StreamWriter file)
        {
            if (file == null)
                throw new Exception("SerializeXML failed: The Filestream was null.");

            base.SerializeXML(file);

            XmlSerializer writer = new XmlSerializer(typeof(VisualObject));

            writer.Serialize(file, this);

        }
    }
}
