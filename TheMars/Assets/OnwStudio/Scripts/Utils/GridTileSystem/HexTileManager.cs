using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using UnityEngine.Pool;

namespace Onw.GridTile
{
    public sealed class HexTileManager : MonoBehaviour
    {
        [field: SerializeField] public float TileRadius { get; set; } = 1f;
        [field: SerializeField] public int MapWidth { get; set; } = 60;
        [field: SerializeField] public int MapHeight { get; set; } = 60;
        [field: SerializeField] public LayerMask Layer { get; set; }
        [field: SerializeField] public Material LineMaterial { get; set; }

        [InspectorButton("Generate Hex Tile Map")]
        public void GenerateHexTileMap()
        {
            float tileWidth = TileRadius * Mathf.Sqrt(3);
            float tileHeight = TileRadius * 1.5f;
            
            for (int row = 0; row < MapHeight; row++)
            {
                for (int col = 0; col < MapWidth; col++)
                {
                    float x = col * tileWidth + (row % 2 == 0 ? 0 : tileWidth / 2);
                    float z = row * tileHeight;
                    Vector3 tilePosition = new(x, 0, z);
                    GameObject hexTileObj = new($"HexTile_{row}_{col}");
                    HexTile hexTile = hexTileObj.AddComponent<HexTile>();
                    hexTile.InitializeTile(tilePosition, TileRadius, Layer, LineMaterial);
                }
            }
        }
    }
}