using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace mkv_sama
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MkvFileRep LoadedMkv { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            //var b = new Binding("AudioStreams");
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AudioStreamItem sel = (AudioStreamItem)AudioStreamPicker.SelectedItem;
            ((Label)sender).Content = sel?.Index.ToString();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            // Check filetype
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) { return; }
            string filepath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            // Actually Do something

            try
            {
                LoadedMkv = MkvFileRep.BuildMkvFileRep(filepath);
            } catch(FileLoadException fle)
            {
                MessageBox.Show($"Error Processing file {fle.FileName}");
            } catch(FileNotFoundException fnfe)
            {
                MessageBox.Show(fnfe.ToString());
            } catch(FileFormatException ffe)
            {
                MessageBox.Show(ffe.ToString());
            }

            AudioStreamPicker.ItemsSource = LoadedMkv.AudioStreams;
            InputFilename.Text = LoadedMkv.Filename;
            OutputFilename.Text = Path.ChangeExtension(LoadedMkv.Filepath, ".mp4");
            OutputFilename.IsEnabled = true;
            ConvertButton.IsEnabled = true;
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: make directory if it doesn't exist

            // Make sure in case of overwriting.
            if (File.Exists(OutputFilename.Text))
            {
                if (MessageBox.Show("File exists. Overwrite?", "Overwrite file?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                {
                    return;
                }
            }

            // Call FFMpeg
            if (FFMpeg.ConvertWithSingleAudio(LoadedMkv.Filepath, OutputFilename.Text, ((AudioStreamItem)AudioStreamPicker.SelectedItem).Index) > 0)
            {
                MessageBox.Show("An error happened while converting. Check the result.");
            };
        }
    }
}
