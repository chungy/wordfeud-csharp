/*
 * This file is part of wordfeud-csharp.
 *
 * Wordfeud-csharp is free software: you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version.
 *
 * Wordfeud-csharp is distributed in the hopes that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with wordfeud-csharp.  If not, see
 * <http://www.gnu.org/licenses/>.
 */

using System;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Text;

namespace Wordfeud.Client
{
    public class Game
    {
        private readonly Board _board;
        public Board Board
        {
            get { return _board; }
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public DateTime Updated { get { return Epoch.AddSeconds(Response["content"]["game"]["updated"].Value<ulong>()); } }
        public DateTime Created { get { return Epoch.AddSeconds(Response["content"]["game"]["created"].Value<ulong>()); } }
        public bool Running { get { return Response["content"]["game"]["is_running"].Value<bool>(); } }
        //public IList<Message> Chat { get; internal set; }

        public string GameID { get { return Response["content"]["game"]["id"].Value<string>(); } }

        public byte MoveCount { get { return Response["content"]["game"]["move_count"].Value<byte>(); } }
        public byte BagCount { get { return Response["content"]["game"]["bag_count"].Value<byte>(); } }
        public byte PassCount { get { return Response["content"]["game"]["pass_count"].Value<byte>(); } }
        //public byte RuleSet { get { return Response["content"]["game"]["ruleset"].Value<byte>(); } }
        public RuleSets.RuleSet RuleSet { get { return RuleSets.RuleSet.GetRuleset(Response["content"]["game"]["ruleset"].Value<byte>()); } }
        public bool GameOver { get { return EndGame != 0; } }
        public bool OurTurn { get { return CurrentPlayer == (Players[0]["id"].Value<string>().Equals(Session.UserID) ? 0 : 1); } }
        public string WhoseTurn { get { return Players[CurrentPlayer]["username"].Value<string>(); } }

        //seen_finished bool no idea what this is for

        public Player Player
        {
            get
            {
                foreach (JToken token in Players)
                {
                    if (token["username"].Value<string>().Equals(Session.Username, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return new Player(token);
                    }
                }
                return null;
            }
        }

        public Player Opponent
        {
            get
            {
                foreach (JToken token in Players)
                {
                    if (!token["username"].Value<string>().Equals(Session.Username, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return new Player(token);
                    }
                }
                return null;
            }
        }

        public Move LastMove { get { return new Move(Response["content"]["game"]["last_move"], new[] { Player, Opponent }); } }

        //====

        private Session Session { get; set; }
        private byte CurrentPlayer { get { return Response["content"]["game"]["current_player"].Value<byte>(); } }
        private byte EndGame { get { return Response["content"]["game"]["end_game"].Value<byte>(); } }

        private JObject Response { get; set; }
        private JToken Players { get { return Response["content"]["game"]["players"]; } }
        private ushort BoardID { get { return Response["content"]["game"]["board"].Value<ushort>(); } }


        //====

        internal Game(Session session, uint id)
        {
            Session = session;

            Response = Session.Request("/wf/game/" + id + "/");
            if (Response == null) { throw new WebException(); }

            JObject board = Session.Request("/wf/board/" + BoardID + "/");
            if (board == null) { throw new WebException(); }
            _board = new Board(board);

            LoadTiles();
        }

        private void LoadTiles()
        {
            JToken tiles = Response["content"]["game"]["tiles"];
            if (tiles == null) { throw new WebException(); }

            foreach (JToken tile in tiles)
            {
                int x = tile[0].Value<int>();
                int y = tile[1].Value<int>();
                char letter = tile[2].Value<char>();
                bool blank = tile[3].Value<string>() == "true";

                Board[x, y].Letter = letter;
                Board[x, y].Value = RuleSet.Value(letter);
                Board[x, y].Blank = blank;
            }
        }

        public override string ToString() { return "Vs. " + Opponent.UserName; }


        public struct WordPlay
        {
            public string Word;
            public int X;
            public int Y;
            public bool Vertical;
        }

        public bool Play(WordPlay word)
        {
            int dx = word.Vertical ? 0 : 1;
            int dy = word.Vertical ? 1 : 0;

            int x = word.X;
            int y = word.Y;

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"ruleset\":0,\"move\":[");
            bool first = true;
            foreach (char letter in word.Word)
            {
                if (Board[x, y].Letter == ' ')
                {
                    if (!first)
                        sb.Append(",");
                    first = false;
                    string ls = letter.ToString(CultureInfo.InvariantCulture);
                    string upper = ls.ToUpper();
                    string blank = "false";
                    if (upper != ls)
                        blank = "true";
                    sb.Append("[" + x + "," + y + ",\"" + upper + "\"," + blank + "]");
                }
                x += dx;
                y += dy;
            }
            sb.Append("]}");

            string data = sb.ToString();
            JObject req = Session.Request("/wf/game/" + GameID + "/move/", data);
            if (req == null) { return false; }
            string status = req["status"].Value<string>();
            return (status == "success");
        }

        public bool Pass()
        {
            JObject req = Session.Request("/wf/game/" + GameID + "/pass/");
            if (req == null) { return false; }
            string status = req["status"].Value<string>();
            return (status == "success");
        }
    }
}
