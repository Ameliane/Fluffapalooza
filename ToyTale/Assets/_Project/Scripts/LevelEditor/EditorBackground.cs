using UnityEngine;
using System.Collections;

public class EditorBackground : MonoBehaviour
{
    [SerializeField]
    float m_Height = 1;

    bool m_Selected = false;
    Collider2D collision = null;
    LevelEditor m_Editor = null;

    void Start()
    {
        collision = GetComponent<Collider2D>();
        m_Editor = FindObjectOfType<LevelEditor>();
        SetSelected(false);
    }

    public void Click(Vector3 aPosition, bool aTouchBegan)
    {
        if (m_Editor.GetEditMode() == LevelEditor.EditMode.Move)
        {
            if (aTouchBegan)
            {
                SetSelected(true);
            }

            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            pos.x = Mathf.Round(pos.x / 0.25f) * 0.25f;
            pos.y = Mathf.Floor(pos.y) - 0.5f;
            pos.z = transform.position.z;
            transform.position = pos;
        }
        else if (m_Editor.GetEditMode() == LevelEditor.EditMode.Clear)
        {
            m_Editor.RemoveBackground(gameObject);
        }
    }

    public bool GetSelected()
    {
        return m_Selected;
    }

    public void SetSelected(bool aSelected)
    {
        m_Selected = aSelected;
        if (!m_Selected) return;

        EditorBackground[] backgrounds = FindObjectsOfType<EditorBackground>();
        foreach (EditorBackground b in backgrounds)
        {
            if (b != this) b.SetSelected(false);
        }
    }

    //This ALSO needs to go away
    void OnGUI()
    {
        if (!m_Selected) return;

        //Depth
        float x = Camera.main.WorldToScreenPoint(transform.position).x;
        float y = Screen.height - Camera.main.WorldToScreenPoint(transform.position + transform.up).y;

        if (GUI.Button(new Rect(x + 20, y - 50 * m_Height, 30, 30), ">")) transform.position += Vector3.forward;
        if (GUI.Button(new Rect(x - 60, y - 50 * m_Height, 30, 30), "<")) transform.position -= Vector3.forward;

        GUI.Label(new Rect(x - 25, y - 70 * m_Height, 120, 30), "Depth");
        GUI.Label(new Rect(x - 10, y - 50 * m_Height, 120, 30), transform.position.z.ToString());
    }
}
