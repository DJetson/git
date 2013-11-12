using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using WindowsGameOne.Extensions;

namespace WindowsGameOne.Classes
{
    [Serializable()]
    public class UIButton : UIFrame
    {

        protected SpriteBatch _FontSpriteBatch;
        public virtual SpriteBatch FontSpriteBatch
        {
            get { return _FontSpriteBatch; }
            set { _FontSpriteBatch = value; }
        }

        protected SpriteFont _SpriteFont;
        public virtual SpriteFont SpriteFont
        {
            get { return _SpriteFont; }
            set { _SpriteFont = value; }
        }

        [XmlElement("ButtonText")]
        protected String _ButtonText;
        public virtual String ButtonText
        {
            get { return _ButtonText; }
            set { _ButtonText = value; }
        }

        [XmlElement("FontColor")]
        protected Color _FontColor;
        public virtual Color FontColor
        {
            get { return _FontColor; }
            set { _FontColor = value; }
        }

        public Vector2 StringSize
        {
            get {
                if (String.IsNullOrEmpty(ButtonText))
                    return new Vector2(0, 0);
                return SpriteFont.MeasureString(ButtonText); }
        }

        public UIButton()
        {
            ZIndex = (float)RenderingLayer.UILayerForeground;
        }

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            base.LoadResources(Content, GraphicsDevice);
            FontSpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFont = Content.Load<SpriteFont>("ButtonFont");
        }

        public override void Draw()
        {
            base.Draw();

            if (IsFixed)
            {
                FontSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            }
            else
            {
                FontSpriteBatch.Begin(SpriteSortMode.Deferred,
                         BlendState.AlphaBlend,
                         SamplerState.LinearClamp,
                         DepthStencilState.Default,
                         RasterizerState.CullNone,
                         null,
                         CameraObject.Camera.GetCameraTransform());
            }

            FontSpriteBatch.DrawString(SpriteFont, ButtonText, new Vector2(Area.Center.X, Area.Center.Y), FontColor, 0, new Vector2(StringSize.X / 2, StringSize.Y / 2), 1, SpriteEffects.None, ZIndex);
            //FontSpriteBatch.DrawString(SpriteFont, ButtonText, Position, FontColor,0,new Vector2(StringSize.X / 2, StringSize.Y /2),1,SpriteEffects.None,ZIndex);
            FontSpriteBatch.End();
        }

        /// <summary>
        /// Loads all buttons contained in the supplied XML. This is typically called by the LoadManager
        /// </summary>
        /// <param name="XML">The source XML</param>
        /// <returns>A collection of UI Buttons</returns>
        new public static List<UIButton> Load(XDocument XML)
        {
            List<UIButton> Buttons = new List<UIButton>();

            Buttons = (from e in XML.Descendants("Object")
                       where (string)e.Attribute("Type") == ("UIButton")
                       select new UIButton()
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
                           ResourceName = (string)e.Element("UIFrame").Attribute("ResourceName"),
                           FontColor = new Color(e.Element("UIButton").Element("FontColor").AttributeValueNull("R").ConvertToInt(),
                                                 e.Element("UIButton").Element("FontColor").AttributeValueNull("G").ConvertToInt(),
                                                 e.Element("UIButton").Element("FontColor").AttributeValueNull("B").ConvertToInt(),
                                                 e.Element("UIButton").Element("FontColor").AttributeValueNull("A").ConvertToInt()),
                           ButtonText = (string)e.Element("UIButton").Attribute("Text")
                       } as UIButton).ToList<UIButton>();
            return Buttons;
        }
    }
}
