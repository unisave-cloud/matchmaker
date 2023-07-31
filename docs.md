Unisave Matchmaker Documentation
================================

There are many ways in which games solve the "multiplayer" aspect of gameplay. The matchmaker asset focuses on games that group players into separate *rooms*, where only players within one room interact. A room might be temporary or long-lived, public or private, allow or prevent players from joining and leaving at any moment, but what defines the room is that the players inside the room somehow play together.

The matchmaker asset solves these problems:

- creation, lifetime, and destruction of rooms
- tracking which players are in which rooms
- controlling how players join these rooms

The matchmaker asset does NOT solve the communication between players within a room. To synchronize player positions, moves, stats use some other networking solution, such as:

- [Photon](https://www.photonengine.com/)
- [Mirror](https://assetstore.unity.com/packages/tools/network/mirror-129321)
- [Fish-Net](https://assetstore.unity.com/packages/tools/network/fish-net-networking-evolved-207815)
- [Unisave Broadcasting](https://unisave.cloud/docs/broadcasting)

When you use one of these networking solutions, you often need to know which room ID to join, which server URL to connect to, which region to use (europe, asia, us, ...). This matchmaker asset knows about all the rooms everywhere and it tracks for you these connection details. It's simply a one central place to manage all the rooms.


## High-level structure

The asset covers a number of distinct, but related responsibilities:

- **Lobby Management** Lobby stands for the place outside rooms. When a player is in the lobby, they can choose a room to join or they can create their own custom room. Your game may also want to make sure there always exists at least one public room, and it may want to destroy unnecessary rooms.
- **Room Management** A room tracks connected players, decides whether a player can join, and stores public metadata about the room (such as the game mode or the currently played map). A room can be in many states, such as being set up (custom room), waiting for players to connect, having a match running, voting for the next match, being completed and waiting for players to leave.
- **Matchmaking** Matchmaking is the process of creating rooms and joining players into them automatically, with the player giving up control. You can join a player into an existing public room based on their ranking, you can pair up players and create one-on-one matches, or you can create more complex room structures, such as a daily championship for all online players.


## Lobby management


### Listing available rooms

To get the list of existing rooms, simply add a `using Unisave.Matchmaker;` to your mono behaviour file and then call the `this.WatchLobby` method when you want to start watching:

```cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Unisave.Matchmaker;

public class MyLobbyController : MonoBehaviour
{
    void Start()
    {
        this.WatchLobby(watch => {
            watch.OnRoomListUpdate(OnRoomListUpdate);
        });
    }

    void OnRoomListUpdate(List<Room> rooms)
    {
        // render the list of rooms
    }
}
```


### Joining a room

A player joins a room in two steps. First, you join the room in the matchamker asset. When that is successful, you join the room with the other networking systems and start communicating with other players. When that succeeds, you tell the matchmaker that you are now fully connected.

Another words, you first *join* the room and then you *connect* to the room. So you can be joined, but not connected (at the beginning or if your internet connection breaks).

```
[not present] --> [connecting] --> [present]
```

To join a room you call `this.JoinRoom`:

```cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Unisave.Matchmaker;

public class MyLobbyController : MonoBehaviour
{
    async void JoinRoom(Room roomFromTheRoomsList)
    {
        var result = await this.JoinRoom(roomFromTheRoomsList);

        if (result.Success)
        {
            // now you can connect to the room with your
            // real-time networking library, load scene, etc...
            Debug.Log("Joined the room ID: " + result.Room.Id);

            // variable result.Room contains the updated state of the room
            // with you already joined, whereas the argument
            // roomFromTheRoomsList still holds the old (now obsolete) state

            // we will load a scene that contains a "MyRoomController" script
            // and that script will connect to photon and then handle the room
            MyRoomController.JoinedRoom = result.Room; // see the script below
            SceneManager.LoadScene("Room");
        }
        else
        {
            // the room may not want us, it might have gotten
            // full already, or some other problem
            Debug.Log("Joining room failed: " + result.RejectionReason);
        }
    }
}
```

TODO: join from the server

TODO: join with friends

TODO: the room should be able to deny the joining

Once the room scene is loaded, you need to observe the room for changes and also report back to the room whether you are connected or not (whether the real-time connection has been established):

```cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Unisave.Matchmaker;

public class MyRoomController : MonoBehaviour
{
    // lobby sets this value and then loads the scene containing this script
    // (initializes the "room" variable in Start)
    public static Room JoinedRoom = null;

    // the room we are in
    private Room room;

    void Start()
    {
        // accept the room value given to us from the previous scene
        room = JoinedRoom ?? throw new ArgumentException("No room given");
        JoinedRoom = null;

        // start watching the room, so that we have the latest room state
        this.WatchRoom(room, watch => {
            watch.OnRoomUpdate(OnRoomUpdate)
        });

        // connect to room in Photon (or other multiplayer system)
        ConnectToPhoton(room.Id);
    }

    void OnRoomUpdate(Room room)
    {
        // remember the new state
        this.room = room;

        // re-render the UI
        // ...
    }

    // this is called by Photon when the connection is established
    void OnConnectedToPhoton()
    {
        // tell the room that we are no-longer "connecting" but that
        // we are in-fact "present" now
        this.SignalConnected(room);
    }
}
```

TODO: connection from an authoritative real-time server


### Defining a room type

```cs
using Unisave.Matchmaker.Backend;

public class RegularRoom : Room<RegularPlayerMember>
{
    // public title of the room
    public string title;

    // to limit the number of joined players
    public int capacity;

    // what map is being played
    public string map;
}

public class RegularPlayerMember : PlayerMember
{
    // what name does the player publicly display in the room
    public string name;

    // what character skin is the player using in the room
    public string skin;

    // Do not store health, score, location, or anything that changes
    // too often or does not need to be visible in the lobby.
}
```


### Creating rooms

You can create a room and join it immediately like this:

```cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Unisave.Matchmaker;

public class MyLobbyController : MonoBehaviour
{
    async void CreateRoom(string title)
    {
        var result = await this.CreateRoom<RegularRoom>(room => {
            room.title = title;
            room.map = "mountains";
            room.capacity = 16;
        });

        if (result.Success)
        {
            // the room has been created and you have joined the room 
            Debug.Log("Created & joined the room ID: " + result.Room.Id);

            // now you can connect to the room, like it's explained
            // in the "how to join a room" section
        }
        else
        {
            // you may not have the right to create custom rooms
            // (say only premium players can)
            Debug.Log("Creating room failed: " + result.RejectionReason);
        }
    }
}
```

TODO: creating rooms from the server

TODO: testing whether a player can create a room
