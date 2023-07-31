using System;
using UnityEngine;
using Unisave.Matchmaker.Backend;
using Unisave.Facets;

namespace Unisave.Matchmaker
{
    /// <summary>
    /// Allows you to interact with the matchmaker from mono behaviours
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        public static UnisaveOperation CreateRoom<T>(
            this MonoBehaviour caller,
            Action<T> builder
        ) where T : Room, new()
        {
            // NOTE:
            // This method is the client-side equivalent of Room.Create<T>()

            T room = new T();

            // TODO: fire event

            builder?.Invoke(room);

            // TODO: fire event

            return caller.CallFacet(
                (MatchmakerFacet f) => f.ClientCreatesRoom(room)
            );
        }

        public static void WatchRoom<T>(
            this MonoBehaviour caller,
            T roomToWatch,
            Action<T> callback
        )
        {
            // TODO
        }
    }
}
