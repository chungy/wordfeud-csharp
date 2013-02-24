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
using System.Globalization;

namespace Wordfeud.Client.RuleSets
{
    public class en_us : RuleSet
    {
        public enum Frequencies
        {
            A = 10,
            B = 2,
            C = 2,
            D = 5,
            E = 12,
            F = 2,
            G = 3,
            H = 3,
            I = 9,
            J = 1,
            K = 1,
            L = 4,
            M = 2,
            N = 6,
            O = 7,
            P = 2,
            Q = 1,
            R = 6,
            S = 5,
            T = 7,
            U = 4,
            V = 2,
            W = 2,
            X = 1,
            Y = 2,
            Z = 1,
            _ = 2
        }

        public enum Values
        {
            A = 1,
            B = 4,
            C = 4,
            D = 2,
            E = 1,
            F = 4,
            G = 3,
            H = 4,
            I = 1,
            J = 10,
            K = 5,
            L = 1,
            M = 3,
            N = 1,
            O = 1,
            P = 4,
            Q = 10,
            R = 1,
            S = 1,
            T = 1,
            U = 2,
            V = 4,
            W = 4,
            X = 8,
            Y = 4,
            Z = 10,
            _ = 0
        }

        public override byte Frequency(char Letter)
        {
            try { return (byte)(int)Enum.Parse(typeof(Frequencies), Letter.ToString(CultureInfo.InvariantCulture)); }
            catch { return 0; }
        }

        public override byte Value(char Letter)
        {
            try { return (byte)(int)Enum.Parse(typeof(Values), Letter.ToString(CultureInfo.InvariantCulture)); }
            catch { return 0; }
        }

        public override byte TileCount { get { return 103; } }
    }
}
