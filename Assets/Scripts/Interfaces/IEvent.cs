using UnityEngine;
using System.Collections;

/// <summary>
/// Based on SSG Eventsystem
/// </summary>
public interface IEvent
{
    int GetIdentity();
}

public abstract class Event<T> : IEvent
{
    /// returns the unique type identifier of this eventtype
    public static int Identity
    {
        get
        {
            return typeof(T).Name.GetHashCode();
        }
    }

    public int GetIdentity()
    {
        return Identity;
    }
}
