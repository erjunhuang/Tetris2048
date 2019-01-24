using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerData
{	
	public int version = 1;
    static protected PlayerData m_Instance;
    static public PlayerData instance { get { return m_Instance; } }
    private string PlayInfoFile = Application.persistentDataPath + "/TetrisGameData.bin";

    static public void CreateForPlayInfo()
    {
        if (m_Instance == null)
        {
            m_Instance = new PlayerData();
        }
        if (File.Exists(m_Instance.PlayInfoFile))
        {
            // If we have a save, we read it.
            m_Instance.ReadPlayerInfo();
        }
        else
        {
            // If not we create one with default data.
            m_Instance.PlayInfoSave();
        }
        Debug.Log("PlayInfoFile:" + m_Instance.PlayInfoFile);
    }
    public void PlayInfoSave()
    {
        BinaryWriter w = new BinaryWriter(new FileStream(PlayInfoFile, FileMode.OpenOrCreate));
		w.Write (version);
        w.Write((int)Managers.Game.stats.currentGameMode);
        for (int i= 0; i < Managers.Game.stats.gameModeIsLock.Length; i++){
            w.Write(Managers.Game.stats.gameModeIsLock[i]);
        }
        w.Write(Managers.Game.stats.playerMoney);

        w.Write(Managers.Game.stats.highScore);
        w.Write(Managers.Game.stats.totalScore);
        w.Write(Managers.Game.stats.timeSpent);
        w.Write(Managers.Game.stats.numberOfGames);
        w.Write(Managers.Game.stats.isOpenMusic);
        w.Write(Managers.Game.stats.gamePlayTime);
        w.Write(Managers.Game.stats.ObstacleIndex);
        w.Write(Managers.Game.stats.watchVideoTime);
        w.Write(Managers.Game.stats.startWatchVideoTime);

        w.Write(Managers.Game.isGameActive);
        if (Managers.Game.isGameActive)
        {   
            int ArrayCount = 0;
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 11; x++)
                {
                    if (Managers.Game.stats.gameGridcolForConnect[x].row[y] >=0)
                    {
                        ArrayCount++;
                    }
                }
            }
            w.Write(ArrayCount);

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 11; x++)
                {
                    if (Managers.Game.stats.gameGridcolForConnect[x].row[y]>=0)
                    {
                        w.Write(x);
                        w.Write(y);
                        w.Write(Managers.Game.stats.gameGridcolForConnect[x].row[y]);
                    }
                }
            }

            w.Write(Managers.Game.stats.currentShapeIdColor);
             

            w.Write(Managers.Score.currentScore);
			Debug.Log ("Managers.Score.currentScore111:" + Managers.Score.currentScore);
            w.Write(Managers.Score.highScore);
            w.Write(Managers.Game.stats.currentGameTimeSpent);
            w.Write(Managers.Game.stats.nextShapeID);
            w.Write(Managers.Game.stats.nextShapeIsBmob);
        }
        w.Close();

    }
    public void ReadPlayerInfo()
    {
        BinaryReader r = new BinaryReader(new FileStream(PlayInfoFile, FileMode.Open));
		version = r.ReadInt32 ();
        Managers.Game.stats.currentGameMode = (GameMode)r.ReadInt32();
        for (int i = 0; i < Managers.Game.stats.gameModeIsLock.Length; i++)
        {
            Managers.Game.stats.gameModeIsLock[i]= r.ReadBoolean();
        }
        Managers.Game.stats.playerMoney = r.ReadInt32();

        Managers.Game.stats.highScore = r.ReadInt32();
        Managers.Game.stats.totalScore = r.ReadInt32();
        Managers.Game.stats.timeSpent = r.ReadSingle();
        Managers.Game.stats.numberOfGames = r.ReadInt32();
        Managers.Game.stats.isOpenMusic = r.ReadBoolean();
        Managers.Game.stats.gamePlayTime = r.ReadInt32();
        Managers.Game.stats.ObstacleIndex = r.ReadInt32();
        Managers.Game.stats.watchVideoTime = r.ReadInt32();
        Managers.Game.stats.startWatchVideoTime = r.ReadInt32();


        Managers.Game.isGameActive = r.ReadBoolean();
        if (Managers.Game.isGameActive)
        {
            Managers.Game.stats.CrearGrid();
            int ArrayCount = r.ReadInt32();
            for (int i = 1; i <= ArrayCount; i++)
            {
                int x = r.ReadInt32();
                int y = r.ReadInt32();
                int idColor = r.ReadInt32();
               Managers.Game.stats.gameGridcolForConnect[x].row[y] = idColor;
            }

            Managers.Game.stats.currentShapeIdColor = r.ReadInt32();

            Managers.Score.currentScore= r.ReadInt32();
            Managers.Score.highScore = r.ReadInt32();
            Managers.Game.stats.currentGameTimeSpent = r.ReadSingle();
            Managers.Game.stats.nextShapeID= r.ReadInt32();
            Managers.Game.stats.nextShapeIsBmob = r.ReadBoolean();
        }
         
        r.Close();

        Managers.Game.OnConnected();
    }
}
