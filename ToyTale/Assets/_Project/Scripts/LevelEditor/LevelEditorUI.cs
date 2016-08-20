using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Events;

public class LevelEditorUI : MonoBehaviour
{
    [System.Serializable]
    public class Tabs
    {
        public string m_TabName;
        public bool m_IsGeneral = true;
        public GameObject m_SpecificPrefab;
        public Sprite[] m_Images;
    }

    [Header("NAVIGATION STUFF")]
    public GameObject m_CurrentTab;
    private GameObject m_CurrentPlacement;

    [Header("SETUP STUFF")]
    public GameObject m_TabButtonPrefab;
    public GameObject m_PanelPrefab;

    public Tabs[] m_Tabs;

    // Use this for initialization
    void Start()
    {
        m_CurrentTab.SetActive(true);
    }

    public void SwitchTab(GameObject aTab)
    {
        m_CurrentTab.SetActive(false);

        m_CurrentTab = aTab;

        m_CurrentTab.SetActive(true);
    }

    public void SelectPlacement(GameObject aObj)
    {
        if (m_CurrentPlacement)
            m_CurrentPlacement.transform.FindChild("Outline").gameObject.SetActive(false);

        m_CurrentPlacement = aObj;

        m_CurrentPlacement.transform.FindChild("Outline").gameObject.SetActive(true);
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        Delete();
        SetUp();
    }

    void SetUp()
    {
        for (int i = 0; i < m_Tabs.Length; i++)

        {
            // Set Up the Tab button
            GameObject g = Instantiate(m_TabButtonPrefab);
            g.GetComponentInChildren<Text>().text = m_Tabs[i].m_TabName;
            g.transform.SetParent(transform, false);
            g.transform.position += new Vector3(0, -100 * i, 0);

            // If general template, set it up
            if (m_Tabs[i].m_IsGeneral)
            {
                GameObject panel = Instantiate(m_PanelPrefab);
                panel.SetActive(false);
                panel.transform.SetParent(transform, false);

                UnityEditor.Events.UnityEventTools.AddObjectPersistentListener(g.GetComponent<Button>().onClick, SwitchTab, panel.gameObject);

                for (int j = 0; j < m_Tabs[i].m_Images.Length; j++)
                {
                    GameObject obj = Instantiate(m_Tabs[i].m_SpecificPrefab);
                    obj.transform.FindChild("Image").GetComponent<Image>().sprite = m_Tabs[i].m_Images[j];

                    obj.transform.SetParent(panel.transform, false);
                    obj.transform.position += new Vector3(0, -100 * j, 0);

                    UnityEditor.Events.UnityEventTools.AddObjectPersistentListener(obj.GetComponent<Button>().onClick, SelectPlacement, obj.gameObject);
                }
            }
            else
            {
                GameObject panel = Instantiate(m_Tabs[i].m_SpecificPrefab);
                panel.SetActive(true);
                panel.transform.SetParent(transform, false);

                UnityEditor.Events.UnityEventTools.AddObjectPersistentListener(g.GetComponent<Button>().onClick, SwitchTab, panel.gameObject);

            }
        }
    }

    void Delete()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
