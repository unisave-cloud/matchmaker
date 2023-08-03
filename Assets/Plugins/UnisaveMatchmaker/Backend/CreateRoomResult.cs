using System;
using Unisave.Serialization;

namespace Unisave.Matchmaker.Backend
{
    /// <summary>
    /// Represents the result of a room creation
    /// </summary>
    public class CreateRoomResult<TRoom> : CreateRoomResult where TRoom : Room
    {
        /// <summary>
        /// The room that was created (or was to be created)
        /// </summary>
        public TRoom Room => (TRoom) room;
    }

    public abstract class CreateRoomResult
    {
        /// <summary>
        /// True if the room was created
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// If the creation was rejected, this is the reason for the rejection
        /// </summary>
        public string RejectionReason { get; set; }

        /// <summary>
        /// References the room that was to be created
        /// </summary>
        protected Room room;
        
        public static CreateRoomResult FromSuccess(Room room)
        {
            var result = CreateForRoom(room);

            result.Success = true;
            result.RejectionReason = null;
            
            return result;
        }
        
        public static CreateRoomResult FromRejection(
            Room room,
            string rejectionReason = null
        )
        {
            var result = CreateForRoom(room);
            
            result.Success = false;
            result.RejectionReason = rejectionReason ?? "UNKNOWN";
            
            return result;
        }
        
        public static CreateRoomResult CreateForRoom(Room room)
        {
            Type type = typeof(CreateRoomResult<>).MakeGenericType(
                room.GetType()
            );

            return (CreateRoomResult) Activator.CreateInstance(type);
        }
    }
}