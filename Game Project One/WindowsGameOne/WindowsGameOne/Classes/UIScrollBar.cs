using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WindowsGameOne.BaseClasses;
using WindowsGameOne.Extensions;
namespace WindowsGameOne.Classes
{
    public enum Orientation { Vertical, Horizontal };
    public class UIScrollBar : UIFrame
    {
        private Orientation _Orientation;
        public Orientation Orientation
        {
            get { return _Orientation; }
            set { _Orientation = value; }
        }

        /// <summary>
        /// The total area serviced by the scroll bar
        /// </summary>
        private Rectangle _ScrollArea;
        public Rectangle ScrollArea
        {
            get { return _ScrollArea; }
            set { _ScrollArea = value; }
        }

        /// <summary>
        /// The visible area used by the scroll bar
        /// </summary>
        private Rectangle _ViewArea;
        public Rectangle ViewArea
        {
            get { return _ViewArea; }
            set { _ViewArea = value; }
        }

        /// <summary>
        /// The scroll handle
        /// </summary>
        private UIButton _ScrollButton;
        public UIButton ScrollButton
        {
            get { return _ScrollButton; }
            set { _ScrollButton = value; }
        }

        /// <summary>
        /// A UIButton which decreases the scroll amount
        /// </summary>
        private UIButton _DecreaseButton;
        public UIButton DecreaseButton
        {
            get { return _DecreaseButton; }
            set { _DecreaseButton = value; }
        }

        /// <summary>
        /// A UIButton which increases the scroll amount
        /// </summary>
        private UIButton _IncreaseButton;
        public UIButton IncreaseButton
        {
            get { return _IncreaseButton; }
            set { _IncreaseButton = value; }
        }

        public UIScrollBar()
        {
            ZIndex = (float)RenderingLayer.UILayerForeground;

        }



        private void InitializeScrollBar()
        {
            //Get the orientation and Set the UIButton sizes to squares that have sides 
            //of the same length as the minor axis (i.e. X-axis for vertical, Y-axis for horizontal
            InitializeScrollButtons();
            CameraObject.CameraChanged += CameraObject_CameraChanged;
            //_DecreaseButton = InitializeScrollButton(DecreaseButton);
            //_IncreaseButton = InitializeScrollButton(IncreaseButton);
        }

        //int SideLength = 0;

        //    if (Orientation == Orientation.Horizontal)
        //    {
        //        SideLength = (int)Size.Y;
        //        DecreaseButton.Position = new Vector2((int)Position.X, (int)Position.Y);
        //        IncreaseButton.Position = new Vector2(Area.Right - SideLength, (int)Position.Y);
        //        ScrollButton.Position = new Vector2((int)Position.X + SideLength, (int)Position.Y);
        //        ScrollButton.Size = new Vector2(((float)ViewArea.Width / (float)ScrollArea.Width) * (Size.X - (SideLength * 2)), Size.Y);

        //        //ScrollButton.Size = new Vector2((Game1.ViewportSize.X / Game1.CurrentLevel.Size.X) * (Size.X - (SideLength * 2)), Size.Y);
        //    }
        //    else if (Orientation == Orientation.Vertical)
        //    {
        //        SideLength = (int)Size.X;
        //        DecreaseButton.Position = new Vector2((int)Position.X, (int)Position.Y);
        //        IncreaseButton.Position = new Vector2((int)Position.X, Area.Top - SideLength);
        //        ScrollButton.Position = new Vector2((int)Position.X, (int)Position.Y + SideLength);
        //        ScrollButton.Size = new Vector2(Size.X, ((float)ViewArea.Height / (float)ScrollArea.Height) * (Size.Y - (SideLength * 2)));

        //        //ScrollButton.Size = new Vector2(Size.X, (Game1.ViewportSize.Y / Game1.CurrentLevel.Size.Y) * (Size.Y - (SideLength * 2)));

        //    }

