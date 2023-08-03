using System;
using System.Collections.Generic;
using Unisave.Matchmaker.Backend;

namespace Unisave.Matchmaker.Examples.AppleThrowGame.Backend
{
    public class CustomRoom : Room<CustomRoomPlayerMember>
    {
        public int capacity;
        public string level; // desert, mountains, etc.
    }
}
