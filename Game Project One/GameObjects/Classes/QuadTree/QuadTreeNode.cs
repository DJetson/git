using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace GameObjects.Classes.QuadTree
{
    public class QuadTreeNode<T> where T : GameObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        private Rect _Bounds;
        public Rect Bounds
        {
            get { return _Bounds; }
            set { _Bounds = value; NotifyPropertyChanged("Bounds"); }
        }

        private List<QuadTreeNode<T>> _Nodes = new List<QuadTreeNode<T>>(4);
        public List<QuadTreeNode<T>> Nodes
        {
            get { return _Nodes; }
            set { _Nodes = value; NotifyPropertyChanged("Nodes"); }
        }

        public Boolean IsEmpty
        {
            get { return (Bounds.IsEmpty || Nodes.Count == 0); }
        }

        private List<T> _Items = new List<T>();
        public List<T> Items
        {
            get { return _Items; }
            set { _Items = value; NotifyPropertyChanged("Item"); }
        }

        public int Count
        {
            get 
            {
                int c = 0;

                foreach (T Item in Items)
                {
                    if (Item != null)
                        c++;
                }
                foreach (QuadTreeNode<T> Node in Nodes) 
                    c += Node.Count; 
                return c;
            }
        }

        public List<T> Children
        {
            get
            {
                List<T> Items = new List<T>();
                foreach (QuadTreeNode<T> Node in Nodes)
                    Items.AddRange(Node.Children);

                Items.AddRange(Items);
                return Items;
            }
        }

        public List<T> GetAreaContents(Rect Target)
        {
            List<T> Contents = new List<T>();

            if (Items != null && Items.Count > 0)
            {
                foreach (T Item in Items)
                {
                    if (Target.IntersectsWith(Item.Bounds))
                        Contents.Add(Item);
                }
            }
            foreach (QuadTreeNode<T> Node in Nodes)
            {
                if (Node.IsEmpty)
                    continue;

                if (Node.Bounds.Contains(Target))
                {
                    Contents.AddRange(Node.GetAreaContents(Target));
                    break;
                }

                if (Target.Contains(Node.Bounds))
                {
                    Contents.AddRange(Node.Children);
                    continue;
                }

                if (Node.Bounds.IntersectsWith(Target))
                    Contents.AddRange(Node.GetAreaContents(Target));
            }
            return Contents;
        }

        public void Insert(T NewItem)
        {
            if (Bounds.Contains(NewItem.Bounds) == false)
                return;

            if (Nodes.Count == 0)
                InitializeNodes();

            foreach (QuadTreeNode<T> Node in Nodes)
            {
                if (Node.Bounds.Contains(NewItem.Bounds))
                {
                    Node.Insert(NewItem);
                    return;
                }
            }

            Items.Add(NewItem);
        }

        public void ForEach(QuadTree<T>.QuadTreeAction Action)
        {
            Action(this);

            foreach (QuadTreeNode<T> Node in Nodes)
                Node.ForEach(Action);
        }

        private void InitializeNodes()
        {
            if (Bounds.Size.Height == 0 || Bounds.Size.Width == 0)
                return;

            double ChildWidth = (Bounds.Size.Width / 2.0);
            double ChildHeight = (Bounds.Size.Height / 2.0);

            Nodes.Add(new QuadTreeNode<T>(new Rect(Bounds.Location, new Size(ChildWidth, ChildHeight))));
            Nodes.Add(new QuadTreeNode<T>(new Rect(new Point(Bounds.Left, Bounds.Top + ChildHeight), new Size(ChildWidth, ChildHeight))));
            Nodes.Add(new QuadTreeNode<T>(new Rect(new Point(Bounds.Left + ChildWidth, Bounds.Top), new Size(ChildWidth, ChildHeight))));
            Nodes.Add(new QuadTreeNode<T>(new Rect(new Point(Bounds.Left + ChildWidth, Bounds.Top + ChildHeight), new Size(ChildWidth, ChildHeight))));
        }

        public QuadTreeNode(Rect bounds)
        {
            Bounds = bounds;
        }
    }
}
