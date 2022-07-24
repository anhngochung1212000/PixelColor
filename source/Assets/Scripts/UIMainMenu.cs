using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserData
{
    public string id;
    public bool isPaited;
}

public class UIMainMenu : MonoBehaviour
{
    public static string Key = "UserData";
    List<UserData> userDatas = new List<UserData>();
    [System.Serializable]
    public class MainMenuPixelArtDic : SerializableDictionary<string, PixelArtSpriteData> { }
    public MainMenuPixelArtDic paramDic = new MainMenuPixelArtDic();

    public GameObject prefabPixelArtItem;
    public RectTransform content;


    void Start()
    {
        if (PlayerPrefs.HasKey(Key))
        {
            userDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserData>>(PlayerPrefs.GetString(Key));
        }
        else
        {
            foreach (var item in paramDic)
            {
                var userData = new UserData() { id = item.Key };
                userDatas.Add(userData);
            }
            PlayerPrefs.SetString(Key, Newtonsoft.Json.JsonConvert.SerializeObject(userDatas));
        }

        foreach (var item in userDatas)
        {
            var pixelOject = Instantiate(prefabPixelArtItem, content);
            var pixelArtItem = pixelOject.GetComponent<UIPixelArtItem>();
            pixelArtItem.id = item.id;
            pixelArtItem.mainMenu = this;
            pixelArtItem.SetImage(item.isPaited ? paramDic[item.id].textureModel : paramDic[item.id].textureModelGray);
        }
    }


    public void OnLoadPixelColorScene(string id)
    {
        XepHinhSo.id = id;
        XepHinhSo.hasModel3D = paramDic[id].isModel3D;
        SceneManager.LoadSceneAsync("XepHinhSo");
    }
}
