using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XnaGameObjects.BaseClasses;
using XnaGameObjects.Classes;

namespace XnaGameObjects.Managers
{
    /// <summary>
    /// This class is responsible for loading level data
    /// </summary>
    public class LoadManager
    {
        //private static Level _CurrentLevel;
        private static LoadManager _LoadManager;
        private static List<GameObjectBase> _GameObjects;
        private static ContentManager _Content;
        private static GraphicsDevice _GraphicsDevice;
        private static Boolean _IsLoadComplete;

        //public static Level CurrentLevel
        //{
        //    get { return _CurrentLevel; }
        //}

        /// <summary>
        /// Gets the collection of loaded objects
        /// </summary>
        public static List<GameObjectBase> GameObjects
        {
            get { return _GameObjects; }
        }

        public static ContentManager Content
        {
            get { return _Content; }
        }

        public static GraphicsDevice GraphicsDevice
        {
            get { return _GraphicsDevice; }
        }

        public static Boolean IsLoadComplete
        {
            get { return _IsLoadComplete; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoadManager(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            //if (Content == null)
            //    throw new Exception("LoadManager could not be initialized. ContentManager cannot be null. Make sure that the LoadManager is created by the main Game class Initialize() method.");
            if (GraphicsDevice == null)
                throw new NullReferenceException("LoadManager could not be initialized. GraphicsDevice cannot be null. Make sure that the LoadManager is created by the main Game class Initialize() method.");

            _LoadManager = _LoadManager ?? this;
            _Content = _Content ?? Content;
            _GraphicsDevice = _GraphicsDevice ?? GraphicsDevice;
        }

        /// <summary>
        /// Load objects from a file
        /// </summary>
        /// <param name="Path">The path of the file to load</param>
        public static void LoadFromXML(String Path)
        {
            ///Clear the current object list
            ClearObjects();

            _IsLoadComplete = false;

            ///Parse the XML File and Create Objects
            ParseXML(Path);

            ///Load the resources associated with all game objects
            LoadObjectResources();
        }

        private static void LoadObjectResources()
        {
            Boolean Loaded = true;

            foreach (var g in LoadManager.GameObjects)
            {
                if (g is VisualObject)
                    (g as VisualObject).LoadResources(Content, GraphicsDevice);

                if (g.IsLoaded == false)
                    Loaded = false;
            }

            _IsLoadComplete = Loaded;
        }

        private static void ParseXML(String Path)
        {
            var XML = XDocument.Load(Path);

            ///Make sure the GameObjects collection is initialized
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();
            _GameObjects = _GameObjects.Union<GameObjectBase>(GridObject.Load(XML)).ToList<GameObjectBase>();
        }

        /// <summary>
        /// Clears the GameObjects collection
        /// </summary>
        public static void ClearObjects()
        {
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();
            _GameObjects.Clear();
        }

        //public static void SetLevel(String Path)
        //{
        //    var XML = XDocument.Load(Path);

        //    ///Make sure the GameObjects collection is initialized
        //    _CurrentLevel = Level.Load(XML);
        //    _CurrentLevel.LoadResources(Content, GraphicsDevice);
        //}
    }
}
