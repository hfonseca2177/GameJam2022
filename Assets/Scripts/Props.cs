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
    }
