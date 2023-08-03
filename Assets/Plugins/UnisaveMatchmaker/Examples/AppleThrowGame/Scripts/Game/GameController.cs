using System;
using System.Collections.Generic;
using Unisave.Broadcasting;
using Unisave.Facets;
using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    public class GameController : MonoBehaviour
    {
        private int currentRound = 0;

        private readonly Dictionary<string, float> playerScores
            = new Dictionary<string, float>();
        
        public void PrepareNewGame(string roomId)
        {
            currentRound = 0;
            playerScores.Clear();
        }

        public void StartNextRound()
        {
            currentRound += 1;
        }
        
        // public async void Connect(string roomId)
        // {
        //     ChannelSubscription subscription = await this.CallFacet(
        //         (AppleThrowGameFacet f) => f.JoinBroadcastingChannel(roomId)
        //     );
        //
        //     FromSubscription(subscription)
        //         .Forward<AppleThrownMessage>(OnAvatarMoved)
        //         .ElseLogWarning();
        // }
        //
        // void OnAvatarMoved(AppleThrownMessage message)
        // {
        //     
        // }
    }
}