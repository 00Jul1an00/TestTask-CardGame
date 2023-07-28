using UnityEngine;
using TMPro;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _winnerText;

    private void Start()
    {
        GameManager.Instance.PlayerWon += OnPlayerWon;
        GameManager.Instance.BotWon += OnBotWon;
        gameObject.SetActive(false);
    }

    private void OnPlayerWon()
    {
        gameObject.SetActive(true);
        _winnerText.text = "PLAYER WON";
    }


    private void OnBotWon()
    {
        gameObject.SetActive(true);
        _winnerText.text = "BOT WON";
    }
}
