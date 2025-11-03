using Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EasingExample : MonoBehaviour
    {
        delegate float EasingFunction(float f);

        private void OnEnable()
        {
            //StartCoroutine(BlendLogic(MathUtil.Linear));
            //StartCoroutine(BlendLogic(MathUtil.EaseIn));
            //StartCoroutine(BlendLogic(MathUtil.EaseOut));
            //StartCoroutine(BlendLogic(MathUtil.Smoothstep));
            //StartCoroutine(BlendLogic(MathUtil.EaseOutElastic));
            StartCoroutine(BlendLogic(MathUtil.OutBounce));
        }

        IEnumerator BlendLogic(EasingFunction func)
        {
            const int NUM_POINT = 100;
            const float SIZE = 3.5f;

            // create easing curve
            GameObject template = transform.Find("EasingTemplate").gameObject;
            GameObject go = Instantiate(template, template.transform.parent);
            go.SetActive(true);
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i <= NUM_POINT; ++i)
            {
                float x = i / (float)NUM_POINT;
                float y = func(x);
                positions.Add(new Vector3(Mathf.LerpUnclamped(-SIZE, SIZE, x), 
                                          Mathf.LerpUnclamped(-SIZE, SIZE, y), 
                                          0.0f));
            }
            LineRenderer lr = go.GetComponent<LineRenderer>();
            lr.positionCount = positions.Count;
            lr.SetPositions(positions.ToArray());

            // calculate ball position
            Transform ball = go.transform.Find("Ball");
            while (true)
            {
                float fPeriodicTime = Time.time % 5.0f;
                float x = Mathf.Clamp01(fPeriodicTime / 4.0f);
                float y = func(x);
                ball.transform.position = new Vector3(Mathf.LerpUnclamped(-SIZE, SIZE, x), 
                                                      Mathf.LerpUnclamped(-SIZE, SIZE, y), 
                                                      0.0f);
                yield return null;
            }
        }
    }
}