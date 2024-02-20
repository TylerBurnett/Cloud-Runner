using Cinemachine;
using Game.Global;
using System.Collections;
using UnityEngine;

namespace Game.Player
{
    public delegate void CameraShakeEvent(float shakeTimeInSeconds);

    public delegate void CameraShakeEventWithIntensity(float shakeTimeInSeconds, float intensity);

    public class CameraShake : MonoBehaviour
    {
        private const float DefaultShakeIntensity = 3.0f;

        private CinemachineBasicMultiChannelPerlin _cameraShake;

        protected void Awake()
        {
            CinemachineVirtualCamera cinemaMachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cameraShake = cinemaMachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            EventService<CameraShakeEvent>.Register((float timeInSeconds) => StartCoroutine(ShakeCamera(timeInSeconds)));

            EventService<CameraShakeEventWithIntensity>.Register((float timeInSeconds, float intensity) => StartCoroutine(ShakeCamera(timeInSeconds, intensity)));
        }

        public IEnumerator ShakeCamera(float timeInSeconds, float intensity = DefaultShakeIntensity)
        {
            _cameraShake.m_AmplitudeGain = intensity;

            yield return new WaitForSeconds(timeInSeconds);

            _cameraShake.m_AmplitudeGain = 0;
        }
    }
}