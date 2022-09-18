

    using UnityEngine;
    using Cinemachine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraSwitcher: MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private int _priority = 0;

        private void OnTriggerEnter2D(Collider2D col)
        {
            _camera.Priority = _priority;
        }
    }
