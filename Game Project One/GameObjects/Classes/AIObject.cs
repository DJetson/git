using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameObjects.Classes
{
    public abstract class AIObject : GameObject
    {


        public AIObject()
        {
        }

        public override void Update(long ElapsedTime)
        {
            ProcessAI();
            
            Adjustment = new Vector(0, 0);
            Acceleration.Current = NextAcceleration;
            Velocity.Current = NextVelocity;
            Position.Current = NextPosition;

            Angle = NextAngle;
        }

        public abstract void ProcessAI();

    }
}
