namespace Unisave.Matchmaker.Backend
{
    /// <summary>
    /// Represents a player joined in a room.
    /// 
    /// You may override this class to store additional information,
    /// like the chosen player skin, color, etc.
    /// Anything that does not change often throughout the match.
    /// </summary>
    public class PlayerMember
    {
        /// <summary>
        /// ID of the player
        /// </summary>
        [field: SerializeAs("id")]
        public string Id { get; set; }

        /// <summary>
        /// What connection status is the player in
        /// </summary>
        public string status = StatusConnecting;
        
        /// <summary>
        /// The player has begun connecting but is not fully in the room yet.
        /// (say the Photon is not yet connected, the room scene is not loaded)
        /// </summary>
        public const string StatusConnecting = "connecting";
        
        /// <summary>
        /// The player has confirmed to be fully inside the room.
        /// </summary>
        public const string StatusPresent = "present";
    }
}