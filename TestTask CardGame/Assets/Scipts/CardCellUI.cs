using UnityEngine;
using UnityEngine.UI;
using System;

public class CardCellUI : MonoBehaviour
{
    private Button _cellButton;
    private RectTransform _rectTransform;

    public event Action CellButtonClicked;

    private void OnEnable()
    {
        _cellButton = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
        _cellButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _cellButton.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        CellButtonClicked?.Invoke();
    }

    public void DisplayCard(CardLogic card)
    {
        card.gameObject.SetActive(true);
        card.transform.parent = transform;
        card.transform.position = transform.position;
        card.transform.localScale = Vector3.one;
        var cardRect = card.gameObject.GetComponent<RectTransform>();
        cardRect.sizeDelta = _rectTransform.sizeDelta;
    }
}
