using System;
using Chonker.Core.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Chonker.Core.Editor.UI
{
    [CustomEditor(typeof(InverseMask))]
    public class InverseMaskEditor : UnityEditor.Editor
    {
        private InverseMask inverseMask;
        private SerializedProperty _maskGraphic;

        private void OnEnable() {
            inverseMask = ((InverseMask)target);

            _maskGraphic = serializedObject.FindProperty("_maskGraphic");
            inverseMask.Image.hideFlags = HideFlags.NotEditable;

        }

        private void OnDisable() {
            if (inverseMask.Image) {
                inverseMask.Image.hideFlags = HideFlags.None;
            }
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_maskGraphic);
            serializedObject.ApplyModifiedProperties();
        }
    }
}