
    using System;
    using UnityEngine;

    /// <summary>
    /// Checkpoint collider
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckpointMark: MonoBehaviour
    {

        public static Action<Vector2> OnCheckpointEnter;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                OnCheckpointEnter?.Invoke(transform.position);
            }
        }
    }
