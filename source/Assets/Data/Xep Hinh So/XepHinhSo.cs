using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class XepHinhSo : MonoBehaviour
{
    public Texture2D tex;
    public int col = 30;
    public int row = 30;
    public float alphaValue = 0.1f;
    public Vector2 texSize = new Vector2(512, 512);
    public GameObject obj;
    List<Color> colors = new List<Color>();
    public List<GameObject> listObj = new List<GameObject>();
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        float scale = texSize.x / texSize.y;
        float disX = scale / col;
        float disY = 1f / row;

        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Color tempColor = tex.GetPixel(i, j);

                if (tempColor.CompareTwoColor(Color.white) || tempColor.a < alphaValue)
                    continue;

                GameObject temp = Instantiate(obj);
                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                obj.GetComponent<Renderer>().GetPropertyBlock(materialPropertyBlock);


                materialPropertyBlock.SetColor("_BaseColor", tempColor);

                temp.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);


                if (colors.Count == 0 || !CheckColorAvaible(tempColor))
                {
                    Debug.LogError(tempColor);
                    colors.Add(tempColor);
                }

                Vector3 pos = new Vector3(disX * i, 0, disY * j);

                temp.transform.position = pos;
                temp.transform.localScale = Vector3.one * disY /*new Vector3(disX,0,disY)*/;
                var index = colors.FindIndex(p => p.CompareTwoColor(tempColor));
                temp.GetComponent<Renderer>().material.SetFloat("_CurNum", index+1);
            }
        }
        string a = "";
        //Debug.LogError(Newtonsoft.Json.JsonConvert.SerializeObject(colors));
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
