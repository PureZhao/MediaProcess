using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GifTools.Core
{
    public static class AlignUtils
    {
        public static void HorizontalCenterAlign(params Control[] controls)
        {
            // 找最高的那一个组件
            Control highestControl = controls[0];
            for(int i = 1; i < controls.Length; i++)
            {
                if (controls[i].Height > highestControl.Height)
                {
                    highestControl = controls[i];
                }
            }

            for(int i = 0 ; i < controls.Length; i++)
            {
                if (controls[i] != highestControl)
                {
                    int offset = highestControl.Height - controls[i].Height;
                    int x = controls[i].Location.X;
                    int y = highestControl.Location.Y + offset / 2;
                    controls[i].Location = new Point(x, y);
                }
            }
        }
    }
}
