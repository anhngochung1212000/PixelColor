using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;

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
        XepHinhSo.onUnlockPiece += OnUnlockPiece;
    }

    void OnDestroy()
    {
        XepHinhSo.onLoadUIColorItem -= OnLoadUIColorItem;
        XepHinhSo.onUnlockPiece -= OnUnlockPiece;
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

    void OnUnlockPiece(int number)
    {
        var count = XepHinhSo.pieceDic[number].Count(p => p.isUnlock);
        colorItemDic[number].SetCountNumber(count);
        CheckAllColorPainted();
    }

    void CheckAllColorPainted()
    {
        bool isFinish = true;
        foreach (var item in colorItemDic)
        {
            if (item.Value.isPainted)
                continue;

            isFinish = false;
            break;
        }

        if(isFinish)
        {
            XepHinhSo.Instance.SaveData();
            CameraController.Instance.BackToRootPoint();
            gameObject.SetActive(false);
        }

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

    public void OnButtonBackMainMenuClicked()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
