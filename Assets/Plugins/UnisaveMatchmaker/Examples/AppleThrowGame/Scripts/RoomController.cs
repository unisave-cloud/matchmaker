using System;
using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    public class RoomController : MonoBehaviour
    {
        // lobby sets this value and then loads the scene containing this script
        public static CustomRoom joinedRoom = null;
        
        // the room we are in
        private CustomRoom room;
        
        private void OnEnable()
        {
            // accept the room value given to us from the previous scene
            room = joinedRoom ?? throw new ArgumentException("No room given");
            joinedRoom = null;
            
            // connect to the room
            
            // finalize room joining
            
            this.WatchRoom(joinedRoom, OnRoomUpdate);
        }

        void OnRoomUpdate(CustomRoom newRoom)
        {
            // re-render UI
            
            // watch interesting fields for change
            
            // etc...
        }
    }
}