using System;
using Unisave.Facets;
using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;
using Unisave.Serialization;
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
            await this.WatchRoom(room, watch => {
                watch.OnRoomUpdate(OnRoomUpdate);
                watch.MessageRouter
                    .Forward<AppleThrownMessage>(m => { Debug.Log("Apple thrown!"); })
                    .ElseLogWarning();
            });
            
            // TODO: ping "connected"
        }

        void OnRoomUpdate(CustomRoom newRoom)
        {
            // remember the new room
            room = newRoom;
            
            Debug.Log("New room received: " + Serializer.ToJsonString(room));
            
            // re-render UI
            
            // watch interesting fields for change
            
            // etc...
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.CallFacet((GameFacet f) =>
                    f.SetMyRoomAppleColor(room.Id, AppleColor.Red)
                );
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                this.CallFacet((GameFacet f) =>
                    f.SetMyRoomAppleColor(room.Id, AppleColor.Yellow)
                );
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.CallFacet((GameFacet f) =>
                    f.SetMyRoomAppleColor(room.Id, AppleColor.Green)
                );
            }
        }
    }
}