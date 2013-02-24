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

namespace Wordfeud.Client.RuleSets
{
    public abstract class RuleSet
    {
        public static RuleSet GetRuleset(byte id)
        {
            switch(id)
            {
                case 0:
                    return new en_us();
                default:
                    throw new NotSupportedException();
            }
        }
        public abstract byte Frequency(char Letter);
        public abstract byte Value(char Letter);
        public abstract byte TileCount { get; }
    }
}
