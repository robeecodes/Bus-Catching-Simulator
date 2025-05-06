using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SmokeScreen : MonoBehaviour
{
    [SerializeField] private Volume volume;
    
    private DepthOfField _dof;
    private ColorAdjustments _colorAdjustments;
    
    private AudioSource _smokeSFX;

    [Header("Smoke Effect Settings")]
    [SerializeField] private float smokeDuration = 10f;
    
    // Default values for the properties
    private readonly float _defaultPostExposure = 0f;
    private readonly float _defaultContrast = 0f;
    private readonly float _defaultHueShift = 0f;
    private readonly float _defaultSaturation = 0f;
    private readonly float _defaultFocusDistance = 30f;
    
    private readonly float _targetPostExposure = 0.5f;
    private readonly float _targetContrast = -25.64f;
    private readonly float _targetHueShift = 180f;
    private readonly float _targetSaturation = -100f;
    private readonly float _targetFocusDistance = 1.3f;

    private void Start()
    {
        // Get volume components
        if (volume.profile.TryGet<DepthOfField>(out DepthOfField tmp))
        {
            _dof = tmp;
        }
        
        if (volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments cA))
        {
            _colorAdjustments = cA;
        }

        // Get the smoke SFX if present
        TryGetComponent<AudioSource>(out _smokeSFX);
    }

    public void SmokeUp()
    {
        GameManager.Instance.isScreenSmoke = true;
        _dof.active = true;
        _colorAdjustments.active = true;
        
        StartCoroutine(LerpEffects(_targetPostExposure, _targetContrast, _targetHueShift, _targetSaturation, _targetFocusDistance, smokeDuration));
    }

    public void SmokeDown()
    {
        GameManager.Instance.isScreenSmoke = false;   
        StartCoroutine(LerpEffects(_defaultPostExposure, _defaultContrast, _defaultHueShift, _defaultSaturation, _defaultFocusDistance, smokeDuration));
    }

    private IEnumerator LerpEffects(float targetPostExposure, float targetContrast, float targetHueShift, float targetSaturation, float targetFocusDistance, float duration)
    {
        float elapsedTime = 0f;

        // Store current values
        float startPostExposure = _colorAdjustments.postExposure.value;
        float startContrast = _colorAdjustments.contrast.value;
        float startHueShift = _colorAdjustments.hueShift.value;
        float startSaturation = _colorAdjustments.saturation.value;
        float startFocusDistance = _dof.focusDistance.value;

        if (!_dof.active) _dof.active = true;
        
        // Lerp to target values over time
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            _colorAdjustments.postExposure.value = Mathf.Lerp(startPostExposure, targetPostExposure, t);
            _colorAdjustments.contrast.value = Mathf.Lerp(startContrast, targetContrast, t);
            _colorAdjustments.hueShift.value = Mathf.Lerp(startHueShift, targetHueShift, t);
            _colorAdjustments.saturation.value = Mathf.Lerp(startSaturation, targetSaturation, t);

            _dof.focusDistance.value = Mathf.Lerp(startFocusDistance, targetFocusDistance, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set after lerp
        _colorAdjustments.postExposure.value = targetPostExposure;
        _colorAdjustments.contrast.value = targetContrast;
        _colorAdjustments.hueShift.value = targetHueShift;
        _colorAdjustments.saturation.value = targetSaturation;
        _dof.focusDistance.value = targetFocusDistance;
        
        if (Mathf.Approximately(_dof.focusDistance.value, _defaultFocusDistance)) _dof.active = false;
    }

    public void SmokeSFX()
    {
        if (_smokeSFX)
        {
            _smokeSFX.pitch = Random.Range(0.5f, 1.2f);
            _smokeSFX.PlayOneShot(_smokeSFX.clip);
        }
    }
}