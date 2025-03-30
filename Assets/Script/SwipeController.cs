using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SwipeController : MonoBehaviour
{
    [SerializeField] private GameObject lvlPages;
    [SerializeField] private Vector3 pageStep;
    [SerializeField] private RectTransform lvlPagesRect;
    [SerializeField] private float tweenTime;
    [SerializeField] private Ease tweenType;
    [SerializeField] private CanvasGroup lvlPagesInteract;
    private int currentPage;
    private Vector3 targetPos;
    private int maxPage;

    private void Awake()
    {
        currentPage = 1;
        maxPage = lvlPages.transform.childCount;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    private async void MovePage()
    {
        lvlPagesRect.DOAnchorPos(targetPos, tweenTime).SetEase(tweenType);
        await lvlPagesInteract.DOFade(0, tweenTime / 2).AsyncWaitForCompletion();
        lvlPagesInteract.DOFade(1, tweenTime);
        
    }
}
