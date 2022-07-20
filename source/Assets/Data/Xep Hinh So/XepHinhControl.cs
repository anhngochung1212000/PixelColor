using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XepHinhControl : MonoBehaviour
{
    bool isMouseDown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }


        if (isMouseDown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Select stage    
                hit.transform.gameObject.GetComponent<XepHinhPixel>().Play();
            }
        }
    }
}
