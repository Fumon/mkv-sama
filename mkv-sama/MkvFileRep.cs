using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace mkv_sama
{
    public class MkvFileRep
    {
        public string Filepath { get; }
        public string Filename { get => Path.GetFileName(Filepath); }
        public ObservableCollection<AudioStreamItem> AudioStreams { get; }

        public static MkvFileRep BuildMkvFileRep(string filepath)
        { 
            // Check for file existence & actually a file (should shortcircuit so no try catch needed)
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("Could not find file", filepath);
            } else if(Path.GetExtension(filepath) != ".mkv" || (File.GetAttributes(filepath) & FileAttributes.Directory) == FileAttributes.Directory )
            {
                throw new FileFormatException("Must be a .mkv");
            }

            var a = FFProbe.GetAudioStreams(filepath);

            // Convert to AudioStreamItems
            List<AudioStreamItem> _as = new();
            a.Streams.ForEach(stream => {
                _as.Add(new AudioStreamItem(stream.Index, stream.Tags["title"]));
                Trace.WriteLine(stream);
            });

            return new MkvFileRep(filepath, new ObservableCollection<AudioStreamItem>(_as));
        }

        private MkvFileRep(string _Filepath, ObservableCollection<AudioStreamItem> _AudioStreams)
        {
            Filepath = _Filepath;
            AudioStreams = _AudioStreams;
        }
        
        public MkvFileRep()
        {
            Filepath = "";
            AudioStreams = new();
        }
    }
}
