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

using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Wordfeud.Client
{
    internal static class Http
    {
        public static string Request(string address, string post)
        {
            ServicePointManager.Expect100Continue = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = ((post != null) ? ("POST") : ("GET"));

            request.Expect = null;
            request.UserAgent = "User-Agent: WebFeudClient/1.2.11 (Android 2.3.3)";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");

            if (post != null)
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postBytes = encoding.GetBytes(post);
                request.ContentType = "application/json";
                request.ContentLength = postBytes.Length;

                Stream newStream = request.GetRequestStream();
                newStream.Write(postBytes, 0, postBytes.Length);
                newStream.Close();
            }

            using (Stream responseStream = smartUnpack((HttpWebResponse)request.GetResponse()))
            {
                if (responseStream == null) { throw new WebException("Response was null."); }
                StreamReader Reader = new StreamReader(responseStream, Encoding.Default);
                string responseString = Reader.ReadToEnd();
                responseStream.Close();
                return responseString;
            }
        }

        private static Stream smartUnpack(HttpWebResponse response)
        {
            Stream responseStream = response.GetResponseStream();
            if (response.ContentEncoding.ToLower().Contains("gzip")) { responseStream = new GZipStream(responseStream, CompressionMode.Decompress); }
            return responseStream;
        }
    }
}
