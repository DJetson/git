using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GPOne.Objects
{
    public static class LaunchConditions
    {
        private static Rect GetWorldBounds()
        {
            return WorldObject.CurrentWorld.Bounds;
        }

        private static Vector GetWorldMinimum()
        {
            return new Vector(GetWorldBounds().Left, GetWorldBounds().Top);
        }

        private static Vector GetWorldMaximum()
        {
            return new Vector(GetWorldBounds().Right, GetWorldBounds().Bottom);
        }
        public static Vector GetRandomPosition()
        {

            Random Generator = new Random((int)DateTime.Now.Ticks);

            int WorldTop = (int)GetWorldMinimum().Y;
            int WorldLeft = (int)GetWorldMinimum().X;
            int WorldBottom = (int)GetWorldMaximum().Y;
            int WorldRight = (int)GetWorldMaximum().X;

            Vector Position = new Vector(Generator.Next(WorldLeft,WorldRight),Generator.Next(WorldTop,WorldBottom));

            return Position;
        }
    }
}
