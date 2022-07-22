using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XepHinhControl : MonoBehaviour
{
    public static XepHinhControl Instance;
    bool isMouseDown;
    public bool isLongClick;
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            timer = 0;
            isMouseDown = true;
        }
        isLongClick = IsLongClick();
        if (isLongClick || isMouseDown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Select stage    
                hit.transform.gameObject.GetComponent<XepHinhPixel>().UnlockPiece();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
    }

    float timer;
    bool IsLongClick()
    {
       
        if(isMouseDown && IsUnLockPiece())
        {
            timer += Time.deltaTime;
            if (timer > 0.3f)
                return true;
        }
       
        return false;
    }

    bool IsUnLockPiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Select stage    
            var xepHinhPixel = hit.transform.gameObject.GetComponent<XepHinhPixel>();
            if (xepHinhPixel != null && xepHinhPixel.number == XepHinhSo.Instance.numberSelected)
                return true;
        }
        return false;
    }
}
