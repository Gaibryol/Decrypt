using System.Collections.Generic;

public static class Constants
{
    public enum GameStates { MainMenu = 0, Credits = 1, Settings = 2, Game = 3, }

	public enum SubState{Playing = 0,Pause = 1, Help = 2, Hack = 3,Complete = 4}

	public enum LetterColors { Blue = 0, Green = 1, Orange = 2, Pink = 3, Purple = 4, Red = 5, Yellow = 6 }

	public enum HackFunction {None = 0, Apply = 1, Update = 2, RightClick = 3, GetLength = 4}

	public enum EffectType {Falling = 0, Obscure = 1, DecryptTime = 2, RightClick = 3, Lines = 4, Reveal = 5}
	
	public enum GameMode {SinglePlayer= 0, Multiplayer = 1, Both = 2} 

	public enum RightClickTarget {None = 0, Word = 1, All = 2}

	public const float MaxTime = 10f;
	public const float PointsPerLetter = 200;

	public const float DecryptTime = 25f;

	public const int WarningLimit = 1;



    //H(Destroy a word every 25 seconds - Two words come down every 10 seconds)
    //I(2x the Points - Lose 2 Lines)
}
