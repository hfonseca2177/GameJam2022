
    using System;
    using UnityEngine;

    /// <summary>
    /// Enemy aggro box
    /// </summary>
    public class AggroBox: MonoBehaviour
    {
        
        [SerializeField] private Enemy _enemy;
        public static Action<Player, Enemy> OnAggroRange;
        
        [Header("Debug range")]
        [SerializeField] private bool _enableGizmos;
        [SerializeField] private Color _gizmosColor = Color.red;

        [SerializeField] private BoxCollider2D _aggroBoxCollider2D;

        private void OnDrawGizmos()
        {
            if (!_enableGizmos) return;
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireCube(_aggroBoxCollider2D.bounds.center, _aggroBoxCollider2D.size);
        }
        private void OnTriggerStay2D(Collider2D other)
        {   
            if (!other.CompareTag("Player")) return;
            Player player = other.gameObject.GetComponent<Player>();
            OnAggroRange?.Invoke(player, _enemy);
            _enemy.CastSpell(player);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _enemy.StopCast();
        }
    }
