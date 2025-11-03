using Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LerpExample : MonoBehaviour
    {
        delegate float EasingFunction(float f);

        private void OnEnable()
        {
            float A = 5.0f;
            float B = -7.0f;

            Vector2 vA = new Vector2(-21, 32);
            Vector2 vB = new Vector2(64, 0);

            for (float f = 0.0f; f <= 1.0f; f += 0.01f)
            {
                float fLerpedValue = A * (1.0f - f) + B * f;

                //Vector2 vLerpedVector = vA * (1.0f - f) + vB * f;
                Vector2 vLerpedVector = Vector2.Lerp(vA, vB, MathUtil.OutBounce(f));

                //Debug.Log("Lerped Value at " + Mathf.RoundToInt(f * 100.0f) + "%: " + fLerpedValue.ToString("0.00"));
                //Debug.Log("Lerped Value at " + Mathf.RoundToInt(f * 100.0f) + "%: " + vLerpedVector);
            }

            // create flashing color
            Color highlightColor = new Color(1.0f, 0.5f, 0.0f);
            Color blendColor = new Color(1.0f, 0.0f, 0.5f);
            Color drawColor = Color.Lerp(highlightColor, blendColor, 0.5f + Mathf.Sin(Time.time * 6.0f) * 0.5f);

            //StartCoroutine(GameOver(MathUtil.Linear));
            //StartCoroutine(GameOver(MathUtil.EaseIn));
            //StartCoroutine(GameOver(MathUtil.EaseOut));
            //StartCoroutine(GameOver(MathUtil.Smoothstep));
            //StartCoroutine(GameOver(MathUtil.EaseOutElastic));
            //StartCoroutine(GameOver(MathUtil.OutBounce));
        }

        IEnumerator GameOver(EasingFunction func)
        {
            // play game
            yield return new WaitForSeconds(2.0f);

            // blend in overlay
            Image overlay = transform.Find("GameOver/Overlay").GetComponent<Image>();
            for (float f = 0.0f; f <= 1.0f; f += Time.deltaTime * 0.8f)
            {
                Color c = new Color(0.0f, 0.0f, 0.0f, func(f) * 0.8f);
                overlay.color = c;
                yield return null;
            }

            // move in bar
            RectTransform bar  = transform.Find("GameOver/Bar").GetComponent<RectTransform>();
            RectTransform title = bar.Find("Title").GetComponent<RectTransform>();
            Vector2 vSource = bar.anchoredPosition;
            for (float f = 0.0f; f <= 1.0f; f += Time.deltaTime * 1.2f)
            {
                bar.anchoredPosition = Vector2.LerpUnclamped(vSource, Vector2.zero, func(f));
                title.localScale = Vector3.one * Mathf.LerpUnclamped(0.5f, 1.0f, func(f));
                yield return null;
            }
        }
    }
}