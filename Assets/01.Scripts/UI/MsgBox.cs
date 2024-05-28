using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class MsgBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _msgText;

    public string Text { get => _msgText.text; set => _msgText.text = value; }

    public RectTransform RectTrm;

    private void Awake()
    {
        RectTrm = transform as RectTransform;
    }

    public void MoveAnchor(Vector2 anchorPos, Action Callback)
    {
        RectTrm.DOAnchorPos(anchorPos, 0.2f).OnComplete(()=> Callback?.Invoke());
    }

    public void CloseTweenIfExist()
    {
        RectTrm.DOKill();
    }
}
