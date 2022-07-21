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


    Camera mainCamera;
    public float zoomSpeed;
    void Awake()
    {
        XepHinhSo.onLoadUIColorItem += OnLoadUIColorItem;
        mainCamera = Camera.main;
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
    }
    float delta = 0.5f;
    float max = 1.5f;
    float min = 0.2f;

    public void ZoomInCamera()
    {
        if (mainCamera.orthographicSize - delta <= min)
        {
            mainCamera.DOOrthoSize(min, 0.25f);
            return;
        }

        mainCamera.DOOrthoSize(mainCamera.orthographicSize - delta, 0.25f);
    }

    public void ZoomOutCamera()
    {
        if (mainCamera.orthographicSize + delta >= max)
        {
            mainCamera.DOOrthoSize(max, 0.25f);
            return;
        }
           
        mainCamera.DOOrthoSize(mainCamera.orthographicSize + delta, 0.25f);
    }
}
