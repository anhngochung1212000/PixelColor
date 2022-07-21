using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class XepHinhSo : MonoBehaviour
{
    public static XepHinhSo Instance;
    public static Action<List<Color>> onLoadUIColorItem;

    [SerializeField] Texture2D textureModel;
    [SerializeField] Texture2D textureModelGray;
    [SerializeField] float alphaValue = 0.1f;
    [SerializeField] GameObject obj;
    [SerializeField] Dictionary<int, List<XepHinhPixel>> pieceDic = new Dictionary<int, List<XepHinhPixel>>();

    int col = 30;
    int row = 30;
    Vector2 textureSize = new Vector2(512, 512);
    List<Color> colors = new List<Color>();
    string colorDefaultStr = "#A1A1A1";
    Material tempMat;
    int numberSelected = 0;
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

    void GeneratePiecePixel(bool isMainModel)
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
                tempMat = temp.GetComponent<Renderer>().material;
                tempMat.SetColor("_BaseColor", tempColor);
                tempMat.SetFloat("_Anim", isMainModel ? 0 : 1);
                index++;
            }
        }
    }


    void Update()
    {
        if (mainCamera.orthographicSize >= 0.7f)
            GeneratePiecePixel(false);
        else
            GeneratePiecePixel(true);
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

}
