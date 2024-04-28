using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float XMove { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public event Action<bool> OnFireKeyEvent;
    public event Action OnJumpKeyEvent;

    void Update()
    {
        CheckMoveInput();
        CheckJumpInput();
        CheckMouseInput();
    }

    private void CheckMoveInput()
    {
        XMove = Input.GetAxisRaw("Horizontal");
    }

    private void CheckJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpKeyEvent?.Invoke();
        }
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnFireKeyEvent?.Invoke(true);
        }
        if(Input.GetMouseButtonUp(0))
        {
            OnFireKeyEvent?.Invoke(false);
        }

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
