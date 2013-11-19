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
using System.Collections.ObjectModel;
using Logging.Objects;
using Logging;

namespace LogSample
{
	//LAUNCH FLAGS
	//ENABLE_LOGGING = BEGIN LOGGING AT APPLICATION STARTUP
	//ENABLE_DEVELOPER_MODE = UNLOCK DEVELOPER COMMANDS
	//public enum LaunchConfig { ENABLE_LOGGING = 0x01, ENABLE_DEVELOPER_MODE = 0x02};




	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Compile Log
		/// </summary>

		public MainWindow()
		{
			InitializeComponent();

			Log.Initialize(this);
		}

		/// <summary>
		/// Generate an Info Log Message
		/// </summary>
		/// <param name="sender">The TestInfo Button Control</param>
		/// <param name="e">Additional arguments pertaining to the Click event</param>
		private void TestInfo_Click(object sender, RoutedEventArgs e)
		{
			Log.i("This is an Info Message", MessageType.Register(typeof(TypeOne)));
			Log.i("This is an Info Message", MessageType.Register(typeof(TypeTwo)));
			Log.i("This is an Info Message", MessageType.Register(typeof(TypeThree)));
		}
		enum TypeOne { };
		enum TypeTwo { };
		enum TypeThree { };
		/// <summary>
		/// Generate a Warning Log Message
		/// </summary>
		/// <param name="sender">The TestWarning Button Control</param>
		/// <param name="e">Additional arguments pertaining to the Click event</param>
		private void TestWarning_Click(object sender, RoutedEventArgs e)
		{
			Log.w("This is a Warning Message");
		}

		/// <summary>
		/// Generate an Error Log Message
		/// </summary>
		/// <param name="sender">The TestError Button Control</param>
		/// <param name="e">Additional arguments pertaining to the Click event</param>
		private void TestError_Click(object sender, RoutedEventArgs e)
		{
			Log.e("This is an Error Message");
		}

		/// <summary>
		/// Generate a Debug Log Message
		/// </summary>
		/// <param name="sender">The TestDebug Button Control</param>
		/// <param name="e">Additional arguments pertaining to the Click event</param>
		private void TestDebug_Click(object sender, RoutedEventArgs e)
		{
			//Log.d("This is a Debug Message");
		}

		/// <summary>
		/// Generate a Binding Error Log Message
		/// </summary>
		/// <param name="sender">The TestBindingError Button Control</param>
		/// <param name="e">Additional arguments pertaining to the Click event</param>
		private void TestBindingError_Click(object sender, RoutedEventArgs e)
		{
			//Add Binding Error Output Call here
		}

		/// <summary>
		/// Exit the application
		/// </summary>
		/// <param name="sender">The Exit Button Control</param>
		/// <param name="e">Additional arguments pertaining to the Click event</param>
		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
		}

		private void TestInfo_MouseEnter(object sender, MouseEventArgs e)
		{
			Log.RegisterMessageType(typeof(MouseEventHandler));
			//Log.RegisterMessageType(typeof(MouseEventHandler), typeof(MainWindow));
			//Log.i("Type: MouseEvent[Explicit] Parent:MainWindow[Explicit]", new MessageType() { 
			//    Parent = typeof(MainWindow), 
			//    Children = new List<MessageType>(), 
			//    Description = "Mouse Events handled by the Main Window", 
			//    Name="MouseEvent", 
			//    Type = ((MouseEventHandler)TestInfo_MouseEnter).GetType()});
		}

		private void TestInfo_MouseLeave(object sender, MouseEventArgs e)
		{

		}

		private void TestWarning_MouseEnter(object sender, MouseEventArgs e)
		{

		}

		private void TestWarning_MouseLeave(object sender, MouseEventArgs e)
		{

		}

		private void TestError_MouseEnter(object sender, MouseEventArgs e)
		{

		}

		private void TestError_MouseLeave(object sender, MouseEventArgs e)
		{

		}
	}
}
