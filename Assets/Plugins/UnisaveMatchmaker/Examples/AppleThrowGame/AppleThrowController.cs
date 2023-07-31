using System;
using Unisave.Broadcasting;
using Unisave.Facets;
using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    /// <summary>
    /// Controls the avatar turn-based game
    /// </summary>
    public class AppleThrowController : UnisaveBroadcastingClient
    {
        private void OnEnable()
        {
            Connect("foo");
        }

        public async void Connect(string roomId)
        {
            ChannelSubscription subscription = await this.CallFacet(
                (AppleThrowGameFacet f) => f.JoinBroadcastingChannel(roomId)
            );

            FromSubscription(subscription)
                .Forward<AppleThrownMessage>(OnAvatarMoved)
                .ElseLogWarning();
        }

        void OnAvatarMoved(AppleThrownMessage message)
        {
            
        }
    }
}