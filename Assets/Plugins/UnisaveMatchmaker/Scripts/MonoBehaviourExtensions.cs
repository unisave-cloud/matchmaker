using System;
using UnityEngine;
using Unisave.Matchmaker.Backend;
using Unisave.Facets;
using Object = System.Object;

namespace Unisave.Matchmaker
{
    /// <summary>
    /// Allows you to interact with the matchmaker from mono behaviours
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        public static UnisaveOperation<CreateRoomResult<TRoom>> CreateRoom<TRoom>(
            this MonoBehaviour caller,
            Action<TRoom> builder
        ) where TRoom : Room, new()
        {
            // NOTE:
            // This method is the client-side equivalent of Room.Create<T>()

            TRoom room = new TRoom();

            // TODO: fire event

            builder?.Invoke(room);

            // TODO: fire event

            return new UnisaveOperation<CreateRoomResult<TRoom>>(
                caller,
                async () => {
                    CreateRoomResult result = await caller.CallFacet(
                        (MatchmakerFacet f) => f.ClientCreatesRoom(room)
                    );
                    return new CreateRoomResult<TRoom>(result);
                }
            );
        }

        public static UnisaveOperation<RoomWatcher<TRoom>> WatchRoom<TRoom>(
            this MonoBehaviour caller,
            TRoom roomToWatch,
            Action<RoomWatcher<TRoom>> builder
        ) where TRoom : Room
        {
            bool exists = caller.TryGetComponent<RoomWatcherComponent>(
                out var watcherComponent
            );
            
            if (!exists)
                watcherComponent = caller.gameObject.AddComponent<RoomWatcherComponent>();

            var watcher = new RoomWatcher<TRoom>(watcherComponent, roomToWatch);
            
            builder.Invoke(watcher);
            
            return new UnisaveOperation<RoomWatcher<TRoom>>(
                caller, watcher.Connect()
            );
        }
    }
}
