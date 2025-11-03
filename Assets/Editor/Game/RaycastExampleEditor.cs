using Math;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Game
{
    [CustomEditor(typeof(RaycastExample))]
    public class RaycastExampleEditor : Editor
    {
        private Vector3[]   m_vertices;
        private int[]       m_triangles;
        private int         m_passiveControl;
        private Ray?        m_ray;
        private int         m_iHitTriangle;
        private int         m_iExitTriangle;

        private void OnEnable()
        {
            m_passiveControl = GUIUtility.GetControlID(FocusType.Passive);
            RaycastExample re = target as RaycastExample;
            MeshFilter mf = re.GetComponent<MeshFilter>();
            Mesh mesh = mf.sharedMesh;
            m_vertices = mesh.vertices;
            m_triangles = mesh.triangles;
            Tools.current = Tool.None;
        }

        protected void DrawTriangle(int iTriangle, Color color)
        {
            Vector3[] triangle = new Vector3[]{
                    m_vertices[m_triangles[iTriangle + 0]],
                    m_vertices[m_triangles[iTriangle + 1]],
                    m_vertices[m_triangles[iTriangle + 2]]
                };

            Handles.color = color;
            Handles.DrawAAConvexPolygon(triangle);
            Handles.color = Color.black;
            for (int i = 0; i < 3; ++i)
            {
                Handles.SphereHandleCap(0, triangle[i], Quaternion.identity, 0.3f, EventType.Repaint);
                Handles.DrawLine(triangle[i], triangle[(i + 1) % 3], 3.0f);
            }
        }

        private void OnSceneGUI()
        {
            MathUtilEditor.DrawGrid(Vector3.zero, Vector3.forward, Vector3.right, Color.black);

            // draw hit triangle?
            if (m_iHitTriangle >= 0)
            {
                DrawTriangle(m_iHitTriangle, new Color(1.0f, 0.75f, 0.0f, 0.7f));
            }

            // draw exit triangle?
            if (m_iExitTriangle >= 0)
            {
                DrawTriangle(m_iExitTriangle, new Color(1.0f, 0.1f, 0.0f, 0.7f));
            }

            // draw ray
            if (m_ray.HasValue)
            {
                Handles.color = Color.red;
                Handles.SphereHandleCap(0, m_ray.Value.origin, Quaternion.identity, 0.3f, EventType.Repaint);
                Handles.DrawLine(m_ray.Value.origin, m_ray.Value.origin + m_ray.Value.direction * 1000.0f, 3.0f);
            }

            // raycast
            if (Event.current.type == EventType.MouseDown &&
                Event.current.button == 0 &&
                !Event.current.alt)
            {
                Event.current.Use();
                GUIUtility.hotControl = m_passiveControl;
                m_ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                // triangle test
                float fClosestTriangle = float.MaxValue;
                float fFurthestTriangle = -float.MaxValue;
                m_iHitTriangle = -1;
                m_iExitTriangle = -1;

                for (int i = 0; i < m_triangles.Length; i += 3)
                {
                    Vector3 vHit;
                    if (MathUtil.RayTriangleIntersection(m_ray.Value,
                                                        m_vertices[m_triangles[i + 0]],
                                                        m_vertices[m_triangles[i + 1]],
                                                        m_vertices[m_triangles[i + 2]],
                                                        out vHit, out _))
                    {
                        float fDistance = Vector3.Distance(m_ray.Value.origin, vHit);
                        if (fDistance < fClosestTriangle)
                        {
                            fClosestTriangle = fDistance;
                            m_iHitTriangle = i;
                        }

                        if (fDistance > fFurthestTriangle)
                        {
                            fFurthestTriangle = fDistance;
                            m_iExitTriangle = i;
                        }
                    }
                }
            }
        }
    }
}