using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GameObjects.Classes
{
    public class KeyboardInput
    {
        public Key LeftKey = Key.A;
        public Key RightKey = Key.D;
        public Key UpKey = Key.W;
        public Key DownKey = Key.S;
        public Key FireUpKey = Key.Up;
        public Key FireDownKey = Key.Down;
        public Key FireLeftKey = Key.Left;
        public Key FireRightKey = Key.Right;

        public Boolean IsLeftKeyDown = false;
        public Boolean IsRightKeyDown = false;
        public Boolean IsUpKeyDown = false;
        public Boolean IsDownKeyDown = false;
        public Boolean IsFireUpKeyDown = false;
        public Boolean IsFireDownKeyDown = false;
        public Boolean IsFireLeftKeyDown = false;
        public Boolean IsFireRightKeyDown = false;

        public Vector MousePosition = new Vector(0,0);

        public KeyboardInput()
        {
        }

        public void Update()
        {
            IsLeftKeyDown = Keyboard.IsKeyDown(LeftKey);
            IsRightKeyDown = Keyboard.IsKeyDown(RightKey);
            IsUpKeyDown = Keyboard.IsKeyDown(UpKey);
            IsDownKeyDown = Keyboard.IsKeyDown(DownKey);
            IsFireLeftKeyDown = Keyboard.IsKeyDown(FireLeftKey);
            IsFireRightKeyDown = Keyboard.IsKeyDown(FireRightKey);
            IsFireUpKeyDown = Keyboard.IsKeyDown(FireUpKey);
            IsFireDownKeyDown = Keyboard.IsKeyDown(FireDownKey);

            Point p = Mouse.GetPosition(WorldObject.CurrentWorld.Control);
            MousePosition = new Vector(p.X, p.Y);
        }
    }
}
