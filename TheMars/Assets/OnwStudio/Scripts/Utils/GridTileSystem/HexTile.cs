using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Onw.Attribute;
using Onw.Extensions;
using UnityEngine;

namespace Onw.GridTile
{
    public class HexTile : MonoBehaviour
    {
        [SerializeField, InitializeRequireComponent] private LineRenderer _lineRenderer;
        [SerializeField, ReadOnly] private Vector3[] _hexCorners = new Vector3[7];
        
        public LayerMask LayerMask { get; private set; }
        public float TileRadius { get; }
        public Material LineMaterial { get; private set; }

        public void InitializeTile(in Vector3 tilePosition, float radius, in LayerMask layer, Material material)
        {
            LayerMask = layer;
            transform.position = tilePosition;

            _lineRenderer = gameObject.GetOrAddComponent<LineRenderer>();
            _lineRenderer.positionCount = 7;
            _lineRenderer.loop = true;
            _lineRenderer.widthMultiplier = 0.1f;
            _lineRenderer.material = LineMaterial;
            _lineRenderer.useWorldSpace = false;

            for (int i = 0; i < 6; i++)
            {
                float angleDeg = 60 * i;
                float angleRad = Mathf.PI / 180 * angleDeg;
                _hexCorners[i] = new(radius * Mathf.Cos(angleRad), 0, radius * Mathf.Sin(angleRad));
            }
            
            _hexCorners[6] = _hexCorners[0];  // 첫 번째 코너로 돌아옴
            
            for (int i = 0; i < _hexCorners.Length; i++)
            {
                Vector3 worldPoint = transform.position + _hexCorners[i];
                Ray ray = new Ray(worldPoint + Vector3.up * 10, Vector3.down);

                if (Physics.Raycast(ray, out RaycastHit hit, 20f, LayerMask))
                {
                    _hexCorners[i].y = hit.point.y;
                }
            }
            
            _lineRenderer.SetPositions(_hexCorners);
        }
    }
}