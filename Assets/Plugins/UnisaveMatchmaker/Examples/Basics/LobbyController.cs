using System.Collections;
using System.Collections.Generic;
using Unisave.Matchmaker;
using Unisave.Matchmaker.Backend;
using UnityEngine;
using Unisave;
using Unisave.Facets;
using Unisave.Matchmaker.Examples.Basics.Backend;

namespace Unisave.Matchmaker.Examples.Basics
{
    public class LobbyController : MonoBehaviour
    {
        void Start()
        {
            // RenderRooms(null);
        }

        void RenderRooms(List<Room> rooms)
        {
            Debug.Log("Rendering rooms:");
            foreach (Room r in rooms)
            {
                ExampleRoom room = (ExampleRoom)r;
            
                string level = room.level;
                int players = room.PlayerCount;
                int capacity = room.capacity;
                
                Debug.Log($"    {level} {players}/{capacity}");
            }
        }

        public async void CreateCustomRoom()
        {
            await this.CreateRoom<ExampleRoom>(r => {
                r.capacity = 16;
                r.level = "desert";
            });

            // TODO: get the room back (because we need the ID and so on...)
            
            Debug.Log("Room created!");
            
            // TODO: call "JoinRoom"
        }

        /// <summary>
        /// Call this method to initiate the process of joining a room
        /// </summary>
        public void JoinRoom(ExampleRoom room)
        {
            // Here you can connect to Photon, Mirror, Fish-Net server,
            // load scenes, do whatever it takes to establish a connection
            // with the other players in the room. You can also just open
            // some "loading" screen and do all that there.
            
            // You can use room fields, such as room ID, server URL and other
            // values to establish the connection.
            
            // In this example we enable the "RoomScene" component.
            // This component contains the "MatchController" that uses
            // Unisave broadcasting to implement a simple turn-based game.
        }
    }
}
