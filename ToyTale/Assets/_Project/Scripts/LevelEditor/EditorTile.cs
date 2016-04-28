using UnityEngine;
using System.Collections;

public class EditorTile : MonoBehaviour
{
    public enum TileState
    {
        Empty,
        Ground
    }

    [SerializeField]
    Sprite m_EmptySprite;

    [SerializeField]
    Sprite m_GroundSprite;

    TileState m_State = TileState.Empty;
    SpriteRenderer m_Renderer = null;
    LevelEditor m_Editor = null;

    void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Editor = FindObjectOfType<LevelEditor>();
    }
    
    public void Click()
    {
        if (m_Editor == null) return;
        
        switch (m_Editor.GetEditMode())
        {
            case LevelEditor.EditMode.Place:
                SetTileState(TileState.Ground);
                break;

            case LevelEditor.EditMode.Clear:
                SetTileState(TileState.Empty);
                break;

            default:
                return;
        }
    }

    public void SetTileState(TileState aState)
    {
        if (m_State == aState) return;

        m_State = aState;
        switch(m_State)
        {
            case TileState.Empty:
                m_Renderer.sprite = m_EmptySprite;
                break;

            case TileState.Ground:
                m_Renderer.sprite = m_GroundSprite;
                break;

            default:
                Debug.Log("I don't know how to deal with that tile state, and I'm kinda freaking out here...");
                break;
        }
    }

    public TileState GetTileState()
    {
        return m_State;
    }
}
