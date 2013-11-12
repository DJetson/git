using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaGameObjects.Classes;

namespace XnaGameObjects.Managers
{

    public enum ManagerOperation { Added, Removed, Modified };

    public class TilesetsChangedEventArgs : EventArgs
    {
        public Guid Key { get; set; }
        public Texture2D Tileset { get; set; }
        public Tileset Set { get; set; }
        public ManagerOperation Operation { get; set; }
    }

    public class TileManager
    {
        #region TileManager Events
        public static event EventHandler<TilesetsChangedEventArgs> TilesetsChanged;
        private static void OnTilesetsChanged(TilesetsChangedEventArgs e)
        {
            EventHandler<TilesetsChangedEventArgs> handler = TilesetsChanged;
            if (handler != null) { handler(_TileManager, e); }
        }

        private static void RaiseTilesetsChanged(Guid key, Texture2D tileset, Tileset set, ManagerOperation operation)
        {
            OnTilesetsChanged(new TilesetsChangedEventArgs() { Key = key, Tileset = tileset, Set = set, Operation = operation });
        }
        #endregion TileManager Events

        #region TileManager Properties
        private static TileManager _TileManager;

        private Dictionary<Guid, Tileset> _Sets = new Dictionary<Guid,Tileset>();
        public static Dictionary<Guid, Tileset> Sets
        {
            get { return _TileManager._Sets; }
        }
        #endregion TileManager Properties


        public TileManager()
        {
            _TileManager = _TileManager ?? this;
        }

        //public static void AddTileset(String key, Texture2D Texture)
        //{
        //    _TileManager._Tilesets = _TileManager._Tilesets ?? new Dictionary<String, Texture2D>();
        //    if (String.IsNullOrEmpty(key))
        //        throw new Exception("AddTileset failed. The supplied Texture key is invalid");
        //    if (Texture == null)
        //        throw new Exception("AddTileset failed. The supplied Texture is invalid");
        //    if (_TileManager._Tilesets.Keys.Contains(key))
        //        throw new Exception("AddTileset failed. Tileset already exists");

        //    _TileManager._Tilesets.Add(key, Texture);
        //    RaiseTilesetsChanged(key, Texture, null, ManagerOperation.Added);
        //}

        public static void AddTileset(Guid key, Tileset Tiles)
        {
            _TileManager._Sets = _TileManager._Sets ?? new Dictionary<Guid, Tileset>();
            if (key == Guid.Empty)
                throw new Exception("AddTileset failed. The supplied Texture key is invalid");
            if (Tiles == null)
                throw new Exception("AddTileset failed. The supplied Texture is invalid");
            //if (_TileManager._Sets.Keys.Contains(key))
            //    throw new Exception("AddTileset failed. Tileset already exists");

            _TileManager._Sets.Add(key, Tiles);
            RaiseTilesetsChanged(key, null, Tiles, ManagerOperation.Added);
        }


        //public static void RemoveTileset(String key)
        //{
        //    _TileManager._Tilesets = _TileManager._Tilesets ?? new Dictionary<String, Texture2D>();
        //    if (_TileManager._Tilesets.Keys.Contains(key) == false)
        //        throw new Exception("RemoveTileset failed. The specified Texture key could not be found.");

        //    Texture2D tileset = _TileManager._Tilesets[key];
        //    _TileManager._Tilesets.Remove(key);
        //    RaiseTilesetsChanged(key, tileset, null, ManagerOperation.Removed);
        //}

        public static void RemoveTileset(Guid key)
        {
            _TileManager._Sets = _TileManager._Sets ?? new Dictionary<Guid, Tileset>();
            if (_TileManager._Sets.Keys.Contains(key) == false)
                throw new Exception("RemoveTileset failed. The specified Texture key could not be found.");

            Tileset tileset = _TileManager._Sets[key];
            _TileManager._Sets.Remove(key);
            RaiseTilesetsChanged(key, null, tileset, ManagerOperation.Removed);
        }
    }
}
