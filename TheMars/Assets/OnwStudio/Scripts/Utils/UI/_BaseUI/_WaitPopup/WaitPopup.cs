using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class WaitPopup : BaseUI
{
    [Header("Wait Text")]
    [SerializeField] private TMP_Text _waitText;

    protected override void EnableUI()
        => StartCoroutine(iEWaitEvent());

    private IEnumerator iEWaitEvent()
    {
        string getDotFromCount(int count) => (count % 3) switch
        {
            0 => ".",
            1 => "..",
            2 => "...",
            _ => string.Empty
        };

        int count = 0;

        while (true)
        {
            yield return CoroutineExtensions.CoroutineHelper.WaitForSeconds(1f);
            count++;
            _waitText.text = $"잠깐 기다려주세요 {getDotFromCount(count)}";
        }
    }
}
