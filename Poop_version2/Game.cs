using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Net.WebSockets;


namespace Poop_version2
{
    class Game
    {
        public Player you;
        public string status;
        public Timer gameTime;
        public GameArea form;
        public Movable platform;
        public GameInfoHolder infoHolder;
        //use to keep track of garbage collection to manually trigger garbage collection.
        protected static int counter = 0;
        const int playerSize = 25;


        //the length of how much the poop goes down .
        public int goDown = 12;



        public Game(GameArea form)
        {



            //TODO

            /////////////////////////////////////////////////////////

            /**
             * 
             * Client connects to server in this piece of code please
             * PSEUDO CODE INVOLVING THE SERVER
             * 
             * if(webSocket == null){
             *      webSocket = new WebSocket();
             *      gameinfo = new GameInfoHolder(string stringFromBroadcastAll);
             *      
             * }
             * 
             * you = new Player(int UniqueIDFromServer, System.IO.Path.Combine(Application.StartupPath, @"..\..\Images\nerd.png"));
             * 
             * 
             */
            gameTime = new Timer
            {
                //Enabled = true,
                Interval = 5
            };
            gameTime.Tick += new EventHandler(OnGameTimeTick);
            this.form = form;
            Movable.mainForm = form;


            //TODO
            ///////////////////////DELETE THIS PIECE OF CODE AFTER WE IMPLEMENT THE SERVER CODE//////////////////////////////////
            you = CreatePlayer(0);

            infoHolder = new GameInfoHolder("", this);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //draws the bottom line
            platform = new Movable
            {
                Size = new Size(Boundary.windowSizeX, 3),
                Location = new Point(Boundary.startX, Boundary.endY),
                BackColor = Color.Black
            };


            platform.BringToFront();

        }
        private void OnGameTimeTick(object sender, EventArgs e)
        {

            //keeps track of how fast the poop falls
            //and how often the garbage collection is triggered.
            counter++;
            if (counter == 100)
            {
                if (goDown < 1000)
                {
                    goDown += 1;

                }
                System.GC.Collect();
                counter = 0;
            }

            //following code triggered every 5 clicks

            if((counter % 2) == 0)
            {

                int checkBottom = Boundary.endY - 70;
                foreach (Poop p in infoHolder.poops.ToArray())
                {

                    if (p.Location.Y > checkBottom)
                    {
                        //poop checking for if it hits the ground
                        if (p.Bounds.IntersectsWith(platform.Bounds))
                        {
                            infoHolder.poops.Remove(p);
                            p.Remove();
                            you.SetScore(Score.score++);


                        }
                        if (you.GetActive() && p != null && p.Bounds.IntersectsWith(you.Bounds))
                        {
                            you.SetScore(Score.score);
                            infoHolder.poops.Remove(p);
                            p.Remove();
                            you.GameLose();
                        }
                    }
                }

            }
            //todo
            /*
             * 
             * Send server information of the player
             * send 
             * you.x;
             * you.score;
             * 
             * 
             * 
             */

        }

        public Player CreatePlayer(int playerNum)
        {
            Player newPlayer = new Player(playerNum, System.IO.Path.Combine(Application.StartupPath, @"..\..\Images\nerd.png"))
            {
                Size = new Size(playerSize, playerSize),
                Location = new Point(
                    Boundary.startX + Boundary.windowSizeX / 2,
                    Boundary.startY + Boundary.windowSizeY - playerSize)
            };
            newPlayer.SetLabel(form.GetScoreLabel(newPlayer.playerNum));
            return newPlayer;
        }
    }
}