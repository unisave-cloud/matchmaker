using System;
using Unisave.Facades;
using Unisave.Serialization;
using Unisave;
using LightJson;

namespace Unisave.Matchmaker.Backend
{
    /// <summary>
    /// Represents a multiplayer room
    /// </summary>
    public class Room
    {
        public const string RoomsCollectionName = "matchmakerRooms";

        [SerializeAs("_key")]
        public string id;

        public bool isVisibleInLobby;

        // TODO: remember all players here

        public Room()
        {
            this.id = Guid.NewGuid().ToString();
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
    }
}
