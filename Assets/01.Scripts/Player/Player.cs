using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.U2D.Animation;

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
    public ProjectileLauncher LauncherCompo { get; private set; }
    public Collider2D ColliderCompo { get; private set; }
    public Health HealthCompo { get; private set; }

    [HideInInspector] public bool canStateChangeable = true;
    public bool isDead;

    private PlayerStateMachine stateMachine;

    [HideInInspector] public NetworkVariable<int> selectCharacterIndex;
    [HideInInspector] public NetworkVariable<FixedString64Bytes> userName;
    [HideInInspector] public NetworkVariable<bool> isReady;

    [HideInInspector] public Vector2 knockBackPower;

    private SpriteLibrary _spriteLib;
    private Transform _visualTrm;


    private void Awake()
    {
        selectCharacterIndex = new NetworkVariable<int>(0);
        userName = new NetworkVariable<FixedString64Bytes>();
        isReady = new NetworkVariable<bool>(false);

        PlayerInput = GetComponent<PlayerInput>();
        MovementCompo = GetComponent<PlayerMovement>();
        MovementCompo.Initialize(this);
        ColliderCompo = GetComponent<Collider2D>();
        HealthCompo = GetComponent<Health>();

        _visualTrm = transform.Find("Visual");
        AnimCompo = _visualTrm.GetComponent<Animator>();
        _spriteLib = _visualTrm.GetComponent<SpriteLibrary>();

        LauncherCompo = transform.Find("ArrowParent").GetComponent<ProjectileLauncher>();
        LauncherCompo.Initialize(this);

        stateMachine = new PlayerStateMachine();

        stateMachine.AddState(PlayerStateEnum.Idle, new PlayerIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(PlayerStateEnum.Run, new PlayerRunState(this, stateMachine, "Run"));
        stateMachine.AddState(PlayerStateEnum.Air, new PlayerAirState(this, stateMachine, "Air"));
        stateMachine.AddState(PlayerStateEnum.Hit, new PlayerHitState(this, stateMachine, "Hit"));
    }

    private void Start()
    {
        stateMachine.InitState(PlayerStateEnum.Idle, this);
    }

    public void SetHit(Vector2 knockBackPower)
    {
        this.knockBackPower = knockBackPower;
        LauncherCompo.CancelCharge();
        stateMachine.ChangeState(PlayerStateEnum.Hit);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            PlayerInput.OnFireKeyEvent += HandleFireKeyEvent;
            GameManager.Instance.ActivePlayer = this;
        }

        if(IsServer)
        {
            UserData data =  AppHost.Instance.NetServer.GetUserData(OwnerClientId);
            userName.Value = data.playerName;
        }

        selectCharacterIndex.OnValueChanged += HandleCharacterChanged;
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            PlayerInput.OnFireKeyEvent -= HandleFireKeyEvent;
            GameManager.Instance.ActivePlayer = null;
        }
        selectCharacterIndex.OnValueChanged -= HandleCharacterChanged;
    }

    

    private void HandleCharacterChanged(int previousValue, int newValue)
    {
        Data = GameManager.Instance.GetCharDataByIndex(newValue);
        _spriteLib.spriteLibraryAsset = Data.spriteSet;
    }

    private void HandleFireKeyEvent(bool isDown)
    {
        if (MovementCompo.isKnockBack) return;

        LauncherCompo.SetCharge(isDown);
    }

    #region Flip Character
    public bool IsFacingRight()
    {
        return Mathf.Approximately(_visualTrm.eulerAngles.y, 0);
    }

    public void HandleSpriteFlip(Vector3 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            _visualTrm.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            _visualTrm.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
    #endregion

    private void Update()
    {
        if (!IsOwner) return;

        if (MovementCompo.isKnockBack) return; //in knockback state, return

        stateMachine.currentState.Update();

        Vector3 mousePos = PlayerInput.MousePosition;
        HandleSpriteFlip(mousePos);
    }


    [ServerRpc]
    public void SelectCharacterServerRpc(int index)
    {
        GameManager.Instance.SelectCharacter(index, OwnerClientId);
    }
}

