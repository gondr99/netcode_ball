using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth;

    private Player _owner;

    private void Awake()
    {
        currentHealth = new NetworkVariable<int>();
        _owner = GetComponent<Player>();
    }

    public void ResetHealth()
    {
        currentHealth.Value = _owner.Data.maxHealth;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            ResetHealth();
        }
        currentHealth.OnValueChanged += HandleHealthChanged;
        HandleHealthChanged(0, currentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        currentHealth.OnValueChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int previousValue, int newValue)
    {
        //change gague, etc..
    }

    #region Server OnlySection
    public void TakeDamage(int damage, Vector3 normal, float power)
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value - damage, 0, _owner.Data.maxHealth);

        //only send to owner
        ClientRpcParams sendParam = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { OwnerClientId }
            }
        };

        SetHitStateToOwnerClientRpc(-normal * power, sendParam);
    }
    #endregion

    //this rpc will be invoke with only owner
    [ClientRpc]
    public void SetHitStateToOwnerClientRpc(Vector3 knockBackPower, ClientRpcParams param)
    {
        _owner.SetHit(knockBackPower);
    }
}
