#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom Editor GUI and Top Menu options to build or adjust the VIT Entrance Gate directly in Unity.
/// </summary>
public class VITGateBuilderEditor : EditorWindow
{
    [MenuItem("VIT Gate/Generate Gate & Road Scene (1-Click)", false, 1)]
    public static void GenerateSceneMenu()
    {
        // Check if an existing builder GameObject is in scene
        VITGateSceneBuilder existing = FindObjectOfType<VITGateSceneBuilder>();
        if (existing == null)
        {
            GameObject go = new GameObject("VIT_Entrance_Gate_System");
            existing = go.AddComponent<VITGateSceneBuilder>();
            Undo.RegisterCreatedObjectUndo(go, "Create Gate Builder");
        }

        existing.BuildCompleteScene();
        Selection.activeGameObject = existing.gameObject;
        EditorUtility.DisplayDialog("VIT Gate Generator", "The Gate, Pillars, Upper Surface, and Road have been generated successfully!", "OK");
    }

    [MenuItem("VIT Gate/Clear Gate Scene", false, 2)]
    public static void ClearSceneMenu()
    {
        VITGateSceneBuilder existing = FindObjectOfType<VITGateSceneBuilder>();
        if (existing != null)
        {
            Undo.DestroyObjectImmediate(existing.gameObject);
            Debug.Log("[VIT Gate] Gate system removed from scene.");
        }
        else
        {
            Debug.LogWarning("[VIT Gate] No VITGateSceneBuilder found to clear.");
        }
    }

    [MenuItem("VIT Gate/Open Gate Builder Window...", false, 20)]
    public static void OpenWindow()
    {
        VITGateBuilderEditor window = GetWindow<VITGateBuilderEditor>("VIT Gate Builder");
        window.minSize = new Vector2(380, 520);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("VIT ENTRANCE GATE BUILDER", titleStyle);
        GUILayout.Space(5);
        EditorGUILayout.HelpBox("Generate a 3D entrance gate featuring two pillars, an upper covering surface (architrave/roof), movable gate wings, asphalt road, road markings, and lighting.", MessageType.Info);

        GUILayout.Space(15);

        GUI.backgroundColor = new Color(0.2f, 0.8f, 0.4f);
        if (GUILayout.Button("★ Generate Full Gate & Road Scene", GUILayout.Height(45)))
        {
            GenerateSceneMenu();
        }

        GUI.backgroundColor = Color.white;
        GUILayout.Space(10);

        if (GUILayout.Button("Select Gate In Hierarchy", GUILayout.Height(30)))
        {
            VITGateSceneBuilder builder = FindObjectOfType<VITGateSceneBuilder>();
            if (builder != null)
            {
                Selection.activeGameObject = builder.gameObject;
                EditorGUIUtility.PingObject(builder.gameObject);
            }
        }

        GUILayout.Space(10);
        GUI.backgroundColor = new Color(0.9f, 0.3f, 0.3f);
        if (GUILayout.Button("Clear Gate Scene", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Confirm Delete", "Are you sure you want to remove the gate system?", "Yes, Delete", "Cancel"))
            {
                ClearSceneMenu();
            }
        }

        GUI.backgroundColor = Color.white;
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Interactive Controls in Play Mode:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("• Press [Space] or [E] to Open/Close the Gate.");
        EditorGUILayout.LabelField("• Hold [Right Mouse] + WASD to fly camera freely.");
    }
}
#endif
