using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Onw.HexGrid
{
    public interface IHexGrid
    {
        IReadOnlyList<string> Properties { get; }
        HexCoordinates HexPoint { get; }
        Vector3 TilePosition { get; }
        Vector2 NormalizedPosition { get; }
        event UnityAction<IHexGrid> OnHighlightTile;
        event UnityAction<IHexGrid> OnExitTile;
        event UnityAction<IHexGrid> OnMouseDownTile;
        event UnityAction<IHexGrid> OnMouseUpTile;
        void AddProperty(string property);
        bool RemoveProperty(string property);
    }

}

