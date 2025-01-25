#if DEBUG
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class DebugSettingDrawer : OdinValueDrawer<DebugSetting>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        DebugSetting debugSetting = ValueEntry.SmartValue;

        EditorGUILayout.BeginHorizontal();
        debugSetting.Value = EditorGUILayout.ToggleLeft(debugSetting.keyRaw, debugSetting.Value);
        EditorGUILayout.EndHorizontal();
    }
}
#endif