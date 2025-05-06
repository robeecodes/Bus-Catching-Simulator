using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class BusArrival : MonoBehaviour
{
    [Header("Audio")]
    private AudioSource _busSound;
    
    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 230f;

    [FormerlySerializedAs("leftEyeCamera")]
    [Header("FOV Warp Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private float startFOV = 60f;
    [SerializeField] private float endFOV = 90f;
    [SerializeField] private float fovWarpDuration = 250f;

    private bool _distorting = false;
    private bool _shaking = false;
    private float _fadeTimer;
    private float _fovTimer;

    private void Start()
    {
        _busSound = GetComponent<AudioSource>();
        GameManager.Instance.OnTimeChanged += OnTimeChanged;

        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0;

        if (cam != null) cam.fieldOfView = startFOV;
    }

    private void Update()
    {
        if (_distorting)
        {
            UpdateFadeAndWarp();
        }
    }

    private void OnTimeChanged(int newTime)
    {
        if (newTime < 23) return;

        _busSound.Play();
        _distorting = true;
        StartCoroutine(FadeAway());

        GameManager.Instance.OnTimeChanged -= OnTimeChanged;
    }

    private void UpdateFadeAndWarp()
    {
        // Fade
        if (fadeCanvasGroup && _fadeTimer < fadeDuration)
        {
            _fadeTimer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(_fadeTimer / fadeDuration);
        }

        // FOV warp
        if (cam && _fovTimer < fovWarpDuration)
        {
            _fovTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_fovTimer / fovWarpDuration);
            float newFOV = Mathf.Lerp(startFOV, endFOV, t);
            cam.fieldOfView = newFOV;
        }
    }

    private IEnumerator FadeAway()
    {
        // Wait before starting shake
        yield return new WaitForSeconds(22f);
        
        // Full fadeout after a few more seconds
        yield return new WaitForSeconds(10f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
