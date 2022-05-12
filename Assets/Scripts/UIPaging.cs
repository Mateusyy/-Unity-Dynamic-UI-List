using TMPro;
using UnityEngine;

public class UIPaging : MonoBehaviour
{
    [SerializeField] private ImageLoaderSO imageLoader;
    [SerializeField] private TextMeshProUGUI pageNumberText;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        ShowOrHideContent();
    }

    private void Update()
    {
        if (!imageLoader.fetching)
            pageNumberText.text = $"{page}/{imageLoader.PagesNumber}";
    }

    public void ShowOrHideContent()
    {
        if (imageLoader.paging) 
            Show();
        else 
            Hide();
    }

    int page = 1;
    public void NextPageButton_Action()
    {
        if (page < imageLoader.PagesNumber)
        {
            page++;

            if (imageLoader.fetchingDataCoroutine != null)
                StopCoroutine(imageLoader.fetchingDataCoroutine);
            imageLoader.fetchingDataCoroutine = StartCoroutine(imageLoader.GetImagesData(page - 1));
        }
    }

    public void PreviousPageButton_Action()
    {
        if (page > 1)
        {
            page--;

            if (imageLoader.fetchingDataCoroutine != null)
                StopCoroutine(imageLoader.fetchingDataCoroutine);
            imageLoader.fetchingDataCoroutine = StartCoroutine(imageLoader.GetImagesData(page - 1));
        }
    }

    private void Show()
    {
        if (!canvasGroup) return;

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        if (!canvasGroup) return;

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
