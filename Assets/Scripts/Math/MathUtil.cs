using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Math
{
    public static class MathUtil
    {
        public static float Linear(float f)
        {
            return f;
        }

        public static float EaseIn(float f)
        {
            return f * f;
        }

        public static float EaseOut(float f)
        {
            return 1.0f - EaseIn(1.0f - f);
        }

        public static float Smoothstep(float f)
        {
            return f * f * (3.0f - 2.0f * f);
        }

        public static float EaseOutElastic(float f)
        {
            float p = 0.5f;
            return Mathf.Pow(2, -10 * f) * Mathf.Sin((f - p / 4) * (2 * Mathf.PI) / p) + 1;
        }

        public static float OutBounce(float f)
        {
            float div = 2.75f;
            float mult = 7.5625f;

            if (f < 1 / div)
            {
                return mult * f * f;
            }
            else if (f < 2 / div)
            {
                f -= 1.5f / div;
                return mult * f * f + 0.75f;
            }
            else if (f < 2.5 / div)
            {
                f -= 2.25f / div;
                return mult * f * f + 0.9375f;
            }
            else
            {
                f -= 2.625f / div;
                return mult * f * f + 0.984375f;
            }
        }

        public static float AngleBetween(Vector2 vA, Vector2 vB)
        {
            float fDot = Vector2.Dot(vA, vB) / (vA.magnitude * vB.magnitude);
            return Mathf.Acos(fDot) * Mathf.Rad2Deg;
        }

        public static float SignedAngleBetween(Vector2 vA, Vector2 vB)
        {
            Vector2 vRight = new Vector2(vA.y, -vA.x);
            float fDot = Vector2.Dot(vA, vB) / (vA.magnitude * vB.magnitude);
            float fSign = Mathf.Sign(Vector2.Dot(vB, vRight));
            return Mathf.Acos(fDot) * Mathf.Rad2Deg * fSign;
        }

        public static Vector3 ClosestPointOnSegment(Vector3 p, Vector3 vA, Vector3 vB)
        {
            Vector3 vAtoP = p - vA;
            Vector3 vAB = vB - vA;

            // closest to A?
            float fDot1 = Vector3.Dot(vAtoP, vAB);
            if (fDot1 <= 0.0f)
            {
                return vA;
            }

            // closest to B?
            Vector3 vBtoP = p - vB;
            Vector3 vBA = vA - vB;
            float fDot2 = Vector3.Dot(vBtoP, vBA);
            if (fDot2 <= 0.0f)
            {
                return vB;
            }

            // inbetween A & B
            return vA + vAB.normalized * (fDot1 / vAB.magnitude);
        }

        public static bool GetSide(Vector3 p, Plane plane)
        {
            return Vector3.Dot(plane.normal, p) + plane.distance >= 0.0f;
        }

        public static Vector3 ClosestPointOnPlane(Vector3 p, Plane plane)
        {
            float fDistance = Vector3.Dot(plane.normal, p) + plane.distance;
            return p - plane.normal * fDistance;
        }

        public static bool PointInTriangle(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2, out Vector3 vBaryCoords)
        {
            // Compute vectors        
            Vector3 v0 = p2 - p0;
            Vector3 v1 = p1 - p0;
            Vector3 v2 = p - p0;

            // Compute dot products
            float dot00 = Vector3.Dot(v0, v0);
            float dot01 = Vector3.Dot(v0, v1);
            float dot02 = Vector3.Dot(v0, v2);
            float dot11 = Vector3.Dot(v1, v1);
            float dot12 = Vector3.Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
            float w = 1.0f - (u + v);
            vBaryCoords = new Vector3(w, v, u);

            // Check if point is in triangle
            return u >= 0.0f &&
                   v >= 0.0f &&
                   u + v <= 1.0f;
        }

        public static bool RayTriangleIntersection(Ray ray, 
                                                   Vector3 p0, Vector3 p1, Vector3 p2, 
                                                   out Vector3 vHitPoint, 
                                                   out Vector3 vBaryCoords)
        {
            vHitPoint = Vector3.zero;
            vBaryCoords = Vector3.zero;

            // do we hit the triangle's plane or not?
            Plane plane = new Plane(p0, p1, p2);
            float fDistanceToPlane = 0.0f;
            if (!plane.Raycast(ray, out fDistanceToPlane))
            {
                return false;
            }

            // calculate point on plane
            //Vector3 vPointOnPlane = ray.GetPoint(fDistanceToPlane);
            Vector3 vPointOnPlane = ray.origin + ray.direction * fDistanceToPlane;

            if (PointInTriangle(vPointOnPlane, p0, p1, p2, out vBaryCoords))
            {
                vHitPoint = vPointOnPlane;
                return true;
            }

            // nope!
            return false;
        }
    }
}