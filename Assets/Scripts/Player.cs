using System;
using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

/// <summary>
/// Player character
/// </summary>
public class Player : MonoBehaviour
{
    
    [Header("Movement variables")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckDistance = 0.5f;

    [Header("Targetable range from player")] 
    [SerializeField] private float _range;
    [SerializeField] private LayerMask _targetableMask;
    [Header("Debug range")]
    [SerializeField] private bool _enableGizmos;
    [SerializeField] private Color _gizmosColor = Color.cyan;
    [SerializeField] private bool _logAllEvents;
    
    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _boxCollider2D;
    private bool _isGrounded;
    private Vector2 _direction;

    //Properties
    public Vector3 Position => transform.position;

    public Vector2 Velocity => _rigidbody2D.velocity;

    //Events
    public static Action<Player> OnDeath;
    public static Action<Player> OnDisplace;
    public static Action<Player> OnHasAvailableTarget;
    


    #region BuiltinMethods

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        TargetableObject.OnTargetClick += OnTargetClickEvent;
    }


    private void OnDisable()
    {
        TargetableObject.OnTargetClick -= OnTargetClickEvent;
    }

    private void Update()
    {
        CheckGround();
        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        
    }

    private void FixedUpdate()
    {
        OnMove();
    }
    
    
    private void OnDrawGizmos()
    {
        if (!_enableGizmos) return;
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, _range);  
    }

    #endregion

    #region MovementRelated

    
    private void CheckGround()
    {
        var bounds = _boxCollider2D.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, _groundCheckDistance, _groundMask);
        _isGrounded = hit.collider != null;
    }

    private void OnMove()
    {
        _direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_direction.magnitude > 0)
        {
            if(_logAllEvents) Debug.Log("OnMove");
            _rigidbody2D.velocity = new Vector2(_direction.x * _movementSpeed, _rigidbody2D.velocity.y);
        }
        Flip();
    }

    private void Jump()
    {
        if(_logAllEvents) Debug.Log("Jump");
        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    #endregion

    private void Flip()
    {
        if (_direction.x == 0) return;
        if(_logAllEvents) Debug.Log("Flip");
        Vector2 characterScale = transform.localScale;
        characterScale.x = (_direction.x < 0) ? 1 : -1;
        transform.localScale = characterScale;
    }
    
    public void Displace(Vector2 newPosition)
    {
        if(_logAllEvents) Debug.Log("Displace");
        //_rigidbody2D.MovePosition(newPosition);
        _rigidbody2D.transform.position = newPosition;
        //_rigidbody2D.velocity = Vector2.zero;
    }
    
    private void OnTargetClickEvent(TargetableObject targetable)
    {
        if(_logAllEvents) Debug.Log("OnTargetClickEvent");
        if (!targetable.IsValidTarget()) return;
        targetable.DisplaceWithForce(this);
        OnDisplace?.Invoke(this);
    }


    public void RespawnAt(Vector2 checkpoint)
    {
        if(_logAllEvents) Debug.Log("RespawnAt");
        _rigidbody2D.MovePosition(checkpoint);
    }

    public void TakeDamage()
    {
        if(_logAllEvents) Debug.Log("TakeDamage");
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2);
        OnDeath?.Invoke(this);
    }
    
    private void CheckValidTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _range, _targetableMask);
        if (hit != null)
        {
            /*TargetableObject[] targetableObjects = hit.GetComponents<TargetableObject>();
            OnHasAvailableTarget?.Invoke(this, targetableObjects);*/
            OnHasAvailableTarget?.Invoke(this);
        }
            
    }
    

}
