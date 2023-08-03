using System;
using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    public class RoomController : MonoBehaviour
    {
        // lobby sets this value and then "loads the scene" containing this script
        // (in reality we just enable and disable game objects
        // because this is an example that has to fit into one scene)
        public static CustomRoom joinedRoom = null;
        
        // the room we are in
        private CustomRoom room;
        
        private async void OnEnable()
        {
            // accept the room value given to us from the previous scene
            room = joinedRoom ?? throw new ArgumentException("No room given");
            joinedRoom = null;
            
            // start watching the room for changes
            await this.WatchRoom(joinedRoom, watch => {
                watch.OnRoomUpdate(OnRoomUpdate);
                watch.MessageRouter
                    .Forward<AppleThrownMessage>(m => { Debug.Log("Apple thrown!"); })
                    .ElseLogWarning();
            });
            
            // TODO: ping "connected"
        }

        void OnRoomUpdate(CustomRoom newRoom)
        {
            Debug.Log("New room received!");
            
            // re-render UI
            
            // watch interesting fields for change
            
            // etc...
        }
    }
}