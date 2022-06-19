using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    public float speed;
    private float _inputX;
    private float _inputY;
    private Vector2 _movementInput;

    private Animator[] _animators;

    private bool _isMoving;

    private bool _inputDisable;
    
    //使用工具动画
    private float _mouseX;
    private float _mouseY;
    private bool _useTool;
    
    
    
    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        _useTool = true;
        _inputDisable = true;
        yield return null;
        foreach (var anim in _animators)
        {
            anim.SetTrigger("useTool");
            //人物的面朝方向
            anim.SetFloat("InputX", _mouseX);
            anim.SetFloat("InputY", _mouseY);
        }
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        yield return new WaitForSeconds(0.25f);
        //等待动画结束
        _useTool = false;
        _inputDisable = false;
    }
     
    
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animators = GetComponentsInChildren<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.MouseClickedEvent += OnMouseClickedEvent;
    }


    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.MouseClickedEvent += OnMouseClickedEvent;
    }

    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnAfterSceneLoadedEvent()
    {
        _inputDisable = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        _inputDisable = true;
    }
    private void OnMouseClickedEvent(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        if (_useTool)
            return;

        //TODO:执行动画
        if (itemDetails.itemType != ItemType.Seed && itemDetails.itemType != ItemType.Commodity && itemDetails.itemType != ItemType.Furniture)
        {
            _mouseX = mouseWorldPos.x - transform.position.x;
            _mouseY = mouseWorldPos.y - transform.position.y;

            if (Mathf.Abs(_mouseX) > Mathf.Abs(_mouseY))
                _mouseY = 0;
            else
                _mouseX = 0;

            StartCoroutine(UseToolRoutine(mouseWorldPos, itemDetails));
        }
        else
        {
            EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        }
    }

    private void Update()
    {
        if (_inputDisable == false)
            PlayerInput();
        else
            _isMoving = false;
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        if(!_inputDisable) 
            Movement();
    }

    private void PlayerInput()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");

        _movementInput = new Vector2(_inputX, _inputY).normalized;
        if (!Input.GetButton("Run"))
        {
            _movementInput *= 0.5f;
            _inputX *= 0.45f;
            _inputY *= 0.45f;
        }
        _isMoving = _movementInput != Vector2.zero;
    }

    private void Movement()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _movementInput * speed * Time.deltaTime);
    }

    private void SwitchAnimation()
    {
        foreach (var anim in _animators)
        {
            anim.SetBool("IsMoving", _isMoving);
            anim.SetFloat("MouseX", _mouseX);
            anim.SetFloat("MouseY", _mouseY);

            if (_isMoving)
            {
                anim.SetFloat("InputX", _inputX);
                anim.SetFloat("InputY", _inputY);
            }
        }
    }
}