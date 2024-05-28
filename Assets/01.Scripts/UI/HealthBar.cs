using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _barTrm;
    [SerializeField] private Health _health;
    [SerializeField] private Player _player;

    private void Start()
    {
        _health.currentHealth.OnValueChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(int previousValue, int newValue)
    {
        float ratio = newValue / (float)_player.Data.maxHealth;
        _barTrm.localScale = new Vector3(ratio, 1, 1);
    }
}
