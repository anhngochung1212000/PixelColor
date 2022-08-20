using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


#if UNITY_EDITOR

[CustomEditor(typeof(PlaneXepHinhManager))]
[CanEditMultipleObjects]
public class PlaneXepHinhManagerEditor : Editor
{
    PlaneXepHinhManager planeXepHinh;
    void OnEnable()
    {
        planeXepHinh = target as PlaneXepHinhManager;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Export Mesh"))
        {
            planeXepHinh.ExportMesh();
        }
    }
}
#endif
