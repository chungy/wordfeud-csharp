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
using Newtonsoft.Json.Linq;

namespace Wordfeud.Client
{
    public class Player
    {
        public string UserName { get { return PlayerToken["username"].Value<string>(); } }
        public byte Position { get { return PlayerToken["position"].Value<byte>(); } }
        public short Score { get { return PlayerToken["score"].Value<short>(); } }
        public uint ID { get { return PlayerToken["id"].Value<uint>(); } }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public DateTime? Updated
        {
            get
            {
                if (PlayerToken["avatar_updated"] == null) { return null; }
                return Epoch.AddSeconds(PlayerToken["avatar_updated"].Value<ulong>());
            }
        }

        public IList<Tile> Rack
        {
            get
            {
                if (PlayerToken["rack"] == null) { return null; }
                List<Tile> temp = new List<Tile>();
                foreach (JObject rackTile in PlayerToken["rack"])
                {
                    temp.Add(new Tile(rackTile));
                }
                return temp;
            }
        }

        private JToken PlayerToken { get; set; }

        public Player(JToken token) { PlayerToken = token; }

        /*public override bool Equals(object obj)
        {
            if (!(obj is Player)) { return false; }
            return Equals(obj as Player);
        }

        public bool Equals(Player other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            return ReferenceEquals(this, other) || Equals(other.ID, ID);
        }

        public override int GetHashCode() { return ID.GetHashCode(); }*/
    }
}
