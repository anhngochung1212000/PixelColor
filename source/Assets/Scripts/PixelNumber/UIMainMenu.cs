using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PixelNumberData
{
    public string id;
    public bool isPaited;
}

public class PixelNumberGame
{
    public int hintCount = 3;
    public int bombCount = 3;
    public int paintCount = 1;
    public List<PixelNumberData> pixelNumberDatas = new List<PixelNumberData>();
}

public class UIMainMenu : MonoBehaviour
{
    public static string Key = "UserData";
    public static UIMainMenu Instance;
    PixelNumberGame userDatas = new PixelNumberGame();
    [System.Serializable]
    public class MainMenuPixelArtDic : SerializableDictionary<string, PixelArtSpriteData> { }
    public MainMenuPixelArtDic paramDic = new MainMenuPixelArtDic();

    public GameObject prefabPixelArtItem;
    public RectTransform content;
    List<UIPixelArtItem> uIPixelArtItems = new List<UIPixelArtItem>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(Key))
        {
            userDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<PixelNumberGame>(PlayerPrefs.GetString(Key));
        }
        else
        {
            foreach (var item in paramDic)
            {
                var userData = new PixelNumberData() { id = item.Key };
                userDatas.pixelNumberDatas.Add(userData);
            }
            PlayerPrefs.SetString(Key, Newtonsoft.Json.JsonConvert.SerializeObject(userDatas));
        }

        foreach (var item in userDatas.pixelNumberDatas)
        {
            var pixelOject = Instantiate(prefabPixelArtItem, content);
            var pixelArtItem = pixelOject.GetComponent<UIPixelArtItem>();
            pixelArtItem.id = item.id;
            pixelArtItem.mainMenu = this;
            pixelArtItem.SetImage(item.isPaited ? paramDic[item.id].textureModel : paramDic[item.id].textureModelGray);
            uIPixelArtItems.Add(pixelArtItem);
        }
        OnLoadPixelColorScene(userDatas.pixelNumberDatas[0].id);
    }

    public void LoadData()
    {
        userDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<PixelNumberGame>(PlayerPrefs.GetString(Key));
        for(int i = 0; i< userDatas.pixelNumberDatas.Count; i++)
        {
            var pixelArtItem = uIPixelArtItems[i];
            var item = userDatas.pixelNumberDatas[i];
            pixelArtItem.SetImage(item.isPaited ? paramDic[item.id].textureModel : paramDic[item.id].textureModelGray);
        }
    }


    public void OnLoadPixelColorScene(string id)
    {
        XepHinhSo.id = id;
        XepHinhSo.hasModel3D = paramDic[id].isModel3D;
        XepHinhSo.Instance.LoadData();
        //SceneManager.LoadSceneAsync("XepHinhSo");
    }
}
