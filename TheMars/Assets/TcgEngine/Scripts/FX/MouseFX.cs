﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TcgEngine.FX
{
    /// <summary>
    /// FX that follows the mouse
    /// </summary>

    public class MouseFX : MonoBehaviour
    {
        public float speed = 20f;

        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.forward, 0f);
            plane.Raycast(ray, out float dist);
            Vector3 tpos = ray.GetPoint(dist);
            transform.position = Vector3.Lerp(transform.position, tpos, speed * Time.deltaTime);
        }
    }
}
