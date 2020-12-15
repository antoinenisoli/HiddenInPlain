using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;

public abstract class BaseMessage
{
    public abstract string Type { get; }

    protected string playerId;
    protected float x, y;

    public virtual void Receive(Message m)
    {
        uint index = 0;
        playerId = m.GetString(index++);
        x = m.GetFloat(index++);
        y = m.GetFloat(index++);
    }
}
