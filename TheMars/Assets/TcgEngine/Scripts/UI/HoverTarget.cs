﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TcgEngine.UI
{
    /// <summary>
    /// Target in the scene that can be hovered (and text will appear)
    /// </summary>

    public class HoverTarget : MonoBehaviour
    {
        [TextArea(5, 7)]
        public string text;
        public float delay = 0.5f;
        public int text_size = 22;
        public int width = 350;
        public int height = 140;

        //private LangTableText ltable;
        private float timer = 0f;
        private bool hover = false;

        private void Awake()
        {
            //ltable = GetComponent<LangTableText>();
        }

        private void Start()
        {
            if (HoverTextBox.Get() == null)
            {
                Instantiate(AssetData.Get().hover_text_box, Vector3.zero, Quaternion.identity);
            }
        }

        private void Update()
        {
            if (hover)
            {
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    HoverTextBox.Get().Show(this);
                }
            }
        }

        public string GetText()
        {
            //if (ltable != null)
            //    return ltable.GetTranslation(text);
            return text;
        }

        private void OnMouseEnter()
        {
            if (GameUI.IsOverUI())
                return;

            timer = 0f;
            hover = true;
        }

        private void OnMouseExit()
        {
            timer = 0f;
            hover = false;
        }

        private void OnDisable()
        {
            hover = false;
        }

        public bool IsHover()
        {
            return hover;
        }
    }
}