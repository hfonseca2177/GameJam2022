
using Cinemachine;
using UnityEngine;

public class CameraShaker: MonoBehaviour
{
    public static CameraShaker Instance { get; private set; }
    

    private ICinemachineCamera _currentVC;
    private float _elapsedTime;
    private float _duration = 0f;
    private float _initialIntensity;
    private bool _shakeIt;

    private void Awake()
    {
        Instance = this;
    }

    public void ShakeCamera(float intensity, float duration)
    {
        SetIntensity(intensity);
        _initialIntensity = intensity;
        _duration = duration;
        _elapsedTime = 0;
        _shakeIt = true;
    }

    private void Update()
    {
        if (_shakeIt)
        {
            if (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
            }
            else
            {
                _shakeIt = false;
            }
        }
        else
        {
            if (!(_elapsedTime > 0)) return;
            _elapsedTime -= Time.deltaTime;
            float intensity = Mathf.Lerp(_initialIntensity, 0, 1-(_elapsedTime / _duration));
            SetIntensity(intensity);
        }
    }

    private void SetIntensity(float intensity)
    {
        var brain = CinemachineCore.Instance.GetActiveBrain(0);
        _currentVC = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            ((CinemachineVirtualCamera)_currentVC).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if(cinemachineBasicMultiChannelPerlin!=null) cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }

}
