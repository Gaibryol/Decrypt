public static class Constants
{
    public enum GameStates { MainMenu = 0, Credits = 1, Settings = 2, Game = 3, Tutorial = 4 }

	public enum SubState { Playing = 0,Pause = 1, Help = 2, Hack = 3, Complete = 4 }

	public enum LetterColors { Blue = 0, Green = 1, Orange = 2, Pink = 3, Purple = 4, Red = 5, Yellow = 6 }

	public const float MaxTime = 10f;
	public const float PointsPerLetter = 200;

	public const float DecryptTime = 25f;

	public const int WarningLimit = 1;
}
