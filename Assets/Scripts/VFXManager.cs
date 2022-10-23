
    using System.Collections;
    using UnityEngine;
    

    public class VFXManager: MonoBehaviour
    {

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _displaceSound;
        [SerializeField] private AudioClip _deathSound;
        [SerializeField] private AudioClip _impactSound;
        
        #region BuiltinMethodsdw

        private void OnEnable()
        {
            Player.OnDisplace += OnPlayerDisplaceEvent;
            Player.OnDeath += OnPlayerDeathEvent;
            Projectile.OnProjectileHit += OnProjectileHitEvent;
            CheckpointMark.OnCheckpointEnter += OnCheckpointEnterEvent;
            Enemy.OnDeath += OnEnemyDeathEvent;
            Enemy.OnCastSpell += OnEnemyCastSpellEvent;
            Props.OnDestroy += OnPropsDestroyEvent;
            AggroBox.OnAggroRange += OnAggroRangeEvent;
            Spike.OnEnemyHitSpike += OnEnemyHitSpikeEvent;
            Spike.OnPlayerHitSpike += OnPlayerHitSpikeEvent;
            
        }

        private void OnDisable()
        {
            Player.OnDisplace -= OnPlayerDisplaceEvent;
            Player.OnDeath -= OnPlayerDeathEvent;
            Projectile.OnProjectileHit -= OnProjectileHitEvent;
            CheckpointMark.OnCheckpointEnter -= OnCheckpointEnterEvent;
            Enemy.OnDeath -= OnEnemyDeathEvent;
            Enemy.OnCastSpell -= OnEnemyCastSpellEvent;
            Props.OnDestroy -= OnPropsDestroyEvent;
            AggroBox.OnAggroRange -= OnAggroRangeEvent;
            Spike.OnEnemyHitSpike -= OnEnemyHitSpikeEvent;
            Spike.OnPlayerHitSpike -= OnPlayerHitSpikeEvent;
        }

        #endregion
        
        
        private void OnCheckpointEnterEvent(Vector2 checkpoint)
        {
           //TODO play sound
        }


        private void OnProjectileHitEvent(Projectile projectile, Player player)
        {
            CameraShaker.Instance.ShakeCamera(5.0f,1.0f);
            _audioSource.PlayOneShot(_impactSound);
        }

        

        private void OnPlayerDeathEvent(Player player)
        {
            _audioSource.PlayOneShot(_deathSound);
        }

        private void OnPlayerDisplaceEvent(Player player)
        {
            CameraShaker.Instance.ShakeCamera(0.7f,0.5f);
            _audioSource.PlayOneShot(_displaceSound);
        }
        
        
        private void OnEnemyDeathEvent(Enemy enemy)
        {
            //TODO play sound and particle
        }
        
        
        private void OnEnemyCastSpellEvent(Enemy enemy)
        {
            AudioClip ac = enemy.GetSpellClip();
            if(ac!=null) _audioSource.PlayOneShot(ac);
        }

        
        private void OnPropsDestroyEvent(Props props)
        {
            //TODO play sound
        }

        
        private void OnAggroRangeEvent(Player player, Enemy enemy)
        {
            //TODO screen border red
        }
        
        
        private void OnPlayerHitSpikeEvent(Player player)
        {
            //TODO sfx
        }

        private void OnEnemyHitSpikeEvent(Enemy enemy)
        {
            //TODO sfx
        }
        
        
        
    }
