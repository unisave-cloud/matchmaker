using System;
using System.Collections;
using System.Collections.Generic;
using Unisave.Facades;
using Unisave.Serialization;
using Unisave;
using LightJson;

namespace Unisave.Matchmaker.Backend
{
    public class Room<TPlayer> : Room where TPlayer : PlayerMember
    {
        [field: SerializeAs("players")]
        public List<TPlayer> Players { get; set; } = new List<TPlayer>();

        public override int PlayerCount => Players?.Count ?? 0;
        
        #region "User-definabale callbacks"

        protected virtual void OnPlayerJoining(TPlayer player) { }
        
        protected virtual bool CanClientCreateRoom(out string rejectionReason)
        {
            rejectionReason = "CLIENT_ROOM_CREATION_DISABLED";
            return false;
        }
        
        #endregion
    }
    
    /// <summary>
    /// Represents a multiplayer room
    /// </summary>
    public abstract class Room
    {
        public const string RoomsCollectionName = "matchmakerRooms";

        [field: SerializeAs("_key")]
        public string Id { get; set; }

        [field: SerializeAs("isVisibleInLobby")]
        public bool IsVisibleInLobby { get; set; }
        
        public abstract int PlayerCount { get; }

        public Room()
        {
            Id = Guid.NewGuid().ToString();
        }

        public static T Create<T>(Action<T> builder) where T : Room, new()
        {
            // TODO: verify this is called from the server
            // and if not, tell the developer to use the MonoBehav extension method

            T room = new T();

            // TODO: fire event

            builder?.Invoke(room);

            // TODO: fire event

            room.Save();

            return room;
        }

        public void Save()
        {
            JsonObject document = Serializer.ToJson<Room>(this);

            // TODO: create collection if missing

            DB.Query(@"
                INSERT @document INTO @@collection
                    OPTIONS { overwrite: true }
            ")
                .Bind("@collection", RoomsCollectionName)
                .Bind("document", document)
                .Run();
        }
        
        #region "User-definabale callbacks"

        public void CallOnPlayerJoining(string playerId)
        {
            // TODO
            
            //
        }
        
        #endregion
    }
}
