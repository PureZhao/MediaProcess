using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GifTools.Utils
{
    
    public static class GifUtils
    {
        private static int PropertyTagFrameDelay { get; } = 0x5100;
        private static int PropertyTagLoopCount { get; } = 0x5101;
        private static short PropertyTagTypeLong { get; } = 4;
        private static short PropertyTagTypeShort { get; } = 3;
        private static int UIntBytes { get; } = 4;
        private static ImageCodecInfo encoder;
        private static EncoderParameters firstFrameParams;
        private static EncoderParameters otherFrameParams;
        private static EncoderParameters writeOpParams;
        private static PropertyItem delaySetting;
        private static PropertyItem loopSetting;
        // Ensure filename is existed
        public static Bitmap[] GetGifSequence(string filename)
        {
            Image src = Image.FromFile(filename);
            int width = src.Width;
            int height = src.Height;
            FrameDimension dimension = new FrameDimension(src.FrameDimensionsList[0]);
            int frameCount = src.GetFrameCount(dimension);
            Bitmap[] sequence = new Bitmap[frameCount];
            for (int i  = 0; i < frameCount; i++)
            {
                src.SelectActiveFrame(dimension, i);
                Bitmap frame = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(frame);
                g.Clear(Color.Transparent);
                g.DrawImage(src, 0, 0);
                sequence[i] = frame;
            }

            return sequence;
        }

        // Ensure not Zero and file existed
        public static MemoryStream MakeGif(string[] filenames)
        {
            if(encoder == null)
            {
                encoder = ImageCodecUtils.GetCodecInfo(ImageFormat.Gif);
            }

            if(firstFrameParams == null)
            {
                firstFrameParams = new EncoderParameters(1);
                firstFrameParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
            }
            if (otherFrameParams == null)
            {
                otherFrameParams = new EncoderParameters(1);
                otherFrameParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionTime);
            }
            if (writeOpParams == null)
            {
                writeOpParams = new EncoderParameters(1);
                writeOpParams.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.Flush);
            }
            if(delaySetting == null)
            {
                // 延迟设置（显然PropertyItem 没有其他方法去创建实例）
                delaySetting = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                delaySetting.Id = PropertyTagFrameDelay;
                delaySetting.Type = PropertyTagTypeLong;
                // 长度
                delaySetting.Len = filenames.Length * UIntBytes;
                // The value is an array of 4-byte entries: one per frame.
                // Every entry is the frame delay in 1/100-s of a second, in little endian.
                delaySetting.Value = new byte[filenames.Length * UIntBytes];
            }
            if(loopSetting == null)
            {
                // 设置循环
                loopSetting = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                loopSetting.Id = PropertyTagLoopCount;
                loopSetting.Type = PropertyTagTypeShort;
                loopSetting.Len = 1;
                // 0 代表永远循环
                loopSetting.Value = BitConverter.GetBytes((ushort)0);
            }

            // 设置每帧延迟 例如 100 为 1秒 5 则为 0.05秒
            var frameDelayBytes = BitConverter.GetBytes((uint)5);

            for (int i = 0; i < filenames.Length; i++)
            {
                Array.Copy(frameDelayBytes, 0, delaySetting.Value, i * UIntBytes, UIntBytes);
            }
            Bitmap r = null;
            MemoryStream stream = new MemoryStream();
            for (int i = 0; i < filenames.Length; i++)
            {

                Bitmap bitmap = new Bitmap(filenames[i]);
                if(r == null)
                {
                    r = bitmap;
                    r.SetPropertyItem(delaySetting);
                    r.SetPropertyItem(loopSetting);
                    r.Save(stream, encoder, firstFrameParams);
                }
                else
                {
                    r.SaveAdd(bitmap, otherFrameParams);
                }

            }
            r.SaveAdd(writeOpParams);
            
            return stream;
        }
    }
}
