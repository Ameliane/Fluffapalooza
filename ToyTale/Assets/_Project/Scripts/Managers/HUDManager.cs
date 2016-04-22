using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Drag(GameObject aThis)
    {
        aThis.transform.position = Input.mousePosition;
    }

    public void Drop()
    {
        Debug.Log("Drop");
    }

    public void Select()
    {
        Debug.Log("Select");
    }

    public void Deselect()
    {
        Debug.Log("Deselect");
    }

    public void Move()
    {
        Debug.Log("Move");
    }

    public void BeginDrag()
    {
        Debug.Log("BeginDrag");
    }

    public void EndDrag()
    {
        Debug.Log("EndDrag");
    }
    
}
