using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XepHinhPixel : MonoBehaviour
{
    public bool isTurn = false;
    float count = 0;
    float duration = 0.25f;
    Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;

    }

    void Update()
    {
        //if (count > 0)
        //{
        //    count -= Time.deltaTime;

        //    material.SetFloat("_Anim", Mathf.Clamp(1f - (count / duration), 0f, 1f));
        //}
    }

    public void SetPieceNumberBGColor(Color color)
    {
        material.SetColor("_BackgroundColor", color);
    }

    public void Play()
    {
        if (!isTurn)
        {
            count = duration;
            isTurn = true;
        }
    }

}
