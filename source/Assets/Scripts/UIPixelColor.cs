using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;
using UIAnimatorCore;

public class UIPixelColor : MonoBehaviour
{
    public static UIPixelColor Instance;
    [SerializeField] UIAnimator uIAnimator;
    [SerializeField] GameObject prefabColorItem;
    [SerializeField] ParticleSystem partical;
    [SerializeField] RectTransform content;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Sprite[] bombSprites;
    [SerializeField] Image imageBomb;
    [SerializeField] TMPro.TMP_Text textBombCount;
    [SerializeField] TMPro.TMP_Text textHintCount;
    [SerializeField] TMPro.TMP_Text textPaintCount;
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
        content.anchoredPosition = new Vector2(0, content.anchoredPosition.y);
        partical.gameObject.SetActive(false);
        uIAnimator.PlayAnimation(AnimSetupType.Intro);
        foreach (var item in colorItemDic)
        {
            Destroy(item.Value.gameObject);
        }
        colorItemDic.Clear();
        for (int i = 0; i < colors.Count; i++)
        {
            var gameObject = Instantiate(prefabColorItem, content);
            UIColorItem item = gameObject.GetComponent<UIColorItem>();
            item.parent = this;
            colorItemDic.Add(i, item);

            item.SetBackGroundColor(colors[i]);
            item.SetNumberText(i);
        }

        colorItemSelected = colorItemDic[0];
        colorItemSelected.Selected(true);

        if(CameraController.Instance != null && CameraController.Instance.gameObject.activeSelf)
        {
            CameraController.Instance.hasMainModel = true;
            CameraController.Instance.OnZoomComplete();
        }

        var userDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<PixelNumberGame>(PlayerPrefs.GetString(UIMainMenu.Key));
        textBombCount.text = userDatas.bombCount.ToString();
        textHintCount.text = userDatas.hintCount.ToString();
        textPaintCount.text = userDatas.paintCount.ToString();

    }

    public void ZoomInCamera()
    {
        if (CameraController.Instance != null && CameraController.Instance.gameObject.activeSelf)
            CameraController.Instance.ZoomInCamera();
        if (CameraController3D.Instance != null && CameraController3D.Instance.gameObject.activeSelf)
            CameraController3D.Instance.ZoomInCamera();

        UIMainMenu.Instance.gameObject.SetActive(false);
    }

    public void ZoomOutCamera()
    {
        if (CameraController.Instance != null && CameraController.Instance.gameObject.activeSelf)
            CameraController.Instance.ZoomOutCamera();
        if (CameraController3D.Instance != null && CameraController3D.Instance.gameObject.activeSelf)
            CameraController3D.Instance.ZoomOutCamera();
    }

    void OnUnlockPiece(int number)
    {
        var count = XepHinhSo.pieceDic[number].Count(p => p.isUnlock);
        colorItemDic[number].SetCountNumber(count);
        CheckAllColorPainted();
    }

    public void NextColorSelected()
    {
        var index = colorItemDic.Values.ToList().FindIndex(p => p == colorItemSelected);
        UIColorItem item = null;

        int space = 1;
        bool flag = false;

        while(index != -1 && !flag)
        {
           
            if ((index + space) >= colorItemDic.Values.Count)
                break;

            item = colorItemDic.Values.ElementAt(index + space);
            if(!item.isPainted)
            {
                flag = true;
                item.OnColorItemClicked();
            }
            space++;
        }

        if(index == -1 || !flag)
        {
            item = colorItemDic.Values.FirstOrDefault(p => !p.isPainted);
            if (item != null)
            {
                item.OnColorItemClicked();
            }
        }
       

        if (item != null)
            SnapTo(item.transform);
    }

    public void SnapTo(Transform target)
    {
        Canvas.ForceUpdateCanvases();
        var position = (Vector2)scrollRect.transform.InverseTransformPoint(content.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position+new Vector3(-150,0,0));
        //position.x += 600f;

        if (Mathf.Abs(position.x) < content.sizeDelta.x)
            content.DOAnchorPos(new Vector2(position.x, content.anchoredPosition.y),0.5f);
        else
            content.DOAnchorPos(new Vector2(-content.sizeDelta.x, content.anchoredPosition.y), 0.5f);

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
            if (CameraController.Instance != null && CameraController.Instance.gameObject.activeSelf)
                CameraController.Instance.BackToRootPoint();
            if (CameraController3D.Instance != null && CameraController3D.Instance.gameObject.activeSelf)
                CameraController3D.Instance.BackToRootPoint();

            partical.gameObject.SetActive(true);
            UIMainMenu.Instance.LoadData();
            uIAnimator.PlayAnimation(AnimSetupType.Outro);
        }

    }

    public void OnPaintButtonClicked()
    {
        UnSelectedBombUI();
        XepHinhSo.Instance.PaintPieces();
    }

    public void OnBackButtonClicked()
    {
        if (CameraController.Instance != null && CameraController.Instance.gameObject.activeSelf)
            CameraController.Instance.BackToRootPoint();
        if (CameraController3D.Instance != null && CameraController3D.Instance.gameObject.activeSelf)
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
