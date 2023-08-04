using System;
using System.Threading.Tasks;
using Unisave.Broadcasting;
using Unisave.Facets;
using Unisave.Matchmaker.Backend;

namespace Unisave.Matchmaker
{
    public class RoomWatcher<TRoom>
        where TRoom : Room
    {
        private RoomWatcherComponent component;
        
        public TRoom Room { get; set; }

        public MessageRouterBuilder MessageRouter { get; private set; }
        
        private MessageRouter router;

        private event Action<TRoom> OnRoomUpdateEvent; 

        public RoomWatcher(RoomWatcherComponent component, TRoom room)
        {
            this.component = component;
            Room = room;
            
            router = new MessageRouter();
            MessageRouter = new MessageRouterBuilder(router);

            OnRoomUpdateEvent = null;
        }
        
        public async Task<RoomWatcher<TRoom>> Connect()
        {
            ChannelSubscription subscription = await component.CallFacet(
                (MatchmakerFacet f) => f.WatchRoom(Room.Id)
            );

            component.PublicFromSubscription(subscription)
                .Forward<RoomUpdateMessage>(RoomUpdateMessageReceived)
                .Else(router.RouteMessage);

            // just for convenience so that we can
            // directly create a UnisaveOperation
            return this;
        }

        private void RoomUpdateMessageReceived(RoomUpdateMessage msg)
        {
            var newRoom = msg.GetRoom<TRoom>();

            Room = newRoom;
            
            OnRoomUpdateEvent?.Invoke(newRoom);
        }

        /// <summary>
        /// Registers a listener to be called whenever the room changes
        /// </summary>
        /// <param name="listener"></param>
        public void OnRoomUpdate(Action<TRoom> listener)
        {
            OnRoomUpdateEvent += listener;
        }
    }
}