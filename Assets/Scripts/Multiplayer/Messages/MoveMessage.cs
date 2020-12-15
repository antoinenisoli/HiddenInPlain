using PlayerIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMessage : BaseMessage
{
    public const string MessageType = "Move";
    public override string Type => MessageType;

    public override void Receive(Message m)
    {
        playerId = m.GetString(0);
        x = m.GetFloat(1);
        y = m.GetFloat(2);
        MainGameplay.instance.MovePlayer(playerId, new Vector2(x, y));
    }

    public static void Send(float x, float y)
    {
        NetworkManager.Instance.m_IOconnection.Send(MessageType, x, y);
    }

    public override string ToString()
    {
        return playerId + " move to " + new Vector2(x,y);
    }
}
