using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace mkv_sama
{
    public static class FFMpeg
    {
        private static readonly string ffmpegpath = @"C:\Users\signu\bin\ffmpeg-2021-01\bin\ffmpeg.exe";
        public static int ConvertWithSingleAudio(string inFilePath, string outFilePath, uint audioStreamIndex)
        {
            var pstart = new ProcessStartInfo
            {
                FileName = ffmpegpath,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                ArgumentList =
                {
                    "-i", inFilePath,
                    "-y", // Say yes to overwrite
                    "-hide_banner",
                    "-map", "0:0", "-c:v", "copy",
                    "-map", $"0:{audioStreamIndex}",
                    outFilePath
                }
            };

            var p = Process.Start(pstart);
            Debug.Write(p.StandardError.ReadToEnd());
            p.WaitForExit();
            return p.ExitCode;
        }
    }
}
