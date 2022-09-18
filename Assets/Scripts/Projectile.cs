using System;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Spell projectable or particle
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : TargetableObject
{

    [SerializeField, Tooltip("Set to 0 or less to ignore it")] private float _timeLife=10f;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isFixed;
    //If time life is set to 0 or less, ignore it
    private const float IgnoreTimeLife = 0;
     
    private float _timeElapsed;
    
    public static Action<Projectile, Player> OnProjectileHit;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
        OnProjectileHit?.Invoke(this, player);
        player.TakeDamage();
        Dissipate();
    }

    private void Start()
    {
        if (_isFixed) return;
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.velocity = new Vector2(-transform.right.x * _speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;
        CheckLifeTime();
        
    }
    
    
    private void Dissipate()
    {
        Destroy(gameObject);
    }

    private void CheckLifeTime()
    {
        if (_timeLife <= IgnoreTimeLife) return;
        if (_timeElapsed > _timeLife)
        {
            Dissipate();
        }
    }
    
}
