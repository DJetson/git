using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameObjects.Classes.QuadTree
{
    public class QuadTree<T> where T : GameObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        private QuadTreeNode<T> _Root;
        public QuadTreeNode<T> Root
        {
            get { return _Root;}
            set { _Root = value; NotifyPropertyChanged("Root");}
        }

        private Rect _Bounds;
        public Rect Bounds
        {
            get { return _Bounds; }
            set { _Bounds = value; NotifyPropertyChanged("Bounds"); }
        }

        public int Count
        {
            get { return Root.Count; }
        }

        public QuadTree(Rect bounds)
        {
            Bounds = bounds;
            Root = new QuadTreeNode<T>(Bounds);
        }

        public void Insert(T Item)
        {
            Root.Insert(Item);
        }

        public List<T> GetAreaContents(Rect Target)
        {
            return Root.GetAreaContents(Target);
        }

        public delegate void QuadTreeAction(QuadTreeNode<T> obj);

        public void ForEach(QuadTreeAction action)
        {
            Root.ForEach(action);
        }

    }
}
