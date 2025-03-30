using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject lvlPages;
    private GameObject lvlButtons;
    
    private void Awake()
    {
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLvl", 1);
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void LoadLvl(int lvlId)
    {
        string lvlName = "SampleScene" ;
        SceneManager.LoadScene(lvlName);
    }

    private void ButtonsToArray()
    {
        int lvlCount = 0;
        int childCount = lvlPages.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            lvlButtons = lvlPages.transform.GetChild(i).gameObject;
            lvlCount += lvlButtons.transform.childCount;
        }

        buttons = new Button[lvlCount];
        int buttonIndex = 0;
        for (int i = 0; i < childCount; i++)
        {
            lvlButtons = lvlPages.transform.GetChild(i).gameObject;
            int childButtonCount = lvlButtons.transform.childCount;
            for (int j = 0; j < childButtonCount; j++)
            {
                buttons[buttonIndex] = lvlButtons.transform.GetChild(j).gameObject.GetComponent<Button>();
                buttonIndex++;
            }
        }
    }
}
