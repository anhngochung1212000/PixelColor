using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[System.Serializable]
public class PixelArtTextureData
{
    public Texture2D textureModel;
    public Texture2D textureModelGray;
}

[System.Serializable]
public class PixelArtSpriteData
{
    public Sprite textureModel;
    public Sprite textureModelGray;
}

public class XepHinhSo : MonoBehaviour
{
    public static XepHinhSo Instance;
    public static Action<List<Color>> onLoadUIColorItem;
    public static string id;
    [System.Serializable]
    public class GroupPixelArtDic : SerializableDictionary<string, PixelArtTextureData> { }
    public GroupPixelArtDic paramDic = new GroupPixelArtDic();


    Texture2D textureModel;
    Texture2D textureModelGray;
    [SerializeField] float alphaValue = 0.1f;
    [SerializeField] GameObject obj;
    public static Dictionary<int, List<XepHinhPixel>> pieceDic = new Dictionary<int, List<XepHinhPixel>>();
    public static Action<int> onUnlockPiece;
    int col = 30;
    int row = 30;
    Vector2 textureSize = new Vector2(512, 512);
    List<Color> colors = new List<Color>();
    string colorDefaultStr = "#A1A1A1";
    Material tempMat;
    [HideInInspector]public int numberSelected = 0;
    bool isGenerate;
    Camera mainCamera;
    List<GameObject> pieces = new List<GameObject>();

    float scale = 0;
    float disX, disY;

    void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (string.IsNullOrEmpty(id))
            id = paramDic.Keys.ToList()[0];

        textureModel = paramDic[id].textureModel;
        textureModelGray = paramDic[id].textureModelGray;

        textureSize.x = col = textureModel.width;
        textureSize.y = row = textureModel.height;

        scale = textureSize.x / textureSize.y;
        disX = scale / col;
        disY = 1f / row;

        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Color tempColor = textureModel.GetPixel(i, j);

                if (tempColor.CompareTwoColor(Color.white) || tempColor.a < alphaValue)
                    continue;

                GameObject temp = Instantiate(obj);
                var piecePixel = temp.GetComponent<XepHinhPixel>();

                tempMat = temp.GetComponent<Renderer>().material;

                tempMat.SetColor("_BaseColor", tempColor);



                if (colors.Count == 0 || !CheckColorAvaible(tempColor))
                {
                    //Debug.LogError(tempColor);
                    colors.Add(tempColor);
                }

                Vector3 pos = new Vector3(disX * i, 0, disY * j);

                temp.transform.position = pos;
                temp.transform.localScale = Vector3.one * disY;

                var number = colors.FindIndex(p => p.CompareTwoColor(tempColor));
                piecePixel.number = number;

                List<XepHinhPixel> xepHinhPixels;
                if (pieceDic.ContainsKey(number))
                    xepHinhPixels = pieceDic[number];
                else
                {
                    xepHinhPixels = new List<XepHinhPixel>();
                    pieceDic[number] = xepHinhPixels;
                }
                xepHinhPixels.Add(piecePixel);


                tempMat.SetFloat("_CurNum", number);
                tempMat.SetFloat("_Anim", 1);
                temp.GetComponent<Renderer>().material = tempMat;
                pieces.Add(temp);

            }
        }

        SetPieceArrayColor(numberSelected, Color.gray);
        onLoadUIColorItem?.Invoke(colors);
        isGenerate = true;

    }

    public void GenerateColorOfPiece(bool isMainModel)
    {
        var texture2D = isMainModel ? textureModel : textureModelGray;
        int index = 0;
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Color tempColor = texture2D.GetPixel(i, j);

                if (tempColor.CompareTwoColor(Color.white) || tempColor.a < alphaValue)
                    continue;

               

                if(index >= pieces.Count)
                {
                    Debug.LogError("index out of range!");
                    return;
                }

                GameObject temp = pieces[index];
                index++;

                if (!isMainModel && temp.GetComponent<XepHinhPixel>().isUnlock)
                    continue;

                tempMat = temp.GetComponent<Renderer>().material;
                tempMat.SetColor("_BaseColor", tempColor);
               
            }
        }
    }

    public void AnimationBlockPiece(bool isMainModel)
    {
        var texture2D = isMainModel ? textureModel : textureModelGray;
        int index = 0;
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Color tempColor = texture2D.GetPixel(i, j);

                if (tempColor.CompareTwoColor(Color.white) || tempColor.a < alphaValue)
                    continue;

                if (index >= pieces.Count)
                {
                    Debug.LogError("index out of range!");
                    return;
                }
                GameObject temp = pieces[index];
                temp.GetComponent<XepHinhPixel>().AnimationAnim(isMainModel);
                index++;
            }
        }
    }


    void Update()
    {
       
    }

    public void SetPieceArrayColor(int number , Color color)
    {
        if(!pieceDic.ContainsKey(number))
        {
            Debug.LogError("Dont have number!");
            return;
        }

        foreach (var pixel in pieceDic[numberSelected])
        {
            Color colorDefault;
            ColorUtility.TryParseHtmlString(colorDefaultStr, out colorDefault);
            pixel.SetPieceNumberBGColor(colorDefault);
        }


        foreach (var pixel in pieceDic[number])
        {
            pixel.SetPieceNumberBGColor(color);
        }

        numberSelected = number;
    }

    bool CheckColorAvaible(Color color)
    {
        foreach (var item in colors)
        {
            if (item.CompareTwoColor(color))
                return true;
        }
        return false;
    }

    public void PaintPieces()
    {
        foreach (var pixel in pieceDic[numberSelected])
        {
            pixel.UnlockPiece();
        }
    }

    public void SaveData()
    {
        var userDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserData>>(PlayerPrefs.GetString(UIMainMenu.Key));
        var data = userDatas.FirstOrDefault(p => p.id == id);
        data.isPaited = true;
        PlayerPrefs.SetString(UIMainMenu.Key, Newtonsoft.Json.JsonConvert.SerializeObject(userDatas));
    }

    public void Hint()
    {
        var pieces = pieceDic[numberSelected];
        if (pieces.Count <= 0)
            return;

        var target = pieces.FirstOrDefault(p => !p.isUnlock);
        if (target == null)
            return;

        var pos = new Vector3(target.transform.position.x, mainCamera.transform.position.y, target.transform.position.z);
        CameraController.Instance.MoveToPosition(pos);
    }

}
