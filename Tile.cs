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

using Newtonsoft.Json.Linq;

namespace Wordfeud.Client
{
    public class Tile
    {
        private char _letter = ' ';
        public char Letter
        {
            get { return _letter; }
            set { _letter = value; }
        }

        public byte X { get; set; }
        public byte Y { get; set; }

        public byte Value { get; set; }
        public bool Blank { get; set; }
        public TileType Type { get; set; }

        internal Tile() { }
        internal Tile(JToken token)
        {
            X = token[0].Value<byte>();
            Y = token[1].Value<byte>();
            Letter = token[2].Value<char>();
            Blank = token[3].Value<bool>();
        }

        public enum TileType
        {
            Normal = 0,
            DoubleLetter = 1,
            TripleLetter = 2,
            DoubleWord = 3,
            TripleWord = 4
        }
    }
}
