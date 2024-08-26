using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Onw.Attribute;
using TMPro;
using UnityEngine.Events;

namespace Onw.GridTile
{
    public readonly struct TileArgs
    {
        public MeshRenderer TileRenderer { get; }
        public MeshFilter MeshFilter { get; }
        public MeshCollider Collider { get; }
        public Vector2Int TilePoint { get; }

        public TileArgs(MeshRenderer meshRenderer, MeshFilter meshFilter, MeshCollider meshCollider, Vector2Int tilePoint)
        {
            TileRenderer = meshRenderer;
            MeshFilter = meshFilter;
            Collider = meshCollider;
            TilePoint = tilePoint;
        }
    }
    
    public sealed class GridTile : MonoBehaviour
    {
        public GridManager GridManager
        {
            get => _gridManager;
            set
            {
                if (_gridManager) return;

                _gridManager = value;
            }
        }
        
        public Vector2Int TilePoint
        {
            get => _tilePoint ?? Vector2Int.zero;
            set => _tilePoint ??= value;
        }

        private MeshRenderer _tileRenderer;
        private MeshCollider _meshCollider;
        private MeshFilter _meshFilter;
        private Vector2Int? _tilePoint = null;
        
        [SerializeField, ReadOnly] private GridManager _gridManager;
        [SerializeField] private List<string> _properties = new(); // .. 각 타일에 속성을 넣을 수 있습니다

        private void Awake()
        {
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _tileRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        
        private void Start()
        {
            _tileRenderer.material.color = Color.white;
            Mesh mesh = new();
            float tileSize = _gridManager.TileSize;

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
            return _properties.Any(someProperty => someProperty == property);
        }

        public void PushProperty(string property)
        {
            _properties.Add(property);
        }

        public void RemoveProperty(string property)
        {
            _properties.Remove(property);
        }
        
        private void OnMouseEnter()
        {
            _gridManager.OnHighlightTile.Invoke(new(_tileRenderer, _meshFilter, _meshCollider, TilePoint));
        }

        private void OnMouseExit()
        {
            _gridManager.OnExitTile.Invoke(new(_tileRenderer, _meshFilter, _meshCollider, TilePoint));
        }

        private void OnMouseUp()
        {
            _gridManager.OnClickTile.Invoke(new(_tileRenderer, _meshFilter, _meshCollider, TilePoint));            
        }
    }
}
