using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _timeOnTurn;

    public float TimeLeft { get; private set; }

    public event Action TimeEnd;
    public event Action TimeChanged;

    private void Start()
    {
        TimeLeft = _timeOnTurn;
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0)
        {
            TimeLeft = _timeOnTurn;
            TimeEnd?.Invoke();
        }

        TimeChanged?.Invoke();
    }

    public void RestartTimer()
    {
        TimeLeft = _timeOnTurn;
    }
}
