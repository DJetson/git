using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using WindowsGameOne.BaseClasses;
using WindowsGameOne.Classes;
using WindowsGameOne.Interfaces;

namespace WindowsGameOne.Managers
{
    /// <summary>
    /// This class is responsible for managing all game objects. This includes updating each
    /// object individually for each iteration of the main game loop..\
    /// </summary>
    public class ObjectManager
    {
        /// <summary>
        /// The collection of managed game objects
        /// </summary>
        private static List<GameObjectBase> _GameObjects = new List<GameObjectBase>();
        public static List<GameObjectBase> GameObjects
        {
            get { return _GameObjects; }
        }

        private static List<IVisualElement> _RenderList = new List<IVisualElement>();

        /// <summary>
        /// A singleton reference to the ObjectManager
        /// </summary>
        //private static ObjectManager Objects = null;

        //private static GameTime _GameTime;
        //public static GameTime GameTime
        //{
        //    get { return _GameTime; }
        //}

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ObjectManager()
        {
            //Initialize the singleton
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();
            _RenderList = _RenderList ?? new List<IVisualElement>();
            //Objects = Objects ?? this;
        }

        /// <summary>
        /// Gets the singleton instance of the Object Manager
        /// </summary>
        /// <returns>The Object Manager</returns>
        //public static ObjectManager GetObjectManager()
        //{
        //    return Objects ?? new ObjectManager();
        //}

        /// <summary>
        /// Adds an object to the ObjectManager
        /// </summary>
        /// <param name="NewObject">The object to add</param>
        public void AddObject(GameObjectBase NewObject)
        {
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();

            if (NewObject.IsLoaded == false)
                throw new Exception("AddObject() failed. The object could not be added because one or more of its resources wasn't properly loaded.");

            if (GameObjects.Contains(NewObject) == false)
            {
                if (NewObject is VisualObject)
                    InsertVisualObject(NewObject as VisualObject);
                else
                    GameObjects.Add(NewObject);
            }
        }

        private void InsertVisualObject(VisualObject NewObject)
        {
            int Last = 0;

            while (Last < GameObjects.Count && GameObjects[Last] is VisualObject)
                Last++;

            if (Last == 0)
            {
                GameObjects.Add(NewObject);
                return;
            }

            int j = 0;
            while (j < Last && (GameObjects[j] as VisualObject).ZIndex > NewObject.ZIndex)
                j++;

            GameObjects.Insert(j, NewObject);
        }

        /// <summary>
        /// Adds a collection of objects to the ObjectManager
        /// </summary>
        /// <param name="NewObjects">The collection of objects to add</param>
        public void AddObjects(List<GameObjectBase> NewObjects)
        {
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();

            foreach (GameObjectBase g in NewObjects)
                AddObject(g);
        }

        /// <summary>
        /// Removes an object from the ObjectManager
        /// </summary>
        /// <param name="Removed">The object to remove</param>
        public void RemoveObject(GameObjectBase Removed)
        {
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();

            GameObjects.Remove(Removed);
        }

        /// <summary>
        /// Removes a collection of objects from the ObjectManager
        /// </summary>
        /// <param name="Removed">The collection of objects to remove</param>
        public void RemoveObjects(List<GameObjectBase> Removed)
        {
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();

            foreach (GameObjectBase g in Removed)
                RemoveObject(g);
        }

        public void GetLoadedObjects(Boolean Clear = true)
        {
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();

            if (Clear)
                GameObjects.Clear();

            AddObjects(LoadManager.GameObjects);
        }

        /// <summary>
        /// Iterates through all of the managed objects and calls their individual Update methods
        /// </summary>
        public void UpdateAll(GameTime gameTime)
        {
            //_GameTime = LastUpdateTime ?? gameTime;
            _GameObjects = _GameObjects ?? new List<GameObjectBase>();

            foreach (GameObjectBase g in GameObjects)
                g.Update(gameTime);
        }

        public void DrawAll()
        {

            foreach (GameObjectBase g in GameObjects)
            {
                if (g is IVisualElement)
                    (g as IVisualElement).Draw();
            }
        }
    }
}
