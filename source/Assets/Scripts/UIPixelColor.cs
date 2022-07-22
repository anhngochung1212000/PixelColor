using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIPixelColor : MonoBehaviour
{
    [SerializeField] GameObject prefabColorItem;
    [SerializeField]
    RectTransform content;
    Dictionary<int, UIColorItem> colorItemDic = new Dictionary<int, UIColorItem>();
    [HideInInspector] public UIColorItem colorItemSelected;
    void Awake()
    {
        XepHinhSo.onLoadUIColorItem += OnLoadUIColorItem;
    }

    void OnDestroy()
    {
        XepHinhSo.onLoadUIColorItem -= OnLoadUIColorItem;
    }

    void OnLoadUIColorItem(List<Color> colors)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            UIColorItem item;
            if(colorItemDic.ContainsKey(i))
            {
                item = colorItemDic[i];
            }
            else
            {
                var gameObject = Instantiate(prefabColorItem, content);
                item = gameObject.GetComponent<UIColorItem>();
                item.parent = this;
                colorItemDic.Add(i, item);
            }

            item.SetBackGroundColor(colors[i]);
            item.SetNumberText(i);
        }

        colorItemSelected = colorItemDic[0];
        colorItemSelected.Selected(true);
        CameraController.Instance.hasMainModel = true;
        CameraController.Instance.OnZoomComplete();
    }

    public void ZoomInCamera()
    {
        CameraController.Instance.ZoomInCamera();
    }

    public void ZoomOutCamera()
    {
        CameraController.Instance.ZoomOutCamera();
    }

    public void OnPaintButtonClicked()
    {
        XepHinhSo.Instance.PaintPieces();
    }

    public void OnBackButtonClicked()
    {
        CameraController.Instance.BackToRootPoint();
    }

    public void OnHintButtonClicked()
    {
        XepHinhSo.Instance.Hint();
    }

}
