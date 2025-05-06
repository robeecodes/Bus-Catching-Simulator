using TMPro;
using UnityEngine;

public class BusTime : MonoBehaviour
{
    private TextMeshProUGUI _text;
    
    [Header("Flicker Settings")]
    public float flickerSpeed = 5f;
    public float flickerIntensity = 0.1f;
    private string _originalText;
    private bool _isFreaky = false;
    
    [Header("Scanline Settings")]
    public float scanlineSpeed = 0.2f;
    public float scanlineStrength = 0.05f;
    
    [Header("Color Tint Settings")]
    public Color normalColor = Color.white;
    public Color tintColor = new Color(0.8f, 0.6f, 1f);
    public float tintSpeed = 0.5f;
    
    [Header("Freakout Settings")]
    public float glitchFrequency = 0.05f; // How often to scramble
    public float heavyFlickerIntensity = 0.5f;
    public Color freakyColor = Color.red;
    public float scrambleChance = 0.7f;
    
    private float _nextGlitchTime = 0f;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _originalText = _text.text;

        // Listen for time change
        GameManager.Instance.OnTimeChanged += OnTimeChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnTimeChanged -= OnTimeChanged;
    }

    private void Update()
    {
        if (!_text) return;

        if (!_isFreaky)
        {
            NormalVisuals();
        }
        else
        {
            FreakyVisuals();
        }
    }

    private void NormalVisuals()
    {
        float time = Time.time;

        // Flicker brightness
        float flicker = 1f + Mathf.Sin(time * flickerSpeed) * flickerIntensity;

        // Scanlines
        float scanline = 1f - (Mathf.Sin(time * scanlineSpeed * 2f * Mathf.PI) * scanlineStrength);

        // Tint
        float tintAmount = (Mathf.Sin(time * tintSpeed) + 1f) * 0.5f;
        Color finalColor = Color.Lerp(normalColor, tintColor, tintAmount);

        _text.color = finalColor * flicker * scanline;
        _text.text = _originalText;
    }

    private void FreakyVisuals()
    {
        float time = Time.time;

        // Heavy flicker
        float flicker = 1f + Mathf.Sin(time * flickerSpeed * 2f) * heavyFlickerIntensity;

        _text.color = freakyColor * flicker;

        // Randomly glitch the text
        if (Time.time >= _nextGlitchTime)
        {
            _text.text = ScrambleText(_originalText);
            _nextGlitchTime = Time.time + Random.Range(glitchFrequency * 0.5f, glitchFrequency * 1.5f);
        }
    }

    private string ScrambleText(string text)
    {
        char[] chars = text.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (Random.value > scrambleChance) // 30% chance per character to scramble
            {
                chars[i] = (char)Random.Range(33, 126); // printable ASCII range
            }
        }
        return new string(chars);
    }

    private void OnTimeChanged(int newTime)
    {
        if (newTime >= 21) // 9pm
        {
            _isFreaky = true;
        }
    }
}