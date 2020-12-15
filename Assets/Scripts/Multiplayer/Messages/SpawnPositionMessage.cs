using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SpawnPositionMessage : BaseMessage
{
    public const string MessageType = "SpawnPosition";

    public override string Type => MessageType;

    public override void Receive(Message m)
    {
        base.Receive(m);
        MainGameplay.instance.CreatePlayer(playerId, x, y);
    }

    public override string ToString()
    {
        return "spawn player of id (" + playerId + ") at " + new Vector2(x, y);
    }
}
