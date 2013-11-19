using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Logging.Objects
{
    #region Class Description
    ///Log Message
    ///1.    Timestamp
    ///      Assigned by a call to DateTime.Now. Should probably happen in the constructor because that is the closest to when
    ///      Log was called on to post a message.
    ///
    ///2.    MessageSource
    ///	    Assigned by doing a StackTrace back n+1 TraceLevels in the stack, where n is the number of intermediary functions between
    ///	    the Source and the call to StackTrace.
    ///
    ///3.    TraceLevel
    ///	    The degree of criticality of the Message. 
    ///	    This can be assigned in one of several ways, the list below is in order of descending precedence:
    ///		I.		It is assigned automatically when one of the following three posting functions is called:
    ///					A. public static void Log:i(String OptionalMessage)
    ///					B. public static void Log.d(String OptionalMessage)
    ///					C. public static void Log.e(String OptionalMessage)
    ///		II.		If any other posting method is used, the TraceLevel may be passed as an optional parameter
    ///		III.	If any other posting method is used and the TraceLevel is not provided, the TraceLevel is
    ///				assigned by the DefaultTraceLevel of the associated MessageType.
    ///		IV.		If the associated MessageType is automatically registered by the posting method (i.e. It was
    ///				not a registered type prior to calling the posting method) the TraceLevel is assigned by
    ///				the Log's DefaultTraceLevel, which can be specified as a parameter in Log.Initialize, or is
    ///				otherwise set to INFO automatically. The Log's DefaultTraceLevel is applied to all implicitly
    ///				registered MessageTypes, which then apply the same TraceLevel to new postings.
    ///
    ///4.    Detail
    ///      This is set by either passing the MessageDetail when a posting method is used or if none is specified, it will be assigned by the
    ///      MessageType's DefaultDetail, or lastly by the Log's DefaultDetail
    ///5.    MessageType
    ///      The MessageType is optionally passed to the posting method, if none is specified the logger attempts to automatically register a new type
    ///      in the following way:
    ///          I.  If the invoking class has been registered, the MessageType is set to the invoking class type. Otherwise, a stacktrace is done and
    ///              the type at each level of the stack is checked for registration with the logger. If a registered type is found, then each type
    ///              between the registered type and the invoking class is registered, finally the invoking class is registered and the message is
    ///              typed to the invoking class.
    ///          II. If no registered type is found in the call stack, then the message is typed as a sub type of the root (All Types, General)
    ///
    #endregion Class Description
    public class Message : Control
    {
        #region Timestamp
        /// <summary>
        /// A DateTime object that keeps track of when this Message was created
        /// </summary>
        private DateTime _Timestamp;
        public DateTime Timestamp
        {
            get { return _Timestamp; }
        }
        #endregion Timestamp

        #region Message Source
        /// <summary>
        /// The originating object of the message
        /// </summary>
        private String _Source;
        public String Source
        {
            get { return _Source; }
        }
        #endregion Message Source

        #region Message Detail
        /// <summary>
        /// The detail string of the message
        /// </summary>
        private String _Detail;
        public String Detail
        {
            get { return _Detail; }
        }
        #endregion Message Detail

        #region Message Trace Level
        /// <summary>
        /// The message trace level
        /// </summary>
        private TraceLevel _TraceLevel;
        public TraceLevel TraceLevel
        {
            get { return _TraceLevel; }
        }
        #endregion Message Trace Level

        #region Message Type
        /// <summary>
        /// The Message Type for the message
        /// </summary>
        private MessageType _MessageType;
        public MessageType MessageType
        {
            get { return _MessageType; }
        }
        #endregion Message Type

        #region Message Constructor
        /// <summary>
        /// Creates a new Message using the provided message detail, type and trace level
        /// </summary>
        /// <param name="MessageDetail">The message detail string</param>
        /// <param name="MessageType">The message type</param>
        /// <param name="MessageTraceLevel">The message trace level</param>
        public Message(String MessageDetail = null, MessageType MessageType = null, TraceLevel MessageTraceLevel = TraceLevel.None)
        {
            //Timestamp and source are always automatically generated
            _Timestamp = GetTimestamp();
            _Source = GetSource();

            //Detail, Type and TraceLevel may be provided when a new message is created, but once created, messages may not be altered.
            _Detail = MessageDetail;
            _MessageType = MessageType;
            _TraceLevel = MessageTraceLevel;

            //If no detail was provided use the default detail
            if(String.IsNullOrEmpty(_Detail))
                _Detail = GetDetail();

            //If no type was provided use the default type
            if (_MessageType == null)
                _MessageType = GetType();

            //If no trace level was provided use the default trace level
            if (_TraceLevel == TraceLevel.None)
                _TraceLevel = GetTraceLevel();
        }
        #endregion Message Constructor

        #region Get Message Source
        /// <summary>
        /// Gets the source of this message
        /// </summary>
        /// <returns>The originating object for this message</returns>
        private String GetSource()
        {
            return String.Format("<{0}.{1}>", new StackFrame(2).GetMethod().DeclaringType.Name, new StackFrame(2).GetMethod().Name);
        }
        #endregion Get Message Source

        #region Get Message Type
        /// <summary>
        /// Gets the type of this message
        /// </summary>
        /// <returns>The message type</returns>
        new private MessageType GetType()
        {
            //Get the message type
            return null;
        }
        #endregion Get Message Type

        #region Get Message Trace Level
        /// <summary>
        /// Gets the trace level for this message
        /// </summary>
        /// <returns>The message trace level</returns>
        private TraceLevel GetTraceLevel()
        {
            //Get the type's default trace level
            //if this message required a new type to be registered, use the Log's default trace level

            return TraceLevel.None;
        }
        #endregion Get Message Trace Level

        #region Get Message Detail
        /// <summary>
        /// Gets the message detail string
        /// </summary>
        /// <returns>The message detail</returns>
        private String GetDetail()
        {
            //Get the type's default Detail
            //If this message required a new type to be registered, use the Log's default message detail

            return null;
        }
        #endregion Get Message Detail

        #region Get Message Timestamp
        /// <summary>
        /// Gets the timestamp for the message
        /// </summary>
        /// <returns>The message timestamp</returns>
        private DateTime GetTimestamp()
        {
            return DateTime.Now;
        }
        #endregion Get Message Timestamp
    }
}
