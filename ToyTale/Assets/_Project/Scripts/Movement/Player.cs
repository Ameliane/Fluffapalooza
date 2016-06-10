using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    Rigidbody2D m_Body;

    bool m_Alive = true;
    bool m_OnGround = false;

    void Start()
    {
        m_Body = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (!m_Alive) return;

        m_Body.velocity = Vector2.Scale(m_Body.velocity, Vector2.up);
        transform.position += Vector3.right * 1.5f * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
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
        m_Body.AddForce(Vector2.up * 220);
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
