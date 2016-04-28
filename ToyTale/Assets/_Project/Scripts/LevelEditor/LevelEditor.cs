using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class LevelEditor : MonoBehaviour
{
    public enum EditMode
    {
        Move,
        Place,
        Clear
    }

    const int SaveGameVersionNum = 0;

    [SerializeField]
    GameObject m_Tile;
    List<GameObject> m_Tiles;

    EditMode m_Mode = EditMode.Move;
    LineRenderer m_EndLine;
    Vector2 m_PreviousTouchPosition = Vector2.zero;
    float m_ScrollSpeed = 0.02f;
    string m_FileName = "Level";
    int m_Width = 0;
    int m_Height = 0;
    int m_EndPoint = 12;
    
    void Start()
    {
        m_Tiles = new List<GameObject>();
        m_EndLine = gameObject.AddComponent<LineRenderer>();
        ResizeLevel(16, 8);

        Color green = new Color(0, 1, 0, 0.5f);
        m_EndLine.material = new Material(Shader.Find("Sprites/Default"));
        m_EndLine.SetColors(green, green);
        m_EndLine.SetVertexCount(2);
        m_EndLine.SetPosition(0, new Vector3(m_EndPoint, -0.5f, -0.25f));
        m_EndLine.SetPosition(1, new Vector3(m_EndPoint, m_Height - 0.5f, -0.25f));
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) m_PreviousTouchPosition = Input.GetTouch(0).position;

            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (m_Mode == EditMode.Move)
                {
                    Vector3 diff = Input.GetTouch(0).position - m_PreviousTouchPosition;
                    Camera.main.transform.position -= diff * m_ScrollSpeed;
                    m_PreviousTouchPosition = Input.GetTouch(0).position;
                }
                else
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
                    if (hitInfo)
                    {
                        EditorTile tile = hitInfo.transform.GetComponent<EditorTile>();
                        if (tile) tile.Click();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0)) m_PreviousTouchPosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            if (m_Mode == EditMode.Move)
            {
                Vector3 diff = Input.mousePosition - new Vector3(m_PreviousTouchPosition.x, m_PreviousTouchPosition.y);
                Camera.main.transform.position -= diff * m_ScrollSpeed;
                m_PreviousTouchPosition = Input.mousePosition;
            }
            else
            {
                Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
                if (hitInfo)
                {
                    EditorTile tile = hitInfo.transform.GetComponent<EditorTile>();
                    if (tile) tile.Click();
                }
            }
        }
    }

    void ResizeLevel(int aWidth, int aHeight)
    {
        if (aWidth < m_Width) //If width is decreasing, remove tiles from the end of rows
        {
            List<GameObject> toRemove = new List<GameObject>();
            for (int y = 0; y < m_Height; y++)
            {
                for (int x = aWidth; x < m_Width; x++)
                {
                    toRemove.Add(m_Tiles[y * m_Width + x]);
                }
            }

            foreach (GameObject tile in toRemove)
            {
                m_Tiles.Remove(tile);
                Destroy(tile);
            }
        }
        else if (aWidth > m_Width) //If width in increasing, insert tiles at the end of rows
        {
            for (int y = 0; y < m_Height; y++)
            {
                for (int x = m_Width; x < aWidth; x++)
                {
                    m_Tiles.Insert(y * aWidth + x, Instantiate(m_Tile, new Vector3(x, y, 0), Quaternion.identity) as GameObject);
                }
            }
        }
        m_Width = aWidth;



        if (aHeight < m_Height) //If height is decreasing, remove rows
        {
            for (int y = aHeight; y < m_Height; y++)
            {
                for (int x = 0; x < aWidth; x++)
                {
                    Destroy(m_Tiles[m_Tiles.Count - 1]);
                    m_Tiles.RemoveAt(m_Tiles.Count - 1);
                }
            }
        }
        else if (aHeight > m_Height) //If height is increasing, add rows
        {
            for (int y = m_Height; y < aHeight; y++)
            {
                for (int x = 0; x < aWidth; x++)
                {
                    m_Tiles.Add(Instantiate(m_Tile, new Vector3(x, y, 0), Quaternion.identity) as GameObject);
                }
            }
        }
        m_Height = aHeight;

        MoveEndpoint(m_EndPoint);
    }

    void MoveEndpoint(int aColumn)
    {
        int endPoint = aColumn;
        if (endPoint > m_Width - 1) endPoint = m_Width - 1;
        if (endPoint < 0) endPoint = 0;

        m_EndLine.SetPosition(0, new Vector3(endPoint, -0.5f, -0.25f));
        m_EndLine.SetPosition(1, new Vector3(endPoint, m_Height - 0.5f, -0.25f));

        m_EndPoint = endPoint;
    }

    public EditMode GetEditMode()
    {
        return m_Mode;
    }

    void SaveLevel(string aFileName)
    {
        string savePath = GetSaveFilePath(aFileName);

        Debug.Log("Saving Level: " + savePath);

        //Populate save data
        SaveData levelData = new SaveData();
        levelData.Width = m_Width;
        levelData.Height = m_Height;
        levelData.EndPoint = m_EndPoint;
        levelData.Tiles = new List<EditorTile.TileState>();

        //Populate tiles
        foreach (GameObject tile in m_Tiles)
        {
            EditorTile t = tile.GetComponent<EditorTile>();
            if (t) levelData.Tiles.Add(t.GetTileState());
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(savePath);

        //Save version number, so we don't try to load in bad data
        binaryFormatter.Serialize(file, SaveGameVersionNum);

        //Save level data
        binaryFormatter.Serialize(file, levelData);

        //Clean up
        file.Close();

        Debug.Log("Save Successful!");
    }

    IEnumerator LoadLevel(string aFileName)
    {
        string loadPath = GetSaveFilePath(aFileName);

        Debug.Log("Loading Level: " + loadPath);

        //Make sure that the load path is real
        if (!File.Exists(loadPath))
        {
            Debug.Log("File doesn't exist: " + loadPath);
            yield break;
        }

        //Create the file stream and formatter
        BinaryFormatter loadFormatter = new BinaryFormatter();
        FileStream loadStream = File.Open(loadPath, FileMode.Open);

        //Make sure the version number is correct
        int versionNumber = (int)loadFormatter.Deserialize(loadStream);
        if (versionNumber != SaveGameVersionNum)
        {
            Debug.LogWarning("Loading an incompatible version number (File version: " + versionNumber + ", Expected version: " + SaveGameVersionNum + ".  Your level may load incorrectly.");
        }

        //Load save data
        SaveData levelData = (SaveData)loadFormatter.Deserialize(loadStream);
        ResizeLevel(levelData.Width, levelData.Height);
        MoveEndpoint(levelData.EndPoint);

        yield return null;

        //Load Tiles
        for (int i = 0; i < levelData.Tiles.Count; i++)
        {
            EditorTile tile = m_Tiles[i].GetComponent<EditorTile>();

            switch (levelData.Tiles[i])
            {
                case EditorTile.TileState.Ground:
                    m_Mode = EditMode.Place;
                    break;

                default:
                    m_Mode = EditMode.Clear;
                    break;
            }

            if (tile) tile.Click();
        }
        
        //Close the file stream
        loadStream.Close();

        m_Mode = EditMode.Move;

        Debug.Log("Load Successful!");
    }

    string GetSaveFilePath(string aFileName)
    {
        return Application.persistentDataPath + "/" + aFileName;
    }

    //This needs to not be a thing anymore, but it's good for just getting stuff working
    void OnGUI()
    {
        //Width
        if (GUI.Button(new Rect(Screen.width - 180, Screen.height - 40, 30, 30), ">")) ResizeLevel(m_Width + 4, m_Height);
        if (GUI.Button(new Rect(Screen.width - 260, Screen.height - 40, 30, 30), "<")) ResizeLevel(m_Width - 4, m_Height);

        GUI.Label(new Rect(Screen.width - 225, Screen.height - 60, 120, 30), "Width");
        GUI.Label(new Rect(Screen.width - 210, Screen.height - 40, 120, 30), m_Width.ToString());

        //Height
        if (GUI.Button(new Rect(Screen.width - 40, Screen.height - 40, 30, 30), ">")) ResizeLevel(m_Width, m_Height + 4);
        if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 40, 30, 30), "<")) ResizeLevel(m_Width, m_Height - 4);

        GUI.Label(new Rect(Screen.width - 85, Screen.height - 60, 120, 30), "Height");
        GUI.Label(new Rect(Screen.width - 70, Screen.height - 40, 120, 30), m_Height.ToString());

        //End Point
        if (GUI.Button(new Rect(Screen.width - 300, Screen.height - 40, 30, 30), ">")) MoveEndpoint(m_EndPoint + 1);
        if (GUI.Button(new Rect(Screen.width - 380, Screen.height - 40, 30, 30), "<")) MoveEndpoint(m_EndPoint - 1);

        GUI.Label(new Rect(Screen.width - 360, Screen.height - 60, 120, 30), "End Point");
        GUI.Label(new Rect(Screen.width - 330, Screen.height - 40, 120, 30), m_EndPoint.ToString());

        //Mode
        if (GUI.Button(new Rect(10, 10, 60, 30), "Move")) m_Mode = EditMode.Move;
        if (GUI.Button(new Rect(10, 50, 60, 30), "Place")) m_Mode = EditMode.Place;
        if (GUI.Button(new Rect(10, 90, 60, 30), "Clear")) m_Mode = EditMode.Clear;

        //Save and Load
        m_FileName = GUI.TextField(new Rect(10, Screen.height - 110, 120, 20), m_FileName);
        if (GUI.Button(new Rect(10, Screen.height - 80, 120, 30), "Save")) SaveLevel(m_FileName);
        if (GUI.Button(new Rect(10, Screen.height - 40, 120, 30), "Load")) StartCoroutine(LoadLevel(m_FileName)); ;
    }
}

[Serializable]
public class SaveData
{
    public int Width;
    public int Height;
    public int EndPoint;
    public List<EditorTile.TileState> Tiles;
}
