using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GifTools.Core
{
    class GifMaker
    {
        string filename;
        public GifMaker() { }
        public void SetSrc(string filename)
        {
            this.filename = filename;
        }
    }
}
