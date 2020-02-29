using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Net.WebSockets;
using Poop_version2;

namespace Poop_version2
{
    class GameInfoHolder
    {
        const int playerSize = 25;

        string status { get; set; }
        int noOfPlayers;
        //this list of player includes yourself to make code less complex.
        ArrayList players = new ArrayList();
        public ArrayList poops = new ArrayList();
        //game
        Game game;
        public GameInfoHolder(string serverString, Game game)
        {
            if (serverString.Length > 0)
            {
                this.game = game;
                //Separating serverString using delimiter ! 
                // Separates "0status!1Players!2Poops
                string[] wholeInfo = serverString.Split('!');
                //updating the status.
                status = wholeInfo[0];

                //obtaining player information.
                string[] serverPlayerString = wholeInfo[1].Split('@');

                //create all the players (initially)
                // warning might cause a race condition or inconstencies in the list..
                //goes through all the players to add them.

                noOfPlayers = serverPlayerString.Length;
                for (int i = 0; i < serverPlayerString.Length; i++)
                {
                    string[] playerDesc = serverPlayerString[i].Split('#');
                    int n;
                    bool isNumber = int.TryParse(playerDesc[0], out n);
                    if (isNumber)
                    {
                        Console.WriteLine("54: Game info Holder: ");
                        if (game.you.playerNum == n)
                        {
                            players.Add(game.you);
                        }
                        else
                        {
                            players.Add((Player)createPlayer(n));

                        }
                    }
                }
            }
        } 
        
        public Player createPlayer(int playerNum)
        {
            
            Player newPlayer = new Player(playerNum, System.IO.Path.Combine(Application.StartupPath, @"..\..\Images\nerd.png"))
            {
                Size = new Size(playerSize, playerSize),
                Location = new Point(
                    Boundary.startX + Boundary.windowSizeX / 2,
                    Boundary.startY + Boundary.windowSizeY - playerSize)
            };
            newPlayer.SetLabel(game.form.GetScoreLabel(newPlayer.playerNum));
            return newPlayer;
            
        }
        public void update(string serverString)
        {

            if(serverString.Length > 0)
            {
                //Separating serverString using delimiter ! 
                // Separates "0status!1Players!2Poops
                string[] wholeInfo = serverString.Split('!');
                //updating the status.
                status = wholeInfo[0];

                    //obtaining player information.
                    string[] serverPlayerString = wholeInfo[1].Split('@');

           
                    //case where # of players changes IE: if player joins. current game only allows player to join, and players cant leave. 
                    if (noOfPlayers != serverPlayerString.Length)
                    {
                        for (int i = noOfPlayers ; i < serverPlayerString.Length; i++)
                        {
                            string[] playerDesc = serverPlayerString[i].Split('#');

                            int n;
                            bool isNumber = int.TryParse(playerDesc[0], out n);
                            if (isNumber)
                            {
                                players.Add(new Player(Int32.Parse(playerDesc[0]), System.IO.Path.Combine(Application.StartupPath, @"..\..\Images\nerd.png")));
                            }
                    }
                        noOfPlayers = serverPlayerString.Length;

                    }

                    //checking for movement for player active status.

                        for (int i = noOfPlayers; i < serverPlayerString.Length; i++)
                        {
                            string[] playerDesc = serverPlayerString[i].Split('#');
                            //move player because player is active.
                            if(playerDesc[3] == "active")
                            {
                                int n;
                                bool isNumber = int.TryParse(playerDesc[1], out n);
                                int score;
                                bool isScore= int.TryParse(playerDesc[2], out score);
                                if (isNumber && isScore)
                                {
                                    players[i] = new Point(n, ((Player)players[i]).Location.Y);
                                    ((Player)players[i]).SetScore(score);
                                }

                            }
                            //check if player just died by seeing if old active status is true and new status is dead so it goes from alive -> dead
                            else if(((Player)players[i]).GetActive() && (playerDesc[3] == "dead"))
                            {
                               ((Player)players[i]).GameLose();
                            }
                            //other case is the player is dead so don't do anything.
                        }

              //poop falls if the game is active
                if (status == "active"){

                    //adding the poop!
                    if (wholeInfo[2] != null && wholeInfo[2].Length > 0)
                    {
                        string[] poopList = wholeInfo[2].Split('@');
                        for (int i = 0; i < poopList.Length; i++)
                        {
                            int n;
                            bool isNumber = int.TryParse(poopList[0], out n);
                            if (isNumber)
                            {
                                poops.Add(new Poop(n, System.IO.Path.Combine(Application.StartupPath, @"..\..\Images\poop.png")));

                            }
                        }

                    }
                }
                //inactive game makes no poops fall.
                else
                {
                    game.gameTime.Enabled = false;
                }
            }
            

        }

    }
}
