using LightJson;
using Unisave.Broadcasting;
using Unisave.Serialization;

namespace Unisave.Matchmaker.Backend
{
    /// <summary>
    /// Sent by the server whenever a room changes
    /// </summary>
    public class RoomUpdateMessage : BroadcastingMessage
    {
        public Room room;

        public TRoom GetRoom<TRoom>() where TRoom : Room
        {
            return (TRoom) room;
        }

        public static RoomUpdateMessage FromRoom(Room room)
        {
            return new RoomUpdateMessage {
                room = room
            };
        }
    }
}