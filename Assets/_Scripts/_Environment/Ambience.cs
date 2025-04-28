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
        street.volume = 0f;
        creep.volume = 0f;
        street.enabled = true;
        creep.enabled = false;
        
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
        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, time / duration);
            yield return null;
        }
        
        if (audioSource.volume <= 0f) audioSource.enabled = false;
    }
    
    private void OnTimeChanged(int newTime)
    {
        if (newTime < 21) return;
        creep.enabled = true;
        StartCoroutine(Fade(creep, 1000f, creepTargetVolume));
        StartCoroutine(Fade(street, 3000f, 0f));
        GameManager.Instance.OnTimeChanged -= OnTimeChanged;
    }
}