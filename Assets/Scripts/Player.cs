using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _fallForce = 1;
    private Vector2 _direction;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private LayerMask _groundMask;

    private Vector2 _gravity;

    public Vector3 Position => transform.position;

    public Vector3 Velocity => _rigidbody2D.velocity;

    public void Displace(Vector3 newPosition)
    {
        _rigidbody2D.MovePosition(newPosition);
    }

    #region BuiltinMethods

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

    }

    private void Start()
    {
        _gravity = new Vector2(0, -Physics2D.gravity.y);
        Debug.Log(-Physics2D.gravity.y);
    }

    private void OnEnable()
    {
        TargetableObject.OnTargetClick += OnTargetClickEvent;
    }

    private void OnTargetClickEvent(TargetableObject obj)
    {
        if (obj.IsValidTarget(this))
        {
            obj.DisplaceWithForce(this);
        }
        
    }

    private void OnDisable()
    {

        TargetableObject.OnTargetClick -= OnTargetClickEvent;
    }

    private void Update()
    {
        CheckGround();
        if (IsGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    { 
       
        //Jump();
       OnMove();
    }

    private void CheckGround()
    {
        var bounds = _boxCollider2D.bounds;
       RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, 0.5f, _groundMask);
       IsGrounded = hit.collider != null;
       /*//Falling 
       if (_rigidbody2D.velocity.y < 0)
       {
           _rigidbody2D.velocity -= _gravity * _fallForce * Time.deltaTime;
       }*/
    }

    private void OnMove()
    {
        _direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_direction.magnitude > 0)
        {
            _rigidbody2D.velocity = new Vector2(_direction.x * _movementSpeed, _rigidbody2D.velocity.y);
        }
        
    }

    private void Jump()
    {
        //_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    
    }


    #endregion

}
