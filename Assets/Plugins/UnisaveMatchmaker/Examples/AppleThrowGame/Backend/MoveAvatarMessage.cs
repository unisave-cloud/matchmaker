using Unisave.Broadcasting;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame.Backend
{
    public class AppleThrownMessage : BroadcastingMessage
    {
        public string playerId;
        public float angle;
    }
}