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
        private JsonObject serializedRoom;

        public TRoom GetRoom<TRoom>() where TRoom : Room
        {
            return (TRoom) Serializer.FromJson<Room>(serializedRoom);
        }

        public static RoomUpdateMessage FromRoom(Room room)
        {
            return new RoomUpdateMessage {
                serializedRoom = Serializer.ToJson<Room>(room)
            };
        }
    }
}