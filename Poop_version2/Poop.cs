using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Poop_version2
{
    class Poop : Movable
    {
        public Poop(int randomNumber, String fileName)
        {

            Random random = new Random();
          //  int randomNumber = random.Next(Boundary.startX, Boundary.endX - 20);
            this.Size = new Size(20, 20);
            this.Location = new Point(randomNumber, Boundary.startY);
            this.BackColor = Color.Transparent;
            setImage(fileName);
        }

        //for setting poop based on incoming location
        public Poop(String fileName, Point location)
        {
            this.Size = new Size(20, 20);
            this.Location = location;

            this.BackColor = Color.Transparent;
            setImage(fileName);
        }
    }
}
