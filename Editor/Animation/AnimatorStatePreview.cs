using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Chonker.Core.Editor.Animation
{
    /*
     * borrowed from Warped Imagination https://www.youtube.com/watch?v=BbNjqlCY9bc
     */
    [CustomPreview(typeof(AnimatorState))]
    public class AnimatorStatePreview : ObjectPreview
    {
        private UnityEditor.Editor _preview;
        private int _animationClipId;

        public override void Initialize(Object[] targets) {
            base.Initialize(targets);

            if (targets.Length > 1 || Application.isPlaying) return;

            AnimationClip animationClip = GetAnimationClip(target as AnimatorState);
            if (animationClip) {
                
                _preview = UnityEditor.Editor.CreateEditor(animationClip);
                _animationClipId = animationClip.GetInstanceID();
            }
                
        }

        public override void Cleanup() {
            base.Cleanup();
            CleanupPreviewEditor();
        }

        public override bool HasPreviewGUI() {
            return _preview?.HasPreviewGUI() ?? false;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            GUI.Label(r, target.name);
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background) {
            base.OnInteractivePreviewGUI(r, background);
            AnimationClip animationClip = GetAnimationClip(target as AnimatorState);

            if (animationClip && animationClip.GetInstanceID() != _animationClipId) {
                CleanupPreviewEditor();
                _preview = UnityEditor.Editor.CreateEditor(animationClip);
                _animationClipId = animationClip.GetInstanceID();
                return;
            }

            if (_preview != null && animationClip.GetInstanceID() != _animationClipId) {
                _preview.OnInteractivePreviewGUI(r, background);
            }
        }

        AnimationClip GetAnimationClip(AnimatorState state) {
            return state?.motion as AnimationClip;
        }

        private void CleanupPreviewEditor() {
            if (_preview != null) {
                Object.DestroyImmediate(_preview);
            }
        }
    }
}