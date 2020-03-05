using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace Poop_version2
{
    public class Game
    {
        HubConnection hubConnection;
        IHubProxy hubProxy;

        public int you;
        public string status;
        public bool active;
        public List<Player> players = new List<Player>();
        public List<Poop> poops = new List<Poop>();
        public Timer gameTime;        
        public GameArea form;
        public Movable platform;
        public GameInfoHolder infoHolder;
        //use to keep track of garbage collection to manually trigger garbage collection.
        protected static int counter = 0;
        const int playerSize = 20;

        //the length of how much the poop goes down
        public int goDown = 12;

        public Game(GameArea form)
        {
            configureConnection();
             
            // you = new Player(int UniqueIDFromServer, System.IO.Path.Combine(Application.StartupPath, @"..\..\Images\nerd.png"));

            this.form = form;
            Movable.mainForm = form;
            
            //draws the bottom line
            platform = new Movable
            {
                Size = new Size(Boundary.windowSizeX, 3),
                Location = new Point(Boundary.startX, Boundary.endY),
                BackColor = Color.Black
            };

            platform.BringToFront();
        }
        
        public void Render(GameInfoHolder gameInfo)
        {
            List<Player> newPlayers = gameInfo.players;

            // if old poop does not exist in new poop, remove it
            foreach (Player player in players)
            {
                if (!newPlayers.Exists(x => x.playerNumber.Equals(player.playerNumber)))
                {
                    player.Remove();
                }
            }

            foreach (Player player in newPlayers)
            {
                // if there is a poop with same id, update location
                if (players.Find(x => x.playerNumber.Equals(player.playerNumber)) != null)
                {
                    players.Find(x => x.playerNumber.Equals(player.playerNumber)).Location = new Point(player.x, player.y);
                }
                // if it is a brandnew poop, create new poop
                else
                {
                    players.Add(CreatePlayer(player.playerNumber));
                }
            }


            List<Poop> newPoops = gameInfo.poops;            

            // if old poop does not exist in new poop, remove it
            foreach (Poop poop in poops)
            {
                if (!newPoops.Exists(x => x.id.Equals(poop.id)))
                {
                    poop.Remove();
                }
            }

            foreach (Poop poop in newPoops)
            {                
                // if there is a poop with same id, update location
                if (poops.Find(x => x.id.Equals(poop.id)) != null)
                {
                    poops.Find(x => x.id.Equals(poop.id)).Location = new Point(poop.x, poop.y);
                }
                // if it is a brandnew poop, create new poop
                else
                {                    
                    poops.Add(new Poop(poop.id, poop.x, System.IO.Path.Combine(Application.StartupPath, @"..\..\Images\poop.png")));
                }
            }
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
            return newPlayer;
        }

        internal List<Player> GetPlayers()
        {
            return players;
        }

        public Player GetPlayer(int index)
        {
            return players.Find(x => x.playerNumber.Equals(index));
        }

        internal List<Poop> GetPoops()
        {
            return poops;
        }

        delegate void MyLogDelegate(string text);

        private void MyLog(string text)
        {
            if (this.form.InvokeRequired)
            {
                MyLogDelegate t = new MyLogDelegate(MyLog);
                this.form.Invoke(t, text);
            }
            else
            {
                form.Log(text);
            }   
        }

        delegate void NewPlayerDelegate(int playerNumber);

        private void NewPlayer(int playerNumber)
        {
            if (this.form.InvokeRequired)
            {
                NewPlayerDelegate t = new NewPlayerDelegate(NewPlayer);
                this.form.Invoke(t, playerNumber);
            }
            else
            {
                players.Add(CreatePlayer(playerNumber));
                you = playerNumber;
            }
        }

        private GameInfoHolder RestoreGameInfoHolder(string jsonObject)
        {
            GameInfoHolder result = null;
            if (this.form.InvokeRequired)
            {
                this.form.BeginInvoke(new Action(() => result = JsonConvert.DeserializeObject<GameInfoHolder>(jsonObject)));
            }
            else
            {
                result = JsonConvert.DeserializeObject<GameInfoHolder>(jsonObject);
            }
            return result;
        }

        private async void configureConnection()
        {                                    
            hubConnection = new HubConnection("https://signalrwebserver20200303060919.azurewebsites.net");
            hubProxy = hubConnection.CreateHubProxy("PoopHub");
            hubProxy.On<int>("AssignPlayerNumber", (playerNumber) =>
            {
                MyLog("Player number is assigned: " + playerNumber.ToString());
                NewPlayer(playerNumber);
            });
            hubProxy.On("GameStartAtServer", () =>
            {
                MyLog("game is started at the server");
            });
            hubProxy.On<string>("debuggingMessage", (msg) =>
            {
                MyLog(msg + " from server recieved.");
            });
            hubProxy.On<string>("Update", (jsonObject) =>
            {                                
                Console.WriteLine(jsonObject);
                Render(JsonConvert.DeserializeObject<GameInfoHolder>(jsonObject));
            });
            await ConnectServerAsync();
        }

        private async Task ConnectServerAsync()
        {
            try
            {
                await hubConnection.Start();
                await hubProxy.Invoke("ConnectClient");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void Ready()
        {
            try
            {             
                await hubProxy.Invoke("Ready", 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void PlayerMove(int x)
        {
            try
            {                
                await hubProxy.Invoke("PlayerMove", you, x);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}