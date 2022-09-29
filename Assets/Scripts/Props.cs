using System.Collections;
using System;
using UnityEngine;

    /// <summary>
    /// Any scenario object that can be used by the player 
    /// </summary>
    public class Props: TargetableObject
    {
        [SerializeField] private bool _destructible;
        public static Action<Props> OnDestroy;
        
        private bool _isMoving;
        public bool IsMoving => _isMoving;


        protected override void Update()
        {
            base.Update();
            CheckIsMoving();
        }

        public void TakeDamage()
        {
            if (_destructible)
            {
                StartCoroutine(DestroyProp());    
            }
        }

        private IEnumerator DestroyProp()
        { 
            yield return new WaitForSeconds(2);
            OnDestroy?.Invoke(this);
            Destroy(this);
        }

        public void StopMovement()
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            _rigidbody2D.velocity = Vector2.zero;
        }

        private void CheckIsMoving()
        {
            _isMoving = _rigidbody2D.velocity.magnitude > 1f;
        }
        
        
    }
