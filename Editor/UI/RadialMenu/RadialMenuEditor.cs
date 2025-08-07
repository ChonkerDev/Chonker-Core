using System;
using UnityEditor;
using UnityEngine;

namespace Chonker.Core.Editor.UI.RadialMenu
{
    [CustomEditor(typeof(Core.UI.RadialMenu))]
    public class RadialMenuEditor : UnityEditor.Editor
    {
        SerializedProperty _numWedges,
            _radius,
            _iconOffset,
            _wedgeRotationOffset,
            _wedgeColor,
            _audioSource,
            _backgroundColor,
            _CenterBlockerCollider,
            uiCamera,
            wedgeContainer,
            _backgroundImage,
            _updateWedgeOnHover,
            _onHoverScale,
            _onHoverColor,
            _hoverTransitionTime,
            _activationTransition,
            _onHoverEnterWedgeSoundClip,
            _onSubmitWedgeSoundClip,
            TrackMouse,
            OnWedgeUnhover,
            OnWedgeHover,
            OnWedgeSubmitted;

        private Core.UI.RadialMenu UnderlyingRadialMenu;
        private bool showComponentReferences = false;

        private void OnEnable() {
            wedgeContainer = serializedObject.FindProperty("wedgeContainer");
            _CenterBlockerCollider = serializedObject.FindProperty("_CenterBlockerCollider");
            _backgroundImage = serializedObject.FindProperty("_backgroundImage");
            uiCamera = serializedObject.FindProperty("uiCamera");
            _audioSource = serializedObject.FindProperty("_audioSource");

            _numWedges = serializedObject.FindProperty("_numWedges");
            _radius = serializedObject.FindProperty("_radius");
            _iconOffset = serializedObject.FindProperty("_iconOffset");
            _wedgeRotationOffset = serializedObject.FindProperty("_wedgeRotationOffset");
            _wedgeColor = serializedObject.FindProperty("_wedgeColor");
            _backgroundColor = serializedObject.FindProperty("_backgroundColor");
            _updateWedgeOnHover = serializedObject.FindProperty("_updateWedgeOnHover");
            _onHoverScale = serializedObject.FindProperty("_onHoverScale");
            _onHoverColor = serializedObject.FindProperty("_onHoverColor");
            _hoverTransitionTime = serializedObject.FindProperty("_hoverTransitionTime");
            _activationTransition = serializedObject.FindProperty("_activationTransition");
            TrackMouse = serializedObject.FindProperty("TrackMouse");
            _onHoverEnterWedgeSoundClip = serializedObject.FindProperty("_onHoverEnterWedgeSoundClip");
            _onSubmitWedgeSoundClip = serializedObject.FindProperty("_onSubmitWedgeSoundClip");

            OnWedgeUnhover = serializedObject.FindProperty("OnWedgeUnhover");
            OnWedgeHover = serializedObject.FindProperty("OnWedgeHover");
            OnWedgeSubmitted = serializedObject.FindProperty("OnWedgeSubmitted");
            UnderlyingRadialMenu = (Core.UI.RadialMenu)target;
        }

        public override void OnInspectorGUI() {
            showComponentReferences = EditorGUILayout.Foldout(showComponentReferences, "Component References", true);

            if (showComponentReferences) {
                EditorGUILayout.PropertyField(uiCamera);
                EditorGUILayout.PropertyField(_backgroundImage);
                EditorGUILayout.PropertyField(_CenterBlockerCollider);
                EditorGUILayout.PropertyField(wedgeContainer);
                EditorGUILayout.PropertyField(_audioSource);
            }

            DisplayTransformData();
            DisplayWedgeDisplayData();
            DisplayAudio();
            DisplayOther();
            DisplayEvents();
            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayTransformData() {
            EditorGUILayout.LabelField("Transform Data", EditorStyles.boldLabel);
            int originalNumWedges = _numWedges.intValue;
            int originalRadius = _radius.intValue;
            float originalIconOffset = _iconOffset.floatValue;
            int originalWedgeRotationOffset = _wedgeRotationOffset.intValue;
            EditorGUILayout.PropertyField(_numWedges);
            EditorGUILayout.PropertyField(_radius);
            EditorGUILayout.PropertyField(_iconOffset);
            EditorGUILayout.PropertyField(_wedgeRotationOffset);
            bool valueUpdated = originalNumWedges != _numWedges.intValue || originalRadius != _radius.intValue ||
                                originalIconOffset != _iconOffset.floatValue ||
                                originalWedgeRotationOffset != _wedgeRotationOffset.intValue;
            if (valueUpdated) {
                serializedObject.ApplyModifiedProperties();
                UnderlyingRadialMenu.rebuildMenu();
            }
        }

        private void DisplayWedgeDisplayData() {
            EditorGUILayout.LabelField("Display Data", EditorStyles.boldLabel);
            Color previousWedgeColor = _wedgeColor.colorValue;
            Color previousBackgroundColor = _backgroundColor.colorValue;
            EditorGUILayout.PropertyField(_activationTransition);
            EditorGUILayout.PropertyField(_wedgeColor);
            EditorGUILayout.PropertyField(_backgroundColor);
            EditorGUILayout.PropertyField(_updateWedgeOnHover);
            if (_updateWedgeOnHover.boolValue) {
                EditorGUILayout.PropertyField(_onHoverScale);
                EditorGUILayout.PropertyField(_onHoverColor);
                EditorGUILayout.PropertyField(_hoverTransitionTime);
            }

            if (previousWedgeColor != _wedgeColor.colorValue ||
                previousBackgroundColor != _backgroundColor.colorValue) {
                serializedObject.ApplyModifiedProperties();
                UnderlyingRadialMenu.refreshVisualData();
            }
        }

        private void DisplayAudio() {
            EditorGUILayout.PropertyField(_onHoverEnterWedgeSoundClip);
            EditorGUILayout.PropertyField(_onSubmitWedgeSoundClip);
        }

        private void DisplayOther() {
            EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(TrackMouse);
        }

        private void DisplayEvents() {
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(OnWedgeHover);
            EditorGUILayout.PropertyField(OnWedgeUnhover);
            EditorGUILayout.PropertyField(OnWedgeSubmitted);
        }
    }
}