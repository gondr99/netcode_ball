using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float XMove { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public event Action OnFireKeyEvent;
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
            OnFireKeyEvent?.Invoke();
        }

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
