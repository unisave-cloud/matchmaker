using Unisave.Broadcasting;
using UnityEngine;

namespace Unisave.Matchmaker
{
    public class RoomWatcherComponent : UnisaveBroadcastingClient
    {
        /// <summary>
        /// Exposes the FromSubscription method publicly
        /// </summary>
        public MessageRouterBuilder PublicFromSubscription(
            ChannelSubscription subscription
        ) => FromSubscription(subscription);
    }
}