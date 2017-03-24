using UnityEngine;
using System.Collections;

// subject to change according to structure and level details
public enum GameState
{
	None,
	Idle,
	Runner,
    LeftStrafe,
    RightStrafe,
	FishPond2D,
	QuizScene,
	GameOver,
	GameComplete}
;

public enum BerriesType
{
	None,
	Black,
	Red}
;

public enum SoundGameMode
{
	None = -1,
	Color,
	Piano}
;

public enum SpawnItemsType
{
	None = -1,
	Berry,
	Fish,
	Question}
;

