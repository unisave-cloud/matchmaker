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
                string level = "desert";
                int players = 0;
                int capacity = 16;
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
        }
    }
}
