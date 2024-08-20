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

            foreach (Vector3 point in vertices.Select(vertex => obj.transform.TransformPoint(vertex)))
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

        public static float GetMaxYFromVertices(GameObject obj)
        {
            return GetVertices(obj)
                .Select(vertex => obj.transform.TransformPoint(vertex))
                .Select(point => point.y)
                .Prepend(float.NegativeInfinity)
                .Max();
        }

        public static float GetMinYFromVertices(GameObject obj)
        {
            return GetVertices(obj)
                .Select(vertex => obj.transform.TransformPoint(vertex))
                .Select(point => point.y)
                .Prepend(float.PositiveInfinity)
                .Min();
        }

        public static List<Vector3> GetVertices(GameObject obj)
        {
            List<Vector3> vertices = GetVerticesFromSkinndedMeshRenderer(obj);

            if (vertices.Count == 0)
            {
                vertices.AddRange(GetVerticesFromMeshFilter(obj));
            }

            return vertices;
        }

        public static List<Vector3> GetVerticesFromSkinndedMeshRenderer(GameObject obj)
        {
            List<Vector3> vertices = new();

            SkinnedMeshRenderer[] filters = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (SkinnedMeshRenderer skinnedMeshRenderer in filters)
            {
                vertices.AddRange(skinnedMeshRenderer.sharedMesh.vertices);
            }

            return vertices;
        }

        public static List<Vector3> GetVerticesFromMeshFilter(GameObject obj)
        {
            List<Vector3> vertices = new List<Vector3>();

            MeshFilter[] filters = obj.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter meshFilter in filters)
            {
                vertices.AddRange(meshFilter.mesh.vertices);
            }

            return vertices;
        }
    }
}