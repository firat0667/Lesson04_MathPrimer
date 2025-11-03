using Math;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [CustomEditor(typeof(PlaneExample))]
    public class PlaneExampleEditor : Editor
    {
        private Vector3     vNormal = new Vector3(1, 2, 3);
        private float       m_fDistance = 2.0f;
        private Vector3[]   m_points = new Vector3[] { Vector3.forward, Vector3.up, Vector3.right };

        Vector3 vTestPoint = Vector3.zero;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            m_fDistance = EditorGUILayout.Slider("Distance", m_fDistance, -10.0f, 10.0f);

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI()
        {
            PlaneExample pe = target as PlaneExample;

            MathUtilEditor.DrawGrid(Vector3.zero, Vector3.up, Vector3.right, Color.black);
            MathUtilEditor.DrawGrid(Vector3.zero, Vector3.forward, Vector3.right, Color.black);
            MathUtilEditor.DrawGrid(Vector3.zero, Vector3.up, Vector3.forward, Color.black);

            // plane from normal & distance
            Plane plane = new Plane(vNormal, m_fDistance);
            vNormal = Handles.DoPositionHandle(vNormal, Quaternion.identity);
            MathUtilEditor.DrawVector(vNormal, Color.green);
            MathUtilEditor.DrawPlane(plane, Color.yellow);

            vTestPoint = Handles.DoPositionHandle(vTestPoint, Quaternion.identity);
            Vector3 vClosestPointOnPlane = MathUtil.ClosestPointOnPlane(vTestPoint, plane);
            Handles.color = new Color(1.0f, 0.5f, 0.0f);
            Handles.SphereHandleCap(0, vClosestPointOnPlane, Quaternion.identity, 0.2f, EventType.Repaint);
            Handles.DrawLine(vTestPoint, vClosestPointOnPlane, 2.0f);

            // plane from 3 points
            /*
            Handles.color = Color.black;
            for (int i = 0; i < 3; ++i)
            {
                m_points[i] = Handles.DoPositionHandle(m_points[i], Quaternion.identity);
                Handles.SphereHandleCap(0, m_points[i], Quaternion.identity, 0.2f, EventType.Repaint);
            }
            Plane plane = new Plane(m_points[0], m_points[1], m_points[2]);
            MathUtilEditor.DrawPlane(plane, bIsOnPositiveSide ? Color.green : Color.red);
            */
        }
    }
}