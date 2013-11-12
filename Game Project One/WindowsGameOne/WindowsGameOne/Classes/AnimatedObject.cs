using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using WindowsGameOne.BaseClasses;
using WindowsGameOne.Interfaces;

namespace WindowsGameOne.Classes
{
    [Serializable]
    public class AnimatedObject : VisualObject, IVisualElement
    {

        [NonSerialized()]
        private PlayMode _State;
        public PlayMode State
        {
            get { return _State; }
            private set { _State = value; Animations[CurrentAnimation].State = State; }
        }

        [XmlArray()]
        public Dictionary<String, AnimatedSprite> Animations = new Dictionary<string, AnimatedSprite>();

        [NonSerialized]
        protected String _CurrentAnimation;
        public virtual String CurrentAnimation
        {
            set
            {
                if (Animations != null && CurrentAnimation != null && Animations[CurrentAnimation] != null)
                    Animations[CurrentAnimation].State = PlayMode.Stop;

                _CurrentAnimation = value;

            }
            get { return _CurrentAnimation; }
        }

        public override SpriteBatch Sprite
        {
            get { return Animations[CurrentAnimation].SpriteSheet; }
        }

        public override Texture2D Texture
        {
            get { return Animations[CurrentAnimation].Texture; }
        }

        public override Rectangle? SourceRectangle
        {
            get { return Animations[CurrentAnimation].CurrentFrame; }
        }

        public override void Update(GameTime gameTime)
        {
            Animations[CurrentAnimation].State = State;
            Animations[CurrentAnimation].Update(gameTime);
        }

        public AnimatedObject()
        {
            ZIndex = (float)RenderingLayer.PlayerLayer;
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

            Sprite.Draw(Texture,
                        new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                        SourceRectangle,
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        SpriteEffects.None, ZIndex);

            Sprite.End();
        }

        public void AddAnimation(String ResourceName)
        {
            Animations.Add(ResourceName, new AnimatedSprite(ResourceName));
        }

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            if (_IsLoaded)
                return;

            foreach (KeyValuePair<String, AnimatedSprite> Animation in Animations)
            {
                if (Animation.Value.IsLoaded)
                    continue;

                Animation.Value.Initialize(GraphicsDevice);
                Animation.Value.Load(Content);
            }

            _IsLoaded = true;
        }

        public void Play(Boolean Continuous = true)
        {
            if (Continuous)
                State = PlayMode.Continous;
            else
                State = PlayMode.Play;
        }

        public void Pause()
        {
            State = PlayMode.Play;
        }

        public void Stop()
        {
            State = PlayMode.Stop;
        }

        public override void SerializeXML(StreamWriter file)
        {
            if (file == null)
                throw new Exception("SerializeXML failed: The Filestream was null.");

            base.SerializeXML(file);

            XmlSerializer writer = new XmlSerializer(typeof(AnimatedObject));

            writer.Serialize(file, this);

        }

        public static List<AnimatedObject> Load(XDocument XML)
        {
            List<AnimatedObject> AnimatedObjects = new List<AnimatedObject>();

            AnimatedObjects = (from e in XML.Descendants("Object")
                               where (string)e.Attribute("Type") == ("AnimatedObject")
                               select new AnimatedObject()
                               {
                                   Position = new Vector2((float)e.Element("Position").Attribute("X"),
                                                          (float)e.Element("Position").Attribute("Y")),
                                   Size = new Vector2((float)e.Element("Size").Attribute("Width"),
                                                      (float)e.Element("Size").Attribute("Height")),
                                   CurrentAnimation = (string)e.Element("Animations").Attribute("CurrentAnimation"),
                                   Animations = (from a in e.Descendants("Animation")
                                                 select new
                                                 {
                                                     Key = (string)a.Attribute("Key"),
                                                     Value = new AnimatedSprite((string)a.Attribute("Key"),
                                                         (double)a.Element("FrameSize").Attribute("Width"),
                                                         (double)a.Element("FrameSize").Attribute("Height"),
                                                         (double)a.Element("SheetSize").Attribute("Width"),
                                                         (double)a.Element("SheetSize").Attribute("Height"),
                                                         (int)a.Attribute("FrameCount"),
                                                         (int)a.Attribute("FPS"))
                                                 }).ToDictionary(g => g.Key, g => g.Value)
                               } as AnimatedObject
                            ).ToList<AnimatedObject>();

            return AnimatedObjects;
        }
    }
}
