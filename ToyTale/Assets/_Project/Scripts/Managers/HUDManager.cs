using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HUDManager : MonoBehaviour
{
    public GameObject[] m_Tools;
    public GameObject[] m_ToolPrefabs;
    private Vector3[] m_ToolStartPos;

    // Use this for initialization
    void Start()
    {
        m_ToolStartPos = new Vector3[m_Tools.Length];

        for (int i = 0; i < m_ToolStartPos.Length; i++)
        {
            m_ToolStartPos[i] = m_Tools[i].transform.position;
        }
    }

    public void Drag(GameObject aTool)
    {
        aTool.transform.position = Input.mousePosition;
        //aTool.transform.position = Input.GetTouch(0).position;
    }

    public void BeginDrag(GameObject aTool)
    {
        Debug.Log("BeginDrag");
    }

    public void EndDrag(GameObject aTool)
    {
        int index = 0;
        for (int i = 0; i < m_Tools.Length; i++)
        {
            if (m_Tools[i] == aTool)
            {
                index = i;
            }
        }

        Debug.Log("EndDrag");
        Vector3 newPos = Vector3.zero;
        newPos = Camera.main.ScreenToWorldPoint(aTool.transform.position);
        newPos.z = 0;
        GameObject.Instantiate(m_ToolPrefabs[index], newPos, Quaternion.identity);
        aTool.transform.position = m_ToolStartPos[index];
    }
    
}
