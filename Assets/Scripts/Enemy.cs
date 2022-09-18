using System.Collections;
using System;
using UnityEngine;

/// <summary>
/// Enemy character able to cast spells against player
/// </summary>
public class Enemy : TargetableObject
{

    [SerializeField] private Transform _launchPosition;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _cooldown = 4.0f;
    [SerializeField] private bool _isCastOnce;
    [SerializeField] private float _deathTime = 1.0f;
    private float _timeElapsed;
    private bool _isCooldownUp = true;
    private bool _spellCasted = false;
    

    public static Action<Enemy> OnDeath;
    public static Action<Enemy> OnCastSpell;

    private void Update()
    {
        if (_isCastOnce) return;
        _timeElapsed += Time.deltaTime;
        if (!_spellCasted) return;
        CheckCooldown();
    }

    private void CheckCooldown()
    {
        if (_timeElapsed > _cooldown)
        {
            _isCooldownUp = true;
            _spellCasted = false;
            _timeElapsed = 0.0f;
        }
        else
        {
            _isCooldownUp = false;
        }
    }

    public void CastSpell(Player player)
    {
        if (!_isCooldownUp || (_isCastOnce && _spellCasted)) return;
        Projectile projectile = Instantiate(_projectile, _launchPosition.position, _launchPosition.rotation);
        _spellCasted = true;
        OnCastSpell?.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            Player player = col.gameObject.GetComponent<Player>();
            player.TakeDamage();
        } else if (col.collider.CompareTag("Props"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    { 
        yield return new WaitForSeconds(_deathTime);
        OnDeath?.Invoke(this);
        Destroy(this);
    }
}
