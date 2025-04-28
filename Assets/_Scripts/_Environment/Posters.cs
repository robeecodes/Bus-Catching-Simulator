using System.Collections;
using UnityEngine;

public class Posters : MonoBehaviour
{
    private static readonly int DissolveAmount = Shader.PropertyToID("_Dissolve_Amount");
    private Material _posterMaterial;
    private float _duration = 5f;

    private void Start()
    {
        _posterMaterial = GetComponent<Renderer>().material;
        GameManager.Instance.OnTimeChanged += OnTimeChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnTimeChanged -= OnTimeChanged;
    }

    private void OnTimeChanged(int newTime)
    {
        if (newTime < 21) return;
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
    {
        GameManager.Instance.OnTimeChanged -= OnTimeChanged;
        
        float time = 0f;
        while (time < _duration)
        {
            time += Time.deltaTime;
            float dissolveAmount = Mathf.Clamp01(time / _duration);

            _posterMaterial.SetFloat(DissolveAmount, dissolveAmount);
            yield return null;
        }
    }
}