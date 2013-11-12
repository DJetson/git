using GPOne.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GamepadInput
{
    public class Projectile : GameObject
    {
        public Projectile(Vector Trajectory)
        {
            Position.Current = PlayerObject.PlayerOne.Position.Current;
            Size = new Vector(5, 5);
            Velocity.Current = Trajectory;// new Vector(Trajectory.X / 10000, -Trajectory.Y / 10000);
            Position.Minimum = new Vector(WorldObject.CurrentWorld.Bounds.Left, WorldObject.CurrentWorld.Bounds.Top);
            Position.Maximum = new Vector(WorldObject.CurrentWorld.Bounds.Right, WorldObject.CurrentWorld.Bounds.Bottom);
        }

        public override void Update(long ElapsedTime)
        {
            //base.Update(ElapsedTime);
        }
    }
}
