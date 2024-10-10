using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;

namespace Onw.HexGrid
{
    [System.Serializable]
    public struct AxialCoordinates
    {
        [SerializeField, ReadOnly] private int _q;
        [SerializeField, ReadOnly] private int _r;

        public int Q => _q;
        public int R => _r;
        
        public static implicit operator Vector2(in AxialCoordinates axialCoordinates) => new(axialCoordinates._q, axialCoordinates._r);
        public static implicit operator Vector2Int(in AxialCoordinates axialCoordinates) => new(axialCoordinates._q, axialCoordinates._r);
        public static implicit operator AxialCoordinates(in Vector2 vec) => new(vec);
        public static implicit operator AxialCoordinates(in Vector2Int vec) => new(vec);
        public static implicit operator HexCoordinates(in AxialCoordinates axialCoordinates) => new(axialCoordinates);

        public AxialCoordinates(int q, int r)
        {
            _q = q;
            _r = r;
        }

        public AxialCoordinates(in HexCoordinates hexCoordinates) : this(hexCoordinates.Q, hexCoordinates.R) {}
        public AxialCoordinates(in Vector2Int vec) : this(vec.x, vec.y) {}
        public AxialCoordinates(in Vector2 vec) : this((int)vec.x, (int)vec.y) {}
        
        public override bool Equals(object obj)
        {
            return obj is AxialCoordinates other && _q == other._q && R == other._r;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_q, _r);
        }
        
        public override string ToString()
            => $"Q : {_q} R : {_r}";
    }

    [System.Serializable]
    public struct HexCoordinates
    {
        [SerializeField, ReadOnly] private int _q;
        [SerializeField, ReadOnly] private int _r;
        [SerializeField, ReadOnly] private int _s;

        public int Q => _q;
        public int R => _r;    
        public int S => _s;
        
        public static implicit operator Vector2(in HexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R);
        public static implicit operator Vector2Int(in HexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R);
        public static implicit operator Vector3Int(in HexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R, hexCoordinates.S);
        public static implicit operator HexCoordinates(in Vector2 vec) => new(vec);
        public static implicit operator HexCoordinates(in Vector2Int vec) => new(vec);
        public static implicit operator HexCoordinates(in Vector3Int vec) => new(vec);
        public static implicit operator AxialCoordinates(in HexCoordinates hexCoordinates) => new(hexCoordinates);
        
        public HexCoordinates(int q, int r)
        {
            _q = q;
            _r = r;
            _s = -q - r;
        }

        public HexCoordinates(in AxialCoordinates axialCoordinates) : this(axialCoordinates.Q, axialCoordinates.R) {}
        public HexCoordinates(in Vector2Int vec) : this(vec.x, vec.y) {}
        public HexCoordinates(in Vector2 vec) : this((int)vec.x, (int)vec.y) {}
        public HexCoordinates(in Vector3Int vec) : this(vec.x, vec.y)
        {
            _s = vec.z;
        }

        public override bool Equals(object obj)
        {
            return obj is HexCoordinates other && _q == other._q && _r == other._r && _s == other._s;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_q, _r, _s);
        }
        
        public override string ToString()
            => $"Q : {_q} R : {_r} S : {_s}";
    }
}