using GPOne.BaseClasses;
using GPOne.Interfaces;
using GPOne.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GamepadInput
{
    public class PlayerObject : GameObject
    {

        public static PlayerObject PlayerOne = null;

        public PlayerObject()
        {
            PlayerOne = this;
        }

        protected virtual void GetGamepadInput()
        {
            Velocity.Current = new Vector(WorldObject.PlayerOneInput.LeftStick.Position.X / 10000,
                                      -WorldObject.PlayerOneInput.LeftStick.Position.Y / 10000);

            if (WorldObject.PlayerOneInput.A == true)
            {
                FireProjectile(new Vector(WorldObject.PlayerOneInput.RightStick.Position.X / 10000,
                                           -WorldObject.PlayerOneInput.RightStick.Position.Y / 10000));
                Fill = new SolidColorBrush(Colors.Green);
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

        public override void Update(long ElapsedTime)
        {
            base.Update(ElapsedTime);
            GetGamepadInput();
        }

        public void FireProjectile(Vector Trajectory)
        {
            Projectile p = new Projectile(Trajectory);
            PlayerControl b = new PlayerControl() { DataContext = p };

            ///Add the projectile to the update list
        }
    }
}
