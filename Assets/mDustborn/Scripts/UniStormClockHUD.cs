using UnityEngine;
using TMPro;
using UniStorm;

public class UniStormClockHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeLabel;
    [SerializeField] private bool useLeadingZero = true;

    float _poll = 0f;

    void Update()
    {
        _poll += Time.unscaledDeltaTime;
        if (_poll < 0.2f) return;
        _poll = 0f;

        int h = UniStormSystem.Instance.Hour;    
        int m = UniStormSystem.Instance.Minute;
        if (useLeadingZero)
            timeLabel.text = $"{h:00}:{m:00}";
        else
            timeLabel.text = $"{h}:{m:00}";
    }
}