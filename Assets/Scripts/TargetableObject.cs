using System;
using UnityEngine;

/// <summary>
/// Objects that can be targetable
/// </summary>
public class TargetableObject : MonoBehaviour
{

    [SerializeField] private float _range;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private bool IsHook;
    [SerializeField] private LayerMask _playerMask;
    
    public static Action<TargetableObject> OnTargetClick;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D cursorTextureOver;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    #region BuiltinMethods

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _range);  
    }

    public bool IsValidTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _range, _playerMask);
        return hit != null;
    }

    public void DisplaceWithForce(Player player)
    {
        Displace(player);
        Vector3 playerVelocity = player.Velocity;
        if (!IsHook)
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _rigidbody2D.AddForce(playerVelocity * _rigidbody2D.mass);
        }
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
    
    private void OnMouseUp()
    {
        OnTargetClick?.Invoke(this);
    }

    private void OnMouseOver()
    {
        if (IsValidTarget())
        {
            Cursor.SetCursor(cursorTextureOver, hotSpot, cursorMode);    
        }
        
    }

    private void OnMouseExit()
    {
        if (IsValidTarget())
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }
}
