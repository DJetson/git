using GameObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameObjects.Classes
{
    public class BasicEnemy : EnemyObject
    {

        private enum AIState { Wait, Move, Redirect };
        private double MaxSpeed = 0.00001;
        public double MaxHP = 10;

        private double _Health;
        public double Health
        {
            get { return _Health; }
            set { _Health = value; NotifyPropertyChanged("Health"); }
        }

        private AIState State = AIState.Wait;

        public override void EvaluateWorld()
        {


            SetAIState();

            if (State == AIState.Wait)
            {
                Wait();
            }
            else if (State == AIState.Move)
            {
                //Update according the current velocity
                Move();
            }
            else if (State == AIState.Redirect)
            {
                //Generate a new velocity vector
                Redirect();
            }
        }

        public override void EvaluateSelf()
        {
            DoCollisions();
        }

        public void DoCollisions()
        {
            Rect r = new Rect(new Point(Left, Top), new Point(Right, Bottom));
            foreach (IMovableObject item in WorldObject.CurrentWorld.UpdateList)
            {
                ProjectileObject p = item as ProjectileObject;
                if (p != null)
                {
                    if (r.Contains(new Point(p.Position.Current.X, p.Position.Current.Y)) == true)
                    {
                        CurrentHP -= p.Damage;
                        p.Remove();

                        if (this.CurrentHP <= 0)
                        {
                            Remove();
                            break;
                        }
                    }
                }
            }

        }

        public void Remove()
        {
            WorldObject.CurrentWorld.RemoveObject(Control);
        }

        private void SetAIState()
        {
            //Evaluate our current state and pick a new one if necessary
            //If there is no player, stop moving
            //Pick a random number from 0 to 9. If the number is greater than 8 then change our state to redirect
            Random Generator = new Random((int)DateTime.Now.Ticks);
            int roll = Generator.Next(0, 10);

            if (roll > 8)
                State = AIState.Redirect;
            else
                State = AIState.Move;

        }

        private void Wait()
        {
            //If the current velocity isn't 0, then slow down until we are stopped.
            Velocity = new BoundedVector(0, 0);
        }

        private void Move()
        {
        }

        private void Redirect()
        {
            //Pick a new direction and head that way at max speed.
            Random Generator = new Random();
            Vector newVector = new Vector((Generator.NextDouble() * 2) - 1, (Generator.NextDouble() * 2) - 1);
            newVector.Normalize();
            newVector *= MaxSpeed;
            Velocity.Current = newVector;
            //Change the state back to Move
        }
        public BasicEnemy(Vector StartPosition)
            : base(StartPosition)
        {
            State = AIState.Wait;
            CurrentHP = MaxHP;
        }
    }
}
