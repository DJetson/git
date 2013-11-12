using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WindowsGameOne.BaseClasses;
using WindowsGameOne.Classes;

namespace WindowsGameOne.Managers
{
    /// <summary>
    /// This class is responsible for loading level data
    /// </summary>
    public class LoadManager
    {
        private static Level _CurrentLevel;
        private static LoadManager _LoadManager;
        private static List<GameObjectBase> _GameObjects;
        private static ContentManager _Content;
        private static GraphicsDevice _GraphicsDevice;
        private static Boolean _IsLoadComplete;

        public static Level CurrentLevel
        {
            get { return _CurrentLevel; }
        }

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
            if (Content == null)
                throw new NullReferenceException("LoadManager could not be initialized. ContentManager cannot be null. Make sure that the LoadManager is created by the main Game class Initialize() method.");
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

        public static void Serialize(String FileName, List<GameObjectBase> SerializeList)
        {
            StreamWriter file = new StreamWriter(FileName);
            foreach (GameObjectBase g in SerializeList)
            {
                g.SerializeXML(file);
            }
            file.Close();
        }

        private static void ParseXML(String Path)
        {
            var XML = XDocument.Load(Path);

            ///Make sure the GameObjects collection is initialized
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();

            _GameObjects = _GameObjects.Union<GameObjectBase>(GridObject.Load(XML)).ToList<GameObjectBase>();
            _GameObjects = _GameObjects.Union<GameObjectBase>(AnimatedObject.Load(XML)).ToList<GameObjectBase>();
            _GameObjects = _GameObjects.Union<GameObjectBase>(UIFrame.Load(XML)).ToList<GameObjectBase>();
            _GameObjects = _GameObjects.Union<GameObjectBase>(UIButton.Load(XML)).ToList<GameObjectBase>();
            _GameObjects = _GameObjects.Union<GameObjectBase>(UIScrollBar.Load(XML)).ToList<GameObjectBase>();
            ///This needs to be refactored a bit
            //_GameObjects = (from e in XML.Descendants("Object")
            //                where (string)e.Attribute("Type") == ("AnimatedObject")
            //                select new AnimatedObject()
            //                {
            //                    Position = new Vector2((float)e.Element("Position").Attribute("X"),
            //                                           (float)e.Element("Position").Attribute("Y")),
            //                    Size = new Vector2((float)e.Element("Size").Attribute("Width"),
            //                                       (float)e.Element("Size").Attribute("Height")),
            //                    CurrentAnimation = (string)e.Element("Animations").Attribute("CurrentAnimation"),
            //                    Animations = (from a in e.Descendants("Animation")
            //                                  select new
            //                                  {
            //                                      Key = (string)a.Attribute("Key"),
            //                                      Value = new AnimatedSprite((string)a.Attribute("Key"),
            //                                          (double)a.Element("FrameSize").Attribute("Width"),
            //                                          (double)a.Element("FrameSize").Attribute("Height"),
            //                                          (double)a.Element("SheetSize").Attribute("Width"),
            //                                          (double)a.Element("SheetSize").Attribute("Height"),
            //                                          (int)a.Attribute("FrameCount"),
            //                                          (int)a.Attribute("FPS"))
            //                                  }).ToDictionary(g => g.Key, g => g.Value)
            //                } as GameObjectBase
            //                ).Union<GameObjectBase>(
            //                (from e in XML.Descendants("Object")
            //                 where (string)e.Attribute("Type") == ("UIFrame")
            //                 select new UIFrame()
            //                 {
            //                     Position = new Vector2((float)e.Element("Position").Attribute("X"),
            //                                            (float)e.Element("Position").Attribute("Y")),
            //                     Size = new Vector2((float)e.Element("Size").Attribute("Width"),
            //                                        (float)e.Element("Size").Attribute("Height")),
            //                     Fill = new Color(
            //                                (int)(e.Element("UIFrame").Element("Fill").Attribute("R")),
            //                                (int)(e.Element("UIFrame").Element("Fill").Attribute("G")),
            //                                (int)(e.Element("UIFrame").Element("Fill").Attribute("B")),
            //                                (int)(e.Element("UIFrame").Element("Fill").Attribute("A"))),
            //                     Stroke = new Color((int)e.Element("UIFrame").Element("Stroke").Attribute("R"),
            //                                        (int)e.Element("UIFrame").Element("Stroke").Attribute("G"),
            //                                        (int)e.Element("UIFrame").Element("Stroke").Attribute("B"),
            //                                        (int)e.Element("UIFrame").Element("Stroke").Attribute("A")),
            //                     StrokeThickness = (double)e.Element("UIFrame").Attribute("StrokeThickness"),
            //                     ResourceName = (string)e.Element("UIFrame").Attribute("ResourceName")
            //                 } as UIFrame).Union<GameObjectBase>(
            //                 (from e in XML.Descendants("Object")
            //                  where (string)e.Attribute("Type") == ("UIButton")
            //                  select new UIButton()
            //                  {
            //                      Position = new Vector2((float)e.Element("Position").Attribute("X"),
            //                                             (float)e.Element("Position").Attribute("Y")),
            //                      Size = new Vector2((float)e.Element("Size").Attribute("Width"),
            //                                         (float)e.Element("Size").Attribute("Height")),
            //                      Fill = new Color((int)(e.Element("UIFrame").Element("Fill").Attribute("R")),
            //                                       (int)(e.Element("UIFrame").Element("Fill").Attribute("G")),
            //                                       (int)(e.Element("UIFrame").Element("Fill").Attribute("B")),
            //                                       (int)(e.Element("UIFrame").Element("Fill").Attribute("A"))),
            //                      Stroke = new Color((int)e.Element("UIFrame").Element("Stroke").Attribute("R"),
            //                                         (int)e.Element("UIFrame").Element("Stroke").Attribute("G"),
            //                                         (int)e.Element("UIFrame").Element("Stroke").Attribute("B"),
            //                                         (int)e.Element("UIFrame").Element("Stroke").Attribute("A")),
            //                      StrokeThickness = (double)e.Element("UIFrame").Attribute("StrokeThickness"),
            //                      ResourceName = (string)e.Element("UIFrame").Attribute("ResourceName"),
            //                      FontColor = new Color((int)(e.Element("UIButton").Element("FontColor").Attribute("R")),
            //                                            (int)(e.Element("UIButton").Element("FontColor").Attribute("G")),
            //                                            (int)(e.Element("UIButton").Element("FontColor").Attribute("B")),
            //                                            (int)(e.Element("UIButton").Element("FontColor").Attribute("A"))),
            //                      ButtonText = (string)e.Element("UIButton").Attribute("Text")
            //                  } as UIButton).Union<GameObjectBase>(
            //                 (from e in XML.Descendants("Object")
            //                  where (string)e.Attribute("Type") == ("GridObject")
            //                  select new GridObject()
            //                  {
            //                      GridSize = new Vector2((float)e.Element("GridSize").Attribute("Width"),
            //                                             (float)e.Element("GridSize").Attribute("Height")),
            //                      CellSize = new Vector2((float)e.Element("CellSize").Attribute("Width"),
            //                                             (float)e.Element("CellSize").Attribute("Height")),
            //                      GridColor = new Color((int)(e.Element("GridLines").Element("Color").Attribute("R")),
            //                                            (int)(e.Element("GridLines").Element("Color").Attribute("G")),
            //                                            (int)(e.Element("GridLines").Element("Color").Attribute("B")),
            //                                            (int)(e.Element("GridLines").Element("Color").Attribute("A"))),
            //                      LineThickness = (float)e.Element("GridLines").Attribute("LineThickness")
            //                  } as GridObject)))
            //        ).ToList<GameObjectBase>();
        }

        /// <summary>
        /// Clears the GameObjects collection
        /// </summary>
        public static void ClearObjects()
        {
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();
            _GameObjects.Clear();
        }

        public static void SetLevel(String Path)
        {
            var XML = XDocument.Load(Path);

            ///Make sure the GameObjects collection is initialized
            _CurrentLevel = Level.Load(XML);
            _CurrentLevel.LoadResources(Content, GraphicsDevice);
        }
    }
}
