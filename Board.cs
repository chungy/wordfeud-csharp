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

using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Wordfeud.Client
{
    public class Board
    {
        private readonly Tile[,] _tiles = new Tile[15, 15];

        public Board(JToken token)
        {
            JToken boardmap = token["content"]["board"];
            if (boardmap == null) { throw new WebException(); }

            for (int y = 0; y < 15; y++)
            {
                JToken row = boardmap[y];
                for (int x = 0; x < 15; x++)
                {
                    int val = row[x].Value<int?>() ?? 0;
                    _tiles[x, y] = new Tile { Type = (Tile.TileType)val };
                }
            }
        }

        public Tile this[int i, int j]
        {
            get { return _tiles[i, j]; }
            internal set { _tiles[i, j] = value; }
        }

        public IList<Tile> GetRow(int j)
        {
            Tile[] temp = new Tile[15];
            for (int i = 0; i < 15; i++)
            {
                temp[i] = _tiles[i, j];
            }
            return temp;
        }

        public IList<Tile> GetColumn(int i)
        {
            Tile[] temp = new Tile[15];
            for (int j = 0; j < 15; j++)
            {
                temp[j] = _tiles[i, j];
            }
            return temp;
        }
    }
}
