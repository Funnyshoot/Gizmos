using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

using technical.test.editor;

public class GizmoEditor : EditorWindow
{
    SceneGizmoAsset assetLoaded;
    //string[] buttonString = new string[2]{ "edit", "editing" };
    Gizmo[] savedGizmos; // the last position before update.
    Gizmo[] dataGizmos; //the data we load
    Gizmo[] storageGizmos; // where we write
    GameObject[] SpheresGizmos;
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
        SpheresGizmos = new GameObject[dataGizmos.Length];
        dataGizmos.CopyTo(storageGizmos, 0);
        DrawSphereGizmos();
        //RenderPipelineManager.endFrameRendering += EndFrameRendering;
    }

    private void OnDisable()
    {
        StopEditingGizmo();
        EraseSphereGizmos();
        //RenderPipelineManager.endFrameRendering -= EndFrameRendering;
    }

    private void OnGUI()
    {
        GUILayout.Label("Gizmos Editor", EditorStyles.boldLabel);
        for (int i = 0; i< dataGizmos.Length; ++i)
        {
            storageGizmos[i].Name = EditorGUILayout.TextField("Gizmo: " + dataGizmos[i].Name, dataGizmos[i].Name);
            storageGizmos[i].Position = EditorGUILayout.Vector3Field("Position", dataGizmos[i].Position);
            if (GUILayout.Button("edit"))
            {
                if (editing == i) StopEditingGizmo();
                else
                {
                    StopEditingGizmo();
                    StartEditingGizmo(i);
                }
            }
            //ChangeLabel(EditorGUILayout.Toggle("View in Game", false)); would need listener?

            GUILayout.Space(50);
        }
    }

    private void Update()
    {
        if (!(editing < 0)) EditingGizmo(editing);
    }

    #endregion

    #region call

    void EndFrameRendering(ScriptableRenderContext ctxt, Camera[] cameras) => DrawSphereGizmos();

    #endregion

    #region function

    void DrawSphereGizmos()
    {
        for (int i = 0; i < dataGizmos.Length; ++i)
        {
            SpheresGizmos[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            SpheresGizmos[i].tag = "EditorOnly";
            UpdateSphereGizmos(i);
        }
    }

    void EraseSphereGizmos()
    {
        for (int i = 0; i < dataGizmos.Length; ++i)
        {
            DestroyImmediate(SpheresGizmos[i]);
        }
    }

    void UpdateSphereGizmos()
    {
        for (int i = 0; i < dataGizmos.Length; ++i)
        {
            SpheresGizmos[i].transform.position = storageGizmos[i].Position;
            SpheresGizmos[i].name = storageGizmos[i].Name;
            Debug.Log(SpheresGizmos[i].name);
        }
    }
    void UpdateSphereGizmos(int i)
    {
        SpheresGizmos[i].transform.position = storageGizmos[i].Position;
        SpheresGizmos[i].name = storageGizmos[i].Name;
        Debug.Log(SpheresGizmos[i].name);
    }

    void ChangeLabel(bool change)
    {
        for (int i = 0; i < dataGizmos.Length; ++i)
        {
            if (change) SpheresGizmos[i].tag = "Untagged";
            else SpheresGizmos[i].tag = "EditorOnly";
        }
    }

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
        SetGizmo(gizmoID, storageGizmos[gizmoID]);
        UpdateSphereGizmos(gizmoID);
    }

    public void ResetGizmo(int gizmoID)
    {
        SetGizmo(gizmoID, savedGizmos[gizmoID]);
    }

    private void SetGizmo(int gizmoID, Gizmo g)
    {
        dataGizmos[gizmoID] = g;
        assetLoaded._gizmos=dataGizmos;

    }
    #endregion

    #region Callback Functions
    #endregion
}