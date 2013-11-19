#if DEBUG
#define EnableDebugOutput
#define EnableEventLogOutput
#define DebugMode
#endif

#if Release
#define EnableEventLogOutput
#endif

#define HasStartupOptions
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Logging.Objects;
using System.Collections.Specialized;
using Logging;


namespace Logging
{
    #region Trace Level
    /// <summary>
	/// TraceLevel represents the current state of log output. Each item is a toggle for its stated TraceLevel
	/// Verbose and None are exactly opposite TraceLevel State configurations.
	/// They are listed in ascending order of criticality
	/// </summary>
	[Flags]
	public enum TraceLevel { None = 0x00, Info = 0x01, Warning = 0x02, Error = 0x04, Verbose = 0x07 };
    #endregion Trace Level

    #region Log Mode
    /// <summary>
	/// LogMode represents the current Logging Features that are enabled. Each item is a toggle for its stated Log
	/// Functionality. Below is a brief explanation of the LogMode features
	/// -----------------------------------------------------------------------------------------------------------------------
	/// Modes |
	/// ------
	/// Minimal     - The Log is running in silent mode. All Messages are committed to the Log but no aspect of the Log is outwardly visible
	/// Complete    - The Log contents appear in a Window that is created by the Log class and optionally attached to the main window.
	/// ----------------------------------------------------------------------------------------------------------------------------------------
	/// TraceLevels |
	/// ------------
	/// LogInfos    - The Log records all messages, but the Log Output windows VisibleTraceLevel is set to Info at startup
	/// </summary>
	[Flags]
	public enum LogMode { Minimal = 0x00, ShowInfos = 0x01, ShowWarnings = 0x02, ShowErrors = 0x04, ShowFailures = 0x04, Complete = 0xFF }
    #endregion Log Mode

    /// <summary>
    /// The Log class handles and directs all Log Messages
    /// </summary>
	public static class Log
    {
        #region Complete Message List
        /// <summary>
        /// The complete master Message List. Under normal circumstances it can only be added to. Removal of messages is mostly restricted.
        /// </summary>
		public static ObservableCollection<Message> CompleteMessageList;
        #endregion Complete Message List

        #region Filtered Message List
        /// <summary>
        /// The filtered list includes only the items that remain after the various filters have been applied
        /// </summary>
		public static ObservableCollection<Message> FilteredMessageList;
        #endregion Filtered Message List

        #region Message Types
        /// <summary>
        /// The Message Type Hierarchys root Message
        /// </summary>
		public static MessageType MessageTypes;
        #endregion Message Types

        #region Message Types List
        /// <summary>
        /// A list containing the compiled MessageTypes data structure
        /// </summary>
		public static List<MessageType> MessageTypesList;
        #endregion Message Types List

        #region Log Window
        /// <summary>
        /// An instance of a LogWindow to which output will be directed
        /// </summary>
		public static LogWindow LogWindow;
        #endregion Log Window

        #region Default Trace Level
        /// <summary>
        /// The default visible trace level
        /// </summary>
		public static TraceLevel DefaultMessageTraceLevel = TraceLevel.Info;
        #endregion Default Trace Level

        #region Default Message
        /// <summary>
        /// The default message detail that will be used if none is specified
        /// </summary>
		public static String DefaultMessageDetail = "No additional information is available.";
        #endregion Default Message

        #region Initialize
        /// <summary>
        /// Initializes the Log
        /// </summary>
        /// <param name="Host">The entry point for the Logger</param>
        /// <param name="EnableGUI">Whether to direct the log output to a window</param>
		public static void Initialize(Window Host, Boolean EnableGUI = true)
		{
			InitializeCollections();
			InitializeMessageTypes();
			InitializeLogWindow(Host, EnableGUI);
		}
        #endregion Initialize

        #region Initialize Message Types
        /// <summary>
        /// Initializes the Message Type Hierarchy
        /// </summary>
		private static void InitializeMessageTypes()
		{
			Log.MessageTypes = MessageType.GetRootMessageType();

			MessageType.SetRootMessageType(typeof(Window));
		}
        #endregion Initialize Message Types

        #region Initialize Log Window
        /// <summary>
        /// Initializes the LogWindow
        /// </summary>
        /// <param name="Host">The window to which the LogWindow will be attached</param>
        /// <param name="EnableGUI">Direct Log Output to the LogWindow</param>
		private static void InitializeLogWindow(Window Host, Boolean EnableGUI = true)
		{
			if (EnableGUI)
			{
				Log.LogWindow = new LogWindow();
				Log.AttachToHost(Host);
			}
		}
        #endregion Initialize Log Window

        #region Initialize Collections
        /// <summary>
        /// Initializes the Message Lists
        /// </summary>
		private static void InitializeCollections()
		{
			CompleteMessageList = new ObservableCollection<Message>();
			FilteredMessageList = new ObservableCollection<Message>();
		}
        #endregion Initialize Collections

        #region Subscribe To Updates
        /// <summary>
        /// Attaches an event handler for Collection Changed events
        /// </summary>
        /// <param name="c"></param>
		public static void SubscribeToUpdates(NotifyCollectionChangedEventHandler c)
		{
			CompleteMessageList.CollectionChanged += c;
		}
        #endregion Subscribe To Updates

