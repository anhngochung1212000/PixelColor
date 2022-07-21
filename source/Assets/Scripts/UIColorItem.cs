using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIColorItem : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] TMP_Text textNumber;
    [SerializeField] RectTransform selected;
    [SerializeField] Button buttonClick;

    [HideInInspector] public UIPixelColor parent;

    int number;
    void Awake()
    {
        buttonClick.onClick.AddListener(OnColorItemClicked);
    }

    void OnDestroy()
    {
        buttonClick.onClick.RemoveListener(OnColorItemClicked);
    }

    public void SetNumberText(int number)
    {
        this.number = number;
        textNumber.text = number.ToString();
    }

    public void SetBackGroundColor(Color color)
    {
        background.color = color;
        if(color.r < 0.5f && color.g < 0.5f && color.b < 0.5f)
        {
            textNumber.color = Color.white;
        }
    }

    public void Selected(bool isSelected)
    {
        selected.gameObject.SetActive(isSelected);
    }

    void OnColorItemClicked()
    {
        parent.colorItemSelected.Selected(false);
        Selected(true);
        parent.colorItemSelected = this;

        XepHinhSo.Instance.SetPieceArrayColor(number, Color.gray);
    }
}
