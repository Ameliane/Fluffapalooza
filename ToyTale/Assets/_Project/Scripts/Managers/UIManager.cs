using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject m_CurrentCanvas;
    string m_CurrentScene = "";
    bool m_ShowOptions = false;
    public GameObject m_OptionsMenu;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeCanvas(GameObject aCanvas)
    {
        m_CurrentCanvas.SetActive(false);
        m_CurrentCanvas = aCanvas;
        m_CurrentCanvas.SetActive(true);
    }

    public void ChangeScene(string aScene)
    {
        if(m_CurrentScene.Length > 1)
            SceneManager.UnloadScene(m_CurrentScene);

        if (aScene.Length > 1)
        {
            m_CurrentScene = aScene;
            SceneManager.LoadScene(m_CurrentScene, LoadSceneMode.Additive);
        }
    }

    public void ToggleOptionMenu()
    {
        m_ShowOptions = !m_ShowOptions;
        m_OptionsMenu.SetActive(m_ShowOptions);
    }
}
