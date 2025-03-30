using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private RectTransform PauseButs;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private float TweenTime;
    private bool pauseOn = false;

    private CanvasGroup PauseButsCanvas;

    private void Awake()
    {
        PauseButsCanvas = PauseButs.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseOn)
        {
            PauseIntro();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseOn)
        {
            PauseOutro();
        }
    }
    public void Pause()
    {
        PauseIntro();
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        PauseOutro();
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ButtonHover()
    {

    }

    private void PauseIntro()
    {
        PauseButsCanvas.alpha = 1.0f;
        PauseButsCanvas.blocksRaycasts = true;
        PausePanel.SetActive(true);
        PausePanel.GetComponent<Image>().DOFade(0.8f, TweenTime).SetUpdate(true);
        pauseOn = true;
    }

    private async void PauseOutro()
    {
        PauseButsCanvas.alpha = 0f;
        PauseButsCanvas.blocksRaycasts = false;
        await PausePanel.GetComponent<Image>().DOFade(0, TweenTime).SetUpdate(true).AsyncWaitForCompletion();
        PausePanel.SetActive(false);
        pauseOn = false;
    }
}