        #region Get Filtered Log
        /// <summary>
        /// Filters the Master Message List and returns a filtered list
        /// </summary>
        /// <param name="VisibleLevels">The current Visible Trace Level</param>
        /// <param name="VisibleMessageTypes">A collection of visible message types</param>
        /// <returns>A filtered Message List</returns>
		public static ObservableCollection<Message> GetFilteredLog(TraceLevel VisibleLevels, List<MessageType> VisibleMessageTypes = null)
		{
			ObservableCollection<Message> FilteredLog = new ObservableCollection<Message>();

			foreach (Message m in CompleteMessageList)
				if (VisibleMessageTypes.Contains(m.MessageType) || VisibleLevels.HasFlag(m.TraceLevel))
					FilteredLog.Add(m);

			return FilteredLog;
		}

        /// <summary>
        /// Filters the Master Message List and returns a filtered list
        /// </summary>
        /// <param name="VisibleLevels">The current visible trace level</param>
        /// <returns>A filtered message list</returns>
		public static ObservableCollection<Message> GetFilteredLog(TraceLevel VisibleLevels)
		{
			ObservableCollection<Message> FilteredLog = new ObservableCollection<Message>();

			foreach (Message m in CompleteMessageList)
				if (VisibleLevels.HasFlag(m.TraceLevel))
					FilteredLog.Add(m);

			return FilteredLog;
		}

        /// <summary>
        /// Filters the master message list and returns a filtered list
        /// </summary>
        /// <param name="VisibleMessageTypes">The current visible trace level</param>
        /// <returns>A filtered message list</returns>
		public static ObservableCollection<Message> GetFilteredLog(List<MessageType> VisibleMessageTypes)
		{
			ObservableCollection<Message> FilteredLog = new ObservableCollection<Message>();

			foreach (Message m in CompleteMessageList)
				if (VisibleMessageTypes.Contains(m.MessageType))
					FilteredLog.Add(m);

			return FilteredLog;
		}

        /// <summary>
        /// Filters the master message list and returns a filtered list
        /// </summary>
        /// <returns>A filtered message list</returns>
		public static ObservableCollection<Message> GetFilteredLog()
		{
			ObservableCollection<Message> FilteredLog = new ObservableCollection<Message>(CompleteMessageList);

			return FilteredLog;
		}
        #endregion Get Filtered Log

        #region Attach To Host
        /// <summary>
        /// Attaches the LogWindow to the specified Host Window
        /// </summary>
        /// <param name="LogHost">The Host Window to which the LogWindow will be attached</param>
		public static void AttachToHost(Window LogHost)
		{
			if (LogHost != null)
				LogHost.Initialized += new EventHandler(LogHost_Initialized);

			Log.LogWindow.Show();
		}
        #endregion Attach To Host

        #region LogHost Initialized Event Handler
        /// <summary>
        /// Handles the event raised when the LogWindow is initialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private static void LogHost_Initialized(object sender, EventArgs e)
		{
			if (sender is Window)
				LogWindow.Owner = sender as Window;
		}
        #endregion LogHost Initialized Event Handler

        #region Register Message Type
        /// <summary>
        /// Registers a new Message Type
        /// </summary>
        /// <param name="NewType">The type of the new Message</param>
		public static void RegisterMessageType(Type NewType)
		{
			MessageType.Register(NewType);
			MessageTypesList = Log.MessageTypes.GetAllTypes(MessageTypesList);
		}
        #endregion Register Message Type

        #region Build Message
        /// <summary>
        /// Constructs a new Log Message
        /// </summary>
        /// <param name="MessageDetail">A string containing the message details</param>
        /// <param name="MessageType">The type of the message</param>
        /// <param name="MessageTraceLevel">The trace level of the message</param>
        /// <returns>A newly constructed message</returns>
		private static Message BuildMessage(String MessageDetail = null, MessageType MessageType = null, TraceLevel MessageTraceLevel = TraceLevel.None)
		{
			return new Message(MessageDetail, MessageType, MessageTraceLevel);
		}
        #endregion Build Message

        #region Info Message
        /// <summary>
        /// Sends a new Info Message to the log
        /// </summary>
        /// <param name="Detail">The detail text</param>
        /// <param name="Type">The Message Type</param>
		public static void i(String Detail = null, MessageType Type = null)
		{
			CompleteMessageList.Add(new Message(Detail, Type, TraceLevel.Info));
		}
        #endregion Info Message

        #region Warning Message
        /// <summary>
        /// Sends a new Warning Message to the log
        /// </summary>
        /// <param name="Detail">The detail text</param>
        /// <param name="Type">The Message Type</param>
		public static void w(String Detail = null, MessageType Type = null)
		{
			CompleteMessageList.Add(new Message(Detail, Type, TraceLevel.Warning));
		}
        #endregion Warning Message

        #region Error Message
        /// <summary>
        /// Sends a new Error Message to the log
        /// </summary>
        /// <param name="Detail">The detail text</param>
        /// <param name="Type">The Message Type</param>
		public static void e(String Detail = null, MessageType Type = null)
		{
			CompleteMessageList.Add(new Message(Detail, Type, TraceLevel.Error));
		}
        #endregion Error Message

        #region Debug Message
        /// <summary>
        /// Sends a new Debug Message to the log
        /// </summary>
        /// <param name="MessageDetail">The detail text</param>
        /// <param name="MessageType">The Message Type</param>
        /// <param name="MessageTraceLevel">The trace level</param>
		public static void d(String MessageDetail = null, MessageType MessageType = null, TraceLevel MessageTraceLevel = TraceLevel.None)
		{
			CompleteMessageList.Add(new Message(MessageDetail, MessageType, MessageTraceLevel));
        }
        #endregion Debug Message
    }
}
