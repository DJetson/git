using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace LogSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public StartupEventArgs StartupEventArgs;

        protected override void OnStartup(StartupEventArgs e)
        {
            StartupEventArgs = e;
        }
    }
}
