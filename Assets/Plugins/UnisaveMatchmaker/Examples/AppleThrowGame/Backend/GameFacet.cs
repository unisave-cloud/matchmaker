using System;
using Unisave.Broadcasting;
using Unisave.Facades;
using Unisave.Facets;
using Unisave.Matchmaker.Backend;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame.Backend
{
    public class GameFacet : Facet
    {
        public void SetMyRoomAppleColor(string roomId, AppleColor appleColor)
        {
            if (!Auth.Check())
                throw new InvalidOperationException("Nobody is logged in");
            
            var room = Room.Find(roomId) as CustomRoom;
            
            if (room == null)
                throw new InvalidOperationException(
                    "Requested room does not exist or is of different type"
                );

            CustomRoomPlayerMember player = room.Players.Find(
                p => p.Id == Auth.Id()
            );
            
            if (player == null)
                throw new InvalidOperationException(
                    "You have not joined this room"
                );

            // TODO: make this atomic! ?!?!?!?!?

            player.appleColor = appleColor;
            room.Save();
        }
        
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