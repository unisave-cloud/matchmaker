using System.Collections;
using System.Collections.Generic;
using Unisave.Matchmaker;
using Unisave.Matchmaker.Backend;
using UnityEngine;
using Unisave;
using Unisave.Facets;
using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    /// <summary>
    /// Controls the user interface in the lobby screen
    /// </summary>
    public class LobbyController : MonoBehaviour
    {
        public GameObject lobbyScreen;
        public GameObject roomScreen;
        
        private readonly List<CustomRoom> lobbyRooms = new List<CustomRoom>();
        private AppleColor appleColor = AppleColor.Red;
        
        void Start()
        {
            // TODO: watch lobby
        }

        void RenderUI()
        {
            Debug.Log("Apple: " + appleColor);
            
            Debug.Log("Rendering rooms:");
            foreach (CustomRoom room in lobbyRooms)
            {
                string map = room.map;
                int players = room.PlayerCount;
                int capacity = room.capacity;
                
                Debug.Log($"    {map} {players}/{capacity}");
            }
        }

        /// <summary>
        /// Handles the "Create Room" button click
        /// </summary>
        public async void CreateCustomRoom()
        {
            var result = await this.CreateRoom<CustomRoom>(room => {
                room.IsVisibleInLobby = true;
                room.capacity = 16;
                room.map = "desert";
            });

            if (result.Success)
            {
                ConnectToRoom(result.Room);
            }
            else
            {
                Debug.LogError("Room creation failed: " + result.RejectionReason);
            }
        }

        /// <summary>
        /// Call this method after we join a room to initiate the process
        /// of actually connecting to the room
        /// </summary>
        private async void ConnectToRoom(CustomRoom room)
        {
            // Here you can connect to Photon, Mirror, Fish-Net server,
            // load scenes, do whatever it takes to establish a connection
            // with the other players in the room. You can also just open
            // some "loading" screen and do all that there.
            
            // You can use room fields, such as room ID, server URL and other
            // values to establish the connection.
            
            // In this example we enable the "RoomScene" component which handles
            // the in-room logic.
            
            // In our case, the connection logic also involves setting
            // our apple color for the room we are connecting:
            await this.CallFacet(
                (GameFacet f) => f.SetMyRoomAppleColor(room.Id, appleColor)
            );
            
            // We send the room to the room controller:
            RoomController.joinedRoom = room;
            
            // Then we can switch to the room screen (like loading a scene):
            lobbyScreen.SetActive(false);
            roomScreen.SetActive(true);
            
            // The room controller then pings the server that we are "connected".
        }
    }
}
