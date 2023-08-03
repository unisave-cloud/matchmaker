using System;
using Unisave.Broadcasting;
using Unisave.Facades;
using Unisave.Facets;

namespace Unisave.Matchmaker.Backend
{
    public class MatchmakerFacet : Facet
    {
        public CreateRoomResult ClientCreatesRoom(Room room)
        {
            // TODO: fire "CanClientCreateRoom" check

            // TODO: check the room does not exist yet

            // create the room
            room.Save();
            
            // TODO: join the room with that player

            return CreateRoomResult.FromSuccess(room);
        }

        public ChannelSubscription WatchRoom(string roomId)
        {
            if (!Auth.Check())
                throw new InvalidOperationException("Nobody is logged in");

            Room room = Room.Find(roomId);
            
            if (room == null)
                throw new InvalidOperationException("Requested room does not exist");
            
            // TODO: verify the player is in fact joining/present in the room

            var subscription = Broadcast
                .Channel<RoomChannel>()
                .OfRoom(roomId)
                .CreateSubscription();
            
            Broadcast
                .Channel<RoomChannel>()
                .OfRoom(roomId)
                .Send(RoomUpdateMessage.FromRoom(room));

            return subscription;
        }
    }
}
