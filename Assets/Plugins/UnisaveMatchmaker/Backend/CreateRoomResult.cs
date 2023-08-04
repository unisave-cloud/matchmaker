using System;
using Unisave.Serialization;

namespace Unisave.Matchmaker.Backend
{
    /// <summary>
    /// Represents the result of a room creation
    /// </summary>
    public class CreateRoomResult<TRoom> : CreateRoomResult where TRoom : Room
    {
        /*
         * This class is only used on the client, because it cannot be
         * serialized due to its generic type. That's why it's sent
         * non-generic and then cloned on the client.
         */
        
        /// <summary>
        /// The room that was created (or was to be created)
        /// </summary>
        public TRoom Room => (TRoom) BaseRoom;

        public CreateRoomResult(CreateRoomResult nonGeneric)
        {
            Success = nonGeneric.Success;
            RejectionReason = nonGeneric.RejectionReason;
            BaseRoom = nonGeneric.BaseRoom;
            
            if (!(BaseRoom is TRoom))
                throw new ArgumentException("Given result is of wrong room type.");
        }
    }

    public class CreateRoomResult
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
        /// References the room that was (or was to be) created
        /// as the general <see cref="Room"/> base type
        /// </summary>
        public Room BaseRoom { get; set; }
        
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
            var result = new CreateRoomResult();
            
            result.BaseRoom = room;
            
            return result;
        }
    }
}