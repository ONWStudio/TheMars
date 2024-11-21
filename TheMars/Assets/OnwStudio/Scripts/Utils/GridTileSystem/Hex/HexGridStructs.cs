using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using UnityEngine.Serialization;

namespace Onw.HexGrid
{
    [System.Serializable]
    public struct AxialCoordinates
    {
        [field: SerializeField, ReadOnly] public int Q { get; private set; }
        [field: SerializeField, ReadOnly] public int R { get; private set; }

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

        public AxialCoordinates(in HexCoordinates hexCoordinates) : this(hexCoordinates.Q, hexCoordinates.R) { }
        public AxialCoordinates(in Vector2Int vec) : this(vec.x, vec.y) { }
        public AxialCoordinates(in Vector2 vec) : this((int)vec.x, (int)vec.y) { }

        public override bool Equals(object obj)
        {
            return obj is AxialCoordinates other && Q == other.Q && R == other.R;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Q, R);
        }

        public override string ToString()
            => $"Q : {Q} R : {R}";
    }

    [System.Serializable]
    public struct HexCoordinates
    {
        [field: SerializeField, ReadOnly] public int Q { get; private set; }
        [field: SerializeField, ReadOnly] public int R { get; private set; }
        [field: SerializeField, ReadOnly] public int S { get; private set; }

        public static implicit operator Vector2(in HexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R);
        public static implicit operator Vector2Int(in HexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R);
        public static implicit operator Vector3(in HexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R, hexCoordinates.R);
        public static implicit operator Vector3Int(in HexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R, hexCoordinates.S);
        public static implicit operator HexCoordinates(in Vector2 vec) => new(vec);
        public static implicit operator HexCoordinates(in Vector2Int vec) => new(vec);
        public static implicit operator AxialCoordinates(in HexCoordinates hexCoordinates) => new(hexCoordinates);

        public HexCoordinates(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }

        public HexCoordinates(in AxialCoordinates axialCoordinates) : this(axialCoordinates.Q, axialCoordinates.R) { }
        public HexCoordinates(in Vector2Int vec) : this(vec.x, vec.y) { }
        public HexCoordinates(in Vector2 vec) : this((int)vec.x, (int)vec.y) { }

        public override bool Equals(object obj)
        {
            return obj is HexCoordinates other && Q == other.Q && R == other.R && S == other.S;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Q, R, S);
        }

        public override string ToString()
            => $"Q : {Q} R : {R} S : {S}";
    }
}