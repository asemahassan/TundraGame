using UnityEngine;
using System.Collections;

public class GameOverEvent : Event<GameOverEvent>
{
	public string PlayerName;

	public GameOverEvent (string name)
	{
		PlayerName = name;
	}
}
