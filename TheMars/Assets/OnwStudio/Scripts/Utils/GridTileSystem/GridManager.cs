using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Event;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Onw.GridTile
{
    public sealed class GridManager : MonoBehaviour
    {
        [System.Serializable]
        public class GridRows
        {
            [field: SerializeField] public List<GridTile> Row { get; set; } = new();
        }
        
        private const int GRID_SIZE_MIN = 5;
        private const int GRID_SIZE_MAX = 10;

        [field: SerializeField, Range(5, 50)] public float TileSize { get; set; }
        [field: SerializeField, Range(GRID_SIZE_MIN, GRID_SIZE_MAX)] public int GridSize { get; set; } = 5;

        [field: Header("Events")]
        [field: SerializeField] public SafeUnityEvent<TileArgs> OnHighlightTile { get; private set; } = new();
        [field: SerializeField] public SafeUnityEvent<TileArgs> OnExitTile { get; private set; }= new();
        [field: SerializeField] public SafeUnityEvent<TileArgs> OnClickTile { get; private set; } = new();

        [SerializeField] private List<GridRows> _tileList = new();
        
        public void BakeTiles()
        {
            Debug.Log("Bake");
            
            // GridManager 오브젝트의 위치를 중심으로 격자 시작 위치 설정
            Vector3 startPosition = transform.position;
        
            // 왼쪽 아래에서 시작하도록 오프셋 계산
            float calcTileSize = GridSize * TileSize;
            float startX = startPosition.x - calcTileSize * 0.5f;
            float startZ = startPosition.z - calcTileSize * 0.5f;

            foreach (GridRows rows in _tileList)
            {
                foreach (GridTile tile in rows.Row)
                {
                    DestroyImmediate(tile.gameObject);
                }

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
                    gridTile.CreateTile(this, material, new(x, y));
                    gridRows.Row.Add(gridTile);
                }
            }
        }
    }
}