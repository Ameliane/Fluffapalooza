using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditorUIButton : MonoBehaviour
{
    public InputField m_Input;

    private LevelEditor m_Editor;

    // Use this for initialization
    void Start()
    {
        m_Editor = FindObjectOfType<LevelEditor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EditMode(string aMode)
    {
        switch (aMode)
        {
            case "Move":
                m_Editor.SetEditMode(LevelEditor.EditMode.Move);
                break;

            case "Place":
                m_Editor.SetEditMode(LevelEditor.EditMode.Place);
                break;

            case "Clear":
                m_Editor.SetEditMode(LevelEditor.EditMode.Clear);
                break;

            default:
                break;
        }
    }

    public void SaveLevel()
    {
        m_Editor.SaveLevel(m_Input.text);
    }

    public void LoadLevel()
    {
        m_Editor.StartLoadLevel(m_Input.text);
    }
}
