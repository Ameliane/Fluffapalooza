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
        if (m_Target == null) return;

        transform.position = m_Target.position + m_Offset;
    }

    public void SetTarget(Transform aTarget)
    {
        m_Target = aTarget;
    }

    public void SetOffset(Vector3 aOffset)
    {
        m_Offset = aOffset;
    }
}
