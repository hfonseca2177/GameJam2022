using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Objects that can be targetable
/// </summary>
public class TargetableObject : MonoBehaviour
{

    [SerializeField] private float _range;
    private Rigidbody2D _rigidbody2D;
    public static Action<TargetableObject> OnTargetClick;

    #region BuiltinMethods

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    #endregion
    
    public bool IsValidTarget(Player player)
    {
        return Vector3.Distance(player.Position,  - transform.position) <= _range;
    }

    public void DisplaceWithForce(Player player)
    {
        Displace(player);
        Vector3 playerVelocity = player.Velocity;
        //_rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(playerVelocity * _rigidbody2D.mass);
        
    }

    private void Displace(Player player)
    {
        Vector3 playerPosition = player.Position;
        Vector3 targetPosition = transform.position;
        _rigidbody2D.MovePosition(playerPosition);
        player.Displace(targetPosition);
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUp()
    {
        OnTargetClick?.Invoke(this);
    }
}
