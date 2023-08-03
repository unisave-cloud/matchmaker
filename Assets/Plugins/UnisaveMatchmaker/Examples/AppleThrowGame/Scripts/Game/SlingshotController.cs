using System;
using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    /// <summary>
    /// Controls the slingshot that throws apples for all players
    /// and also for the local player (the one sitting at this PC)
    /// </summary>
    public class SlingshotController : MonoBehaviour
    {
        public GameObject ground;
        public GameObject launchPosition;
        public GameObject arrow;
        public GameObject applePrefab;
        public Sprite[] appleSprites;
        
        private CustomRoomPlayerMember preparedPlayer = null;

        void Start()
        {
            launchPosition.SetActive(false);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var dummyPlayer = new CustomRoomPlayerMember() {
                    appleColor = AppleColor.Green,
                    name = "DummyPlayer"
                };
                
                PrepareToThrow(dummyPlayer);
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (preparedPlayer != null)
                    ReleaseSlingshot();
            }
            
            // update arrow direction
            float angle = GetCurrentAngle();
            arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        /// <summary>
        /// Computes where the slingshot is currently pointing
        /// </summary>
        /// <returns></returns>
        public float GetCurrentAngle()
        {
            const float period = 1f;
            float t = (Time.time % period) / period;
            float zig = Mathf.Abs(1f - t * 2f);
            float angle = Mathf.Lerp(0f, 90f, zig);
            return angle;
        }

        /// <summary>
        /// Returns the correct sprite for a given apple color
        /// </summary>
        Sprite GetAppleSprite(AppleColor appleColor)
        {
            return appleSprites[(int) appleColor];
        }

        /// <summary>
        /// Prepares an apple to be thrown by the local player
        /// </summary>
        public void PrepareToThrow(CustomRoomPlayerMember localPlayer)
        {
            launchPosition.SetActive(true);
            launchPosition.GetComponent<SpriteRenderer>().sprite =
                GetAppleSprite(localPlayer.appleColor);
            preparedPlayer = localPlayer;
        }

        /// <summary>
        /// Throws the prepared apple belonging to local player
        /// </summary>
        public void ReleaseSlingshot()
        {
            if (preparedPlayer == null)
                throw new InvalidOperationException(
                    "Cannot release unprepared slingshot."
                );
            
            ThrowApplePrefab(preparedPlayer, GetCurrentAngle());
            
            launchPosition.SetActive(false);
            preparedPlayer = null;
        }
        
        /// <summary>
        /// Instantiates an apple prefab, sets proper sprite,
        /// attaches a player to it, and throws it into the air
        /// </summary>
        /// <param name="player">The player who is throwing the apple</param>
        /// <param name="angle">The angle to throw at (0 = level, 90 = up)</param>
        public void ThrowApplePrefab(CustomRoomPlayerMember player, float angle)
        {
            GameObject appleInstance = Instantiate(
                applePrefab,
                launchPosition.transform.position,
                Quaternion.identity
            );

            var appleSprite = GetAppleSprite(player.appleColor);

            appleInstance
                .GetComponent<AppleController>()
                .Initialize(ground, appleSprite, player, angle);
        }
    }
}