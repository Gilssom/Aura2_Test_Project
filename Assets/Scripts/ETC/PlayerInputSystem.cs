using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    private bool _attackInput;
    private bool _specialAttackInput;
    private Vector2 _movementInput;
    private bool _jumpInput;
    private bool _changeCameraModeInput;

    public bool AttackInput { get => _attackInput; }
    public bool SpecialAttackInput { get => _specialAttackInput; }
    public Vector2 MovementInput { get => _movementInput; }
    public bool JumpInput { get => _jumpInput; }
    public bool ChangeCameraModeInput { get => _changeCameraModeInput; }

    void Start()
    {

    }

    void Update()
    {
        //_attackInput = Input.GetMouseButtonDown(0);
        //_specialAttackInput = Input.GetMouseButtonDown(1);

       
        //_jumpInput = Input.GetButton("Jump");
        //_changeCameraModeInput = Input.GetKeyDown(KeyCode.F);
    }
}
