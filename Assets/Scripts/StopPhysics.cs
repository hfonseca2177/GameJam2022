
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class StopPhysics: MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Props"))
            {
                Props props = col.GetComponent<Props>();
                if (props != null)
                {
                    props.StopMovement();
                }
            }
                
        }
    }
