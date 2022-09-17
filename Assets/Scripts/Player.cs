using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _movementSpeed;
    private Vector2 _direction;

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
       OnMove();
    }

    public void OnMove()
    {
        _direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_direction.magnitude > 0)
        {
            _rigidbody2D.velocity = new Vector2(_direction.x * _movementSpeed, _rigidbody2D.velocity.y);
        }
        
    }

    public void OnClick()
    {
        Debug.Log("CLICk");
        
    }

    #endregion

}
