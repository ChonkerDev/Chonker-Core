using System;
using System.Collections;
using UnityEngine;

namespace Chonker.Core.Tween
{
    public static class TweenCoroutines
    {
        
        public static IEnumerator RunAnimationCurveTaper(
            float duration,
            AnimationCurve curve,
            Action<float> onUpdate,
            bool runInReverse = false,
            Action onComplete = null
            
            )
        {
            float t = runInReverse ? 1 : 0;
            while (true)
            {
                float deltaTime = Time.deltaTime / duration;
                if (runInReverse) {
                    t -= deltaTime;
                    if (t < 0) {
                        break;
                    }
                }
                else {
                    t+= deltaTime;
                    if(t > 1) {
                        break;
                    }
                }
                
                
                float progress = curve.Evaluate(t);
                onUpdate?.Invoke(progress);
                yield return null;
            }

            onUpdate?.Invoke(1);
            onComplete?.Invoke();
        }
        
        public static IEnumerator RunTaper(
            float duration,
            Action<float> onUpdate,
            Action onComplete = null,
            EaseType easeType = EaseType.Linear)
        {
            float t = 0f;
            Func<float, float> ease = GetEase(easeType);

            while (t < 1)
            {
                t += Time.deltaTime / duration;
                onUpdate?.Invoke(ease(t));
                yield return null;
            }

            onUpdate?.Invoke(ease(1f));
            onComplete?.Invoke();
        }
        
        private static Func<float, float> GetEase(EaseType easeType) => easeType switch
        {
            EaseType.Linear => t => t,
            EaseType.SmoothStep => t => Mathf.SmoothStep(0, 1, t),
            EaseType.EaseInQuad => t => t * t,
            EaseType.EaseOutQuad => t => t * (2 - t),
            EaseType.EaseInOutQuad => t => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t,
            _ => t => t
        };
    }
    

}
