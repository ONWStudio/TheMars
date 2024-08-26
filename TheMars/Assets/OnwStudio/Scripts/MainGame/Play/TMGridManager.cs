using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.GridTile;

namespace TM.Grid
{
    public readonly struct TMGrid
    {
        
    }
    
    public sealed class TMGridManager : MonoBehaviour
    {
        [SerializeField, InitializeRequireComponent] private GridManager _gridManager;
        
        private void Start()
        {
            _gridManager.OnHighlightTile.AddListener(tileArgs => tileArgs.TileRenderer.material.color = Color.yellow);
            _gridManager.OnExitTile.AddListener(tileArgs => tileArgs.TileRenderer.material.color = Color.white);
        }
    }
}