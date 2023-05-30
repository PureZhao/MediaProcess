using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GifTools.Core
{
    public static class GlobalConfig
    {
        public static Font DefaultFont { get; } = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, (byte)(0));
    }
}
