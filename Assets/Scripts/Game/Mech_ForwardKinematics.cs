using Math;
using UnityEngine;

namespace Game
{
    public class Mech_ForwardKinematics : MonoBehaviour
    {
        [Header("Leg References")]
        [SerializeField] private Transform m_leftUpperLeg;
        [SerializeField] private Transform m_rightUpperLeg;
        [SerializeField] private Transform m_pelvis;

        [Header("Animation Settings")]
        [SerializeField] private float m_walkSpeed = 3.0f;     
        [SerializeField] private float m_legAmplitude = 1.0f;  
        [SerializeField] private float m_pelvisBob = 0.1f;     
        [SerializeField] private float m_rotationMin = -46f;
        [SerializeField] private float m_rotationMax = 30f;

        private float m_time;

        private void Update()
        {
            float moveInput = Input.GetAxis("Vertical");
            bool isMoving = Mathf.Abs(moveInput) > 0.1f;

            if (isMoving)
                m_time += Time.deltaTime * m_walkSpeed * Mathf.Abs(moveInput);
            else
                m_time = Mathf.Lerp(m_time, 0, Time.deltaTime * 5f);

            float phaseLeft = (Mathf.Sin(m_time) + 1) * 0.5f;              
            float phaseRight = (Mathf.Sin(m_time + Mathf.PI) + 1) * 0.5f; 

            float easedLeft = MathUtil.Smoothstep(phaseLeft);
            float easedRight = MathUtil.Smoothstep(phaseRight);

            float leftRot = Mathf.Lerp(m_rotationMin, m_rotationMax, easedLeft);
            float rightRot = Mathf.Lerp(m_rotationMin, m_rotationMax, easedRight);

            if (m_leftUpperLeg != null)
                m_leftUpperLeg.localEulerAngles = new Vector3(leftRot * m_legAmplitude, 0f, 0f);

            if (m_rightUpperLeg != null)
                m_rightUpperLeg.localEulerAngles = new Vector3(rightRot * m_legAmplitude, 0f, 0f);

            if (m_pelvis != null)
            {
                float bobOffset = Mathf.Sin(m_time * 2.0f) * m_pelvisBob;
                Vector3 basePos = new Vector3(0f, bobOffset, 0f);
                m_pelvis.localPosition = basePos;
            }
        }
    }
}
