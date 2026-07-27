using UnityEngine;
using UnityEditor;

[System.Serializable]
[CustomEditor(typeof(SelectionManager))]
public class SelectionManagerEditor : Editor
{
    SelectionManager selectionManager;

    void OnEnable()
    {
        selectionManager = (SelectionManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (selectionManager.selectedTag.Length == 0)
        {
            selectionManager.selectedTag = UnityEditorInternal.InternalEditorUtility.tags[0];
        }

        //unit container
        selectionManager.unitsContainer = EditorGUILayout.ObjectField("Units Container:", selectionManager.unitsContainer, typeof(GameObject), true) as GameObject;
        if (selectionManager.unitsContainer)
        {
            GUILayout.Label("Hotkeys", EditorStyles.boldLabel);

            GUILayout.Label("Selection");

            GUILayout.BeginHorizontal();
            GUILayout.Label("    Multi select:");
            selectionManager.keyMultiselect = (KeyCode)EditorGUILayout.EnumPopup("", selectionManager.keyMultiselect, GUILayout.MaxWidth(100));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("    Deselect:");
            selectionManager.keyDeselect = (KeyCode)EditorGUILayout.EnumPopup("", selectionManager.keyDeselect, GUILayout.MaxWidth(100));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.Label("Control Groups");

            GUILayout.BeginHorizontal();
            GUILayout.Label("    Set control group:");
            selectionManager.keySetControlGroup = (KeyCode)EditorGUILayout.EnumPopup("", selectionManager.keySetControlGroup, GUILayout.MaxWidth(100));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("    Add to control group:");
            selectionManager.keyAddToControlGroup = (KeyCode)EditorGUILayout.EnumPopup("", selectionManager.keyAddToControlGroup, GUILayout.MaxWidth(100));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.Label("Selection gameobjects", EditorStyles.boldLabel);

            selectionManager.selectionProjector = EditorGUILayout.ObjectField("Selection:", selectionManager.selectionProjector, typeof(GameObject), true) as GameObject;
            selectionManager.hoverProjector = EditorGUILayout.ObjectField("Hover:", selectionManager.hoverProjector, typeof(GameObject), true) as GameObject;

            EditorGUILayout.Space();

            GUILayout.Label("Filter selectable game objects", EditorStyles.boldLabel);

            selectionManager.selectByTag = GUILayout.Toggle(selectionManager.selectByTag, " Select by Tag");
            if (selectionManager.selectByTag)
            {
                selectionManager.selectedTag = EditorGUILayout.TagField("     Tag:", selectionManager.selectedTag);
            }

            selectionManager.selectByLayer = GUILayout.Toggle(selectionManager.selectByLayer, " Select by Layer");
            if (selectionManager.selectByLayer)
            {
                selectionManager.selectedLayer = EditorGUILayout.LayerField("     Layer:", selectionManager.selectedLayer);
            }

            selectionManager.selectByName = GUILayout.Toggle(selectionManager.selectByName, " Select by Name");
            if (selectionManager.selectByName)
            {
                selectionManager.selectedName = EditorGUILayout.TextField("     Name:", selectionManager.selectedName);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Please link a container object for your selectable units.", MessageType.Warning, true);
        }
    }
}