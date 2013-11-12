using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGameOne.BaseClasses;

namespace WindowsGameOne.Classes
{
    public enum CameraValueType { Position, Zoom, ViewSize};

    public class CameraMovedEventArgs : EventArgs
    {
        public CameraObject Camera { get; set; }
        public CameraValueType ValueTypeChanged { get; set; }
    }

    public class CameraObject
    {
        public static event EventHandler<CameraMovedEventArgs> CameraChanged;
        public static void OnCameraChanged(CameraMovedEventArgs e)
        {
            EventHandler<CameraMovedEventArgs> handler = CameraChanged;
            if (handler != null) { handler(_Camera, e); }
        }

        private Vector2 _ViewportSize;
        public Vector2 ViewportSize
        {
            get { return _ViewportSize; }
            set { _ViewportSize = value; }
        }

        private static CameraObject _Camera;
        public static CameraObject Camera
        {
            get { return _Camera; }
        }

        protected float _Zoom;
        public float Zoom
        {
            get { return _Zoom; }
            set { _Zoom = value; if (_Zoom < 0.1f) _Zoom = 0.1f; }
        }

        protected float _Rotation;
        public float Rotation
        {
            get { return _Rotation; }
            set { _Rotation = value; }
        }

        private Vector2 _Position;
        public Vector2 Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        private Rectangle _View;
        public Rectangle View
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)ViewportSize.X, (int)ViewportSize.Y); }
        }


        public Matrix _Transform;
        private GraphicsDevice _GraphicsDevice;


        public CameraObject(GraphicsDevice graphicsDevice, Vector2 viewportSize)
        {
            _GraphicsDevice = graphicsDevice;
            _ViewportSize = viewportSize;
            _Zoom = 1.0f;
            _Rotation = 0.0f;
            _Position = Vector2.Zero;

            _Camera = _Camera ?? this;
        }

        public void ChangeZoom(float amount)
        {
            _Zoom += amount;
            OnCameraChanged(new CameraMovedEventArgs() { Camera = _Camera, ValueTypeChanged = CameraValueType.Zoom });
        }

        public void Move(Vector2 amount)
        {
            _Position += amount;
            OnCameraChanged(new CameraMovedEventArgs() { Camera = _Camera, ValueTypeChanged = CameraValueType.Position });
        }


        public Matrix GetCameraTransform()
        {
            _Transform =
              Matrix.CreateTranslation(new Vector3(-View.Center.X, -View.Center.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(_GraphicsDevice.Viewport.Width * 0.5f, _GraphicsDevice.Viewport.Height * 0.5f, 0));
            return _Transform;
        }


    }

}
