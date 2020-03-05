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
    public class Player : Movable
    {        
        public int playerNumber { get; set; }
        public bool alive { get; set; }
        public bool ready { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public Player(int num, string fileName)
        {
            playerNumber = num;
            alive = true;
            setImage(fileName);
        }

        [JsonConstructor]
        public Player(int playerNumber, bool alive, bool ready, int x, int y)
        {
            this.playerNumber = playerNumber;
            this.alive = alive;
            this.ready = ready;
            this.x = x;
            this.y = y;
        }

        public void GameLose()
        {
            alive = false;
            this.BackColor = Color.Red;
        }
        public void Revive()
        {
            
            alive = true;
            this.BackColor = Color.Transparent;
        }

        public bool GetAlive()
        {
            return alive;
        }        
    }
}
