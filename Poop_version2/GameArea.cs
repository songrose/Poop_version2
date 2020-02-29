using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poop_version2
{


    public partial class GameArea : Form
    {
        int GameAreaWidth = Boundary.windowSizeX;
        int GameAreaHeight = Boundary.windowSizeY;
        Game game;
        //GameInfoHolder
        GameInfoHolder infoHolder;
        public GameArea()
        {
            game = new Game(this);
            this.Height = GameAreaHeight;
            this.Width = GameAreaWidth;
            InitializeComponent();


            btnStart.Click += new System.EventHandler(StartButtonClicked);
        }
   
        private void GameArea_Paint(object sender, PaintEventArgs e)
        {
            Boundary.drawBorder(e);
        }
        public Label GetScoreLabel(int num)
        {
            switch (num)
            {
                case 1:
                    return label1;
                case 2:
                    return label2;
                case 3:
                    return label3;
                case 4:
                    return label4;
                default:
                    return null;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int x = game.you.Location.X;
            int playerSpeed = 25;

            if (game.you.GetActive() && keyData == Keys.Left && x - 25 >= Boundary.startX)
            {
                //  game.you.Location = new Point(x - playerSpeed, game.you.Location.Y);
                game.you.x = x - playerSpeed;
            
            }
            else if (game.you.GetActive() && keyData == Keys.Right && x + playerSpeed < (Boundary.endX))
            {
               // game.you.Location = new Point(x + playerSpeed, game.you.Location.Y);
                game.you.x = x + playerSpeed;
            }


         

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void StartButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            //SERVER PSEUDO CODE
            /**
             * 
             * if(Game.infoHolder.status == "lobby" || Game.infoHolder.status == "scoreboard"){
             *      send to server new game status so server can go from  lobby||scoreboard -> active
             *      game.Start();
             * }
             * 
             */
            btnStart.Enabled = false;

        }

        public void EnableStartButton()
        {

            btnStart.Enabled = true;
        }
    }
}
