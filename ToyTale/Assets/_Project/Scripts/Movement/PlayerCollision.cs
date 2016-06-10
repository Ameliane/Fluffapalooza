using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    Player m_Player;

    void Start()
    {
        m_Player = transform.parent.GetComponent<Player>();
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (m_Player.GetIsAlive() == false) return;

        m_Player.Die();

        m_Player.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.left * 100, hit.contacts[0].point);
    }
}