        public void SetScrollButtonOffset(Vector2 offset)
        {
            float ScaleFactorX = (float)ScrollButton.Area.Width / (float)(Area.Width - (DecreaseButton.Area.Width + IncreaseButton.Area.Width) + ScrollButton.Area.Width / 2);
            float ScaleFactorY = (float)ScrollButton.Area.Height / (float)(Area.Height - (DecreaseButton.Area.Height + IncreaseButton.Area.Height) + ScrollButton.Area.Height / 2);

            offset.X *= ScaleFactorX;
            offset.Y *= ScaleFactorY;

            if (Orientation == Orientation.Horizontal)
            {

                float NewX = Position.X + DecreaseButton.Area.Height + offset.X;
                
                if (NewX > (Position.X + Size.X) - IncreaseButton.Area.Width - ScrollButton.Size.X)
                    NewX = (Position.X + Size.X) - IncreaseButton.Area.Width - ScrollButton.Size.X;
                else if (NewX < (Position.X + DecreaseButton.Area.Width))
                    NewX = Position.X + DecreaseButton.Area.Width;

                ScrollButton.Position = new Vector2(NewX, Position.Y);
            }
            else if (Orientation == Orientation.Vertical)
            {
                float NewY = Position.Y + DecreaseButton.Area.Height + offset.Y;
                if (NewY > (Position.Y + Size.Y) - IncreaseButton.Area.Height - ScrollButton.Size.Y)
                    NewY = (Position.Y + Size.Y) - IncreaseButton.Area.Height - ScrollButton.Size.Y;
                else if (NewY < (Position.Y + DecreaseButton.Area.Height))
                    NewY = Position.Y + DecreaseButton.Area.Height;

                ScrollButton.Position = new Vector2(Position.X, NewY);
            }
            else
                throw new Exception("SetScrollButtonOffset failed. Undefined Orientation");
        }

        public void SetScrollButtonSize(float size)
        {

            if (Orientation == Orientation.Horizontal)
            {

            }
            else if (Orientation == Orientation.Vertical)
            {

            }
            else
                throw new Exception("SetScrollButtonSize failed. Undefined Orientation");
        }

        public void UpdatePosition(Vector2 NewPosition)
        {
            SetScrollButtonOffset(NewPosition);
            //Get the initial offset
        }

        public void UpdateSize(float Scale)
        {

        }

        void CameraObject_CameraChanged(object sender, CameraMovedEventArgs e)
        {
            if (e.ValueTypeChanged == CameraValueType.Zoom)
                UpdateSize(e.Camera.Zoom);
            else if (e.ValueTypeChanged == CameraValueType.Position)
                UpdatePosition(e.Camera.Position);
            else
                throw new NotImplementedException("CameraObject_CameraChanged failed. There is no handler for CameraChanged events raised by a change in the viewport size.");
        }

