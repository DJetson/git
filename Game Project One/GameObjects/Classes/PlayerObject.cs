using GameObjects.Controls;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace GameObjects.Classes
{
    public class PlayerObject : GameObject
    {

        public DispatcherTimer ReloadTimer;
        private Boolean Reloading = false;

        public override void Update(long ElapsedTime)
        {
            NotifyPropertyChanged("ElapsedTime");
            Adjustment = new Vector(0, 0);
            Acceleration.Current = NextAcceleration;
            Velocity.Current = NextVelocity;
            Position.Current = NextPosition;
            Vector p = new Vector(Position.Current.X + ((WorldObject.CurrentWorld.Camera.ViewportSize.Width / 2) * -WorldObject.CurrentWorld.Camera.Zoom.X),
                                  Position.Current.Y + ((WorldObject.CurrentWorld.Camera.ViewportSize.Height / 2) * -WorldObject.CurrentWorld.Camera.Zoom.Y));
            WorldObject.CurrentWorld.Camera.Position = p;// new Vector(p.X * WorldObject.CurrentWorld.Camera.Zoom.X, p.Y * WorldObject.CurrentWorld.Camera.Zoom.Y);
            Angle = NextAngle;

            GetGamepadInput();
            GetKeyboardInput();

            LastTime = DateTime.Now.Ticks;
        }

        #region XInput

        public void GetKeyboardInput()
        {
            if (WorldObject.PlayerOneInput.Connected == true)
                return;
            Velocity.Current = new Vector(0, 0);
            if (WorldObject.PlayerOneKeyInput.IsLeftKeyDown)
                Velocity.Current += new Vector(-0.0001, 0);
            if (WorldObject.PlayerOneKeyInput.IsRightKeyDown)
                Velocity.Current += new Vector(0.0001, 0);
            if (WorldObject.PlayerOneKeyInput.IsUpKeyDown)
                Velocity.Current += new Vector(0, -0.0001);
            if (WorldObject.PlayerOneKeyInput.IsDownKeyDown)
                Velocity.Current += new Vector(0, 0.0001);

            Vector ProjectileVel = new Vector(0, 0);
            if (WorldObject.PlayerOneKeyInput.IsFireLeftKeyDown)
                ProjectileVel += new Vector(-0.0001, 0);
            if (WorldObject.PlayerOneKeyInput.IsFireRightKeyDown)
                ProjectileVel += new Vector(0.0001, 0);
            if (WorldObject.PlayerOneKeyInput.IsFireUpKeyDown)
                ProjectileVel += new Vector(0, -0.0001);
            if (WorldObject.PlayerOneKeyInput.IsFireDownKeyDown)
                ProjectileVel += new Vector(0, 0.0001);

            if (ProjectileVel.Length != 0 && Reloading == false)
            {
                Vector ProjectileDirection = ProjectileVel;
                ProjectileDirection.Normalize();
                ProjectileObject p = new ProjectileObject(Position.Current + new Vector(CenterX - 5, CenterY - 5) + new Vector(Size.X * ProjectileDirection.X, Size.Y * ProjectileDirection.Y), ProjectileVel);
                ProjectileControl projectile = new ProjectileControl() { DataContext = p };
                WorldObject.CurrentWorld.AddObject(projectile);
                Reloading = true;
            }
        }

        public void GetGamepadInput()
        {
            if (WorldObject.PlayerOneInput.Connected == false)
                return;

            Velocity.Current = new Vector(WorldObject.PlayerOneInput.LeftStick.Position.X / 10000,
                                      -WorldObject.PlayerOneInput.LeftStick.Position.Y / 10000);

            if (WorldObject.PlayerOneInput.RightTrigger > 0)
            {
                Vector2 RightStickDirection = new Vector2(WorldObject.PlayerOneInput.RightStick.Position.X, -WorldObject.PlayerOneInput.RightStick.Position.Y);
                RightStickDirection.Normalize();
                if (RightStickDirection.Length() == 0)
                    return;
                //Fill = new SolidColorBrush(Colors.Green);
                if (Reloading == false)
                {
                    ProjectileObject p = new ProjectileObject(Position.Current + new Vector(CenterX - 5, CenterY - 5) + new Vector(Size.X * RightStickDirection.X, Size.Y * RightStickDirection.Y),
                                                              new Vector(RightStickDirection.X / 10000,
                                                                         RightStickDirection.Y / 10000));
                    //TODO: Create a Projectile Control and use it with the projectile object created above
                    ProjectileControl projectile = new ProjectileControl() { DataContext = p };
                    WorldObject.CurrentWorld.AddObject(projectile);
                    Reloading = true;
                }
            }
            else if (WorldObject.PlayerOneInput.B == true)
            {
                Fill = new SolidColorBrush(Colors.Red);
            }
            else if (WorldObject.PlayerOneInput.X == true)
            {
                Fill = new SolidColorBrush(Colors.Blue);
            }
            else if (WorldObject.PlayerOneInput.Y == true)
            {
                Fill = new SolidColorBrush(Colors.Yellow);
            }
            else
            {
                Fill = new SolidColorBrush(Colors.Black);
            }
        }

        #endregion XInput

        public PlayerObject()
        {
            Position = new BoundedVector(100, 100);
            LastTime = DateTime.Now.Ticks;
            ReloadTimer = new DispatcherTimer() { Interval = new TimeSpan(1250000) };
            ReloadTimer.Tick += ReloadTimer_Tick;
            ReloadTimer.Start();
        }

        void ReloadTimer_Tick(object sender, EventArgs e)
        {
            Reloading = false;
        }


    }
}
