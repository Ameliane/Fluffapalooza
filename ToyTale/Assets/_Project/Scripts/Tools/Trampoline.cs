using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    float m_JumpPower = 7;

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag != "Player") return;

        float angle = Vector2.Angle(hit.contacts[0].normal, transform.up);
        bool fromTop = 1 * Mathf.Sign(hit.rigidbody.velocity.y) * Mathf.Sign(transform.up.y) < 0;
        if (fromTop && angle < 10 || angle > 170)
        {
            Vector2 vel = hit.rigidbody.velocity;
            vel.y = m_JumpPower;
            hit.rigidbody.velocity = vel;
        }
    }
}
