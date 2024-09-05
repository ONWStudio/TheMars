using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Attribute;
using Onw.Event;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Onw.GridTile
{
    public sealed class GridTile : MonoBehaviour
    {
        public IUnityEventListenerModifier<GridTile> OnHighlightTile => _onHighlightTile;
        public IUnityEventListenerModifier<GridTile> OnMouseDownTile => _onMouseDownTile;
        public IUnityEventListenerModifier<GridTile> OnMouseUpTile => _onMouseUpTile;
        public IUnityEventListenerModifier<GridTile> OnExitTile => _onExitTile;

        [field: FormerlySerializedAs("_properties")]
        [field: SerializeField] public List<string> Properties { get; private set; } = new(); // .. 각 타일에 속성을 넣을 수 있습니다

        [field: FormerlySerializedAs("_tileRenderer")]
        [field: SerializeField, ReadOnly] public MeshRenderer TileRenderer { get; private set; }
        [field: FormerlySerializedAs("_meshCollider")]
        [field: SerializeField, ReadOnly] public MeshCollider MeshCollider { get; private set; }
        [field: FormerlySerializedAs("_meshFilter")]
        [field: SerializeField, ReadOnly] public MeshFilter MeshFilter { get; private set; }

        [field: FormerlySerializedAs("_tilePoint")]
        [field: SerializeField, ReadOnly] public Vector2Int TilePoint { get; private set; }

        [SerializeField, ReadOnly] private GridManager _gridManager;

        [SerializeField] private SafeUnityEvent<GridTile> _onHighlightTile = new();
        [SerializeField] private SafeUnityEvent<GridTile> _onMouseDownTile = new();
        [SerializeField] private SafeUnityEvent<GridTile> _onMouseUpTile = new();
        [SerializeField] private SafeUnityEvent<GridTile> _onExitTile = new();
        
        public Vector3 Size => TileRenderer.bounds.size;

        public void CreateTile(GridManager gridManager, Material material, float tileSize, in Vector2Int tilePoint)
        {
            _gridManager = gridManager;
            TilePoint = tilePoint;
            MeshFilter = gameObject.AddComponent<MeshFilter>();
            TileRenderer = gameObject.AddComponent<MeshRenderer>();
            MeshCollider = gameObject.AddComponent<MeshCollider>();

            TileRenderer.sharedMaterial = material;
            
            Mesh mesh = new();

            // 정점 설정
            Vector3[] vertices =
            {
                new(0, 0, 0), new(tileSize, 0, 0),
                new(0, 0, tileSize), new(tileSize, 0, tileSize)
            };

            int[] triangles =
            {
                0, 2,
                1, // 첫 번째 삼각형
                2, 3,
                1 // 두 번째 삼각형
            };

            Vector2[] uv =
            {
                new(0, 0), new(1, 0),
                new(0, 1), new(1, 1)
            };

            // 메쉬에 데이터 할당
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;

            // 노멀 계산 (빛 반사 처리)
            mesh.RecalculateNormals();

            MeshFilter.mesh = mesh;
            MeshCollider.sharedMesh = mesh;
        }
        
        public bool ContainsProperty(string property)
        {
            return Properties.Any(someProperty => someProperty == property);
        }
        
        private void OnMouseEnter()
        {
            _onHighlightTile.Invoke(this);
        }

        private void OnMouseExit()
        {
            _onExitTile.Invoke(this);
        }

        private void OnMouseDown()
        {
            _onMouseDownTile.Invoke(this);
        }

        private void OnMouseUp()
        {
            _onMouseUpTile.Invoke(this);
        }
    }
}
