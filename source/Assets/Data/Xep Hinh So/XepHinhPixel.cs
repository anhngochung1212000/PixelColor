using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Update()
    {

        if (count > 0 && !isUnlock)
        {
            count -= Time.deltaTime;

            if(isMainModel)
                material.SetFloat("_Anim", Mathf.Clamp(count / duration, 0f, 1f));
            else
                material.SetFloat("_Anim", Mathf.Clamp(1f - (count / duration), 0f, 1f));
        }
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

        this.isMainModel = isMainModel;
        count = duration;
        
    }

    public void UnlockPiece()
    {
        if (number != XepHinhSo.Instance.numberSelected)
            return;
        material.SetFloat("_Anim", 1);
        isUnlock = true;
    }

}
