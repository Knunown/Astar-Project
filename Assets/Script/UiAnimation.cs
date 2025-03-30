using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using NUnit.Framework.Constraints;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class UiAnimation : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject lvlsMenu;
    [SerializeField] private float tweenTime = 0.3f;
    private RectTransform mainMenuPos;
    private CanvasGroup mainMenuAct;
    private CanvasGroup lvlsMenuAct;
    private RectTransform lvlsSectPos;
    private float mainMenuPosFade;
    private float lvlsSectPosFade;

    private void Awake()
    {
        //Menu rectpos and alpha
        mainMenuPos = mainMenu.GetComponent<RectTransform>();
        mainMenuAct = mainMenu.GetComponent<CanvasGroup>();

        //Levels rectpos and aplpha
        lvlsSectPos = GameObject.Find("Levels Section").GetComponent<RectTransform>();
        lvlsMenuAct = lvlsMenu.GetComponent<CanvasGroup>();
    }
    public async void PlayButOnClick()
    {
        mainMenuAct.blocksRaycasts = false;
        mainMenuPosFade = mainMenuPos.anchoredPosition.x - 40;
        //Move Away
        mainMenuPos.DOAnchorPosX(mainMenuPosFade, tweenTime);
        //Fade Away
        ShowLvlsMenu();
        await mainMenuAct.DOFade(0, tweenTime).AsyncWaitForCompletion();
        //Deactivate MainMenu after fading away by using async await
    }

    public async void BackButOnClick()
    {
        lvlsMenuAct.blocksRaycasts = false;
        lvlsSectPosFade = lvlsSectPos.anchoredPosition.x + 40;
        //Move Away
        lvlsSectPos.DOAnchorPosX(lvlsSectPosFade, tweenTime);
        //Fade Away

        ShowMainMenu();
        await lvlsMenuAct.DOFade(0, tweenTime).AsyncWaitForCompletion();
        //Deactivate MainMenu after fading away by using async await
    }

    public void ContinueOnClick()
    {
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("UnlockedLvl"));
    }


    private void ShowMainMenu()
    {
        mainMenuPosFade += 40;
        mainMenuPos.DOAnchorPosX(mainMenuPosFade, tweenTime);
        mainMenuAct.DOFade(1, tweenTime);
        mainMenuAct.blocksRaycasts = true;
    }

    private void ShowLvlsMenu()
    {        
        lvlsSectPosFade -= 40;
        lvlsSectPos.DOAnchorPosX(lvlsSectPosFade, tweenTime);
        lvlsMenuAct.DOFade(1, tweenTime);
        lvlsMenuAct.blocksRaycasts = true;
    }

}
