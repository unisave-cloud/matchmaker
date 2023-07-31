using Unisave.Broadcasting;

namespace Unisave.Matchmaker.Examples.AppleThrowGame.Backend
{
    public class AppleThrowGameChannel : BroadcastingChannel
    {
        public SpecificChannel OfRoom(string roomId)
        {
            return SpecificChannel.From<AppleThrowGameChannel>(roomId);
        }
    }
}