using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public sealed class SpriteManager : Singleton<SpriteManager>
{
    [Header("Sprite Atlases")]
    [SerializeField] private StringSpriteDictionary _spriteDictionary = new();

    protected override void Init()
    {

    }

    public Sprite GetSprite(string name)
        => _spriteDictionary.TryGetValue(name, out Sprite sprite) ? sprite : null;

    public void SetImageSprite(string name, Image image)
    {
        Sprite sprite = GetSprite(name);

        image.sprite = sprite;
        image.enabled = sprite;
    }

    public void SetSpriteRendererSprite(string name, SpriteRenderer spriteRenderer)
    {
        Sprite sprite = GetSprite(name);

        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = sprite;
    }
}
