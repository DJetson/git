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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Logging.Controls
{
	/// <summary>
	/// PromptBox is a TextBox which can be given a prompt which will be displayed in the TextBox until the user enters Text.
    /// This control can be used in place of the traditional Label/TextBox combination as the text that would typically be
    /// used for the label is instead displayed inside the TextBox.
	/// </summary>
	public partial class PromptBox : UserControl
	{
		#region TextChanged Routed Event
        
        /// <summary>
        /// A routed event which will be raised when the Text property is changed
        /// </summary>
		public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PromptBox));
		public event RoutedEventHandler TextChanged
		{
			add { AddHandler(TextChangedEvent, value); }
			remove { RemoveHandler(TextChangedEvent, value); }
		}

        /// <summary>
        /// Raises the TextChanged Event
        /// </summary>
		void RaiseTextChangedEvent()
		{
			RoutedEventArgs newEventArgs = new RoutedEventArgs(PromptBox.TextChangedEvent);
			RaiseEvent(newEventArgs);
		}
		#endregion
	
        #region PromptProperty
        /// <summary>
        /// String property which contains the text that will be displayed in the TextBox as a prompt for the user.
        /// </summary>
		public static DependencyProperty PromptProperty = DependencyProperty.Register("Prompt", typeof(String), typeof(PromptBox));
		public String Prompt
		{
			get { return (String)GetValue(PromptProperty); }
			set { SetValue(PromptProperty, value); }
		}
		#endregion
		
        #region TextProperty
		public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(PromptBox), new FrameworkPropertyMetadata(String.Empty));
		public String Text
		{
			get { return (String)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		#endregion

        #region Default Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
		public PromptBox()
		{
			InitializeComponent();
		}
        #endregion Default Constructor

        #region TextChanged Event Handler
        /// <summary>
        /// Handles the TextChanged Event for the TextBox control
        /// </summary>
        /// <param name="sender">The TextBox control whose Text value has changed</param>
        /// <param name="e">Additional arguments associated with this event</param>
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			RaiseTextChangedEvent();
        }
        #endregion TextChanged Event Handler
    }
}
