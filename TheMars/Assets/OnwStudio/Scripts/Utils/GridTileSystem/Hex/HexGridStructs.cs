using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace Onw.HexGrid
{
    [System.Serializable]
    public struct AxialCoordinates
    {
        [field: SerializeField] public int Q { get; set; }
        [field: SerializeField] public int R { get; set; }

        public static implicit operator Vector2(in AxialCoordinates axialCoordinates) => new(axialCoordinates.Q, axialCoordinates.R);
        public static implicit operator Vector2Int(in AxialCoordinates axialCoordinates) => new(axialCoordinates.Q, axialCoordinates.R);
        public static implicit operator AxialCoordinates(in Vector2 vec) => new(vec);
        public static implicit operator AxialCoordinates(in Vector2Int vec) => new(vec);
        public static implicit operator HexCoordinates(in AxialCoordinates axialCoordinates) => new(axialCoordinates);

        public AxialCoordinates(int q, int r)
        {
            Q = q;
            R = r;
        }

        public AxialCoordinates(in HexCoordinates axialCoordinates) : this(axialCoordinates.Q, axialCoordinates.R) {}
        public AxialCoordinates(in Vector2Int vec) : this(vec.x, vec.y) {}
        public AxialCoordinates(in Vector2 vec) : this((int)vec.x, (int)vec.y) {}
    }

    [System.Serializable]
    public struct HexCoordinates
    {
        [field: SerializeField] public int Q { get; set; }
        [field: SerializeField] public int R { get; set; }

        [field: SerializeField, ReadOnly] public int S { get; private set; }
        
        public static implicit operator Vector2(in HexCoordinates axialCoordinates) => new(axialCoordinates.Q, axialCoordinates.R);
        public static implicit operator Vector2Int(in HexCoordinates axialCoordinates) => new(axialCoordinates.Q, axialCoordinates.R);
        public static implicit operator HexCoordinates(in Vector2 vec) => new(vec);
        public static implicit operator HexCoordinates(in Vector2Int vec) => new(vec);
        public static implicit operator AxialCoordinates(in HexCoordinates hexCoordinates) => new(hexCoordinates);
        
        public HexCoordinates(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }

        public HexCoordinates(in AxialCoordinates axialCoordinates) : this(axialCoordinates.Q, axialCoordinates.R) {}
        public HexCoordinates(in Vector2Int vec) : this(vec.x, vec.y) {}
        public HexCoordinates(in Vector2 vec) : this((int)vec.x, (int)vec.y) {}
    }
}