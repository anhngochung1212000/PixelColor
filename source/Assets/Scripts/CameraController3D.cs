using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController3D : MonoBehaviour
{
    public static CameraController3D Instance;

    Camera mainCamera;
    [HideInInspector] public bool hasMainModel;

    float delta = 20f;
    float max = 90f;
    float min = 50f;
    void Awake()
    {
        mainCamera = Camera.main;
        Instance = this;
    }

    //public void OnZoomComplete()
    //{
    //    bool hasZoomType = mainCamera.orthographicSize >= 0.7f;
    //    if (hasZoomType && !hasMainModel)
    //        return;
    //    if (!hasZoomType && hasMainModel)
    //        return;

    //    if (coroutine != null)
    //        StopCoroutine(coroutine);
    //    coroutine = StartCoroutine(Process(hasZoomType));
    //}

    //Coroutine coroutine;
    //IEnumerator Process(bool hasZoomType)
    //{
    //    if (hasZoomType)
    //    {
    //        hasMainModel = false;
    //        XepHinhSo.Instance.GenerateColorOfPiece(hasMainModel);
    //        XepHinhSo.Instance.AnimationBlockPiece(hasMainModel);

    //    }
    //    else
    //    {
    //        hasMainModel = true;
    //        XepHinhSo.Instance.AnimationBlockPiece(hasMainModel);
    //        yield return new WaitForSeconds(0.5f);
    //        XepHinhSo.Instance.GenerateColorOfPiece(hasMainModel);
    //    }
    //}

    public void ZoomOutCamera()
    {
        if (mainCamera.fieldOfView + delta >= max)
        {
            mainCamera.DOFieldOfView(max, 0.25f)/*.OnComplete(() => OnZoomComplete());*/ ;
            return;
        }

        mainCamera.DOFieldOfView(mainCamera.fieldOfView + delta, 0.25f)/*.OnComplete(() => OnZoomComplete());*/ ;
    }

    public void ZoomInCamera()
    {
        if (mainCamera.fieldOfView - delta <= min)
        {
            mainCamera.DOFieldOfView(min, 0.25f)/*.OnComplete(() => OnZoomComplete())*/;
            return;
        }

        mainCamera.DOFieldOfView(mainCamera.fieldOfView - delta, 0.25f)/*.OnComplete(() => OnZoomComplete());*/ ;
    }

    public void BackToRootPoint()
    {
        mainCamera.DOFieldOfView(max, 0.5f)/*.OnComplete(() => OnZoomComplete()); */;
    }
}
