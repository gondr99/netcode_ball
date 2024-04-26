using UnityEngine;

public class ReadyStatusDisplay : MonoBehaviour
{
    [SerializeField] private Player _player;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _player.isReady.OnValueChanged += HandleReadyStatusChanged;
        HandleReadyStatusChanged(false, _player.isReady.Value);
    }

    private void OnDestroy()
    {
        _player.isReady.OnValueChanged -= HandleReadyStatusChanged;
    }

    private void HandleReadyStatusChanged(bool previousValue, bool newValue)
    {
        _spriteRenderer.enabled = newValue;
    }
}
