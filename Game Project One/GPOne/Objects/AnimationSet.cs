using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GPOne.Objects
{
    [Serializable]
    public class AnimationSet : INotifyPropertyChanged
        //, INotifyCollectionChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        //#region INotifyCollectionChanged Implementation
        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        //public void NotifyCollectionChanged(NotifyCollectionChangedAction Action, object Item)
        //{
        //    if (CollectionChanged != null)
        //        CollectionChanged(this, new NotifyCollectionChangedEventArgs(Action, Item));
        //}

        //#endregion INotifyCollectionChanged Implementation

        #region AnimationSet Events
        public event EventHandler<AnimationsChangedEventArgs> AnimationsChanged;

        public void OnAnimationsChanged(AnimationsChangedEventArgs e)
        {
            EventHandler<AnimationsChangedEventArgs> handler = AnimationsChanged;
            if (handler != null) { handler(this, e); }
        }
        #endregion AnimationSet Events

        #region ISerializable Implementation
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", _Name, typeof(String));
            info.AddValue("Animations", _Animations, typeof(Dictionary<String,AnimatedClip>));
        }

        public AnimationSet(SerializationInfo info, StreamingContext context)
        {
            _Name = (string)info.GetValue("Name", typeof(string));
            _Animations = (Dictionary<String, AnimatedClip>)info.GetValue("Animations", typeof(Dictionary<String, AnimatedClip>));
            //Initialize();
            //SetInitialValues();
        }

        public void Serialize(string fileName, IFormatter formatter)
        {
            FileStream s = new FileStream(fileName, FileMode.Create,FileAccess.Write);
            formatter.Serialize(s, this);
            s.Close();
        }


        public static AnimationSet DeserializeItem(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open,FileAccess.Read);
            AnimationSet NewClip = (AnimationSet)(new BinaryFormatter()).Deserialize(fs);
            fs.Close();
            return NewClip;
            
        }
        #endregion ISerializable Implementation


        #region AnimationSet Properties
        private String _Name;
        public String Name
        {
            get { return _Name; }
            set { _Name = value; NotifyPropertyChanged("Name"); }
        }

        private String _FileName;
        public String FileName
        {
            get { return _FileName; }
            set { _FileName = value; NotifyPropertyChanged("FileName"); }
        }

        private Dictionary<String, AnimatedClip> _Animations = new Dictionary<String, AnimatedClip>();
        public Dictionary<String, AnimatedClip> Animations
        {
            get { return _Animations; }
            private set
            {
                _Animations = value;
                NotifyPropertyChanged("Animations");
            }
        }

        private List<AnimatedClip> _Modified;
        public List<AnimatedClip> Modified
        {
            get { return _Modified; }
            set { _Modified = value; NotifyPropertyChanged("Modified"); }
        }

        public AnimatedClip this[String Key]   // Indexer declaration
        {
            get
            {
                // Check the index limits.
                if (Animations.ContainsKey(Key))
                    return Animations[Key];
                else
                    return null;
            }
        }

        public AnimatedClip this[int Index]   // Indexer declaration
        {
            get
            {
                // Check the index limits.
                if (Index < 0 || Index >= Animations.Count)
                    return null;
                else
                    return Animations.ElementAt(Index).Value;
            }
        }

        #endregion AnimationSet Properties

        public AnimationSet(String AnimationSetName = "NewAnimationSet", String fileName = null)
        {
            Name = AnimationSetName;
            if (String.IsNullOrEmpty(fileName))
                FileName = Name;
            else
                FileName = fileName;

            Initialize();
        }

        private void Initialize()
        {
            //Modified = new List<AnimatedClip>();
            Animations = Animations ?? new Dictionary<String, AnimatedClip>();
            //CollectionChanged += AnimationSet_CollectionChanged;
        }

        void AnimationSet_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (AnimatedClip newItem in e.NewItems)
                {
                    Modified.Add(newItem);

                    //Add listener for each item on PropertyChanged event
                    newItem.PropertyChanged += this.PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (AnimatedClip oldItem in e.OldItems)
                {
                    Modified.Add(oldItem);

                    oldItem.PropertyChanged -= this.PropertyChanged;
                }
            }

        }

        /// <summary>
        /// Add an AnimatedClip to the AnimatationSet
        /// </summary>
        /// <param name="NewAnimation">The animation to add</param>
        public void AddAnimation(AnimatedClip NewAnimation)
        {
            AddAnimation(NewAnimation.Name, NewAnimation);
        }

        /// <summary>
        /// Add an AnimatedClip to the AnimationSet
        /// </summary>
        /// <param name="AnimationKey">The Unique Key for the AnimationClip</param>
        /// <param name="NewAnimation">The Animation to Add</param>
        /// <remarks>If the Name of the provided animation does not 
        ///          match the Key provided, the AnimatedClip's Name 
        ///          property is changed to match the Key</remarks>
        public void AddAnimation(String AnimationKey, AnimatedClip NewAnimation)
        {
            if (String.IsNullOrEmpty(AnimationKey) ||
                Animations.ContainsValue(NewAnimation) ||
                Animations.ContainsKey(AnimationKey))
                throw new Exception("All Animations must possess unique Names. AddAnimation() aborted.");

            if (AnimationKey != NewAnimation.Name)
                NewAnimation.Name = AnimationKey;

            //NewAnimation.PropertyChanged += NewAnimation_PropertyChanged;
            Animations.Add(AnimationKey, NewAnimation);
            //NotifyCollectionChanged(NotifyCollectionChangedAction.Add, NewAnimation);
        }

        //void NewAnimation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    AnimatedClip item = sender as AnimatedClip;
        //    if (item != null)
        //        Modified.Add(item);

        //}

        /// <summary>
        /// Removes an AnimationClip from the AnimationSet
        /// </summary>
        /// <param name="AnimationKey">The Key of the AnimationClip to remove</param>
        public void RemoveAnimation(String AnimationKey)
        {
            if (Animations.ContainsKey(AnimationKey) == false)
                throw new Exception("No animation has been specified. This can be caused by specifying a Key which doesn't exist");

            AnimatedClip Removed = Animations[AnimationKey];
            AnimationsChangedEventArgs EventArgs = new AnimationsChangedEventArgs() { Removed = Removed, Key = AnimationKey };
            Animations.Remove(AnimationKey);

            //Removed.PropertyChanged -= NewAnimation_PropertyChanged;
            //NotifyCollectionChanged(NotifyCollectionChangedAction.Remove, Removed);
        }

        public static AnimationSet OpenAnimationSet(String Path)
        {
            return AnimationSet.DeserializeItem(Path);
        }

        public void Serialize(String Path)
        {
            if(String.IsNullOrEmpty(Path))
            if (String.IsNullOrEmpty(Path))
            {

            }
        }

        public void Deserialize(FileStream fs)
        {
        }

    }
}
