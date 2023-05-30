using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GifTools.Core
{
    class GifExtractor
    {
        Image image;
        FrameDimension frameDimension;
        public GifExtractor() { }
        public GifExtractor(string filename) 
        {
            image = Image.FromFile(filename);
            frameDimension = new FrameDimension(image.FrameDimensionsList[0]);
        }

        ~GifExtractor() 
        {
            image.Dispose();
            frameDimension = null;
        }

        public void SetGif(string filename)
        {
            image = Image.FromFile(filename);
            frameDimension = new FrameDimension(image.FrameDimensionsList[0]);
        }

        public Bitmap[] GetSequence()
        {
            int frameCount = image.GetFrameCount(frameDimension);
            Bitmap[] sequence = new Bitmap[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                Bitmap bitmap = new Bitmap(image.Width, image.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(Color.Transparent);
                image.SelectActiveFrame(frameDimension, i);
                g.DrawImage(image, 0, 0);
                sequence[i] = bitmap;
            }

            return sequence;
        }

        public void SaveSequence(string saveDir)
        {
            if (Directory.Exists(saveDir))
            {
                Directory.Delete(saveDir, true);
            }
            Directory.CreateDirectory(saveDir);
            int frameCount = image.GetFrameCount(frameDimension);
            for(int i = 0; i < frameCount; i++)
            {
                string savePath = Path.Combine(saveDir, $"{i}.png");
                image.SelectActiveFrame(frameDimension, i);
                image.Save(savePath, ImageFormat.Png);
            }
        }
    }
}
