using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GifTools.Core
{
    class GifMaker
    {
        string[] filenames;
        Bitmap[] frames;
        const int PropertyTagFrameDelay = 0x5100;
        const int PropertyTagLoopCount = 0x5101;
        const short PropertyTagTypeLong = 4;
        const short PropertyTagTypeShort = 3;
        const int UintBytes = 4;

        ImageCodecInfo encoder;
        EncoderParameters firstFrameParams;
        EncoderParameters otherFrameParams;
        EncoderParameters writeOperationParams;
        PropertyItem delaySetting;
        PropertyItem loopSetting;

        public GifMaker()
        {
            Init();
        }

        public GifMaker(string folderPath, string suffix)
        {
            Init();
            filenames = Directory.GetFiles(folderPath, "*" + suffix);
            frames = new Bitmap[filenames.Length];
            for (int i = 0; i < filenames.Length; i++)
            {
                frames[i] = (Bitmap)Bitmap.FromFile(filenames[i]);
            }
        }

        public GifMaker(string[] filenames)
        {
            Init();
            frames = new Bitmap[filenames.Length];
            for (int i = 0; i < filenames.Length; i++)
            {
                frames[i] = (Bitmap)Bitmap.FromFile(filenames[i]);
            }
        }
        private void Init()
        {
            encoder = ImageCodecUtils.GetCodecInfo(ImageFormat.Gif);
            // 第一帧决定图片形式
            firstFrameParams = new EncoderParameters(1);
            firstFrameParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
            // 其他帧处理
            otherFrameParams = new EncoderParameters(1);
            otherFrameParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionTime);
            // 最后
            writeOperationParams = new EncoderParameters(1);
            writeOperationParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.Flush);

            // 延迟设置（显然PropertyItem 没有其他方法去创建实例）
            delaySetting = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            delaySetting.Id = PropertyTagFrameDelay;
            delaySetting.Type = PropertyTagTypeLong;
            // 长度
            delaySetting.Len = filenames.Length * UintBytes;
            // The value is an array of 4-byte entries: one per frame.
            // Every entry is the frame delay in 1/100-s of a second, in little endian.
            delaySetting.Value = new byte[filenames.Length * UintBytes];

            // 设置循环
            loopSetting = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            loopSetting.Id = PropertyTagLoopCount;
            loopSetting.Type = PropertyTagTypeShort;
            loopSetting.Len = 1;
            // 0 代表永远循环
            loopSetting.Value = BitConverter.GetBytes((ushort)0);
        }
        public void OutputGif(string savePath)
        {
            // 设置每帧延迟 例如 100 为 1秒 5 则为 0.05秒
            var frameDelayBytes = BitConverter.GetBytes((uint)5);

            for (int i = 0; i < filenames.Length; i++)
            {
                Array.Copy(frameDelayBytes, 0, delaySetting.Value, i * UintBytes, UintBytes);
            }

            // 写入文件
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                bool first = true;
                Bitmap map = null;

                foreach (Bitmap frame in frames)
                {
                    if (first)
                    {
                        map = frame;
                        map.SetPropertyItem(delaySetting);
                        map.SetPropertyItem(loopSetting);
                        map.Save(stream, encoder, firstFrameParams);
                        first = false;
                    }
                    else
                    {
                        map.SaveAdd(frame, otherFrameParams);
                    }
                }
                map.SaveAdd(writeOperationParams);
            }

        }

        public void Video2Gif(string filename, string cutTimeFrame, string size)
        {
            string ffmpegPath = "ffmpeg/ffmpeg.exe";
            string outputFilename = "VideoSequence/%03d.png";
            ProcessStartInfo process = new ProcessStartInfo(ffmpegPath)
            {
                WindowStyle = ProcessWindowStyle.Hidden,

                Arguments = " -i " + filename  // 视频路径
                                + " -r 1" //提取图片的帧率
                                + " -y -frames 100 -f image2 -ss " + cutTimeFrame
                                + " -t 513 -s " + size // 设置分辨率
                                + " " + outputFilename,
                RedirectStandardOutput = true,
            };
            Console.WriteLine(process.Arguments);
            Process p = Process.Start(process);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(p.StandardOutput.ReadToEnd().Length);

            p.WaitForExit();
            p.Close();
            p.Dispose();
        }
    }
}
