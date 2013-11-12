using GameObjects.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GameObjects.Classes
{
    public abstract class EnemyObject : AIObject
    {

        public EnemyControl Control;
        public double CurrentHP;

        public EnemyObject()
        {
            Fill = new SolidColorBrush(Colors.Red);
        }

        public EnemyObject(Vector StartPosition)
        {
            Fill = new SolidColorBrush(Colors.Red);
            Position = new BoundedVector(StartPosition.X, StartPosition.Y);
        }

        public override void ProcessAI()
        {
            EvaluateSelf();
            EvaluateWorld();
        }

        /// <summary>
        /// This function is used by subclasses to inform the AI. Different enemies will have
        /// different responses to the players position and proximity and everything else
        /// so overriding this function is where all that will be done.
        /// </summary>
        public abstract void EvaluateWorld();
        public abstract void EvaluateSelf();
    }
}
