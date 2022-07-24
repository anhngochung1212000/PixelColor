using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class XepHinhSo3D : MonoBehaviour
{
    public static Dictionary<int, List<XepHinhPixel>> pieceDic = new Dictionary<int, List<XepHinhPixel>>();
    List<Color> pieces = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        var pixelArray = GetComponentsInChildren<XepHinhPixel>();
        for (int i = 0; i < pixelArray.Length; i++)
        {
            var temp = pixelArray[i];
            var tempMat = temp.GetComponent<Renderer>().material;
            var color = tempMat.GetColor("_BaseColor");
            var index = pieces.FindIndex(p => p.CompareTwoColor(color));
            if(index == -1)
            {
                pieces.Add(color);
                var list = new List<XepHinhPixel>();
                list.Add(temp);
                pieceDic.Add(pieceDic.Count, list);
            }
            else
            {
                var list = pieceDic[index];
                list.Add(temp);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
