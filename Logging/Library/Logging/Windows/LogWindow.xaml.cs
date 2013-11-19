using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Globalization;
using Logging.Controls;
using System.Collections.Specialized;
using Logging;
using Logging.Objects;


namespace Logging
{
    /// <summary>
    /// This window is intended to provide a display for log messages. It can attach to any window in an application and will
    /// close when the attached window is closed.
    /// </summary>
    public partial class LogWindow : Window
    {
        #region Output Message List
        /// <summary>
        /// Exposes a Property containing an observable collection of the items that will be displayed in the LogWindow's message list
        /// </summary>
        public static DependencyProperty OutputMessageListProperty = DependencyProperty.Register("OutputMessageList", typeof(ObservableCollection<Message>), typeof(LogWindow));
        public ObservableCollection<Message> OutputMessageList
        {
            get { return (ObservableCollection<Message>)GetValue(OutputMessageListProperty); }
            set { SetValue(OutputMessageListProperty, value); }
        }
        #endregion Output Message List

        #region Message Types
        /// <summary>
        /// The root element of a collection of hierarchical message types. This is intended as a means of filtering
        /// based on the message source
        /// </summary>
        public static DependencyProperty MessageTypesProperty = DependencyProperty.Register("MessageTypes", typeof(MessageType), typeof(LogWindow));
        public MessageType MessageTypes
        {
            get { return (MessageType)GetValue(MessageTypesProperty); }
            set { SetValue(MessageTypesProperty, value); }
        }
        #endregion Message Types

        #region Type Search Results
        /// <summary>
        /// A collection containing the message types that remain after being filtered through the search criteria
        /// </summary>
        public static DependencyProperty TypeSearchResultsProperty = DependencyProperty.Register("TypeSearchResults", typeof(ObservableCollection<MessageType>), typeof(LogWindow));
        public ObservableCollection<MessageType> TypeSearchResults
        {
            get { return (ObservableCollection<MessageType>)GetValue(TypeSearchResultsProperty); }
            set { SetValue(TypeSearchResultsProperty, value); }
        }
        #endregion Type Search Results

        #region Visible Trace Level
        /// <summary>
        /// A TraceLevel value indicating which TraceLevels will not be filtered out.
        /// </summary>
        public static DependencyProperty VisibleTraceLevelProperty = DependencyProperty.Register("VisibleTraceLevel", typeof(TraceLevel), typeof(LogWindow));
        public TraceLevel VisibleTraceLevel
        {
            get { return (TraceLevel)GetValue(VisibleTraceLevelProperty); }
            set { SetValue(VisibleTraceLevelProperty, value); }
        }
        #endregion Visible Trace Level

        #region Target Message Type
        /// <summary>
        /// The selected message type
        /// </summary>
        public static DependencyProperty TargetMessageTypeProperty = DependencyProperty.Register("TargetMessageType", typeof(String), typeof(LogWindow));
        public String TargetMessageType
        {
            get { return (String)GetValue(TargetMessageTypeProperty); }
            set { SetValue(TargetMessageTypeProperty, value); }
        }
        #endregion Target Message Type

        #region Parent Window
        /// <summary>
        /// The window to which the LogWindow will be attached.
        /// </summary>
        public Window ParentWindow;
        #endregion Parent Window

        #region Default Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LogWindow()
        {
            Initialize();
        }
        #endregion Default Constructor

        #region Initialize
        /// <summary>
        /// Initializes the LogWindow
        /// </summary>
        public void Initialize()
        {
            InitializeComponent();
            VisibleTraceLevel = TraceLevel.Verbose;
            OutputMessageList = new ObservableCollection<Message>();
            Log.SubscribeToUpdates(UpdateFilteredMessageList);
            MessageTypes = MessageType.GetRootMessageType();
        }
        #endregion Initialize

        #region Update Filtered Message List
        /// <summary>
        /// Handles any updates to the MessageList and brings the most recent item into view
        /// </summary>
        /// <param name="sender">The Message List</param>
        /// <param name="e">Additional arguments associated with this event</param>
        void UpdateFilteredMessageList(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Message> CompleteLog = sender as ObservableCollection<Message>;

            Message NewMessage = CompleteLog.Last();

            if (VisibleTraceLevel.HasFlag(NewMessage.TraceLevel))
                OutputMessageList.Add(NewMessage);

            LogMessageList.ScrollIntoView(this.OutputMessageList.Last());
        }
        #endregion Update Filtered Message List

