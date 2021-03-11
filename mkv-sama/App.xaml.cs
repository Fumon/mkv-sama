using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace mkv_sama
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        private void AppStartup(object sender, StartupEventArgs args)
        {
            //TODO: Handle launching via send-to, drag-onto, or terminal
            // Test data
            AddTestAudioStreams();
        }

        private void AddTestAudioStreams()
        {
            //AudioStreams.Add(new(1, "Desktop and Mic"));
            //AudioStreams.Add(new(2, "Desktop"));
            //AudioStreams.Add(new(93, "Mic"));

            //Console.Out.WriteLine("Finished plugging in test data");
        }
    }
}
