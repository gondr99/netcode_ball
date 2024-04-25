using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public PlayerInput PlayerInput { get; private set; }

    [field: SerializeField] public CharDataSO Data { get; private set; }

    public PlayerMovement MovementCompo { get; private set; }

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        MovementCompo = GetComponent<PlayerMovement>();
        MovementCompo.Initialize(this);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            PlayerInput.OnJumpKeyEvent += HandleJumpKeyEvent;
            PlayerInput.OnFireKeyEvent += HandleFireKeyEvent;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            PlayerInput.OnJumpKeyEvent -= HandleJumpKeyEvent;
            PlayerInput.OnFireKeyEvent -= HandleFireKeyEvent;
        }
    }

    private void HandleFireKeyEvent()
    {

    }

    private void HandleJumpKeyEvent()
    {

    }

    private void Update()
    {
        if (!IsOwner) return;
        
        MovementCompo.SetXMove(PlayerInput.XMove);
        
    }

}
