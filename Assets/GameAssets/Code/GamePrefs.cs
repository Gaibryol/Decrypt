using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public class GamePrefs
{
    public bool sync = false;
    public Constants.GameType GameType;
    public int Seed;
    public List<int> WordLengths = new List<int>();
    public int Timer;

    public GamePrefs(Constants.GameType gameType, int seed, List<int> wordLengths, int timer)
    {
        GameType = gameType;
        Seed = seed;
        WordLengths.Clear();
        WordLengths.AddRange(wordLengths);
        if (GameType == Constants.GameType.Timed)
        {
            Timer = timer;
        }
    }

    /// <summary>
    /// Keep empty
    /// </summary>
    public GamePrefs()
    {

    }

    public void UpdatePrefs(GamePrefs prefs)
    {
        GameType = prefs.GameType;
        Seed = prefs.Seed;
        Debug.Log(prefs.WordLengths.Count);
        //WordLengths = prefs.WordLengths;
        Timer = prefs.Timer;
    } 

    public static byte[] Serialize(object obj)
    {
        Debug.Log(JsonUtility.ToJson(obj));
        return Encoding.UTF8.GetBytes(JsonUtility.ToJson(obj));
    }
    
    public static object Deserialize(byte[] data)
    {
        using (var stream = new MemoryStream(data))
        using (var reader = new StreamReader(stream, Encoding.UTF8))
            return JsonSerializer.Create().Deserialize(reader, typeof(GamePrefs)) as GamePrefs;
    }
}
