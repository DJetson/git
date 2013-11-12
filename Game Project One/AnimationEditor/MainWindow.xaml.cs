using GPOne.Interfaces;
using GPOne.Objects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAnimated, INotifyCollectionChanged
    {

        public enum FileDialogType { None, OpenLibrary, SaveLibrary, OpenSet, SaveSet, OpenClip, SaveClip };

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        #region INotifyCollectionChanged Implementation
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void NotifyCollectionChanged(NotifyCollectionChangedAction Action, object Item)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(Action, Item));
        }

        #endregion INotifyCollectionChanged Implementation

        private AnimatedClip _CurrentAnimation;
        public AnimatedClip CurrentAnimation
        {
            get { return _CurrentAnimation; }
            set { _CurrentAnimation = value; NotifyPropertyChanged("CurrentAnimation"); }
        }

        private AnimationSet _AnimationSet = new AnimationSet();
        public AnimationSet AnimationSet
        {
            get { return _AnimationSet; }
            set { _AnimationSet = value; NotifyPropertyChanged("AnimationSet"); NotifyPropertyChanged("ClipList"); }
        }

        public ObservableCollection<KeyValuePair<String, AnimatedClip>> ClipList
        {
            get
            {
                ObservableCollection<KeyValuePair<String, AnimatedClip>> Clips = new ObservableCollection<KeyValuePair<string, AnimatedClip>>();
                foreach (KeyValuePair<String, AnimatedClip> Pair in AnimationSet.Animations)
                    Clips.Add(Pair);
                return Clips;
            }
        }

        public MainWindow()
        {
            DataContext = this;
            PropertyChanged += MainWindow_PropertyChanged;
            InitializeComponent();
            InitializeAnimations();
            //AnimationSet.CollectionChanged += AnimationSet_CollectionChanged;
            CollectionChanged += MainWindow_CollectionChanged;
        }

        void MainWindow_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("ClipList");
        }

        void AnimationSet_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("ClipList");
        }

        void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public void InitializeAnimations()
        {
            AnimationSet.AnimationsChanged += AnimationSet_AnimationsChanged;
            AnimationSet.AddAnimation("Test1", new AnimatedClip("Running",
                  "C:/Users/dmalicoat/Pictures/Animations/StickAnimation-Running.png",
                  192,
                  256));
            //AnimationSet.AddAnimation("Test2", new AnimatedClip("Running",
            //      "C:/Users/dmalicoat/Pictures/Animations/StickAnimation-Running.png",
            //      192,
            //      256));
            //AnimationSet.AddAnimation("Test3", new AnimatedClip("Running",
            //      "C:/Users/dmalicoat/Pictures/Animations/StickAnimation-Running.png",
            //      192,
            //      256));
            //AnimationSet.AddAnimation("Running2", new AnimatedClip("Running2",
            //      "C:/Users/dmalicoat/Pictures/Animations/StickAnimation-Running.png",
            //      192,
            //      256));
            //CurrentAnimation = AnimationSet["Running2"];
            NotifyPropertyChanged("ClipList");

        }

        void AnimationSet_AnimationsChanged(object sender, AnimationsChangedEventArgs e)
        {
            NotifyPropertyChanged("AnimationSet");
            NotifyPropertyChanged("ClipList");
        }

        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ListView Sender = sender as ListView;
            int SelectedIndex = Sender.SelectedIndex;
            CurrentAnimation = AnimationSet[SelectedIndex];
            Clipboard.SetImage(CurrentAnimation.SourceBitmap);
        }

        #region File IO Dialog Functions
        private void ShowOpenFileDialog(FileDialogType DialogType)
        {
            OpenFileDialog OpenFileDialog = GetOpenSetDialog();
            OpenFileDialog.Tag = DialogType;

            OpenFileDialog.FileOk += OpenFileDialog_FileOk;
            OpenFileDialog.ShowDialog();
        }

        private OpenFileDialog GetOpenSetDialog()
        {
            OpenFileDialog NewDialog = new OpenFileDialog();
            NewDialog.Filter = "Animation Set Files|*.asf";

            return NewDialog;
        }
        #endregion File IO Dialog Functions

        private void FileNewSet_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void FileMenuOpenSet_Click_1(object sender, RoutedEventArgs e)
        {
            ShowOpenFileDialog(FileDialogType.OpenSet);
        }

        void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            OpenFileDialog Sender = sender as OpenFileDialog;

            if ((FileDialogType)Sender.Tag == FileDialogType.OpenSet)
                AnimationSet = AnimationSet.OpenAnimationSet(Sender.FileName);
        }

        private void FileMenuSaveClipAs_Click_1(object sender, RoutedEventArgs e)
        {
            if (AnimationList.SelectedIndex < 0 || AnimationList.SelectedIndex > AnimationSet.Animations.Count)
                return;

            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.FileName = AnimationSet[AnimationList.SelectedIndex].Name;
            SaveDialog.Filter = "Animation Clip File|*.acf";
            SaveDialog.AddExtension = true;
            SaveDialog.DefaultExt = ".acf";
            SaveDialog.FileOk += SaveDialog_FileOk;
            SaveDialog.ShowDialog(this as Window);
        }

        void SaveDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            CurrentAnimation.Serialize((sender as SaveFileDialog).FileName);
        }

        private void FileMenuOpenClip_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenDialog = new OpenFileDialog();
            OpenDialog.Filter = "Animation Clip File|*.acf";
            OpenDialog.AddExtension = true;
            OpenDialog.DefaultExt = ".acf";
            OpenDialog.FileOk += OpenDialog_FileOk;
            OpenDialog.ShowDialog();
        }

        void OpenDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            AnimatedClip NewClip = AnimatedClip.DeserializeItem((sender as OpenFileDialog).FileName);

            AnimationSet.AddAnimation(NewClip);
            //AnimationSet.NotifyCollectionChanged(NotifyCollectionChangedAction.Add, NewClip);
            NotifyCollectionChanged(NotifyCollectionChangedAction.Add, NewClip);
            AnimationList.SelectedItem = AnimationSet[NewClip.Name];
            CurrentAnimation = AnimationSet[NewClip.Name];
        }


    }
}
