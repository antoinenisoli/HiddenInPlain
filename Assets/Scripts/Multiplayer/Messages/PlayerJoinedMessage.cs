using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoinedMessage : BaseMessage
{
    public const string MessageType = "PlayerJoined";
    public override string Type => MessageType;

    public override void Receive(Message m)
    {
        base.Receive(m);
        MainGameplay.instance.CreateBot(playerId, x, y);
    }

    public override string ToString()
    {
        return "player joined (" + playerId + ") at : " + new Vector2(x, y);
    }
}
