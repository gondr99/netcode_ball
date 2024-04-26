using Unity.Netcode;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle,
    Run,
    Hit,
    Air
}

public class Player : NetworkBehaviour
{
    public PlayerInput PlayerInput { get; private set; }

    [field: SerializeField] public CharDataSO Data { get; private set; }

    public PlayerMovement MovementCompo { get; private set; }
    public Animator AnimCompo { get; private set; }

    public bool canStateChangeable = true;
    public bool isDead;

    private PlayerStateMachine stateMachine;


    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        MovementCompo = GetComponent<PlayerMovement>();
        MovementCompo.Initialize(this);
        AnimCompo = transform.Find("Visual").GetComponent<Animator>();

        stateMachine = new PlayerStateMachine();

        stateMachine.AddState(PlayerStateEnum.Idle, new PlayerIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(PlayerStateEnum.Run, new PlayerRunState(this, stateMachine, "Run"));
        stateMachine.AddState(PlayerStateEnum.Air, new PlayerAirState(this, stateMachine, "Air"));
    }

    private void Start()
    {
        stateMachine.InitState(PlayerStateEnum.Idle, this);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            PlayerInput.OnFireKeyEvent += HandleFireKeyEvent;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            PlayerInput.OnFireKeyEvent -= HandleFireKeyEvent;
        }
    }

    private void HandleFireKeyEvent()
    {

    }

    #region Flip Character
    public bool IsFacingRight()
    {
        return Mathf.Approximately(transform.eulerAngles.y, 0);
    }

    public void HandleSpriteFlip(Vector3 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
    #endregion

    private void Update()
    {
        if (!IsOwner) return;

        stateMachine.currentState.Update();

        Vector3 mousePos = PlayerInput.MousePosition;
        HandleSpriteFlip(mousePos);
    }

}

