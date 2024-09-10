using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Event;
using UnityEngine.Events;

namespace Onw.GridTile
{
    public interface IReadOnlyGridRows
    {
        public IReadOnlyList<GridTile> Rows { get; }
    }
        
    [System.Serializable]
    public sealed class GridRows : IReadOnlyGridRows
    {
        public IReadOnlyList<GridTile> Rows => Row;

        [field: FormerlySerializedAs("_row")]
        [field: SerializeField, FormerlySerializedAs("<Row>k_BackingField")] public List<GridTile> Row { get; set; } = new();
    }
    
    public sealed class GridManager : MonoBehaviour
    {
        [System.Serializable]
        public sealed class BakeOption
        {
            [field: SerializeField] public bool MaintainingProperties { get; set; } = true;
        }
        
        private const int GRID_SIZE_MIN = 5;
        private const int GRID_SIZE_MAX = 10;

        public int TileCount => GridSize * GridSize;
        
        public event UnityAction<GridTile> OnHighlightTile
        {
            add => _onHighlightTile.AddListener(value);
            remove => _onHighlightTile.RemoveListener(value);
        }
        
        public event UnityAction<GridTile> OnExitTile
        {
            add => _onExitTile.AddListener(value);
            remove => _onExitTile.RemoveListener(value);
        }
        
        public event UnityAction<GridTile> OnMouseDownTile
        {
            add => _onMouseDownTile.AddListener(value);
            remove => _onMouseDownTile.RemoveListener(value);
        }
        
        public event UnityAction<GridTile> OnMouseUpTile
        {
            add => _onMouseUpTile.AddListener(value);
            remove => _onMouseUpTile.RemoveListener(value);
        }

        [field: SerializeField, Range(5, 50)] public float TileSize { get; set; }
        [field: SerializeField, Range(GRID_SIZE_MIN, GRID_SIZE_MAX)] public int GridSize { get; set; } = 5;

        [Header("Events")]
        [SerializeField] private UnityEvent<GridTile> _onHighlightTile = new();
        [SerializeField] private UnityEvent<GridTile> _onExitTile = new();
        [SerializeField] private UnityEvent<GridTile> _onMouseUpTile = new();
        [SerializeField] private UnityEvent<GridTile> _onMouseDownTile = new();

        [SerializeField] private List<GridRows> _tileList = new();

        [field: FormerlySerializedAs("_bakeOption")]
        [field: SerializeField] public BakeOption BakeSettings { get; private set; } = new();

        public IReadOnlyList<IReadOnlyGridRows> TileList => _tileList;

        public bool TryGetTileDataByRay(Ray ray, out GridTile tileData)
        {
            tileData = null;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                tileData = _tileList
                    .SelectMany(row => row.Rows)
                    .FirstOrDefault(tileData => tileData?.MeshCollider == hit.collider);
                
                return tileData;
            }

            return false;
        }
        
        private void Start()
        {
            foreach (GridTile tile in _tileList.SelectMany(rows => rows.Row))
            {
                tile.OnHighlightTile += _onHighlightTile.Invoke;
                tile.OnMouseDownTile += _onMouseDownTile.Invoke;
                tile.OnMouseUpTile += _onMouseUpTile.Invoke;
                tile.OnExitTile += _onExitTile.Invoke;
            }
        }

        public void BakeTiles()
        {
            Debug.Log("Bake");
            
            // GridManager 오브젝트의 위치를 중심으로 격자 시작 위치 설정
            Vector3 startPosition = transform.position;
        
            // 왼쪽 아래에서 시작하도록 오프셋 계산
            float calcTileSize = GridSize * TileSize;
            float startX = startPosition.x - calcTileSize * 0.5f;
            float startZ = startPosition.z - calcTileSize * 0.5f;

            bool canMaintainingProperties = BakeSettings.MaintainingProperties && _tileList.Count == GridSize;
            
            List<List<List<string>>> properties = canMaintainingProperties ? 
                _tileList
                    .Select(rows => rows.Row.Select(tile => tile.Properties).ToList())
                    .ToList() : 
                null;
            
            foreach (GridRows rows in _tileList)
            {
                rows.Row.ForEach(tile => DestroyImmediate(tile.gameObject));
                rows.Row.Clear();
                rows.Row = null;
            }

            _tileList.Clear();
            
            Material material = new(Shader.Find("Universal Render Pipeline/Lit"))
            {
                color = Color.white
            };

            for (int x = 0; x < GridSize; x++)
            {
                GridRows gridRows = new();
                _tileList.Add(gridRows);

                for (int y = 0; y < GridSize; y++)
                {
                    // 타일 생성
                    Vector3 tilePosition = new(
                        startX + x * TileSize,
                        startPosition.y,
                        startZ + y * TileSize);

                    // 새 GameObject 생성
                    GameObject tileObject = new("Tile")
                    {
                        transform =
                        {
                            // 타일 위치 설정
                            position = tilePosition
                        }
                    };

                    tileObject.transform.SetParent(gameObject.transform);
                    GridTile gridTile = tileObject.AddComponent<GridTile>();
                    gridTile.CreateTile(this, material, TileSize, new(x, y));

                    if (canMaintainingProperties)
                    {
                        gridTile.Properties.AddRange(properties[x][y]);
                    }

                    gridRows.Row.Add(gridTile);
                }
            }
        }
    }
}