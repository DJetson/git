using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using WindowsGameOne.BaseClasses;
using WindowsGameOne.Classes;
using WindowsGameOne.Managers;

namespace WindowsGameOne
{

    public enum GameState { MainMenu, InGame, Editor, Quit, LoadEditor, LoadGame };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ObjectManager ObjectManager;
        LoadManager LoadManager;
        //CameraObject Camera;
        private static Vector2 _ViewportSize;
        public static Vector2 ViewportSize
        {
            get { return _ViewportSize; }
        }

        private static Level _CurrentLevel;
        public static Level CurrentLevel
        {
            get { return _CurrentLevel; }
        }

        private static CameraObject _Camera;
        public static CameraObject Camera
        {
            get { return _Camera; }
        }

        GameState State = GameState.Editor;
        //GridObject Grid;

        SpriteFont TestFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Grid = new GridObject();

        }

        public ContentManager GetContentManager()
        {
            return Content;
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return GraphicsDevice;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            _Camera = new CameraObject(GraphicsDevice,new Vector2(1024,600));

            graphics.PreferredBackBufferWidth = (int)Camera.ViewportSize.X;
            graphics.PreferredBackBufferHeight = (int)Camera.ViewportSize.Y;

            graphics.ApplyChanges();

            //if (!graphics.IsFullScreen)
            //    graphics.ToggleFullScreen();
            // TODO: Add your initialization logic here
            _ViewportSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            LoadManager = new LoadManager(Content, GraphicsDevice);
            ObjectManager = new ObjectManager();
            State = GameState.LoadEditor;






            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here

            State = GameState.LoadEditor;

            //LoadManager.SetLevel("C:/DirtWare/Game Project One/WindowsGameOne/WindowsGameOne/Other/TestXML.xml");
            //_CurrentLevel = LoadManager.CurrentLevel;
            //LoadManager.LoadFromXML("C:/DirtWare/Game Project One/WindowsGameOne/WindowsGameOne/Other/TestXML.xml");

            //spriteBatch = new SpriteBatch(GraphicsDevice);
            //TestFont = Content.Load<SpriteFont>("ButtonFont");

            ////Grid.LoadResources(Content, GraphicsDevice);
            //ObjectManager.GetLoadedObjects();
            //foreach (GameObjectBase g in ObjectManager.GameObjects)
            //    if (g is AnimatedObject)
            //        (g as AnimatedObject).Play();
            ////            (ObjectManager.GameObjects[0] as AnimatedObject).Play();

        }

        /// <summary>
        /// Loads the game editor
        /// </summary>
        /// <returns>Whether the loading operations were successful</returns>
        private Boolean LoadEditor()
        {
            LoadManager.SetLevel("C:/DirtWare/Game Project One/WindowsGameOne/WindowsGameOne/Other/Editor.xml");
            _CurrentLevel = LoadManager.CurrentLevel;

            LoadManager.LoadFromXML("C:/DirtWare/Game Project One/WindowsGameOne/WindowsGameOne/Other/Editor.xml");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            TestFont = Content.Load<SpriteFont>("ButtonFont");
            try
            {
                ObjectManager.AddObject(CurrentLevel.Grid);
                ObjectManager.GetLoadedObjects(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads the game editor
        /// </summary>
        /// <returns>Whether the loading operations were successful</returns>
        private Boolean LoadGame()
        {
            LoadManager.SetLevel("C:/DirtWare/Game Project One/WindowsGameOne/WindowsGameOne/Other/Editor.xml");
            _CurrentLevel = LoadManager.CurrentLevel;

            LoadManager.LoadFromXML("C:/DirtWare/Game Project One/WindowsGameOne/WindowsGameOne/Other/Editor.xml");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            TestFont = Content.Load<SpriteFont>("ButtonFont");
            try
            {
                ObjectManager.AddObject(CurrentLevel.Grid);
                ObjectManager.GetLoadedObjects(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                LoadManager.Serialize(@"c:\Test\TestSerial.xml", ObjectManager.GameObjects);

            switch (State)
            {
                case GameState.Editor:
                    UpdateEditor(gameTime);
                    break;
                case GameState.InGame:
                    UpdateInGame(gameTime);
                    break;
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.LoadEditor:
                    if (LoadEditor())
                        State = GameState.Editor;
                    else
                        State = GameState.Quit;
                    break;
                case GameState.Quit:
                    Quit();
                    break;
            }

            UpdateCamera();

            // TODO: Add your update logic here
            ObjectManager.UpdateAll(gameTime);

            base.Update(gameTime);
        }

        protected void UpdateMainMenu(GameTime gameTime)
        {
            if (State != GameState.MainMenu)
                throw new Exception("UpdateMainMenu() Failed. Incorrect GameState");

            //Begin update routine for the main menu
        }

        protected void UpdateInGame(GameTime gameTime)
        {
            if (State != GameState.InGame)
                throw new Exception("UpdateInGame() Failed. Incorrect GameState");

            //Begin Update routine for the main game

        }

        protected void UpdateEditor(GameTime gameTime)
        {
            if (State != GameState.Editor)
                throw new Exception("UpdateEditor() Failed. Incorrect GameState");

            //Begin Update routine for the editor tools
        }

        protected void Quit()
        {
            if (State != GameState.MainMenu)
                throw new Exception("UpdateMainMenu() Failed. Incorrect GameState");

            //Quit the game
        }

        protected void UpdateCamera()
        {
            #region CameraTest DELETE THIS LATER
            Vector2 MoveCamera = new Vector2(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X,
                                             GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y);

            if (MoveCamera.X > 0.1 || MoveCamera.X < -0.1 || MoveCamera.Y > 0.1 || MoveCamera.Y < -0.1)
            {
                MoveCamera.Normalize();
                MoveCamera.X *= 5f;
                MoveCamera.Y *= -5f;

                if (MoveCamera.X != float.NaN && MoveCamera.Y != float.NaN)
                    CameraObject.Camera.Move(MoveCamera);
            }

            Vector2 ZoomCamera = new Vector2(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X,
                                             GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y);

            if (ZoomCamera.X > 0.1 || ZoomCamera.X < -0.1 || ZoomCamera.Y > 0.1 || ZoomCamera.Y < -0.1)
            {
                ZoomCamera.Normalize();
                ZoomCamera.X *= 5f;
                ZoomCamera.Y *= -5f;

                if (ZoomCamera.X != float.NaN && ZoomCamera.Y != float.NaN)
                {
                    float zoom = ZoomCamera.Length();
                    if (ZoomCamera.Y > 0)
                        zoom *= -1;
                    CameraObject.Camera.Zoom += (zoom / 100);
                }
            }
            #endregion CameraTest DELETE THIS LATER
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin();
            //spriteBatch.End();
            //Grid.Draw();

            // TODO: Add your drawing code here
            CurrentLevel.Draw();
            ObjectManager.DrawAll();
            // Draw the sprite.
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.Draw(myTexture, spritePosition, Color.White);
            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
