using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WindowsGameOne.Extensions;
using WindowsGameOne.BaseClasses;

namespace WindowsGameOne.Classes
{
    /// <summary>
    /// This class is the base class for things like buttons, menus, and windows.
    /// Essentially, it is a rectangular graphic that will be drawn behind things like buttons
    /// and text and windows
    /// </summary>
    public class UIFrame : UIObject
    {
        //protected String _ResourceName;
        public override String ResourceName
        {
            get { return _ResourceName; }
            set { _ResourceName = value; }
        }
        //protected Texture2D _BorderTexture;
        //public virtual Texture2D BorderTexture
        //{
        //    get { return _BorderTexture; }
        //    set { _BorderTexture = value; }
        //}

        //protected Rectangle _Area;
        public virtual Rectangle Area
        {
            get
            {
                return new Rectangle((int)(Position.X),
                                     (int)(Position.Y),
                                     (int)(Size.X),
                                     (int)(Size.Y));
            }
            //set { _Area = value; }
        }

        public virtual Rectangle FillRectangle
        {
            get
            {
                return new Rectangle((int)(Position.X + (StrokeThickness / 2)),
                                     (int)(Position.Y + (StrokeThickness / 2)),
                                     (int)(Size.X - StrokeThickness),
                                     (int)(Size.Y - StrokeThickness));
            }
        }

        public virtual Rectangle StrokeRectangle
        {
            get { return Area; }
        }
        protected Color _Fill;
        public virtual Color Fill
        {
            get { return _Fill; }
            set { _Fill = value; }
        }

        protected Color _Stroke;
        public virtual Color Stroke
        {
            get { return _Stroke; }
            set { _Stroke = value; }
        }

        protected float _StrokeThickness;
        public virtual float StrokeThickness
        {
            get { return _StrokeThickness; }
            set { _StrokeThickness = value; }
        }

        public UIFrame()
            : base()
        {
            ZIndex = (float)RenderingLayer.UILayerBackground;
        }



        public override void Draw()
        {
            if (IsFixed)
            {
                _Sprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            }
            else
            {
                _Sprite.Begin(SpriteSortMode.Deferred,
                     BlendState.AlphaBlend,
                     SamplerState.LinearClamp,
                     DepthStencilState.Default,
                     RasterizerState.CullNone,
                     null,
                     CameraObject.Camera.GetCameraTransform());
            }
            //Draw Stroke Rectangle
            //Sprite.Draw(Texture, StrokeRectangle, StrokeRectangle, Stroke,0, new Vector2(StrokeRectangle.Center.X,StrokeRectangle.Center.Y),SpriteEffects.None,ZIndex);
            //Draw Fill Rectangle
            Sprite.Draw(Texture, StrokeRectangle, null, Stroke, 0, new Vector2(0,0), SpriteEffects.None, ZIndex);
            Sprite.Draw(Texture, FillRectangle, null, Fill, 0, new Vector2(0,0), SpriteEffects.None, ZIndex);
            //Sprite.Draw(Texture, FillRectangle, Fill);
            //_Sprite.Draw(_Texture, StrokeRectangle, StrokeRectangle, _Stroke, 0, new Vector2(StrokeRectangle.Center.X, StrokeRectangle.Center.Y), SpriteEffects.None, ZIndex);
            //            _Sprite.Draw(_Texture, FillRectangle, FillRectangle, _Fill, 0, new Vector2(FillRectangle.Center.X, FillRectangle.Center.Y), SpriteEffects.None, ZIndex);
            _Sprite.End();
        }

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            if (_IsLoaded)
                return;

            //_Area = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

            _Sprite = new SpriteBatch(GraphicsDevice);

            //Setup the Fill Texture
            if (String.IsNullOrEmpty(ResourceName))
            {
                _Texture = new Texture2D(GraphicsDevice, 1, 1);
                _Texture.SetData(new Color[] { Color.White });
            }
            else
            {
                _Texture = Content.Load<Texture2D>(ResourceName);
            }
            ////Setup the Stroke Texture
            //_BorderTexture = new Texture2D(GraphicsDevice, 1, 1);
            //_BorderTexture.SetData(new Color[] { Color.White });

            _IsLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            //Check Focus
            //Get Input
            //Respond to input
        }

        public static List<GameObjectBase> Load(XDocument XML)
        {
            List<GameObjectBase> UIFrames = new List<GameObjectBase>();
            UIFrames = (from e in XML.Descendants("Object")
                        where (string)e.Attribute("Type") == ("UIFrame")
                        select new UIFrame()
                        {
                            IsFixed = e.AttributeValueNull("IsFixed").ConvertToBoolean(),
                            Position = new Vector2(e.Element("Position").AttributeValueNull("X").ConvertToFloat(),
                                                   e.Element("Position").AttributeValueNull("Y").ConvertToFloat()),
                            Size = new Vector2(e.Element("Size").AttributeValueNull("Width").ConvertToFloat(),
                                               e.Element("Size").AttributeValueNull("Height").ConvertToFloat()),
                            Fill = new Color(e.Element("UIFrame").Element("Fill").AttributeValueNull("R").ConvertToInt(),
                                             e.Element("UIFrame").Element("Fill").AttributeValueNull("G").ConvertToInt(),
                                             e.Element("UIFrame").Element("Fill").AttributeValueNull("B").ConvertToInt(),
                                             e.Element("UIFrame").Element("Fill").AttributeValueNull("A").ConvertToInt()),
                            Stroke = new Color(e.Element("UIFrame").Element("Stroke").AttributeValueNull("R").ConvertToInt(),
                                               e.Element("UIFrame").Element("Stroke").AttributeValueNull("G").ConvertToInt(),
                                               e.Element("UIFrame").Element("Stroke").AttributeValueNull("B").ConvertToInt(),
                                               e.Element("UIFrame").Element("Stroke").AttributeValueNull("A").ConvertToInt()),
                            StrokeThickness = e.Element("UIFrame").AttributeValueNull("StrokeThickness").ConvertToFloat(),
                            ResourceName = (string)e.Element("UIFrame").Attribute("ResourceName")
                        } as UIFrame).ToList<GameObjectBase>();

            return UIFrames;
        }


    }


}
