using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Model3DRotaion : MonoBehaviour
{
    public Transform model;
    public float sentitive = 0.05f;
    float xAngle = 0;
    float yAngle = 0;
    float xCurAngle = 0;
    float yCurAngle = 0;
    Vector3 startPos;
    [HideInInspector] public bool isRotateByMouse;
    // Start is called before the first frame update
    void Start()
    {
        currentX = xAngle = transform.localEulerAngles.x;
        yAngle = model.localEulerAngles.y;
    }

    float currentX;
    // Update is called once per frame
    void Update()
    {
        if (IsPointerOverUIElement() || XepHinhControl.Instance.isLongClick)
        {
            isRotateByMouse = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isRotateByMouse = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (isRotateByMouse)
            {
                var offset = Input.mousePosition - startPos;

                yAngle = Mathf.Lerp(yAngle, yAngle - offset.x * sentitive, Time.deltaTime);
                currentX = Mathf.Lerp(currentX, currentX - offset.y * sentitive, Time.deltaTime);

                if (Mathf.Abs(currentX) < 100)
                {
                    Quaternion quaternion = Quaternion.Euler(currentX, 0, 0);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, quaternion, Time.deltaTime * 360);
                    xAngle = currentX;
                }
                else
                    currentX = xAngle;


                Quaternion quaternion1 = Quaternion.Euler(0, yAngle, 0);
                model.localRotation = Quaternion.Slerp(model.localRotation, quaternion1, Time.deltaTime * 360);

                //startPos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotateByMouse = false;
        }
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
