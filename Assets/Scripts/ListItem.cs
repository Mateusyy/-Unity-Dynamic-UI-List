using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dateText;

    private TimeSpan creationDate;

    public void Init(Sprite icon, string name, TimeSpan creationDate)
    {
        iconImage.sprite = icon;
        FitAsspectRatio(icon);

        nameText.text = $"<b>Name:</b> {name}";
        this.creationDate = creationDate;
    }

    private void Update()
    {
        dateText.text = $"<b>Time from creation:</b> {CalculateTimeSinceFileCreation(creationDate).Format()}";
    }

    private TimeSpan CalculateTimeSinceFileCreation(TimeSpan creationDate)
    {
        return TimeSpan.FromTicks(DateTime.Now.Ticks) - creationDate;
    }

    private void FitAsspectRatio(Sprite icon)
    {
        AspectRatioFitter aspectRatioFitter = iconImage.GetComponent<AspectRatioFitter>();
        aspectRatioFitter.aspectRatio = icon.rect.height > icon.rect.width ? icon.rect.width / icon.rect.height : icon.rect.width / icon.rect.height;
    }
}
