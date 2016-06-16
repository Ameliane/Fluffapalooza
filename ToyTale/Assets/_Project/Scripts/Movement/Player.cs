using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    float m_Speed = 1.5f;

    [SerializeField]
    float m_JumpForce = 5;

    Rigidbody2D m_Body;

    bool m_Alive = true;
    bool m_OnGround = false;

    void Start()
    {
        m_Body = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        if (!m_Alive) return;

        if (m_Body.velocity.x < m_Speed)
        {
            Vector2 vel = m_Body.velocity;
            vel.x = m_Speed;
            m_Body.velocity = vel;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_OnGround && m_Alive)
        {
            Jump();
        }
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        m_OnGround = true;
    }

    void Jump()
    {
        if (m_Body.velocity.y < m_JumpForce)
        {
            Vector2 vel = m_Body.velocity;
            vel.y = m_JumpForce;
            m_Body.velocity = vel;
        }

        m_OnGround = false;
    }

    public bool GetIsAlive()
    {
        return m_Alive;
    }

    public void Die()
    {
        m_Alive = false;
        m_Body.constraints = RigidbodyConstraints2D.None;
    }
}
