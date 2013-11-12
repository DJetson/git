using GPOne.BaseClasses;
using GPOne.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GPOne.Objects
{
    public class CharacterObject : CharacterBase, IReceivesInput, IAnimated, INotifyPropertyChanged
    {

        private Boolean IsFalling = false;
        private double JumpValue = -0.00004;

        public CharacterObject()
            : base()
        {
            InitializeAnimations();
        }

        #region IReceivesInput Implementation
        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            Vector NewAcceleration = new Vector(Acceleration.Current.X, Acceleration.Current.Y);
            if (e.Key == Key.A)
            {
                NewAcceleration.X = -MaxAcceleration;
                CurrentAnimation.IsReversed = true;
                CurrentAnimation.Play(true);
            }
            if (e.Key == Key.D)
            {
                NewAcceleration.X = +MaxAcceleration;
                CurrentAnimation.IsReversed = false;
                CurrentAnimation.Play(true);
            }
            if (e.Key == Key.W)
                NewAcceleration.Y = -MaxAcceleration;
            if (e.Key == Key.S)
                NewAcceleration.Y = +MaxAcceleration;

            if (e.Key == Key.Space)
            {
                Velocity.Current = Jump();
            }

            NextAcceleration = NewAcceleration;
        }

        public override void OnKeyUp(object sender, KeyEventArgs e)
        {
            Vector NewAcceleration = new Vector(Acceleration.Current.X, Acceleration.Current.Y);
            if ((e.Key == Key.A) || (e.Key == Key.D))
            {
                NewAcceleration.X = 0;
                CurrentAnimation.Stop();
            }
            if ((e.Key == Key.W) || (e.Key == Key.S))
                NewAcceleration.Y = 0;

            if (e.Key == Key.Space)
            {
                CancelJump();
            }

            NextAcceleration = NewAcceleration;
        }
        #endregion IReceivesInput Implementation

        #region Player Actions

        private Vector Jump()
        {
            if (IsFalling == true)
                return Velocity.Current;

            if (NextVelocity.Y >= 0)
                IsFalling = true;

            return new Vector(Velocity.Current.X, Velocity.Current.Y + JumpValue);
        }

        private void CancelJump()
        {
            Velocity.Current = new Vector(Velocity.Current.X, 0);
            IsFalling = true;
        }

        #endregion

        public override void Update(long ElapsedTime)
        {
            this.NextAcceleration += WorldObject.Gravity;
            base.Update(ElapsedTime);

            if (Acceleration.Current.Y > 0)
                IsFalling = true;
            Adjustment = new Vector(0, 0);

            if (Bottom >= WorldObject.CurrentWorld.Bounds.Bottom)
            {
                Adjustment.Y = WorldObject.CurrentWorld.Bounds.Bottom - Bottom;
                Velocity.Current = new Vector(Velocity.Current.X, 0);
                Acceleration.Current = new Vector(Acceleration.Current.X, 0);
                if (IsFalling == true)
                    IsFalling = false;
            }
            else if (Top <= WorldObject.CurrentWorld.Bounds.Top)
            {
                Adjustment.Y = WorldObject.CurrentWorld.Bounds.Top - Top;
                Velocity.Current = new Vector(Velocity.Current.X, 0);
                Acceleration.Current = new Vector(Acceleration.Current.X, 0);
            }

            if (Right >= WorldObject.CurrentWorld.Bounds.Right)
            {
                Adjustment.X = WorldObject.CurrentWorld.Bounds.Right - Right;
                Velocity.Current = new Vector(0, Velocity.Current.Y);
                Acceleration.Current = new Vector(0, Acceleration.Current.Y);
            }
            else if (Left <= WorldObject.CurrentWorld.Bounds.Left)
            {
                Adjustment.X = WorldObject.CurrentWorld.Bounds.Left - Left;
                Velocity.Current = new Vector(0, Velocity.Current.Y);
                Acceleration.Current = new Vector(0, Acceleration.Current.Y);
            }

            this.Position.Current += Adjustment;


        }

        #region IAnimated Implementation

        private AnimationSet _AnimationSet = new AnimationSet();
        public AnimationSet AnimationSet
        {
            get { return _AnimationSet; }
            set { _AnimationSet = value; NotifyPropertyChanged("AnimationSet"); }
        }

        private AnimatedClip _CurrentAnimation;
        public AnimatedClip CurrentAnimation
        {
            get { return _CurrentAnimation; }
            set { _CurrentAnimation = value; NotifyPropertyChanged("CurrentAnimation"); }
        }

        public void InitializeAnimations()
        {
            AnimatedClip NewAnimation = new AnimatedClip("Running", 
                "C:/Users/dmalicoat/Pictures/Animations/StickAnimation-Running.png", 
                192, 
                256);
            AnimationSet.AddAnimation(NewAnimation);
            CurrentAnimation = AnimationSet["Running"];
            //CurrentAnimation.Play(true);
        }

        #endregion IAnimated Implementation
    }
}
