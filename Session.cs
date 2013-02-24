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
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Wordfeud.Client
{
    public class Session
    {
        public string UserID { get; private set; }
        public string Username { get; private set; }
        private readonly string _passhash;

        protected string SessionID { get; private set; }
        protected string Domain { get; private set; }

        private static readonly Random _random = new Random();

        public Session(string username, string password)
        {
            Username = username;
            _passhash = Hash.Password(password);
            Login();
        }

        public IList<Game> Games
        {
            get
            {
                JObject response = Request("/wf/user/games/");
                if (response == null) { throw new WebException(); }

                JToken games = response["content"]["games"];
                //if (!games.Valid) { throw new WebException(); }

                List<Game> gameList = new List<Game>();
                foreach(JToken item in games)
                {
                    if (item["id"] == null) { continue; }
                    gameList.Add(new Game(this, item["id"].Value<uint>()));
                }
                return gameList;
            }
        }

        private void Login()
        {
            string data = "{";
            data += "\"username\": \"" + Username + "\", ";
            data += "\"password\": \"" + _passhash + "\"";
            data += "}";
            
            Domain = "http://game0" + _random.Next(0, 7) + ".wordfeud.com";

            Tuple<string, string> login = Request(Domain + "/wf/user/login/", data, null);
            JObject response = JObject.Parse(login.Item1);
            SessionID = login.Item2;

            if (response == null) { throw new WebException("Response was empty or invalid."); }
            if(response["status"].Value<string>().Equals("error",StringComparison.InvariantCultureIgnoreCase))
            {
                //usually "wrong_password"
                throw new WebException(response["content"]["type"].Value<string>());
            }

            UserID = response["content"]["id"].Value<string>();
            if (UserID == null) { throw new WebException("UserID was null."); }
        }

        private static Tuple<string, string> Request(string address, string post, string sessionId)
        {
            ServicePointManager.Expect100Continue = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = post != null ? "POST" : "GET";

            request.Expect = null;
            request.UserAgent = "User-Agent: WebFeudClient/1.2.11 (Android 2.3.3)";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");

            if (sessionId != null)
            {
                request.CookieContainer = new CookieContainer();
                Cookie c = new Cookie("sessionid", sessionId) { Domain = ".wordfeud.com" };
                request.CookieContainer.Add(c);
            }

            if (post != null)
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(post);
                request.ContentType = "application/json";
                request.ContentLength = byte1.Length;

                Stream newStream = request.GetRequestStream();
                newStream.Write(byte1, 0, byte1.Length);
                newStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string cookieHeader = response.Headers["Set-Cookie"];
            string newSessionId = null;
            if (cookieHeader != null){newSessionId = cookieHeader.Split('=', ';')[1];}

            Stream responseStream = response.GetResponseStream();
            if (response.ContentEncoding.ToLower().Contains("gzip")){responseStream = new GZipStream(responseStream, CompressionMode.Decompress);}
            if (responseStream == null){throw new WebException("Response was null.");}
            StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

            string Html = Reader.ReadToEnd();

            response.Close();
            responseStream.Close();

            return new Tuple<string, string>(Html, newSessionId);
        }

        internal JObject Request(string address, string post)
        {
            return JObject.Parse(Request(Domain + address, post, SessionID).Item1);
        }

        internal JObject Request(string address)
        {
            return JObject.Parse(Request(Domain + address, "[]", SessionID).Item1);
        }
    }
}
