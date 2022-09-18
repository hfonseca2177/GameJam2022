using System;
using UnityEngine;

/// <summary>
/// Objects that can be targetable and displaced with
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TargetableObject : MonoBehaviour
{

    [Header("Targetable range case is different from global player range")]
    [SerializeField] private float _range;
    [Header("Targetable is a fixed hook just used to displace to the its position")]
    [SerializeField] private bool IsHook;
    [SerializeField] private LayerMask _playerMask;
    [Header("Debug range")]
    [SerializeField] private bool _enableGizmos;
    [SerializeField] private Color _gizmosColor = Color.green;
    //Properties
    public Vector2 Position => transform.position;
    
    //Variables
    protected Rigidbody2D _rigidbody2D;
    
    //Events
    public static Action<TargetableObject> OnTargetClick;
    public static Action<TargetableObject> OnMouseOverTargetable;
    public static Action<TargetableObject> OnMouseExitTargetable;

    #region BuiltinMethods

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void OnDrawGizmos()
    {
        if (!_enableGizmos) return;
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    
    
    private void OnMouseUp()
    {
        OnTargetClick?.Invoke(this);
    }

    private void OnMouseOver()
    {
        if (IsValidTarget())
        {
            OnMouseOverTargetable?.Invoke(this);
        }
        
    }

    private void OnMouseExit()
    {
        if (IsValidTarget())
        {
            OnMouseExitTargetable?.Invoke(this);
        }
    }

    #endregion


    public bool IsValidTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _range, _playerMask);
        return hit != null;
    }

    public void DisplaceWithForce(Player player)
    {
        Displace(player);
        Vector3 playerVelocity = player.Velocity;
        if (IsHook) return;
        _rigidbody2D.constraints = RigidbodyConstraints2D.None;
        _rigidbody2D.AddForce(playerVelocity * _rigidbody2D.mass);
    }

    private void Displace(Player player)
    {
        Vector3 playerPosition = player.Position;
        Vector3 targetPosition = transform.position;
        if (!IsHook)
        {
            _rigidbody2D.MovePosition(playerPosition);
        }
        player.Displace(targetPosition);
    }
    
}
