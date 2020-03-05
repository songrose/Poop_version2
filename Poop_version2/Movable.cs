using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Poop_version2
{
    public class Movable
    {

        public PictureBox pictureBox;
        static public Form mainForm;
        public int Height
        {
            get { return pictureBox.Size.Height; }
        }
        public int Width
        {
            get { return pictureBox.Size.Width; }
        }
        public Size Size
        {
            get { return pictureBox.Size; }
            set
            {
                if (value.Height > 0 && value.Width > 0)
                    //pictureBox.Size = value;
                    SizeThread(value);
            }
        }
        private void SizeThread(Size value)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action(() => pictureBox.Size = value));
            }
            else
            {
                pictureBox.Size = value;
            }
        }

        public Rectangle Bounds
        {
            get { return pictureBox.Bounds; }
        }
        public Point Location
        {
            get { return pictureBox.Location; }
            set { LocationThread(value); }
        }
        private void LocationThread(Point value)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action(() => pictureBox.Location = value));
            }
            else
            {
                pictureBox.Location = value;
            }
        }
        public Color BackColor
        {
            get { return pictureBox.BackColor; }
            set { pictureBox.BackColor = value; }
        }
        public void setImage(String fileName)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = Image.FromFile(fileName);
        }
        public Movable()
        {
            pictureBox = new PictureBox();            
            MovavleThread(pictureBox);
        }

        private void MovavleThread(PictureBox pictureBox)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action(() => mainForm.Controls.Add(pictureBox)));
            }
            else
            {
                mainForm.Controls.Add(pictureBox);
            }
        }

        public Movable(Form form)
        {
            pictureBox = new PictureBox();
            form.Controls.Add(pictureBox);
        }
        public void BringToFront()
        {
            pictureBox.BringToFront();
        }
        public void Remove()
        {
            RemoveThread();
        }

        private void RemoveThread()
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action(() => mainForm.Controls.Remove(pictureBox)));
            }
            else
            {
                mainForm.Controls.Remove(pictureBox);
            }
        }
    }
}
