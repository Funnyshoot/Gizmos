using UnityEngine;
using UnityEditor;

public class GizmoEditor : EditorWindow
{
    [MenuItem("Window/Custom/Show Gizmos in Scene")]
    public static void ShowWindow()
    {
        GetWindow<GizmoEditor>("Gizmo Editor");
    }
}
