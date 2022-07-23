using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPixelArtItem : MonoBehaviour
{
    [SerializeField] Image imageModel;
    [SerializeField] Button buttonClick;
    [HideInInspector]public UIMainMenu mainMenu;

    public string id;

    void Awake()
    {
        buttonClick.onClick.AddListener(OnButtonClicked);
    }

    void OnDestroy()
    {
        buttonClick.onClick.RemoveListener(OnButtonClicked);
    }

    public void SetImage(Sprite image)
    {
        imageModel.sprite = image;
    }
    
    void OnButtonClicked()
    {
        mainMenu.OnLoadPixelColorScene(id);
    }
}
