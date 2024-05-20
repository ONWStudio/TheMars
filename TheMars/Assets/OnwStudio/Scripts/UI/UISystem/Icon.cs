using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class Icon : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] Image _petIconImage;

    public ushort PetNo { get; private set; }

    public void SetIcon(ushort petNo)
    {
        PetNo = petNo;
        Sprite icon = PetInfo.GetIcon(PetNo);

        _petIconImage.enabled = icon;
        _petIconImage.sprite = icon;
    }
}
