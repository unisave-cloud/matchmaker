using System;
using System.Collections.Generic;
using Unisave.Matchmaker.Backend;

namespace Unisave.Matchmaker.Examples.Basics.Backend
{
    public class ExampleRoom : Room
    {
        public int capacity;
        public string level; // desert, mountains, etc.

        Dictionary<string, string> playerNames
            = new Dictionary<string, string>();
    }
}
