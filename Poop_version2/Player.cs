using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Poop_version2
{
    class Player : Movable
    {
        public int playerNum;
        public Label scorelabel;
        private int score = 0;
        private bool active;
        public int x;
        public int y;
        
        public Player(int num, string fileName)
        {
            playerNum = num;
            active = true;
            setImage(fileName);
        }
        public void SetScore(int n)
        {
            score = n;
            scorelabel.Text = "Player " + playerNum + " score: " + score;

        }
        public void GameLose()
        {
            active = false;
            this.BackColor = Color.Red;
        }
        public void Revive()
        {
            SetScore(0);
            active = true;
            this.BackColor = Color.Transparent;
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetLabel(Label label)
        {
            scorelabel = label;
            scorelabel.Text = "Player " + playerNum + " score: " + score;
        }
    }
}
