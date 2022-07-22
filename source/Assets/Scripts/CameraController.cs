using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    Vector3 rootPoint;
    public static CameraController Instance;

    [HideInInspector]public bool hasMainModel;

    float delta = 0.25f;
    float max = 1.5f;
    float min = 0.2f;

    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    float z = 0.0f;

    bool isMouseDown;
    void Awake()
    {
        mainCamera = Camera.main;
        Instance = this;
        rootPoint = transform.position;
    }

    void Update()
    {
        if (IsPointerOverUIElement() || XepHinhControl.Instance.isLongClick)
        {
            isMouseDown = false;
            return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            hit_position = Input.mousePosition;
            camera_position = transform.position;
            isMouseDown = true;
        }

        if (isMouseDown && Input.GetMouseButton(0))
        {
            current_position = Input.mousePosition;
            if (Vector3.Distance(current_position, hit_position) > 30f)
                LeftMouseDrag();
        }

        if (Input.GetMouseButtonUp(0))
            isMouseDown = false;
    }

   

    public void ZoomInCamera()
    {
        if (mainCamera.orthographicSize - delta <= min)
        {
            mainCamera.DOOrthoSize(min, 0.25f).OnComplete(() => OnZoomComplete());
            return;
        }

        mainCamera.DOOrthoSize(mainCamera.orthographicSize - delta, 0.25f).OnComplete(() => OnZoomComplete()); ;
    }

    public void OnZoomComplete()
    {
        bool hasZoomType = mainCamera.orthographicSize >= 0.7f;
        if (hasZoomType && !hasMainModel)
            return;
        if (!hasZoomType && hasMainModel)
            return;

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Process(hasZoomType));
    }

    Coroutine coroutine;
    IEnumerator Process(bool hasZoomType)
    {
        if (hasZoomType)
        {
            hasMainModel = false;
            XepHinhSo.Instance.GenerateColorOfPiece(hasMainModel);
            XepHinhSo.Instance.AnimationBlockPiece(hasMainModel);

        }
        else
        {
            hasMainModel = true;
            XepHinhSo.Instance.AnimationBlockPiece(hasMainModel);
            yield return new WaitForSeconds(0.5f);
            XepHinhSo.Instance.GenerateColorOfPiece(hasMainModel);
        }
    }

    public void ZoomOutCamera()
    {
        if (mainCamera.orthographicSize + delta >= max)
        {
            mainCamera.DOOrthoSize(max, 0.25f).OnComplete(() => OnZoomComplete()); ;
            return;
        }

        mainCamera.DOOrthoSize(mainCamera.orthographicSize + delta, 0.25f).OnComplete(() => OnZoomComplete()); ;
    }

    void LeftMouseDrag()
    {
        // From the Unity3D docs: "The z position is in world units from the camera."  In my case I'm using the y-axis as height
        // with my camera facing back down the y-axis.  You can ignore this when the camera is orthograhic.
        current_position.z = hit_position.z = camera_position.y;

        // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(current_position-hit_position)
        // anyways.  
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        // Invert direction to that terrain appears to move with the mouse.
        direction = direction * -1;

        Vector3 position = camera_position + direction;

        transform.position = position;
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

    public void BackToRootPoint()
    {
        transform.DOMove(rootPoint, 0.5f);
        mainCamera.DOOrthoSize(max, 0.5f).OnComplete(() => OnZoomComplete()); ;
    }

    public void MoveToPosition(Vector3 pos)
    {
        transform.DOMove(pos, 0.5f);
        mainCamera.DOOrthoSize(min, 0.5f).OnComplete(() => OnZoomComplete()); ;
    }
}
