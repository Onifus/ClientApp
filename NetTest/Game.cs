using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetTest
{
    public class Game
    {
        public void Draw(PictureBox pictureBox)
        {
            var g = pictureBox.CreateGraphics();

            g.DrawLine(new Pen(Color.Black), 10, 55, 100, 100);
        }
    }
}
