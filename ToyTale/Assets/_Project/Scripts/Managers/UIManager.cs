using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HUDManager : MonoBehaviour
{
    public GameObject m_Tool1;
    public GameObject Tool1Prefab;
    private Vector3 m_Tool1StartPos;

    // Use this for initialization
    void Start()
    {
        m_Tool1StartPos = m_Tool1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

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
        Debug.Log("EndDrag");
        Vector3 newPos = Vector3.zero;
        GameObject.Instantiate(Tool1Prefab, newPos, Quaternion.identity);
        aTool.transform.position = m_Tool1StartPos;
    }
    
}
