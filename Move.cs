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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Wordfeud.Client
{
    public class Move
    {
        public enum MoveType
        {
            Move,
            Swap,
            Pass
        }

        public List<Tile> Tiles { get; set; }
        public short Points { get; set; }
        private MoveType _type;
        public MoveType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Player Player { get; set; }
        public string Word { get; set; }

        public Move(JToken token, IEnumerable<Player> players)
        {
            Tiles = new List<Tile>();
            foreach (JToken tileToken in token["move"]) { Tiles.Add(new Tile(tileToken)); }
            Points = token["points"].Value<short>();
            Enum.TryParse(token["move_type"].Value<string>(), true, out _type);
            Player = players.First(player => player.ID == token["user_id"].Value<uint>());
            Word = token["main_word"].Value<string>();
        }
    }
}
