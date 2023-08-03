using Unisave.Facets;
using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;
using UnityEngine;
using UnityEngine.UI;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    public class LoginController : MonoBehaviour
    {
        public Button johnButton;
        public Button adamButton;
        public Button eveButton;

        public GameObject loginScreen;
        public GameObject lobbyScreen;
        
        void Start()
        {
            johnButton.onClick.AddListener(() => LoginAs("John"));
            adamButton.onClick.AddListener(() => LoginAs("Adam"));
            eveButton.onClick.AddListener(() => LoginAs("Eve"));
            
            EnableButtons();
        }
        
        private async void LoginAs(string playerName)
        {
            // disable the buttons
            // (this is a cheap "loading" screen)
            DisableButtons();

            // send the login request to the server and wait for the response
            await this.CallFacet(
                (AuthFacet f) => f.LogIn(playerName)
            );
            
            // we are logged in, enable the buttons again and show the lobby
            EnableButtons();
            loginScreen.SetActive(false);
            lobbyScreen.SetActive(true);
        }

        private void DisableButtons()
        {
            johnButton.interactable = false;
            adamButton.interactable = false;
            eveButton.interactable = false;
        }
        
        private void EnableButtons()
        {
            johnButton.interactable = true;
            adamButton.interactable = true;
            eveButton.interactable = true;
        }
    }
}