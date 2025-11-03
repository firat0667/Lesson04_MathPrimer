using Math;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [CustomEditor(typeof(VectorExample))]
    public class VectorExampleEditor : Editor
    {
        private Vector3 vA = new Vector3(0, 2);
        private Vector3 vB = new Vector3(2, 1);

        private void OnSceneGUI()
        {
            VectorExample ve = target as VectorExample;
            Text dp = ve.transform.Find("Canvas/DotProduct").GetComponent<Text>();

            MathUtilEditor.DrawGrid(Vector3.zero, Vector3.up, Vector3.right, Color.black);
            
            vA = Handles.DoPositionHandle(vA, Quaternion.identity);
            vB = Handles.DoPositionHandle(vB, Quaternion.identity);
            Vector3 vC = Vector3.Cross(vA, vB);

            MathUtilEditor.DrawVector(vA, Color.red);
            MathUtilEditor.DrawVector(vB, Color.green);
            MathUtilEditor.DrawVector(vC, new Color(1.0f, 0.5f, 0.0f, 0.5f));


            //dp.text = MathUtil.DotProduct(vA.normalized, vB.normalized).ToString("0.00");
            //dp.text = "Angle: " + MathUtil.AngleBetween(vA, vB).ToString("0.00") + " deg";
            //dp.text = "Angle: " + MathUtil.SignedAngleBetween(vA, vB).ToString("0.00") + " deg";

            Canvas.ForceUpdateCanvases();
        }
    }
}