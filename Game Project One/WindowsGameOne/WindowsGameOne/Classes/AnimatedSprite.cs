using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WindowsGameOne.Classes
{
    public enum PlayMode { Stop, Play, Pause, Continous };

    [Serializable]
    [XmlInclude(typeof(AnimatedSprite))]
    public class AnimatedSprite
    {

        /// <summary>
        /// The current state of the animation. Stop - No updates, frame counter remains at 0
        ///                                     Play - The frame counter is updated according to FPS, animation plays to the end and STOPS
        ///                                     Pause - No updates, frame counter remains wherever it was when this state was set
        ///                                     Continuous - The frame counter is updated according to FPS, the animation loops continuously
        /// </summary>
        //        public enum PlayMode { Stop, Play, Pause, Continous };

        /// <summary>
        /// The current state of the animation
        /// </summary>
        [NonSerialized]
        private PlayMode _State;
        public PlayMode State
        {
            get { return _State; }
            set
            {
                if (value == PlayMode.Stop) Stop();
                else if (value == PlayMode.Continous) Play();
                else if (value == PlayMode.Play) Play(false);
                else Pause();
            }
        }

        /// <summary>
        /// Whether the animation should play in reverse
        /// </summary>
        public Boolean IsReversed;

        /// <summary>
        /// The Height of the Sprite Sheet
        /// </summary>
        public double SheetHeight;

        /// <summary>
        /// The Width of the Sprite Sheet
        /// </summary>
        public double SheetWidth;

        /// <summary>
        /// The width of each frame of the animation
        /// </summary>
        public double FrameWidth;

        /// <summary>
        /// The height of each frame of the animation
        /// </summary>
        public double FrameHeight;

        /// <summary>
        /// The total number of frames in the animation
        /// </summary>
        public int FrameCount;

        /// <summary>
        /// The playback speed of the animation
        /// </summary>
        private double _FPS;
        public double FPS
        {
            get { return _FPS; }
            set
            {
                if (value < 1)
                    _FPS = 1;
                else
                    _FPS = value;
            }
        }

        /// <summary>
        /// A multiplier for FPS used to calculate FrameDuration. 
        /// </summary>
        private double _PlaybackSpeed = 0;
        private double PlaybackSpeed
        {
            get { return _PlaybackSpeed; }
            set { if (value < 0) _PlaybackSpeed = 0; else if (value > 1)_PlaybackSpeed = 1; else _PlaybackSpeed = value; }
        }
        /// <summary>
        /// Linear easing function. Accepts a number between 0 and 1
        /// NOTE: This needs serious work
        /// </summary>
        private double _Easing = 1;
        public double Easing
        {
            get { return _Easing; }
            set { if (value < 0) _Easing = 0; else if (value > 1) _Easing = 1; else _Easing = value; }
        }

        private double FrameDuration
        {
            get
            {
                PlaybackSpeed += FPS * Easing;
                return (10000000 / FPS) / PlaybackSpeed;
            }
        }

        /// <summary>
        /// The image resource used by this animation
        /// </summary>
        [NonSerialized]
        public SpriteBatch SpriteSheet;

        /// <summary>
        /// The rendering surface
        /// </summary>
        [NonSerialized]
        public Texture2D Texture;

        /// <summary>
        /// The index of the current frame
        /// </summary>
        [NonSerialized]
        public int CurrentFrameIndex = 0;

        /// <summary>
        /// The source rectangle for the current frame
        /// </summary>
        [NonSerialized]
        public Rectangle? CurrentFrame;

        /// <summary>
        /// The string resource identifier of the animation
        /// </summary>
        public String ResourceName;

        /// <summary>
        /// Whether the image resource for this animation is currently loaded.
        /// </summary>
        [NonSerialized]
        public Boolean IsLoaded = false;

        [NonSerialized]
        private long LastUpdateTime = 0;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="resourceName">The sprite resource name</param>
        public AnimatedSprite(String resourceName = "")
        {
            ResourceName = resourceName;
        }

        /// <summary>
        /// Constructor which takes everything necessary to create a new animation that is ready to be loaded.
        /// </summary>
        /// <param name="resourceName">The name of the sprite resource used by the animation</param>
        /// <param name="frameWidth">The width of the frames in the animation</param>
        /// <param name="frameHeight">The height of the frames in the animation</param>
        /// <param name="sheetWidth">The width of the entire sprite image</param>
        /// <param name="sheetHeight">The height of the entire sprite image</param>
        /// <param name="frameCount">The number of frames in the animation</param>
        /// <param name="fps">The playback speed (frames per second)</param>
        public AnimatedSprite(String resourceName, double frameWidth, double frameHeight,
                                double sheetWidth, double sheetHeight, int frameCount, int fps)
        {
            ResourceName = resourceName;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            SheetWidth = sheetWidth;
            SheetHeight = sheetHeight;
            FrameCount = frameCount;
            FPS = fps;
        }

        /// <summary>
        /// Initializes the Animation
        /// </summary>
        /// <param name="graphics">The graphics device used to render the sprites</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the supplied GraphicsDevice is null</exception>
        public void Initialize(GraphicsDevice graphics)
        {
            try { SpriteSheet = new SpriteBatch(graphics); }
            catch (ArgumentNullException a) { throw new NullReferenceException("AnimatedSprite failed to initialize a SpriteBatch.", a); }
        }

        /// <summary>
        /// Loads the sprite resource
        /// </summary>
        /// <param name="content">The content manager which is responsible for the resource</param>
        /// <exception cref="NullReferenceException">Thrown when the supplied ContentManager is null</exception>
        /// <exception cref="ContentLoadException">Thrown when the ResourceName identifies a resource that is incompatible with the type being loaded"/></exception>
        public void Load(ContentManager content)
        {
            if (content == null)
                throw new NullReferenceException(String.Format("Could not Load the AnimatedSpriteResource \"{0}\" because the ContentManager is null.", ResourceName));

            try { Texture = content.Load<Texture2D>(ResourceName); }
            catch (ContentLoadException c)
            {
                throw new ContentLoadException("AnimatedSprite failed to load a resource.", c);
            }

            UpdateDrawRectangle();
        }

        /// <summary>
        /// Advance the animation by a number of frames
        /// </summary>
        /// <param name="Frames">The number of frames to advance the animation</param>
        public void StepForward(int Frames = 1)
        {
            CurrentFrameIndex = (CurrentFrameIndex + Frames) % FrameCount;
            if (State == PlayMode.Play && CurrentFrameIndex == FrameCount)
                State = PlayMode.Stop;
        }

        /// <summary>
        /// Move the animation backwards by a number of frames
        /// </summary>
        /// <param name="Frames">The number of frames to move the animation backward</param>
        public void StepBackward(int Frames = 1)
        {
            CurrentFrameIndex = ((CurrentFrameIndex - Frames) + FrameCount) % FrameCount;
            if (State == PlayMode.Play && CurrentFrameIndex == 0)
                State = PlayMode.Stop;
        }

        /// <summary>
        /// Play the animation
        /// </summary>
        /// <param name="Continuous">Whether to play the animation continuously or just once</param>
        public void Play(Boolean Continuous = true)
        {
            if (Continuous)
                _State = PlayMode.Continous;
            else
                _State = PlayMode.Play;
        }

        /// <summary>
        /// Pauses the animation. This retains the current frames position.
        /// </summary>
        public void Pause()
        {
            _State = PlayMode.Pause;
        }

        /// <summary>
        /// Stops the animation. This resets the current frame to the first frame
        /// </summary>
        public void Stop()
        {
            _State = PlayMode.Stop;
            CurrentFrameIndex = 0;
            PlaybackSpeed = 0;
        }

        /// <summary>
        /// Update the current frame of the animation
        /// </summary>
        /// <param name="ElapsedTicks">The amount of time since the last update</param>
        public void Update(GameTime gameTime)
        {
            LastUpdateTime += gameTime.ElapsedGameTime.Ticks;
            if (State == PlayMode.Pause || State == PlayMode.Stop)
                return;
            //double FrameDuration = 10000000 / FPS;

            if (LastUpdateTime > FrameDuration)
            {
                if (IsReversed)
                {
                    StepBackward((int)(LastUpdateTime / FrameDuration));
                }
                else
                {
                    StepForward((int)(LastUpdateTime / FrameDuration));
                }
                UpdateDrawRectangle();
                LastUpdateTime = 0;
            }
        }

        public void UpdateDrawRectangle()
        {
            int FramesPerRow = (int)(SheetWidth / FrameWidth);
            int FramesPerColumn = (int)(SheetHeight / FrameHeight);

            int XIndex = (int)(CurrentFrameIndex % FramesPerRow);
            int YIndex = (int)(CurrentFrameIndex / FramesPerRow);

            CurrentFrame = new Rectangle((int)(XIndex * FrameWidth), (int)(YIndex * FrameHeight), (int)FrameWidth, (int)FrameHeight);
        }

        public void SerializeXML(StreamWriter file)
        {
            if (file == null)
                throw new Exception("SerializeXML failed: The Filestream was null.");

            XmlSerializer writer = new XmlSerializer(typeof(AnimatedSprite));

            writer.Serialize(file, this);
        }
    }
}
