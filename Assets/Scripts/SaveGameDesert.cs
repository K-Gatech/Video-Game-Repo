using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveGameDesert : MonoBehaviour
{
    public enum GameTransitionState
    {
        // DesertPlanet
        Started,
        TalkedToAlien,
        ReachedMaze,
        FinishedMaze,
        FoundTreasure1,
        CameToAlienAgain,
        TalkedToAlienAgain,
        FoundTreasure2,
        CameToAlienYetAgain,
        PaidToAlien
    }

    static int SavedCurrentStage = -1;
    public static GameTransitionState SaveState;
    static string SavePath;
    static List<string> SavedDataCache;
    public static DateTime finishTime;

    // Start is called before the first frame update
    void Start()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "savedata.txt");
        Debug.Log("SavePath is " + SavePath);

        SavedDataCache = ReadSaveData();
        if (SavedDataCache != null)
        {
            SavedCurrentStage = 1;
        }
        else
        {
            SavedCurrentStage = 0;
        }

        finishTime = DateTime.MinValue;
    }

    // Update is called once per frame
    void Update()
    {
        switch (SaveState)
        {
            case GameTransitionState.Started:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.TalkedToAlien:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.ReachedMaze:
                {
                    if(SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.FinishedMaze:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData(false);
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.FoundTreasure1:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.CameToAlienAgain:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.TalkedToAlienAgain:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.FoundTreasure2:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.CameToAlienYetAgain:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                    }
                    break;
                }
            case GameTransitionState.PaidToAlien:
                {
                    if (SavedCurrentStage == 0)
                    {
                        string toWrite = ConstructSaveData();
                        StreamWriter writer = new StreamWriter(SavePath, false);
                        writer.Write(toWrite);
                        writer.Close();
                        SavedDataCache = ReadSaveData();
                        SavedCurrentStage = 1;
                        finishTime = DateTime.UtcNow;
                    }
                    break;
                }
        }
    }

    public static void TransitionToState(GameTransitionState newState)
    {
        if (newState != SaveState)
        {
            SaveState = newState;
            SavedCurrentStage = 0;
        }
    }

    public static void SaveHealth()
    {
        string toWrite = ConstructSaveData(false, 100);
        StreamWriter writer = new StreamWriter(SavePath, false);
        writer.Write(toWrite);
        writer.Close();
        SavedDataCache = ReadSaveData();
        SavedCurrentStage = 1;
    }

    private static string ConstructSaveData(bool savePosition = true, float health = 0)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        string data = "";
        // if (savePosition)
        // {
        //     data += "PlayerPosition:" + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z + "\n";
        // }
        // else
        // {
        //     Vector3 lastSavedPosition = GetSavedPlayerPosition();
        //     data += "PlayerPosition:" + lastSavedPosition.x + "," + lastSavedPosition.y + "," + lastSavedPosition.z + "\n";
        // }

        if (health == 0)
        {
            data += "PlayerHealth:" + player.transform.GetChild(1).GetComponent<PlayerHealthController>().GetHealthLevel() + "\n";
        }
        else
        {
            data += "PlayerHealth:" + 100 + "\n";
        }
        data += "Progress:" + SaveState.ToString() + "\n";

        return data;
    }

    private static List<string> ReadSaveData()
    {
        var result = new List<string>();
        if (File.Exists(SavePath))
        {
            StreamReader reader = new StreamReader(SavePath);
            while(!reader.EndOfStream)
            {
                result.Add(reader.ReadLine());
            }

            reader.Close();
            return result;
        }

        return null;
    }

    public static float GetSavedPlayerHealth()
    {
        if (SavedDataCache == null)
        {
            SavedDataCache = ReadSaveData();
        }

        if (SavedDataCache == null)
        {
            return 100;
        }

        string health = SavedDataCache[1].Split(':')[1];
        try
        {
            return float.Parse(health);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return 100;
        }
    }

    public static Vector3 GetSavedPlayerPosition()
    {
        if (SavedDataCache == null)
        {
            SavedDataCache = ReadSaveData();
        }

        if (SavedDataCache == null || SavedDataCache.Count == 0)
        {
            return new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        }
		
		Debug.Log(String.Join(",", SavedDataCache));

        string[] positions = SavedDataCache[0].Split(':')[1].Split(',');
        return new Vector3(float.Parse(positions[0]), float.Parse(positions[1]), float.Parse(positions[2]));
    }

    public static GameTransitionState GetCurrentState()
    {
        if (SavedDataCache == null)
        {
            SavedDataCache = ReadSaveData();
        }

        if (SavedDataCache == null)
        {
            return GameTransitionState.Started;
        }

        GameTransitionState result;
        if (Enum.TryParse(SavedDataCache[1].Split(':')[1], out result))
        {
            return result;
        }
        else
        {
            return GameTransitionState.Started;
        }
    }
}
