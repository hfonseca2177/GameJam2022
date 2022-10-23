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
    protected Collider2D _collider2D;
    
    //Events
    public static Action<TargetableObject> OnTargetClick;
    public static Action<TargetableObject> OnMouseOverTargetable;
    public static Action<TargetableObject> OnMouseExitTargetable;
    public static Action<TargetableObject> OnPlayerCanTarget;
    public static Action<TargetableObject> OnPlayerCannotTarget;
    private float _targetLockRange = 2f;

    public struct CollisionInfo
    {
        public bool InRange;
        public bool IsTargetLocked;

    }

    #region BuiltinMethods

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (IsValidTarget())
        {
            OnPlayerCanTarget?.Invoke(this);
        }
        else
        {
            OnPlayerCannotTarget?.Invoke(this);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (!_enableGizmos) return;
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    
    
    private void OnMouseUp()
    {
        OnTargetClick?.Invoke(this);
    }

    private void OnMouseEnter()
    {
        if (IsValidTarget())
        {
            OnMouseOverTargetable?.Invoke(this);
        }
        
    }

    private void OnMouseExit()
    {
        OnMouseExitTargetable?.Invoke(this);
    }

    #endregion


    public bool IsValidTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _range, _playerMask);
        return hit != null;
    }
    
    public CollisionInfo IsValidTargetInfo()
    {
        CollisionInfo info;
        var position = transform.position;
        Collider2D hit = Physics2D.OverlapCircle(position, _range, _playerMask);
        info.InRange = hit != null;
        Vector3 pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(pointer, position);
        info.IsTargetLocked = distance < _targetLockRange;
        return info;
    }

    public void DisplaceWithForce(Player player)
    {
        Displace(player);
        /*Vector3 playerVelocity = player.Velocity;
        if (IsHook) return;
        _rigidbody2D.constraints = RigidbodyConstraints2D.None;
        _rigidbody2D.AddForce(playerVelocity * _rigidbody2D.mass);*/
    }

    private void Displace(Player player)
    {
        Vector3 playerPosition = player.Position;
        Vector3 targetPosition = transform.position;
        if (!IsHook)
        {
            //_rigidbody2D.MovePosition(playerPosition);
            _rigidbody2D.transform.position = playerPosition;
        }
        player.Displace(targetPosition);
    }
    
}
