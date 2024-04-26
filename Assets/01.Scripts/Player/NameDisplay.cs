using TMPro;
using Unity.Collections;
using UnityEngine;

public class NameDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private Player _player;

    private void Start()
    {
        _player.userName.OnValueChanged += HandleNameChange;
        HandleNameChange(string.Empty, _player.userName.Value);
    }

    private void OnDestroy()
    {
        _player.userName.OnValueChanged -= HandleNameChange;
    }

    private void HandleNameChange(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
        _nameText.text = newValue.ToString();
    }
}
