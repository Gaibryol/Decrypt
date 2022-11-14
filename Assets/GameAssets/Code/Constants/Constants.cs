using System.Collections.Generic;

public static class Constants
{
    public enum GameStates { MainMenu = 0, Credits = 1, Settings = 2, Game = 3, Lobby = 4, Room = 5, End = 6}

	public enum SubState{Playing = 0,Pause = 1, Help = 2, Hack = 3,Complete = 4, Loading = 5}

    public enum GameType { Default = 0, Timed = 1, BR = 2}

    public enum PlayMode { Single = 0, Multi = 1}

	public enum LetterColors { Blue = 0, Green = 1, Orange = 2, Pink = 3, Purple = 4, Red = 5, Yellow = 6 }

    public enum PhotonState { Disconected = 0, Connected = 1}

	public static Hack0 Hack0= new Hack0(); //  "The first letter of each word is in the correct position|Only the bottom row can be seen";
	public static Hack1 Hack1= new Hack1(); //  "The last letter of each word is in the correct position|Only the bottom row can be seen";
	public static Hack2 Hack2= new Hack2(); //"Decrypt the longest words every 25 seconds|Each word has a chance of having a letter obscured";
	public static Hack3 Hack3= new Hack3(); // "Increase the maximum amount of rows by 2|Only the bottom three rows can be seen";
	public static Hack4 Hack4= new Hack4(); // "Right-click to decrypt a word instantly (Up to 4 uses in a game)|Letters only appear when hovering over the letter tile";
	public static Hack5 Hack5= new Hack5(); //"Letters can be seen as they’re falling down|See 1 less row";
	public static Hack6 Hack6= new Hack6(); // "Right-click instantly drops the falling word|See 1 less row";
	public static Hack7 Hack7= new Hack7(); // "Decrypt a random word every 25 seconds|See 3 less row";
	public static Hack8 Hack8= new Hack8(); //"Decrypt all words (One use per game)|See 2 less lines";

    // Battle Royal Positive Hack
    public static Hack20 Hack20 = new Hack20();

    // Battle Royal Negative Hack
    public static Hack30 Hack30 = new Hack30();

    //public enum NegativeHacks { Hack30 = 0}

    public const float MaxTime = 10f;
	public const float PointsPerLetter = 200;

	public const float DecryptTime = 25f;

	public const int WarningLimit = 1;

    public const byte GameStartEventCode = 1;
    public const byte HackSelectedEventCode = 2;

    //H(Destroy a word every 25 seconds - Two words come down every 10 seconds)
    //I(2x the Points - Lose 2 Lines)
}
