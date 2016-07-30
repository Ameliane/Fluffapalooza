using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager i = null;

    public string m_FileName = "test";

    [SerializeField]
    GameObject m_TilePrefab;

    [SerializeField]
    GameObject m_BackgroundPrefab;

    BoxCollider2D m_End;

    int m_World = 0;

    void Awake()
    {
        if (i == null)
            i = this;
        else
            Destroy(gameObject);

        m_End = GetComponent<BoxCollider2D>();
        LoadLevel();
    }
    
    void LoadLevel()
    {
        string loadPath = GetSaveFilePath(m_FileName);

        Debug.Log("Loading Level: " + loadPath);

        //Make sure that the load path is real
        if (!File.Exists(loadPath))
        {
            Debug.LogError("File doesn't exist: " + loadPath);
        }

        //Create the file stream and formatter
        BinaryFormatter loadFormatter = new BinaryFormatter();
        FileStream loadStream = File.Open(loadPath, FileMode.Open);

        //Make sure the version number is correct
        int versionNumber = (int)loadFormatter.Deserialize(loadStream);
        if (versionNumber != LevelEditor.SaveGameVersionNum)
        {
            Debug.LogWarning("Loading an incompatible version number (File version: " + versionNumber + ", Expected version: " + LevelEditor.SaveGameVersionNum + ".  Your level may load incorrectly.");
        }

        //Load save data
        SaveData levelData = (SaveData)loadFormatter.Deserialize(loadStream);
        m_World = levelData.World;

        //Load Tiles
        for (int i = 0; i < levelData.Tiles.Count; i++)
        {
            if (levelData.Tiles[i] == EditorTile.TileState.Ground)
            {
                int x = i % levelData.Width;
                int y = i / levelData.Width;
                Instantiate(m_TilePrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

        //Load Background
        for (int i = 0; i < levelData.Background.Count; i++)
        {
            Instantiate(m_BackgroundPrefab, new Vector3(levelData.Background[i].x, levelData.Background[i].y, levelData.Background[i].z), Quaternion.identity);
        }

        //Load Finish
        m_End.offset = new Vector2(levelData.EndPoint, m_End.offset.y);

        //Close the file stream
        loadStream.Close();
        Debug.Log("Load Successful!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Win
            FindObjectOfType<FollowCam>().SetTarget(null);
            other.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100);
        }
    }

    string GetSaveFilePath(string aFileName)
    {
        return Application.persistentDataPath + "/" + aFileName;
    }

    public int World { get { return m_World; } }
}
