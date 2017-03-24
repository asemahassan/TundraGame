using UnityEngine;
using System.Collections;

public class FishCollectedEvent : Event<FishCollectedEvent>
{

    public int HealthValue;

    public FishCollectedEvent(int health)
    {
        HealthValue = health;
    }
}
