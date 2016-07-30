using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteManager : MonoBehaviour
{
    [SerializeField]
    Sprite[] m_Sprites;

    SpriteRenderer m_Rend;
    Image m_Image;

    void Start()
    {
        m_Rend = GetComponent<SpriteRenderer>();
        m_Image = GetComponent<Image>();

        if (m_Sprites.Length < LevelManager.i.World)
        {
            Debug.LogError("Not enough sprites", gameObject);
            if (m_Rend != null) m_Rend.sprite = m_Sprites[0];
            if (m_Image != null) m_Image.sprite = m_Sprites[0];
            return;
        }

        if (m_Rend != null) m_Rend.sprite = m_Sprites[LevelManager.i.World - 1];
        if (m_Image != null) m_Image.sprite = m_Sprites[LevelManager.i.World - 1];
    }
}