        public override void LoadResources(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            if (_IsLoaded)
                return;

            InitializeScrollBar();
            IncreaseButton.LoadResources(Content, GraphicsDevice);
            DecreaseButton.LoadResources(Content, GraphicsDevice);
            ScrollButton.LoadResources(Content, GraphicsDevice);
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

        private void InitializeScrollButtons()
        {
            IncreaseButton = new UIButton();
            DecreaseButton = new UIButton();
            ScrollButton = new UIButton();
            if (IncreaseButton == null || DecreaseButton == null || ScrollButton == null)
                throw new Exception("InitializeScrollButton failed. A button was null.");

            IncreaseButton.ButtonText = ">";
            DecreaseButton.ButtonText = "<";
            ScrollButton.ButtonText = "|||";

            IncreaseButton.FontColor = Stroke;
            DecreaseButton.FontColor = Stroke;
            ScrollButton.FontColor = Stroke;
            IncreaseButton.StrokeThickness = StrokeThickness;
            DecreaseButton.StrokeThickness = StrokeThickness;
            ScrollButton.StrokeThickness = StrokeThickness;

            IncreaseButton.Fill = Fill.ScaleColor(0.5);
            IncreaseButton.Stroke = Stroke.ScaleColor(1.0);
            DecreaseButton.Fill = Fill.ScaleColor(0.5);
            DecreaseButton.Stroke = Stroke.ScaleColor(1.0);
            ScrollButton.Fill = Fill.ScaleColor(0.5);
            ScrollButton.Stroke = Stroke.ScaleColor(1.0);

            int SideLength = 0;

            if (Orientation == Orientation.Horizontal)
            {
                SideLength = (int)Size.Y;
                DecreaseButton.Position = new Vector2((int)Position.X, (int)Position.Y);
                IncreaseButton.Position = new Vector2(Area.Right - SideLength, (int)Position.Y);
                ScrollButton.Position = new Vector2((int)Position.X + SideLength, (int)Position.Y);
                ScrollButton.Size = new Vector2(((float)ViewArea.Width / (float)ScrollArea.Width) * (Size.X - (SideLength * 2)), Size.Y);

                //ScrollButton.Size = new Vector2((Game1.ViewportSize.X / Game1.CurrentLevel.Size.X) * (Size.X - (SideLength * 2)), Size.Y);
            }
            else if (Orientation == Orientation.Vertical)
            {
                SideLength = (int)Size.X;
                DecreaseButton.Position = new Vector2((int)Position.X, (int)Position.Y);
                IncreaseButton.Position = new Vector2((int)Position.X, Area.Top - SideLength);
                ScrollButton.Position = new Vector2((int)Position.X, (int)Position.Y + SideLength);
                ScrollButton.Size = new Vector2(Size.X, ((float)ViewArea.Height / (float)ScrollArea.Height) * (Size.Y - (SideLength * 2)));

                //ScrollButton.Size = new Vector2(Size.X, (Game1.ViewportSize.Y / Game1.CurrentLevel.Size.Y) * (Size.Y - (SideLength * 2)));

            }

            DecreaseButton.Size = new Vector2(SideLength, SideLength);
            IncreaseButton.Size = new Vector2(SideLength, SideLength);

        }

        public override void Draw()
        {
            base.Draw();
            IncreaseButton.Draw();
            DecreaseButton.Draw();

            ///TODO:Need some way to rotate the graphic by 90 degrees if it is oriented vertically
            ScrollButton.Draw();
        }

        new public static List<UIScrollBar> Load(XDocument XML)
        {
            List<UIScrollBar> ScrollBars = new List<UIScrollBar>();

            ScrollBars = (from e in XML.Descendants("Object")
                          where (string)e.Attribute("Type") == ("UIScrollBar")
                          select new UIScrollBar()
                          {
                              Orientation = (Orientation)System.Enum.Parse(typeof(Orientation), e.AttributeValueNull("Orientation"), true),
                              IsFixed = e.AttributeValueNull("IsFixed").ConvertToBoolean(),
                              ViewArea = new Rectangle(e.Element("ViewTarget").AttributeValueNull("X").ConvertToInt(),
                                                       e.Element("ViewTarget").AttributeValueNull("Y").ConvertToInt(),
                                                       e.Element("ViewTarget").AttributeValueNull("Width").ConvertToInt(),
                                                       e.Element("ViewTarget").AttributeValueNull("Height").ConvertToInt()),
                              ScrollArea = new Rectangle(e.Element("ScrollTarget").AttributeValueNull("X").ConvertToInt(),
                                                         e.Element("ScrollTarget").AttributeValueNull("Y").ConvertToInt(),
                                                         e.Element("ScrollTarget").AttributeValueNull("Width").ConvertToInt(),
                                                         e.Element("ScrollTarget").AttributeValueNull("Height").ConvertToInt()),
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
                          } as UIScrollBar).ToList<UIScrollBar>();

            return ScrollBars;
        }
    }
}
