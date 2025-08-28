#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Chonker.Core.Attributes
{
    [CustomPropertyDrawer(typeof(PrefabModeOnlyAttribute))]
    public class PrefabModeOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();

            if (stage != null)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();

            return stage != null
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0;
        }
    }
}
#endif