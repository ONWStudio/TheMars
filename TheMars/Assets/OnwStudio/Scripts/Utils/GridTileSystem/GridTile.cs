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
    public readonly struct TileData
    {
        public MeshRenderer TileRenderer { get; }
        public MeshFilter MeshFilter { get; }
        public MeshCollider Collider { get; }
        public Vector2Int TilePoint { get; }

        public TileData(MeshRenderer meshRenderer, MeshFilter meshFilter, MeshCollider meshCollider, in Vector2Int tilePoint)
        {
            TileRenderer = meshRenderer;
            MeshFilter = meshFilter;
            Collider = meshCollider;
            TilePoint = tilePoint;
        }
    }
    
    public sealed class GridTile : MonoBehaviour
    {
        public IUnityEventListenerModifier<TileData> OnHighlightTile => _onHighlightTile;
        public IUnityEventListenerModifier<TileData> OnClickTile => _onClickTile;
        public IUnityEventListenerModifier<TileData> OnExitTile => _onExitTile;

        [field: FormerlySerializedAs("_properties")]
        [field: SerializeField] public List<string> Properties { get; private set; } = new(); // .. 각 타일에 속성을 넣을 수 있습니다

        [SerializeField, ReadOnly] private MeshRenderer _tileRenderer;
        [SerializeField, ReadOnly] private MeshCollider _meshCollider;
        [SerializeField, ReadOnly] private MeshFilter _meshFilter;

        [field: FormerlySerializedAs("_tilePoint")]
        [field: SerializeField, ReadOnly] public Vector2Int TilePoint { get; private set; }

        [SerializeField, ReadOnly] private GridManager _gridManager;

        [SerializeField] private SafeUnityEvent<TileData> _onHighlightTile = new();
        [SerializeField] private SafeUnityEvent<TileData> _onClickTile = new();
        [SerializeField] private SafeUnityEvent<TileData> _onExitTile = new();
        
        public Vector3 Size => _tileRenderer.bounds.size;

        public TileData GetTileData()
        {
            return new(_tileRenderer, _meshFilter, _meshCollider, TilePoint);
        }

        public void CreateTile(GridManager gridManager, Material material, float tileSize, in Vector2Int tilePoint)
        {
            _gridManager = gridManager;
            TilePoint = tilePoint;
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _tileRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshCollider = gameObject.AddComponent<MeshCollider>();

            _tileRenderer.sharedMaterial = material;
            
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

            _meshFilter.mesh = mesh;
            _meshCollider.sharedMesh = mesh;
        }
        
        public bool ContainsProperty(string property)
        {
            return Properties.Any(someProperty => someProperty == property);
        }
        
        private void OnMouseEnter()
        {
            _onHighlightTile.Invoke(GetTileData());
        }

        private void OnMouseExit()
        {
            _onExitTile.Invoke(GetTileData());
        }

        private void OnMouseUp()
        {
            _onClickTile.Invoke(GetTileData());
        }
    }
}
