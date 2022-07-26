using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class XepHinhPixel : MonoBehaviour
{
    public bool isUnlock = false;
    float count = 0;
    float duration = 0.25f;
    Material material;
    bool isMainModel;
    
    [HideInInspector]public int number;
    void Awake()
    {
        material = GetComponent<Renderer>().material;

    }

    void OnEnable()
    {
        isUnlock = false;
    }

    public void SetPieceNumberBGColor(Color color)
    {
        material.SetColor("_BackgroundColor", color);
    }

    public void AnimationAnim(bool isMainModel)
    {
        if (isMainModel && material.GetFloat("_Anim") == 0)
            return;
        if (!isMainModel && material.GetFloat("_Anim") == 1)
            return;

        if (isUnlock)
            return;

        this.isMainModel = isMainModel;

        if (isMainModel)
            material.DOFloat(0, "_Anim",0.5f);
        else
            material.DOFloat(1, "_Anim",0.5f);
    }

    public void UnlockPiece(Color color)
    {
        if (number != XepHinhSo.Instance.numberSelected)
            return;

        if(material.GetFloat("_Anim") == 1)
            material.SetColor("_BaseColor", color);

        material.SetFloat("_Anim", 1);
       
        isUnlock = true;
        XepHinhSo.onUnlockPiece?.Invoke(number);
    }

}
