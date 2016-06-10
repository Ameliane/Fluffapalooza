using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
    [SerializeField]
    Transform m_Target;
    Vector3 m_Offset;

    void Start()
    {
        m_Offset = transform.position - m_Target.position;
    }
    
    void Update()
    {
        transform.position = m_Target.position + m_Offset;
    }
}
