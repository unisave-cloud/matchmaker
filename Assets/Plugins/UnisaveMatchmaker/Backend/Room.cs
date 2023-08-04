using System;
using System.Collections;
using System.Collections.Generic;
using Unisave.Facades;
using Unisave.Serialization;
using Unisave;
using LightJson;

namespace Unisave.Matchmaker.Backend
{
    public class Room<TPlayer> : Room where TPlayer : PlayerMember, new()
    {
        [SerializeAs("players")]
        public List<TPlayer> Players { get; set; } = new List<TPlayer>();

        public override int PlayerCount => Players?.Count ?? 0;

        protected override void PerformJoinPlayer()
        {
            // TODO: return the result
            
            // TODO: check if already present and pretend success

            var player = new TPlayer();
            player.Id = Auth.Id();
            
            // TODO: execute user-defined callbacks
            
            Players.Add(player);
        }
        
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

        [SerializeAs("_key")]
        public string Id { get; set; }

        [SerializeAs("isVisibleInLobby")]
        public bool IsVisibleInLobby { get; set; }
        
        #region "Players API"
        
        public abstract int PlayerCount { get; }

        protected abstract void PerformJoinPlayer();
        
        #endregion
        
        public Room()
        {
            Id = Guid.NewGuid().ToString();
        }

        public static TRoom Create<TRoom>(Action<TRoom> builder)
            where TRoom : Room, new()
        {
            // TODO: verify this is called from the server
            // and if not, tell the developer to use the MonoBehav extension method

            TRoom room = new TRoom();

            // TODO: fire event

            builder?.Invoke(room);

            // TODO: fire event

            room.Save();

            return room;
        }

        public static Room Find(string roomId)
        {
            // TODO: return null if collection missing
            
            return DB.Query(@"
                RETURN DOCUMENT(@@collection, @key)
            ")
                .Bind("@collection", RoomsCollectionName)
                .Bind("key", roomId)
                .FirstAs<Room>();
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

        public static void Modify<TRoom>(string roomId, Action<TRoom> modification)
            where TRoom : Room
        {
            // TODO: throw UnisaveMatchmakerException in weird cases
            
            // TODO: if room missing
            
            // TODO: if room of different type

            // TODO: wrap it in write-write conflict retry (5x?) with exponential backoff?
            // something like ethernet does, or try googling.
            // also, try running some benchmarks to *actually* see
            
            // super simple, stupid solution
            var room = Room.Find(roomId) as TRoom;
            modification.Invoke(room);
            room.Save();
        }

        public static void Join(string roomId)
        {
            // TODO: verify someone is logged in
            
            // TODO: create and pass along the JoinRoomResult instance
            
            Room.Modify<Room>(roomId, room => {
                room.PerformJoinPlayer();
            });
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
