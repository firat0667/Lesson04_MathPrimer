using Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Mech_ForwardKinematics : MonoBehaviour
    {
        [SerializeField]
        public Transform    m_upperLeg;

        public void Update()
        {
            float fLegTime = 0.5f + Mathf.Sin(Time.time * 3.0f) * 0.5f;
            float fLegRotation = Mathf.Lerp(-46, 30, fLegTime);
            m_upperLeg.localEulerAngles = new Vector3(fLegRotation, 0.0f, 0.0f);
        }
    }
}