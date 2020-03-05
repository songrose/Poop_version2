using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json;

namespace Poop_version2
{
    public class Poop : Movable
    {
        public string id { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public Poop(string id, int randomNumber, String fileName)
        {
            this.id = id;
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

        [JsonConstructor]
        public Poop(string id, int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }
    }
}
