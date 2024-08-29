using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.GridTile;
using Onw.ServiceLocator;

namespace TM.Grid
{
    public sealed class TMGridManager : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public bool IsBatchMode { get; set; } = false; 
        
        [SerializeField, InitializeRequireComponent] private GridManager _gridManager;
        
        
        
        private void Awake()
        {
            if (!ServiceLocator<TMGridManager>.RegisterService(this))
            {
                ServiceLocator<TMGridManager>.ChangeService(this);
            }
        }
        
        private void Start()
        {
            _gridManager.OnHighlightTile.AddListener(onHighlightTile);
            _gridManager.OnExitTile.AddListener(onExitTile);
        }

        private void onHighlightTile(TileArgs tileArgs)
        {
            if (!IsBatchMode) return;
            
            tileArgs.TileRenderer.material.color = Color.yellow;
        }

        private static void onExitTile(TileArgs tileArgs)
        {
            tileArgs.TileRenderer.material.color = Color.white;
        }
    }
}