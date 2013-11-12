using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GPOne.Objects
{
    public enum PlayMode { Stop, Play, Pause, Continous };

    [Serializable]
    public class AnimatedClip : INotifyPropertyChanged, IDeserializationCallback
    {

        #region INotifyPropertyChanged Implementation
        [field:NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        #region ISerializable Implementation
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(String));
            info.AddValue("FileName", FileName, typeof(String));
            info.AddValue("ImageBytes", ImageBytes, typeof(Byte[]));
            info.AddValue("Width", Width, typeof(double));
            info.AddValue("Height", Height, typeof(double));
            info.AddValue("FPS", FPS, typeof(double));
        }

        private Byte[] SerializeBitmapImage()
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = SourceBitmap;

            encoder.Frames.Add(BitmapFrame.Create(bImg));
            encoder.Save(memoryStream);

            return memoryStream.ToArray();
        }

        private void DeserializeBitmapImage(Byte[] Bytes)
        {
            MemoryStream stream = new MemoryStream(Bytes);
            SourceBitmap = new BitmapImage();
            SourceBitmap.BeginInit();
            SourceBitmap.StreamSource = new MemoryStream(stream.ToArray());
            SourceBitmap.EndInit();

            stream.Close();
        }

        public AnimatedClip(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            FileName = (string)info.GetValue("FileName", typeof(string));
            ImageBytes = (Byte[])info.GetValue("ImageBytes", typeof(Byte[]));
            Width = (double)info.GetValue("Width", typeof(double));
            Height = (double)info.GetValue("Height", typeof(double));
            FPS = (double)info.GetValue("FPS", typeof(double));
        }

        public void Serialize(string fileName)
        {
            FileStream s = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            ImageBytes = SerializeBitmapImage();
            new BinaryFormatter().Serialize(s, this);
            s.Close();
        }


        public static AnimatedClip DeserializeItem(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            AnimatedClip NewClip = (AnimatedClip)(new BinaryFormatter()).Deserialize(fs);
            NewClip.DeserializeBitmapImage(NewClip.ImageBytes);

            fs.Close();

            NewClip.Initialize();
            NewClip.SetInitialValues();

            return NewClip;

        }
        #endregion ISerializable Implementation

        #region AnimatedClip Events
        [field:NonSerializedAttribute()]
        public event EventHandler<AnimationStateChangedEventArgs> StateChanged;

        private void OnStateChanged(AnimationStateChangedEventArgs e)
        {
            EventHandler<AnimationStateChangedEventArgs> handler = StateChanged;
            if (handler != null) { handler(this, e); }
        }
        #endregion AnimatedClip Events

        #region AnimatedClip Properties
        private String _Name;
        public String Name
        {
            get { return _Name; }
            set { _Name = value; NotifyPropertyChanged("Name"); }
        }

        private Byte[] _ImageBytes;
        public Byte[] ImageBytes
        {
            get { return _ImageBytes; }
            set { _ImageBytes = value; NotifyPropertyChanged("ImageBytes"); }
        }

        private String _FileName;
        public String FileName
        {
            get { return _FileName; }
            set { _FileName = value; NotifyPropertyChanged("FileName"); }
        }

        private double _Width;
        public double Width
        {
            get { return _Width; }
            set { _Width = value; NotifyPropertyChanged("Width"); }
        }

        private double _Height;
        public double Height
        {
            get { return _Height; }
            set { _Height = value; NotifyPropertyChanged("Height"); }
        }

        [NonSerializedAttribute()]
        private List<VisualBrush> _Frames = new List<VisualBrush>();
        public List<VisualBrush> Frames
        {
            get { return _Frames; }
            set { _Frames = value; NotifyPropertyChanged("Frames"); NotifyPropertyChanged("LastFrameIndex"); }
        }

        [NonSerializedAttribute()]
        private BitmapImage _SourceBitmap;
        public BitmapImage SourceBitmap
        {
            get { return _SourceBitmap; }
            set { _SourceBitmap = value; NotifyPropertyChanged("SourceBitmap"); }
        }

        [NonSerializedAttribute()]
        private Image _Source;
        public Image Source
        {
            get { return _Source; }
            set { _Source = value; NotifyPropertyChanged("Source"); }
        }

        [NonSerializedAttribute()]
        private Int32Rect _SourceRect;
        public Int32Rect SourceRect
        {
            get { return _SourceRect; }
            set { _SourceRect = value; NotifyPropertyChanged("SourceRect"); }
        }

        public Boolean IsPlaying { get { return (State == PlayMode.Continous || State == PlayMode.Play); } }

        [NonSerializedAttribute()]
        private PlayMode _State;
        private PlayMode State
        {
            get { return _State; }
            set
            {
                AnimationStateChangedEventArgs Args = new AnimationStateChangedEventArgs()
                    {
                        OldState = _State,
                        NewState = value
                    };
                _State = value;
                NotifyPropertyChanged("State");
                NotifyPropertyChanged("IsPlaying");
                OnStateChanged(Args);
            }
        }

        [NonSerializedAttribute()]
        private Boolean _IsReversed;
        public Boolean IsReversed
        {
            get { return _IsReversed; }
            set { _IsReversed = value; NotifyPropertyChanged("IsReversed"); }
        }

        [NonSerializedAttribute()]
        private int _CurrentFrameIndex;
        public int CurrentFrameIndex
        {
            get { return _CurrentFrameIndex; }
            set { _CurrentFrameIndex = value; NotifyPropertyChanged("CurrentFrameIndex"); NotifyPropertyChanged("CurrentFrame"); }
        }

        private double _FPS = 1;
        public double FPS
        {
            get { return _FPS; }
            set
            {
                _FPS = value;
                NotifyPropertyChanged("FPS");
                NotifyPropertyChanged("FrameDuration");
                FrameTimer.Interval = new TimeSpan(FrameDuration);
            }
        }

        public VisualBrush CurrentFrame
        {
            get
            {
                if (Frames.Count == 0)
                    return null;
                return Frames[CurrentFrameIndex];
            }
        }
        public long FrameDuration { get { return (long)(10000000 / _FPS); } }

        public int LastFrameIndex { get { return Frames.Count - 1; } }

        public Boolean IsContinuous { get { return (State == PlayMode.Continous); } }

        [NonSerializedAttribute()]
        private DispatcherTimer FrameTimer = new DispatcherTimer();
        #endregion AnimatedClip Properties

        private void Initialize()
        {

            Frames = Frames ?? new List<VisualBrush>();
            PropertyChanged += AnimatedClip_PropertyChanged;

            FrameTimer = FrameTimer ?? new DispatcherTimer();

            FrameTimer.Tick += FrameTimer_Tick;
            StateChanged += AnimatedClip_StateChanged;

            FPS = 30;
            State = PlayMode.Stop;
        }

        private void SetInitialValues()
        {
            ProcessImageData();
            GenerateFrames();
        }

        private void ProcessImageData()
        {
            if (SourceBitmap == null)
                return;

            SourceRect = new Int32Rect(0, 0, SourceBitmap.PixelWidth, SourceBitmap.PixelHeight);
            Source = new Image() { Source = SourceBitmap };
        }

        public AnimatedClip()
        {
            Initialize();
        }

        public AnimatedClip(String ClipName, String Filename, double FrameWidth, double FrameHeight)
        {
            Name = ClipName;
            FileName = Filename;
            SourceBitmap = new BitmapImage(new Uri(Filename));
            Width = FrameWidth;
            Height = FrameHeight;
            

            Initialize();
            SetInitialValues();
        }

        public void GenerateFrames()
        {
            for (int i = 0; i < SourceRect.Width / Width; i++)
            {
                Frames.Add(new VisualBrush()
                {
                    Viewport = new Rect(0, 0, 1, 1),
                    ViewportUnits = BrushMappingMode.RelativeToBoundingBox,
                    Visual = Source,
                    Viewbox = new Rect(i * Width, 0, Width, Height),
                    ViewboxUnits = BrushMappingMode.Absolute
                });
            }

            CurrentFrameIndex = 0;
        }

        void AnimatedClip_StateChanged(object sender, AnimationStateChangedEventArgs e)
        {
            PlayMode NewState = e.NewState;

            if (NewState == PlayMode.Play || NewState == PlayMode.Continous)
                StartAnimation();
            else if (NewState == PlayMode.Stop || NewState == PlayMode.Pause)
                StopAnimation();
        }

        void FrameTimer_Tick(object sender, EventArgs e)
        {
            if (IsReversed == false)
                StepForward();
            else
                StepBackward();
            if (State == PlayMode.Play && CurrentFrameIndex == 0)
                State = PlayMode.Stop;
        }

        void AnimatedClip_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void StartAnimation()
        {
            if (FrameTimer.IsEnabled == false)
                FrameTimer.IsEnabled = true;
        }

        private void StopAnimation()
        {
            FrameTimer.IsEnabled = false;
            if (State == PlayMode.Stop)
                CurrentFrameIndex = 0;
        }

        public void Play(Boolean Continuous = false)
        {
            if (Continuous)
                State = PlayMode.Continous;
            else
                State = PlayMode.Play;
        }

        public void Stop()
        {
            State = PlayMode.Stop;
        }

        public void Pause()
        {
            State = PlayMode.Pause;
        }

        public void StepForward()
        {
            CurrentFrameIndex = (CurrentFrameIndex + 1) % Frames.Count;
        }

        public void StepBackward()
        {
            if (CurrentFrameIndex > 0)
                CurrentFrameIndex--;
            else
                CurrentFrameIndex = Frames.Count - 1;
        }

        public void OnDeserialization(object sender)
        {
        }
    }


}
