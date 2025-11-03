using Math;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [CustomEditor(typeof(SegmentExample))]
    public class SegmentExampleEditor : Editor
    {
        private Vector3 vA = new Vector2(0, 2);
        private Vector3 vB = new Vector2(2, 1);
        private Vector3 vP = new Vector2(2, 3);

        private void OnSceneGUI()
        {
            MathUtilEditor.DrawGrid(Vector3.zero, Vector3.up, Vector3.right, Color.black);

            vA = Handles.DoPositionHandle(vA, Quaternion.identity);
            vB = Handles.DoPositionHandle(vB, Quaternion.identity);
            vP = Handles.DoPositionHandle(vP, Quaternion.identity);

            Handles.color = Color.black;
            Handles.SphereHandleCap(0, vA, Quaternion.identity, 0.3f, EventType.Repaint);
            Handles.SphereHandleCap(0, vB, Quaternion.identity, 0.3f, EventType.Repaint);
            Handles.DrawLine(vA, vB, 6.0f);

            Handles.color = Color.red;
            Handles.SphereHandleCap(0, vP, Quaternion.identity, 0.3f, EventType.Repaint);
            Vector3 vCP = MathUtil.ClosestPointOnSegment(vP, vA, vB);
            Handles.color = new Color(1.0f, 0.5f, 0.0f);
            Handles.SphereHandleCap(0, vCP, Quaternion.identity, 0.3f, EventType.Repaint);
            Handles.DrawDottedLine(vP, vCP, 6.0f);
        }
    }
}