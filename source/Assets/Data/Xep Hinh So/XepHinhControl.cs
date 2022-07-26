using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class XepHinhControl : MonoBehaviour
{
    public static XepHinhControl Instance;
    bool isMouseDown;
    public bool isLongClick;

    public float bombRadius=0.05f;
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPointerOverUIElement())
            return;

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

            if (!Physics.Raycast(ray, out hit))
                return;

            if (!UIPixelColor.Instance.hasBomb)
                hit.transform.gameObject.GetComponent<XepHinhPixel>().UnlockPiece();
            else
            {
                var colliders = Physics.OverlapSphere(hit.transform.gameObject.transform.position, bombRadius);
                foreach (var collider in colliders)
                {
                    var pixel = collider.gameObject.GetComponent<XepHinhPixel>();
                    if (pixel == null)
                        continue;
                    pixel.UnlockPiece();
                }
                UIPixelColor.Instance.UnSelectedBombUI();
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

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
