using System;
using UnityEngine;

public class MsgCanvas : MonoSingleton<MsgCanvas>
{
    [SerializeField] private MsgBox _msgPrefab;

    private MsgBox _currentBox;
    private float _timer = 0;

    public void ShowMsg(string text, float timeOut)
    {
        if (_currentBox != null)
        {
            CloseCurrent(null);
        }

        _timer = timeOut;
        _currentBox = Instantiate(_msgPrefab, transform);
        _currentBox.Text = text;
        _currentBox.RectTrm.anchoredPosition = new Vector2(0, 120f);
        _currentBox.MoveAnchor(new Vector2(0, -30f), null);
    }

    private void Update()
    {
        if (_currentBox != null)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                CloseCurrent(null);
            }
        }
    }

    private void CloseCurrent(Action Callback)
    {
        var target = _currentBox;
        target.CloseTweenIfExist();
        target.MoveAnchor(new Vector2(0, 120f), () =>
        {
            Destroy(target.gameObject);
            if(target == _currentBox)
                _currentBox = null;
            Callback?.Invoke();
        });
    }
}
