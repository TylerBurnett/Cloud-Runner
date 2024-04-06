using System;
using System.Collections;
using UnityEngine;

namespace Game.Common
{
    public static class Enumerators
    {
        public static IEnumerator WaitForFrames(int frameCount)
        {
            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
        }

        public static IEnumerator MoveTowards(float start, float end, float timeInSeconds, Action<float> SetValue)
        {
            float moveRate = (end - start) / timeInSeconds;
            float current = start;

            while (current != end)
            {
                current = Mathf.MoveTowards(current, end, moveRate * Time.unscaledDeltaTime);
                SetValue(current);

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
