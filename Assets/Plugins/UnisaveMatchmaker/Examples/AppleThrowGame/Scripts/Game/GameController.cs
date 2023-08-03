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
    }
}