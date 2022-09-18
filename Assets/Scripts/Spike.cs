
    using System;
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class Spike: MonoBehaviour
    {

        public static Action<Player> OnPlayerHitSpike;
        public static Action<Enemy> OnEnemyHitSpike;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                Player player = col.gameObject.GetComponent<Player>();
                OnPlayerHitSpike?.Invoke(player);
                player.TakeDamage();
            } else if (col.CompareTag("Enemy"))
            {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();
                OnEnemyHitSpike?.Invoke(enemy);
                enemy.TakeDamage();
            }
          
        }
    }
