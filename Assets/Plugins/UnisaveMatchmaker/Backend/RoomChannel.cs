using Unisave.Broadcasting;

namespace Unisave.Matchmaker.Backend
{
    public class RoomChannel : BroadcastingChannel
    {
        public SpecificChannel OfRoom(string roomId)
        {
            return SpecificChannel.From<RoomChannel>(roomId);
        }
    }
}