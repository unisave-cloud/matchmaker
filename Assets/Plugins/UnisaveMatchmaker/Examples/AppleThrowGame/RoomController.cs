using System;
using Unisave.Matchmaker.Examples.Basics.Backend;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    public class RoomController : MonoBehaviour
    {
        public static ExampleRoom roomToWatch;
        
        private void OnEnable()
        {
            // connect to the room
            
            // finalize room joining
            
            this.WatchRoom(roomToWatch, OnRoomChanged);
            
            // GOALS OF THE MATCHMAKER (room management)
            // 1) Group people into rooms and keep people inside a room
            // updated on who is there and who is who.
            // 2) Track the high-level state of the room
        }

        void OnRoomChanged(ExampleRoom newRoom)
        {
            // re-render UI
            
            // watch interesting fields for change
            
            // etc...
        }
    }
}