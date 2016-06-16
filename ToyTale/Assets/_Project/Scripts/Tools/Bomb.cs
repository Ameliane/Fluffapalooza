using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    float m_Delay = 1;

    [SerializeField]
    float m_Power = 500;

    Rigidbody2D m_Body;

    void Start()
    {
        m_Body = GetComponent<Rigidbody2D>();
        StartCoroutine(explode_cr());
    }

    IEnumerator explode_cr()
    {
        yield return new WaitForSeconds(m_Delay);

        Rigidbody2D[] objects = FindObjectsOfType<Rigidbody2D>();

        foreach (Rigidbody2D body in objects)
        {
            if (body.gameObject.tag != "Player" && body != m_Body)
            {
                float dist = Vector2.Distance(body.transform.position, transform.position);
                body.AddForce((body.transform.position - transform.position) / dist * m_Power);
            }
        }

        Destroy(gameObject);
    }
}
