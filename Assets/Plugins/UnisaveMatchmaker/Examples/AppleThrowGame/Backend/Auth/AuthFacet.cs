using System;
using System.Collections;
using System.Collections.Generic;
using Unisave;
using Unisave.Facets;
using Unisave.Facades;

namespace Unisave.Matchmaker.Examples.AppleThrowGame.Backend
{
    /// <summary>
    /// Dummy login method
    /// </summary>
    public class AuthFacet : Facet
    {
        public void LogIn(string playerName)
        {
            PlayerEntity player = DB.TakeAll<PlayerEntity>()
                .Filter(e => e.name == playerName)
                .First();
            
            if (player == null)
            {
                player = new PlayerEntity();
                player.name = playerName;
                player.Save();
            }

            Auth.Login(player);
        }
    }
}
