using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ProjectileLauncher : NetworkBehaviour
{
    [SerializeField] private Transform _arrowParentTrm;
    [SerializeField] private SpriteRenderer _arrowSprite;
    [SerializeField] private Transform _firePosTrm;

    private Player _player;
    private bool _isCharging = false;
    private float _currentPower = 0;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            _arrowSprite.enabled = false;
        }
    }

    public void Initialize(Player player)
    {
        _player = player;
    }

    public void CancelCharge()
    {
        _isCharging = false;
        _arrowSprite.enabled = false;
        _currentPower = 0;
    }

    public void SetCharge(bool value)
    {
        if (_isCharging && !value)
        {
            FireProjectile();
        }
        _isCharging = value;
        _arrowSprite.enabled = value;
    }

    private void FireProjectile()
    {
        CharDataSO data = _player.Data;
        Vector3 position = _firePosTrm.position;
        Vector2 force = _firePosTrm.right * _currentPower;
        SpawnDummyProjectile(position, force);
        FireServerRpc(position, force);
        _currentPower = 0;
    }

    private void SpawnDummyProjectile(Vector3 position,  Vector2 force)
    {
        Projectile projectile = Instantiate(_player.Data.projectilePrefab, position, Quaternion.identity);
        projectile.Fire(_player.ColliderCompo, force, _player.Data.damage, ProjectileRole.Client);
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 position, Vector2 force)
    {
        Projectile projectile = Instantiate(_player.Data.projectilePrefab, position, Quaternion.identity);
        projectile.Fire(_player.ColliderCompo, force, _player.Data.damage, ProjectileRole.Server);

        SpawnDummyProjectileClientRpc(position, force);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 position, Vector2 force)
    {
        if (IsOwner) return;

        SpawnDummyProjectile(position, force);
    }

    private void Update()
    {
        if (!IsOwner) return;

        CharDataSO data = _player.Data;
        if (_isCharging)
        {
            _currentPower += _player.Data.chargeSpeed * Time.deltaTime;
            _currentPower = Mathf.Clamp(_currentPower, 0, data.throwPower);
        }

        VisualAdjust(data);
    }

    private void VisualAdjust(CharDataSO data)
    {
        float spriteWidth = _currentPower / data.throwPower + 1f;
        _arrowSprite.size = new Vector2(spriteWidth, 1f);

        Vector2 dir = _player.PlayerInput.MousePosition - (Vector2) _arrowParentTrm.position;
        _arrowParentTrm.right = dir.normalized;
    }
}
