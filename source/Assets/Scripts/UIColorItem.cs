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
    [SerializeField] Image imageCount;
    [SerializeField] Image imageCheck;
    [HideInInspector] public UIPixelColor parent;
    [HideInInspector] public bool isPainted;
    [HideInInspector] public Color color;
    int number;
    int totalCount;


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
        totalCount = XepHinhSo.pieceDic[number].Count;
    }

    public void SetCountNumber(float count)
    {
        imageCount.fillAmount = count * 1.0f / totalCount;
        if(imageCount.fillAmount == 1)
        {
            textNumber.gameObject.SetActive(false);
            selected.gameObject.SetActive(false);
            imageCount.gameObject.SetActive(false);
            imageCheck.gameObject.SetActive(true);
            isPainted = true;
        }
    }

    public void SetBackGroundColor(Color color)
    {
        this.color = color;
        background.color = color;
        imageCount.color = color;
        if (color.r < 0.5f && color.g < 0.5f && color.b < 0.5f)
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
