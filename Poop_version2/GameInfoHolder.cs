using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Net.WebSockets;
using Poop_version2;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Poop_version2
{
    public class GameInfoHolder
    {
        public bool active;
        public List<Player> players = new List<Player>();
        public List<Poop> poops = new List<Poop>();

        public GameInfoHolder(Game game)
        {
            active = game.active;
            players = game.GetPlayers();
            poops = game.GetPoops();
        }

        [JsonConstructor]
        public GameInfoHolder(bool active, List<Player> players, List<Poop> poops)
        {
            this.active = active;
            this.players = players;
            this.poops = poops;
        }
    }
}
