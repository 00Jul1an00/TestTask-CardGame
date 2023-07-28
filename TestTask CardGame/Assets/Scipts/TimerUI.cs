using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Timer _timer;

    private void OnEnable() => _timer.TimeChanged += OnTimeChanged;

    private void OnDisable() => _timer.TimeChanged -= OnTimeChanged;

    private void OnTimeChanged()
    {
        _timerText.text = "TIMER: " + Mathf.CeilToInt(_timer.TimeLeft).ToString();
    }
}
