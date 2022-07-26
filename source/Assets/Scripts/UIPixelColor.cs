using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIPixelColor : MonoBehaviour
{
    public static UIPixelColor Instance;
   
    [SerializeField] GameObject prefabColorItem;
    [SerializeField] ParticleSystem partical;
    [SerializeField] RectTransform content;
    [SerializeField] Sprite[] bombSprites;
    [SerializeField] Image imageBomb;
    Dictionary<int, UIColorItem> colorItemDic = new Dictionary<int, UIColorItem>();
    [HideInInspector] public UIColorItem colorItemSelected;
    [HideInInspector] public bool hasBomb;
    void Awake()
    {
        Instance = this;
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

        if(CameraController.Instance != null)
        {
            CameraController.Instance.hasMainModel = true;
            CameraController.Instance.OnZoomComplete();
        }
        
    }

    public void ZoomInCamera()
    {
        if (CameraController.Instance != null)
            CameraController.Instance.ZoomInCamera();
        if (CameraController3D.Instance != null)
            CameraController3D.Instance.ZoomInCamera();
    }

    public void ZoomOutCamera()
    {
        if (CameraController.Instance != null)
            CameraController.Instance.ZoomOutCamera();
        if (CameraController3D.Instance != null)
            CameraController3D.Instance.ZoomOutCamera();
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
            if (CameraController.Instance != null)
                CameraController.Instance.BackToRootPoint();
            if (CameraController3D.Instance != null)
                CameraController3D.Instance.BackToRootPoint();
           
            partical.Play();
            gameObject.SetActive(false);
        }

    }

    public void OnPaintButtonClicked()
    {
        UnSelectedBombUI();
        XepHinhSo.Instance.PaintPieces();
    }

    public void OnBackButtonClicked()
    {
        if (CameraController.Instance != null)
            CameraController.Instance.BackToRootPoint();
        if (CameraController3D.Instance != null)
            CameraController3D.Instance.BackToRootPoint();
    }

    public void OnHintButtonClicked()
    {
        XepHinhSo.Instance.Hint();
    }

    public void OnButtonBackMainMenuClicked()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void OnButtonBombOnClick()
    {
        SelectedBombUI();
    }

    public void UnSelectedBombUI()
    {
        imageBomb.sprite = bombSprites[0];
        hasBomb = false;
    }

    void SelectedBombUI()
    {
        hasBomb = true;
        imageBomb.sprite = bombSprites[1];
    }

}
