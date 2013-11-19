using Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogRefactorSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged, ILogMessage
	{
		#region PropertyChanged EventHandler

		public event PropertyChangedEventHandler PropertyChanged;

		void NotifyPropertyChanged(String Property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(Property));
		}

		#endregion

        private ObservableCollection<NewMessageType> _LogMessageTypes;
        public ObservableCollection<NewMessageType> LogMessageTypes
        {
            get { return _LogMessageTypes; }
            set { _LogMessageTypes = value; NotifyPropertyChanged("LogMessageTypes"); } 
        }

        private NewMessageType _LogMessageType;
        public NewMessageType LogMessageType
        {
            get { return _LogMessageType; }
            set { _LogMessageType = value; NotifyPropertyChanged("LogMessageType"); }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            LogMessageTypes = new ObservableCollection<NewMessageType>();
            LogMessageType = new NewMessageType();

            ClassA a = new ClassA();
            ClassB b = new ClassB();

            List<NewMessageType> list = new List<NewMessageType>();

            list = LogMessageType.GetAllSubTypes(list);

            foreach (NewMessageType m in list)
            {
                LogMessageTypes.Add(m);
            }
        }

        
    }
}
