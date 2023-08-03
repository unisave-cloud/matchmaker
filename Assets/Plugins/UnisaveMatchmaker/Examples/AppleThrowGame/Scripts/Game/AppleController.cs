using Unisave.Matchmaker.Examples.AppleThrowGame.Backend;
using UnityEngine;

namespace Unisave.Matchmaker.Examples.AppleThrowGame
{
    /// <summary>
    /// Controls an apple, flying through the sky
    /// </summary>
    public class AppleController : MonoBehaviour
    {
        public float throwingSpeed = 6f;
        public float rotationSpeed = 360f;
        public float lifetime = 10f;

        private CustomRoomPlayerMember applePlayer;
        private Collider2D groundCollider;
        private Collider2D appleCollider;
        private Rigidbody2D appleRigidBody;

        private float throwStartPositionX;
        private bool landed;
        
        /// <summary>
        /// Called right after instantiation to set up the apple properties
        /// </summary>
        /// <param name="ground">The ground game object the apple hits</param>
        /// <param name="appleSprite">Sprite to use for the apple</param>
        /// <param name="player">The player who is throwing the apple</param>
        /// <param name="angle">The angle to throw at (0 = level, 90 = up)</param>
        public void Initialize(
            GameObject ground,
            Sprite appleSprite,
            CustomRoomPlayerMember player,
            float angle
        )
        {
            GetComponent<SpriteRenderer>().sprite = appleSprite;
            
            groundCollider = ground.GetComponent<Collider2D>();
            appleCollider = GetComponent<Collider2D>();
            appleRigidBody = GetComponent<Rigidbody2D>();
            
            applePlayer = player;

            appleRigidBody.velocity = new Vector2(
                Mathf.Cos(angle / 180f * Mathf.PI) * throwingSpeed,
                Mathf.Sin(angle / 180f * Mathf.PI) * throwingSpeed
            );
            appleRigidBody.angularVelocity = Mathf.Lerp(
                -rotationSpeed, rotationSpeed,
                Random.value
            );

            throwStartPositionX = appleRigidBody.position.x;
            landed = false;
            
            Destroy(gameObject, lifetime);
        }
        
        void Update()
        {
            if (landed)
                return;
            
            if (groundCollider.IsTouching(appleCollider))
            {
                landed = true;
                
                appleRigidBody.angularDrag = 10f;
                
                float distance = appleRigidBody.position.x - throwStartPositionX;
                Debug.Log($"Thrown {distance} meters by {applePlayer.name}");
            }
        }
    }
}