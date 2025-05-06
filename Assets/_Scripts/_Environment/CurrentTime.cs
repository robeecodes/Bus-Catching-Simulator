using TMPro;
using UnityEngine;

public class CurrentTime : MonoBehaviour
{
    private TextMeshProUGUI _timeText;

    void Start()
    {
        _timeText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        var timeOfDay = GameManager.Instance.lightingManager.TimeOfDay;
        var h = (int)timeOfDay;
        var m = (int)((timeOfDay - h) * 60);
        _timeText.text = $"{h:D2}:{m:D2}";

    }
}
