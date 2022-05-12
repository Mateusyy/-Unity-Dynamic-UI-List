using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIMainView : MonoBehaviour
{
    [SerializeField] private ImageLoaderSO imageLoader;
    [SerializeField] private ListItemsPoolSO listItemsPooler;
    [SerializeField] private Transform uiListContent;
    [SerializeField] private TextMeshProUGUI infoText;


    private void OnEnable()
    {
        imageLoader.OnFetchDataStart += ClearUIList;
        imageLoader.OnFetchDataEnd += FillUIList;
    }

    private void OnDisable()
    {
        imageLoader.OnFetchDataStart -= ClearUIList;
        imageLoader.OnFetchDataEnd -= FillUIList;
    }

    private void Start()
    {
        InvokeRepeating("UpdateInfoText", 0f, 1.0f);
        UpdateContentOfUIList();
    }

    public void RefreshContentButton_Action()
    {
        UpdateContentOfUIList();
    }

    private void UpdateContentOfUIList()
    {
        if (imageLoader.fetchingDataCoroutine != null)
            StopCoroutine(imageLoader.fetchingDataCoroutine);
        imageLoader.fetchingDataCoroutine = StartCoroutine(imageLoader.GetImagesData());
    }


    int num = 0;
    private void UpdateInfoText()
    {
        if (imageLoader.fetching)
        {
            num++;
            infoText.text = $"Fetching data{string.Concat(Enumerable.Repeat(".", (num % 3) + 1))}";
        }
        else
        {
            num = 0;
            infoText.text = imageLoader.imagesData.Count > 0 ? string.Empty : "No data to display";
        }
    }

    private void FillUIList()
    {
        foreach (var item in imageLoader.imagesData)
        {
            AddNewUIListItem(item.image, item.name, item.creationDate);
        }
    }

    private void AddNewUIListItem(Sprite image, string name, TimeSpan creationDate)
    {
        ListItem listItem = listItemsPooler.Spawn(uiListContent);
        listItem.Init(image, name, creationDate);
    }

    private void ClearUIList()
    {
        listItemsPooler.Despawn();
    }
}
