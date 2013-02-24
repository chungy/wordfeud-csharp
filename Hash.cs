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

using System.Security.Cryptography;
using System.Text;

namespace Wordfeud.Client
{
    internal static class Hash
    {
        private static readonly SHA1 _sha1 = SHA1.Create();
        //private static UnicodeEncoding _encoding = new UnicodeEncoding();

        public static string Password(string password)
        {
            password += "JarJarBinks9";
            byte[] hash = _sha1.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash) { sb.Append(b.ToString("x2")); }
            return sb.ToString();
        }
    }
}
