using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Onw.Helper
{
    public static class VerticesTo
    {
        public static float GetHeightFromVertices(GameObject obj)
        {
            List<Vector3> vertices = GetVertices(obj);

            if (vertices.Count == 0)
            {
#if DEBUG
                Debug.Log("vertices not found!");
#endif
                return 0f;
            }

            float maxPivot = float.NegativeInfinity;
            float minPivot = float.PositiveInfinity;

            foreach (Vector3 point in vertices.Select(obj.transform.TransformPoint))
            {
                if (point.y > maxPivot)
                {
                    maxPivot = point.y;
                }

                if (point.y < minPivot)
                {
                    minPivot = point.y;
                }
            }

            return maxPivot - minPivot;
        }

        public static float GetZWidthFromVertices(GameObject obj)
        {
            List<Vector3> vertices = GetVertices(obj);

            if (vertices.Count == 0)
            {
                Debug.Log("vertices not found!");

                return 0f;
            }

            float maxPivot = float.NegativeInfinity;
            float minPivot = float.PositiveInfinity;

            foreach (Vector3 point in vertices.Select(obj.transform.TransformPoint))
            {
                if (point.z > maxPivot)
                {
                    maxPivot = point.z;
                }

                if (point.z < minPivot)
                {
                    minPivot = point.z;
                }
            }

            return maxPivot - minPivot;
        }

        public static float GetXWidthFromVertices(GameObject obj)
        {
            List<Vector3> vertices = GetVertices(obj);
            
            if (vertices.Count == 0)
            {
                Debug.Log("vertices not found!");

                return 0f;
            }

            float maxPivot = float.NegativeInfinity;
            float minPivot = float.PositiveInfinity;

            foreach (Vector3 point in vertices.Select(obj.transform.TransformPoint))
            {
                if (point.x > maxPivot)
                {
                    maxPivot = point.x;
                }

                if (point.x < minPivot)
                {
                    minPivot = point.x;
                }
            }

            return maxPivot - minPivot;
        }

        public static float GetMaxYFromVertices(GameObject obj)
        {
            return GetVertices(obj)
                .Select(vertex => obj.transform.TransformPoint(vertex).y)
                .Prepend(float.NegativeInfinity)
                .Max();
        }

        public static float GetMinYFromVertices(GameObject obj)
        {
            return GetVertices(obj)
                .Select(vertex => obj.transform.TransformPoint(vertex).y)
                .Prepend(float.PositiveInfinity)
                .Min();
        }

        public static List<Vector3> GetVertices(GameObject obj)
        {
            List<Vector3> vertices = GetVerticesFromMeshFilter(obj);

            if (vertices.Count == 0)
            {
                vertices.AddRange(GetVerticesFromSkinnedMeshRenderer(obj));
            }

            return vertices;
        }

        public static List<Vector3> GetVerticesFromSkinnedMeshRenderer(GameObject obj)
        {
            return obj
                .GetComponentsInChildren<SkinnedMeshRenderer>()
                .SelectMany(skinnedMeshRenderer => skinnedMeshRenderer.sharedMesh.vertices)
                .ToList();
        }

        public static List<Vector3> GetVerticesFromMeshFilter(GameObject obj)
        {
            return obj
                .GetComponentsInChildren<MeshFilter>()
                .SelectMany(meshFilter => meshFilter.mesh.vertices)
                .ToList();
        }
    }
}