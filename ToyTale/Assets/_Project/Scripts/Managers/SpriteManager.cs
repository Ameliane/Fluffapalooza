using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour
{
    [SerializeField]
    Sprite[] m_Sprites;

    SpriteRenderer m_Rend;

    void Awake()
    {
        m_Rend = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(int aWorld)
    {
        if (m_Sprites.Length < aWorld)
        {
            Debug.LogError("Not enough sprites", gameObject);
            m_Rend.sprite = m_Sprites[0];
            return;
        }

        m_Rend.sprite = m_Sprites[aWorld - 1];
    }
}
