using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XnaControls.Classes;

#pragma warning disable 0067


namespace XnaControls.Controls
{
    public class XNAMouseMoveEventArgs : EventArgs
    {
        public Vector2 LastPosition { get; set; }
        public Vector2 Position { get; set; }
        //TODO: Maybe add mouse buttons arg
    }

    public class XNAMouseButtonEventArgs : EventArgs
    {
        public Vector2 LastPosition { get; set; }
        public Vector2 Position { get; set; }
        public MouseButton MouseButton { get; set; }
        public ButtonState ButtonState { get; set; }
    }
    /// <summary>
    /// Interaction logic for XnaControl.xaml
    /// </summary>
    public partial class XnaControl : UserControl
    {

        #region XnaControl Routed Events
        public static readonly RoutedEvent DrawEvent = EventManager.RegisterRoutedEvent("Draw", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(XnaControl));

        // Provide CLR accessors for the event 
        public event RoutedEventHandler Draw
        {
            add { AddHandler(DrawEvent, value); }
            remove { RemoveHandler(DrawEvent, value); }
        }

        // This method raises the Tap event 
        void RaiseDrawEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(XnaControl.DrawEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion XnaControl Routed Events

        private GraphicsDeviceService graphicsService;
        private XnaImageSource imageSource;
        private Vector2 LastMousePosition = Vector2.Zero;
        private Vector2 MousePosition = Vector2.Zero;

        #region XnaControl Events
        public event EventHandler<XNAMouseMoveEventArgs> XNAMouseMove;
        public event EventHandler<XNAMouseButtonEventArgs> XNAMouseLeftButtonDown;
        public event EventHandler<XNAMouseButtonEventArgs> XNAMouseLeftButtonUp;
        public event EventHandler<XNAMouseButtonEventArgs> XNAMouseRightButtonDown;
        public event EventHandler<XNAMouseButtonEventArgs> XNAMouseRightButtonUp;

        /// <summary>
        /// Raises an event when a mouse move is detected within the XnaControl
        /// </summary>
        /// <param name="e">Current position and previous position</param>
        private void OnXNAMouseMove(XNAMouseMoveEventArgs e)
        {
            EventHandler<XNAMouseMoveEventArgs> handler = XNAMouseMove;
            if (handler != null) { handler(this, e); }
        }

        private void OnXNAMouseLeftButtonDown(XNAMouseButtonEventArgs e)
        {
            EventHandler<XNAMouseButtonEventArgs> handler = XNAMouseLeftButtonDown;
            if (handler != null) { handler(this, e); }
        }
        #endregion XnaControl Events

        /// <summary>
        /// Gets the GraphicsDevice behind the control.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsService.GraphicsDevice; }
        }

        /// <summary>
        /// Invoked when the XnaControl needs to be redrawn.
        /// </summary>
        public Action<GraphicsDevice> DrawFunction;

        public XnaControl()
        {
            InitializeComponent();

            // hook up an event to fire when the control has finished loading
            Loaded += new RoutedEventHandler(XnaControl_Loaded);
        }

        ~XnaControl()
        {
            imageSource.Dispose();

            // release on finalizer to clean up the graphics device
            if (graphicsService != null)
                graphicsService.Release();
        }

        void XnaControl_Loaded(object sender, RoutedEventArgs e)
        {
            // if we're not in design mode, initialize the graphics device
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                InitializeGraphicsDevice();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            // if we're not in design mode, recreate the 
            // image source for the new size
            if (DesignerProperties.GetIsInDesignMode(this) == false &&
                graphicsService != null)
            {
                // recreate the image source
                imageSource.Dispose();
                imageSource = new XnaImageSource(
                    GraphicsDevice, (int)ActualWidth, (int)ActualHeight);
                rootImage.Source = imageSource.WriteableBitmap;
            }

            base.OnRenderSizeChanged(sizeInfo);
        }

        private void InitializeGraphicsDevice()
        {
            if (graphicsService == null)
            {
                // add a reference to the graphics device
                graphicsService = GraphicsDeviceService.AddRef(
                    (PresentationSource.FromVisual(this) as HwndSource).Handle);

                // create the image source
                imageSource = new XnaImageSource(
                    GraphicsDevice, (int)ActualWidth, (int)ActualHeight);
                rootImage.Source = imageSource.WriteableBitmap;

                // hook the rendering event
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }

        /// <summary>
        /// Draws the control and allows subclasses to override 
        /// the default behavior of delegating the rendering.
        /// </summary>
        //protected virtual void Render()
        //{
        //    // invoke the draw delegate so someone will draw something pretty
        //    if (DrawFunction != null)
        //        DrawFunction(GraphicsDevice);
        //}

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            // set the image source render target
            GraphicsDevice.SetRenderTarget(imageSource.RenderTarget);

            // allow the control to draw
            RaiseDrawEvent();
            //            Render();

            // unset the render target
            GraphicsDevice.SetRenderTarget(null);

            // commit the changes to the image source
            imageSource.Commit();
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            MouseState State = Microsoft.Xna.Framework.Input.Mouse.GetState();
            LastMousePosition = new Vector2(MousePosition.X, MousePosition.Y);
            MousePosition = new Vector2(State.X, State.Y);
            OnXNAMouseMove(new XNAMouseMoveEventArgs() { Position = MousePosition, LastPosition = LastMousePosition });
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseState State = Microsoft.Xna.Framework.Input.Mouse.GetState();
            LastMousePosition = new Vector2(MousePosition.X, MousePosition.Y);
            MousePosition = new Vector2(State.X, State.Y);

            ButtonState MouseButtonState = State.LeftButton.HasFlag(ButtonState.Pressed) ? ButtonState.Pressed : ButtonState.Released;

            OnXNAMouseLeftButtonDown(new XNAMouseButtonEventArgs()
            {
                Position = MousePosition,
                LastPosition = LastMousePosition,
                MouseButton = MouseButton.Left,
                ButtonState = MouseButtonState
            });
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
    }
}
