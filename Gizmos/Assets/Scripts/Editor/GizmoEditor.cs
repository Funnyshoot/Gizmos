using UnityEditor;
using UnityEngine;

using technical.test.editor;

public class GizmoEditor : EditorWindow
{
    SceneGizmoAsset assetLoaded;
    //string[] buttonString = new string[2]{ "edit", "editing" };
    Gizmo[] savedGizmos; // the last position before update.
    Gizmo[] dataGizmos; //the data we load
    Gizmo[] storageGizmos; // where we write
    int editing = -1;

    #region System

    [MenuItem("Window/Custom/Show Gizmos in Scene")]
    public static void ShowWindow()
    {
        GetWindow<GizmoEditor>("Gizmos Editor : ");
    }
    private void OnEnable()
    {
        //dataGizmos = AssetDatabase.LoadAssetAtPath<SceneGizmoAsset>("Assets/Data/Editor/Scene Gizmo Asset.asset")._gizmos;

        assetLoaded = AssetDatabase.LoadAssetAtPath<SceneGizmoAsset>("Assets/Data/Editor/test.asset");
        dataGizmos = assetLoaded._gizmos;
        storageGizmos = new Gizmo[dataGizmos.Length];
        savedGizmos = new Gizmo[dataGizmos.Length];
        dataGizmos.CopyTo(storageGizmos, 0);
    }



    private void OnGUI()
    {
        GUILayout.Label("Gizmos Editor", EditorStyles.boldLabel);
        for (int i = 0; i< dataGizmos.Length; ++i)
        {
            GUILayout.BeginHorizontal();
            storageGizmos[i].Name = EditorGUILayout.TextField("Gizmo: " + dataGizmos[i].Name, dataGizmos[i].Name);
            if (GUILayout.Button("edit"))
            {
                if (editing == i) StopEditingGizmo();
                else
                {
                    StopEditingGizmo();
                    StartEditingGizmo(i);
                }
            }
            storageGizmos[i].Position = EditorGUILayout.Vector3Field("Position", dataGizmos[i].Position);
            GUILayout.Space(50);
            GUILayout.EndHorizontal();
        }
    }

    private void Update()
    {
        if (!(editing < 0)) EditingGizmo(editing);
    }

    #endregion

    #region call
    #endregion

    #region function

    private void StopEditingGizmo()
    {
        editing = -1;
        EditorUtility.SetDirty(assetLoaded);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private void StartEditingGizmo(int gizmoID)
    {
        editing = gizmoID;
        savedGizmos[gizmoID] = dataGizmos[editing];
        EditingGizmo(editing);
    }   
    private void EditingGizmo(int gizmoID)
    {
        Debug.Log("editing : " + editing);
        SetGizmo(gizmoID, storageGizmos[gizmoID]);
    }

    public void ResetGizmo(int gizmoID)
    {
        SetGizmo(gizmoID, savedGizmos[gizmoID]);
    }

    private void SetGizmo(int gizmoID, Gizmo g)
    {
        Debug.Log("Set");
        dataGizmos[gizmoID] = g;
        assetLoaded._gizmos=dataGizmos;

    }
    #endregion

    #region Callback Functions
    #endregion
}