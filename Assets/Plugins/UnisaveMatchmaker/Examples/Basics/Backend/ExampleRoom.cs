using System;
using System.Collections.Generic;
using Unisave.Matchmaker.Backend;

namespace Unisave.Matchmaker.Examples.Basics.Backend
{
    public class ExampleRoom : Room<ExamplePlayerMember>
    {
        public int capacity;
        public string level; // desert, mountains, etc.
    }

    public class ExamplePlayerMember : PlayerMember
    {
        public string name;
    }
}
