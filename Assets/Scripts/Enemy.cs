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
    [SerializeField] private bool _isChannel;
    [SerializeField] private float _deathTime = 0.1f;
    [SerializeField] private AudioClip _spellClip;
    private float _timeElapsed;
    private bool _isCooldownUp = true;
    private bool _spellCasted;
    private AnimationName _animationName;
    
    
    public static Action<Enemy> OnDeath;
    public static Action<Enemy> OnCastSpell;

    protected override void Awake()
    {
        base.Awake();
        _animationName = GetComponent<AnimationName>();
    }

    protected override void Update()
    {
        base.Update();
        if (_isChannel) return;
        if (!_spellCasted) return;
        _timeElapsed += Time.deltaTime;
        CheckCooldown();
    }

    private void CheckCooldown()
    {
        if (_timeElapsed < _cooldown) return;
        _isCooldownUp = true;
        _spellCasted = false;
        _timeElapsed = 0.0f;
    }

    public void CastSpell(Player player)
    {
        if (!_isCooldownUp || (_spellCasted)) return;
        if (_isChannel)
        {
            _projectile.gameObject.SetActive(true);
            _spellCasted = true;
        }
        else
        {
            Projectile projectile = Instantiate(_projectile, _launchPosition.position, _launchPosition.rotation);
            _spellCasted = true;
            _timeElapsed = 0;
            _isCooldownUp = false;
        }
        OnCastSpell?.Invoke(this);
    }

    public void StopCast()
    {
        if (_isChannel)
        {
            _projectile.gameObject.SetActive(false);
        }
        _spellCasted = false;
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
        _animationName.PlayDeath();
        yield return new WaitForSeconds(_deathTime);
        OnDeath?.Invoke(this);
    }

    public AudioClip GetSpellClip()
    {
        return _spellClip;
    }
}
