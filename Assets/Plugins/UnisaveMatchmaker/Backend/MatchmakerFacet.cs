using System;
using Unisave.Facets;

namespace Unisave.Matchmaker.Backend
{
    public class MatchmakerFacet : Facet
    {
        public void ClientCreatesRoom(Room room)
        {
            // TODO: fire "CanClientCreateRoom" check

            // TODO: check the room does not exist yet

            // create the room
            room.Save();
        }
    }
}