        #region Message List Collection Changed Handler
        /// <summary>
        /// Handles any changes to the Message List's backing collection
        /// </summary>
        /// <param name="sender">The Message collection</param>
        /// <param name="e">Additional arguments associated with this event</param>
        void MessageList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Message n in e.NewItems)
                AddLogEntryToOutputMessageList(n);
        }
        #endregion Message List Collection Changed Handler

        #region Invalidate Output Message List
        /// <summary>
        /// Forces the output message list to be evaluated regardless of whether or not any events have been raised
        /// </summary>
        void InvalidateOutputMessageList()
        {
            if (OutputMessageList == null)
                OutputMessageList = new ObservableCollection<Message>();
            OutputMessageList.Clear();

            ObservableCollection<Message> FilteredLog = Log.GetFilteredLog(VisibleTraceLevel);

            foreach (Message e in FilteredLog)
                OutputMessageList.Add(e);
        }
        #endregion Invalidate Output Message List

        #region AddLogEntryToOutputMessageList
        /// <summary>
        /// Adds a message to the output message list
        /// </summary>
        /// <param name="e">The message to add</param>
        void AddLogEntryToOutputMessageList(Message e)
        {
            if (OutputMessageList == null)
                OutputMessageList = new ObservableCollection<Message>();

            if (VisibleTraceLevel.HasFlag(e.TraceLevel))
                OutputMessageList.Add(e);
        }
        #endregion AddLogEntryToOutputMessageList

        #region Output Message List Collection Changed
        /// <summary>
        /// Handles any changes to the output message list, forces new messages to scroll into view
        /// </summary>
        /// <param name="sender">The message list</param>
        /// <param name="e">Additional arguments associated with this event</param>
        void OutputMessageList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background,
            (ThreadStart)delegate
            {
                if (this.OutputMessageList != null && this.OutputMessageList.Count != 0)
                    if (this.OutputMessageList.Last() != null)
                        this.LogMessageList.ScrollIntoView(this.OutputMessageList.Last());
            });
        }
        #endregion Output Message List Collection Changed

        #region Owner Closing
        /// <summary>
        /// Closes the LogWindow when its Parent Window closes
        /// </summary>
        /// <param name="sender">The parent window</param>
        /// <param name="e">Additional arguments associated with this event</param>
        void Owner_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.i(Owner.Title + " issued a call to Close the Log...");
            this.Close();
        }
        #endregion Owner Closing

        #region Filter Toggle Button Clicked
        /// <summary>
        /// Filters the Output Message List whenever a TraceLevel button is clicked
        /// </summary>
        /// <param name="sender">The button which was clicked</param>
        /// <param name="e">Additional Arguments associated with this event</param>
        private void FilterToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton b = sender as ToggleButton;
            FilterLogOutput();
        }
        #endregion Filter Toggle Button Clicked

        #region Filter Log Output
        /// <summary>
        /// Filters the output message list and brings the most recent message into view
        /// </summary>
        private void FilterLogOutput()
        {
            InvalidateOutputMessageList();
            if(this.OutputMessageList.Count != 0)
                LogMessageList.ScrollIntoView(this.OutputMessageList.Last());
        }
        #endregion Filter Log Output

        #region Edit Menu Cut Clicked
        /// <summary>
        /// THIS NEEDS TO BE REMOVED AS MESSAGES CANNOT BE CUT FROM THE LOG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_EditCut_Click(object sender, RoutedEventArgs e)
        {
            Log.i("MenuItem Edit->Cut Clicked");
        }
        #endregion Edit Menu Cut Clicked

        #region Edit Menu Copy Clicked
        /// <summary>
        /// Copies the selected messages
        /// </summary>
        /// <param name="sender">The Copy menu item</param>
        /// <param name="e"></param>
        private void Menu_EditCopy_Click(object sender, RoutedEventArgs e)
        {
            Log.i("MenuItem Edit->Copy Clicked");
        }
        #endregion Edit Menu Copy Clicked

        #region Edit Menu Paste Clicked
        /// <summary>
        /// THIS ISN'T PARTICULARLY USEFUL AND SHOULD PROBABLY BE REMOVED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_EditPaste_Click(object sender, RoutedEventArgs e)
        {
            Log.i("MenuItem Edit->Paste Clicked");
        }
        #endregion Edit Menu Paste Clicked

        #region Edit Menu Clear Log Clicked
        /// <summary>
        /// Clears the output message list. Does not clear the actual log
        /// </summary>
        /// <param name="sender">The Clear menu item</param>
        /// <param name="e">Additional arguments associated with this event</param>
        private void Menu_EditClearLog_Click(object sender, RoutedEventArgs e)
        {
            Log.i("MenuItem Edit->ClearLog Clicked");
        }
        #endregion Edit Menu Clear Log Clicked

        #region File Menu Exit Clicked
        /// <summary>
        /// Exits the LogWindow. Log Messages will continue to be added.
        /// </summary>
        /// <param name="sender">The Exit menu item</param>
        /// <param name="e">Additional arguments associated with this event</param>
        private void Menu_FileExit_Click(object sender, RoutedEventArgs e)
        {
            Log.i("MenuItem Edit->ClearLog Clicked");
            Close();
        }
        #endregion File Menu Exit Clicked

        #region Prompt Box Text Changed Handler
        /// <summary>
        /// Handles the PromptBox's TextChanged attached event
        /// </summary>
        /// <param name="sender">The PromptBox</param>
        /// <param name="e">Additional arguments associated with this event</param>
        private void PromptBox_TextChanged(object sender, RoutedEventArgs e)
        {
            String TargetMessageType = (sender as PromptBox).Text;

            if (TargetMessageType == null)
                return;

            List<MessageType> MessageTypeList = new List<MessageType>();
            MessageTypeList = MessageTypes.GetAllTypes(MessageTypeList);

            foreach (MessageType m in MessageTypeList)
            {
                String s = m.Name.ToUpperInvariant();
                if (s.Contains(TargetMessageType.ToUpperInvariant()))
                    m.HasPriority = true;
                else
                    m.HasPriority = false;
            }
        }
        #endregion Prompt Box Text Changed Handler

        #region TreeViewItem Clicked
        /// <summary>
        /// CURRENTLY THIS DOES NOTHING
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem t = sender as TreeViewItem;
        }
        #endregion TreeViewItem Clicked
    }
}
