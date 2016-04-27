using UnityEngine;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour
{
    public enum EditMode
    {
        Move,
        Place,
        Clear
    }

    [SerializeField]
    GameObject m_Tile;
    List<GameObject> m_Tiles;

    EditMode m_Mode = EditMode.Move;
    Vector2 m_PreviousTouchPosition = Vector2.zero;
    float m_ScrollSpeed = 0.02f;
    int m_Width = 0;
    int m_Height = 0;
    
    void Start()
    {
        m_Tiles = new List<GameObject>();
        ResizeLevel(16, 8);
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
    }

    public EditMode GetEditMode()
    {
        return m_Mode;
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

        //Mode
        if (GUI.Button(new Rect(10, 10, 60, 30), "Move")) m_Mode = EditMode.Move;
        if (GUI.Button(new Rect(10, 50, 60, 30), "Place")) m_Mode = EditMode.Place;
        if (GUI.Button(new Rect(10, 90, 60, 30), "Clear")) m_Mode = EditMode.Clear;

        ////Tile index display
        //for (int i = 0; i < m_Tiles.Count; i++)
        //{
        //    Vector3 pos = Camera.main.WorldToScreenPoint(m_Tiles[i].transform.position);
        //    GUI.Label(new Rect(pos.x, -pos.y + 550, 30, 30), i.ToString());
        //}
    }
}
