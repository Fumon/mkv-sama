using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace mkv_sama
{
    static class FFProbe
    {
        private static readonly string ffprobepath = @"C:\Users\signu\bin\ffmpeg-2021-01\bin\ffprobe.exe";
        public static FFProbeAudioResults GetAudioStreams(string filepath)
        {
            // TODO: test for ffprobe executable existence
            // TODO: get ffprobe from PATH or something
            var pstart = new ProcessStartInfo
            {
                FileName = ffprobepath,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                ArgumentList =
                {
                    "-v", "quiet",
                    "-hide_banner",
                    "-of", "json=c=1",
                    "-select_streams", "a",
                    "-show_entries", "stream_tags=title:stream=index",
                    filepath
                }
            };

            var probe = new Process
            {
                StartInfo = pstart
            };

            // Setup output handler
            StringBuilder output = new();
            probe.OutputDataReceived += (sender, e) =>
            {
                output.Append(e.Data);
            };

            probe.Start();

            probe.BeginOutputReadLine();
            // TODO: maybe use a timeout here
            probe.WaitForExit();

            if(probe.ExitCode != 0)
            {
                throw new FileLoadException("ffprobe returned 1", filepath);
            }

            Console.WriteLine(output);

            // Parse result
            FFProbeAudioResults parsedResults = JsonConvert.DeserializeObject<FFProbeAudioResults>(output.ToString());

            return parsedResults;
        }

        public class FFProbeAudioResults
        {
            public class Stream
            {
                [JsonProperty("index")]
                public uint Index { get; set; }
                [JsonProperty("tags")]
                public Dictionary<string, string> Tags { get; set; }
            }

            [JsonProperty("programs")]
            public List<string> Programs { get; set; }
            [JsonProperty("streams")]
            public List<Stream> Streams { get; set; }
        }
    }
}
