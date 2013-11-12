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
using System.Xml.Linq;
using GameObjects.Controls.DeveloperConsole;
using System.Reflection;
using GameObjects.Classes;

namespace GameObjects.Controls.DeveloperConsole
{
    /// <summary>
    /// Interaction logic for DeveloperConsoleControl.xaml
    /// </summary>
    public partial class DeveloperConsoleControl : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }
        #endregion INotifyPropertyChanged Implementation

        private ObservableCollection<String> _ConsoleHistoryList = new ObservableCollection<String>();
        public ObservableCollection<String> ConsoleHistoryList
        {
            get { return _ConsoleHistoryList; }
            set { _ConsoleHistoryList = value; NotifyPropertyChanged("ConsoleHistoryList"); }
        }

        private String _ConsoleInput;
        public String ConsoleInput
        {
            get { return _ConsoleInput; }
            set { _ConsoleInput = value; NotifyPropertyChanged("ConsoleInput"); }
        }

        private string LastCommand = null;

        private static DeveloperConsoleControl DeveloperConsole = null;

        private String Command;
        public Dictionary<String, CommandObject> CommandList = new Dictionary<String, CommandObject>();

        public DeveloperConsoleControl()
        {
            if (DeveloperConsole == null)
                DeveloperConsole = this;
            DataContext = this;
            InitializeComponent();
            ParseCommandList();
        }

        private void ConsolePrompt_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox Sender = sender as TextBox;
            if (e.Key == Key.PageUp && LastCommand != null)
            {
                Sender.Text = LastCommand;
                Command = Sender.Text;
            }

            if (e.Key == Key.Enter)
            {
                Command = Sender.Text;
                Sender.Clear();

                System.Diagnostics.Trace.WriteLine(String.Format("Executing Command: {0}", Command));

                try
                {

                    ConsoleHistoryList.Add(Command);

                    if (ExecuteCommand(Command) == false)
                    {
                        ConsoleHistoryList.Add(String.Format("The command [{0}] was not recognized. For help enter -help", Command));
                        //Add the error message to the console history
                    }
                    else
                        LastCommand = Command;
                }
                catch (Exception ex)
                {
                    ConsoleHistoryList.Add(ex.Message);
                }

            }

            if (ConsoleHistory.Items.Count > 0)
                ConsoleHistory.ScrollIntoView(ConsoleHistory.Items[ConsoleHistory.Items.Count - 1]);
        }

        public static void PublishToConsole(String output)
        {
            DeveloperConsole.ConsoleHistoryList.Add(output);
        }

        public static void PublishToConsole(List<String> Output)
        {
            foreach (String s in Output)
            {
                DeveloperConsole.ConsoleHistoryList.Add(s);
            }
        }

        #region Execute Command: This needs to be cleaned up
        private Boolean ExecuteCommand(String Command)
        {
            ///Attempt to execute the command stored in Execute. 
            ///1. Parse the Execute string.
            return ParseInput(Command);
            ///2. Search the command list.
            ///3. If the command list contains Execute, Do the commmand
            ///3B. Return true
            ///4. If not send an error message to the console
            ///4B. return false
        }

        private Boolean ParseInput(String Command)
        {
            try
            {
                string[] CommandParameters = Command.Split(' ');

                if (CommandList.ContainsKey(CommandParameters[0]))
                {
                    return GetCommandMethod(CommandList[CommandParameters[0]]);
                }

                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private Boolean GetCommandMethod(CommandObject c)
        {
            try
            {
                //TODO: Need to add a check to see if the object is a singleton before
                //      attempting to create a new one.
                var CallObject = Activator.CreateInstance(c.CallTypeAssembly, c.CallType).Unwrap();
                if (CallObject == null)
                    return false;

                MethodInfo method = CallObject.GetType().GetMethod(c.CallFunction);

                if (method == null)
                    return false;

                ExecuteCommandMethod(c, CallObject, method);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void ExecuteCommandMethod(CommandObject c, object CallObject, MethodInfo method)
        {
            method.Invoke(CallObject, null);
        }

        private void ParseCommandList()
        {
            var xdoc = XDocument.Load("C:/DirtWare/Game Project One/GameObjects/Controls/DeveloperConsole/DeveloperCommands.xml");
            var Commands = from e in xdoc.Descendants("Command")
                           select new CommandObject
                           {
                               Name = (String)e.Attribute("Name"),
                               Id = (int)e.Element("Id"),
                               CallTypeAssembly = (string)e.Element("CallTypeAssembly"),
                               CallType = (string)e.Element("CallType"),
                               CallFunction = (string)e.Element("CallFunction"),
                               Description = (string)e.Element("Description")
                           };

            foreach (CommandObject o in Commands)
            {
                CommandList.Add(o.Name, o);
            }
        }
        #endregion Execute Command

        #region Command Methods

        public static void ListAllCommands()
        {
            foreach (KeyValuePair<String, CommandObject> kv in DeveloperConsole.CommandList)
            {
                DeveloperConsole.ConsoleHistoryList.Add(String.Format("{0}: {1}", kv.Value.Name, kv.Value.Description));
            }
        }

        //The following two methods are used to give the console prompt keyboard focus whenever
        //prompt is opened. A whole bunch of different things are called here to prevent
        //the prompt from retaining focus when its invisible, and further to prevent 
        //the keystroke used to open the prompt from actually appearing in the prompt
        //If there is a better way of doing it, these should be changed or removed.

        public static void FocusPrompt()
        {
            DeveloperConsole.ConsolePrompt.Clear();
            DeveloperConsole.ConsolePrompt.Focusable = true;
            DeveloperConsole.ConsolePrompt.IsEnabled = true;
            DeveloperConsole.Visibility = Visibility.Visible;
            DeveloperConsole.ConsolePrompt.Focus();
        }

        public static void UnfocusPrompt()
        {
            DeveloperConsole.ConsolePrompt.Focusable = false;
            DeveloperConsole.Visibility = Visibility.Hidden;
            DeveloperConsole.ConsolePrompt.IsEnabled = false;
        }
        #endregion Command Methods




    }
}
