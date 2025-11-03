using Math;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [CustomEditor(typeof(TriangleExample))]
    public class TriangleExampleEditor : Editor
    {
        private Vector3     m_vBaryCoord = new Vector3(0.33f, 0.33f, 0.34f);

        private Vector3[]   m_points = new Vector3[] { new Vector2(-1, -1), new Vector2(0, 1.5f), new Vector2(1, -1) };
        private Vector3     m_vTest = Vector3.zero;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Bary Centric Coordinate", EditorStyles.boldLabel);

            for (int i = 0; i < 3; ++i)
            {
                //float f = EditorGUILayout.Slider("" + (char)('X' + i), m_vBaryCoord[i], 0.0f, 1.0f);
                float f = EditorGUILayout.Slider("" + (char)('X' + i), m_vBaryCoord[i], -1.0f, 2.0f);
                if (Mathf.Abs(f - m_vBaryCoord[i]) > float.Epsilon)
                {
                    float fRemainder = 1.0f - f;
                    m_vBaryCoord[i] = f;

                    int j = (i + 1) % 3;
                    int k = (i + 2) % 3;

                    float fTotal = m_vBaryCoord[j] + m_vBaryCoord[k];
                    m_vBaryCoord[j] = fRemainder * (fTotal > 0.0f ? (m_vBaryCoord[j] / fTotal) : 0.5f);
                    m_vBaryCoord[k] = fRemainder * (fTotal > 0.0f ? (m_vBaryCoord[k] / fTotal) : 0.5f);
                }
            }

            EditorGUILayout.LabelField("Sum", (m_vBaryCoord.x + m_vBaryCoord.y + m_vBaryCoord.z).ToString("0.00"));
            
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI()
        {
            MathUtilEditor.DrawGrid(Vector3.zero, Vector3.up, Vector3.right, Color.black);

            // Move Test Point?
            Vector3 vNewTest = Handles.DoPositionHandle(m_vTest, Quaternion.identity);
            if (Vector3.Distance(vNewTest, m_vTest) > 0.0001f)
            {
                m_vTest = vNewTest;
                MathUtil.PointInTriangle(m_vTest, m_points[0], m_points[1], m_points[2], out m_vBaryCoord);
                Repaint();
            }

            // draw triangle
            bool bInside = MathUtil.PointInTriangle(m_vTest, m_points[0], m_points[1], m_points[2], out _);
            Handles.color = new Color(bInside ? 0.0f : 1.0f, bInside ? 1.0f : 0.0f, 0.0f, 0.5f);
            Handles.DrawAAConvexPolygon(m_points);

            // triangle outline
            Handles.color = Color.black;
            for (int i = 0; i < m_points.Length; i++)
            {
                m_points[i] = Handles.DoPositionHandle(m_points[i], Quaternion.identity);
                Handles.SphereHandleCap(0, m_points[i], Quaternion.identity, 0.3f, EventType.Repaint);
                Handles.DrawLine(m_points[i], m_points[(i + 1) % m_points.Length], 6.0f);
            }

            // Bary Coord
            Handles.color = Color.red;
            m_vTest = m_points[0] * m_vBaryCoord.x +
                      m_points[1] * m_vBaryCoord.y +
                      m_points[2] * m_vBaryCoord.z;
            Handles.SphereHandleCap(0, m_vTest, Quaternion.identity, 0.4f, EventType.Repaint);
        }
    }
}