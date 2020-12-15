using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerIOClient;

public class StartGameplayMessage : BaseMessage
{
    public const string MessageType = "StartGameplay";
    public override string Type => MessageType;
    int botsCount;

    public override void Receive(Message m)
    {
        uint index = 0;
        botsCount = m.GetInt(index++);

        for (int i = 0; i < botsCount; i++)
        {
            string id = m.GetString(index++);
            float x = m.GetFloat(index++);
            float y = m.GetFloat(index++);
            MainGameplay.instance.CreateBot(id, x, y);
        }
    }

    public override string ToString()
    {
        return "Generate " + botsCount + " bots !";
    }
}
