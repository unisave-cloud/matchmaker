using System;
using Unisave.Broadcasting;
using Unisave.Facades;
using Unisave.Facets;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame.Backend
{
    public class AppleThrowGameFacet : Facet
    {
        public ChannelSubscription JoinBroadcastingChannel(string roomId)
        {
            if (!Auth.Check())
                throw new InvalidOperationException("Nobody is logged in.");
            
            // TODO: verify the player is in fact joining/present in the matchmaker room

            var subscription = Broadcast
                .Channel<AppleThrowGameChannel>()
                .OfRoom(roomId)
                .CreateSubscription();

            return subscription;
        }

        public void SendMoveAvatarMessage(string roomId, float angle)
        {
            if (!Auth.Check())
                throw new InvalidOperationException("Nobody is logged in.");
            
            PlayerEntity player = Auth.GetPlayer<PlayerEntity>();

            var message = new AppleThrownMessage() {
                playerId = player.EntityId,
                angle = angle
            };
            
            Broadcast
                .Channel<AppleThrowGameChannel>()
                .OfRoom(roomId)
                .Send(message);
        }
    }
}