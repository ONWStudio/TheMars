using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.TileSystem
{
    public sealed class Tile3DBuilder : MonoBehaviour
    {
        private const float TILE_MIN = 10f;

        public float TileSize
        {
            get => _tileSize;
            set => _tileSize = Mathf.Min(value);
        } 
        
        [field: SerializeField] public GameObject PivotObject { get; private set; } = null;
     
        [SerializeField, Min(TILE_MIN)] private float _tileSize = TILE_MIN;
    }
}