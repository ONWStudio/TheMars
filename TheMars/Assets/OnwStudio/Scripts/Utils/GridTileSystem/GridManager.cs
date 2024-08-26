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
        private const int GRID_SIZE_MIN = 5;
        private const int GRID_SIZE_MAX = 10;

        [field: SerializeField, Range(5, 50)] public float TileSize { get; set; }
        [field: SerializeField, Range(GRID_SIZE_MIN, GRID_SIZE_MAX)] public int GridSize { get; set; } = 5;

        [field: Header("Events")]
        [field: SerializeField] public SafeUnityEvent<TileArgs> OnHighlightTile { get; private set; } = new();
        [field: SerializeField] public SafeUnityEvent<TileArgs> OnExitTile { get; private set; }= new();
        [field: SerializeField] public SafeUnityEvent<TileArgs> OnClickTile { get; private set; } = new();

        public Material CommonMaterial { get; private set; } = null;

        private void Awake()
        {
            CommonMaterial = new(Shader.Find("Universal Render Pipeline/Lit"))
            {
                color = Color.white
            };
        }
        
        private void Start()
        {
            generateTiles();
        }

        private void generateTiles()
        {
            // GridManager 오브젝트의 위치를 중심으로 격자 시작 위치 설정
            Vector3 startPosition = transform.position;
        
            // 왼쪽 아래에서 시작하도록 오프셋 계산
            float calcTileSize = GridSize * TileSize;
            float startX = startPosition.x - calcTileSize * 0.5f;
            float startZ = startPosition.z - calcTileSize * 0.5f;

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    // 타일 생성
                    Vector3 tilePosition = new(
                        startX + x * TileSize, 
                        startPosition.y, 
                        startZ + y * TileSize);

                    Vector2Int tilePoint = new(x, y);
                    
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
                    gridTile.GridManager = this;
                    gridTile.TilePoint = tilePoint;
                }
            }
        }
    }
}