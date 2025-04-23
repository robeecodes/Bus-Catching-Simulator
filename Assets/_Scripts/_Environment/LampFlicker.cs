using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LampFlicker : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] private AudioSource flickerSFX;

    private readonly float _maxBurst = 3.0f;
    private readonly float _maxInterval = 5.0f;
    private readonly float _maxFlicker = 0.2f;

    private float _baseIntensity;
    private float _timer;
    private float _delay;
    private bool _isFlickering;

    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;
    

    private void Start()
    {
        _baseIntensity = light.intensity;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if ((_timer > _delay) && !_isFlickering)
        {
            _delay = Random.Range(0, _maxInterval);
            StartCoroutine(ToggleLight(Random.Range(0, _maxBurst)));
            _timer = 0;
        }
    }

    private IEnumerator ToggleLight(float duration)
    {
        _isFlickering = true;
        float totalTime = 0;
        float flickerTimer = 0;
        float flickerInterval = Random.Range(0, _maxFlicker);
        
        flickerSFX.pitch = Random.Range(_minPitch, _maxPitch);

        while (totalTime < duration)
        {
            flickerSFX.enabled = true;
            totalTime += Time.deltaTime;
            flickerTimer += Time.deltaTime;
            
            float currentIntensity = light.intensity;
            float targetIntensity = Random.Range(0.5f, _baseIntensity);

            if (flickerTimer > flickerInterval)
            {
                light.intensity = Mathf.Lerp(currentIntensity, targetIntensity, flickerInterval);
                flickerInterval = Random.Range(0, _maxFlicker);
                flickerTimer = 0;
            }
            yield return null;
        }

        light.intensity = Mathf.Lerp(light.intensity, _baseIntensity, _maxFlicker);
        flickerSFX.enabled = false;
        _isFlickering = false;
    }
}