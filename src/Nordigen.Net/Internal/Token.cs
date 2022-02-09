namespace Nordigen.Net.Internal
{
    using Newtonsoft.Json;
    using System;

    internal class Token
    {
        //internal just for the sake of testing
        internal DateTime CreatedUtc { get; set; } = DateTime.UtcNow;


        [JsonConstructor]
        public Token(string access, int accessExpires, string refresh, int refreshExpires)
        {
            Access = access;
            AccessExpires = accessExpires;
            Refresh = refresh;
            RefreshExpires = refreshExpires;
        }

        public string Access { get; }

        public int AccessExpires { get; }

        public string Refresh { get; internal set; }

        public int RefreshExpires { get; internal set; }

        internal bool AccessIsValid(int validForSeconds) => CreatedUtc.AddSeconds(AccessExpires) > DateTime.UtcNow.AddSeconds(-validForSeconds);

        internal bool RefreshIsValid(int validForSeconds) => CreatedUtc.AddSeconds(RefreshExpires) > DateTime.UtcNow.AddSeconds(-validForSeconds);
    }
}

