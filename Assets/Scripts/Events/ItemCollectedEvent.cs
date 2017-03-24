using UnityEngine;
using System.Collections;

public class ItemCollectedEvent : Event<ItemCollectedEvent>
{
    public int HealthValue;

    public ItemCollectedEvent(int health)
    {
        HealthValue = health;
    }
}
