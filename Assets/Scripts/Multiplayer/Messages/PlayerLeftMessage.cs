using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftMessage : BaseMessage
{
    public const string MessageType = "PlayerLeft";
    public override string Type => MessageType;

    public override void Receive(Message m)
    {
        base.Receive(m);
        MainGameplay.instance.RemovePlayer(playerId);
    }

    public override string ToString()
    {
        return "player left : (" + playerId + ")";
    }
}
