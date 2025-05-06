using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ambience : MonoBehaviour
{
    [SerializeField] private AudioSource street;
    [SerializeField] private float streetTargetVolume = 0.7f;
    [SerializeField] private AudioSource creep;
    [SerializeField] private float creepTargetVolume = 0.45f;
    
    private void Awake()
    {
        StartCoroutine(Fade(street, 30f, streetTargetVolume));
        GameManager.Instance.OnTimeChanged += OnTimeChanged;
    }
    
    private void OnDestroy()
    {
        GameManager.Instance.OnTimeChanged -= OnTimeChanged;
    }

    
    private IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
        float time = 0f;
        float startVolume = audioSource.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        audioSource.volume = targetVolume;
        if (Mathf.Approximately(targetVolume, 0f))
            audioSource.enabled = false;
    }

    
    private void OnTimeChanged(int newTime)
    {
        if (newTime < 21) return;
        StartCoroutine(Fade(creep, 5f, creepTargetVolume));
        StartCoroutine(Fade(street, 10f, 0f));
        GameManager.Instance.OnTimeChanged -= OnTimeChanged;
    }
}