using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [Inject] private GameController gameController;
    [Inject] private BumblebeeController player;

    [Header("HUD Components")]
    [SerializeField] private GameObject _breathCycleCounter;
    [SerializeField] private GameObject _nectarCounter;
    // Game Over Screen Objects
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private TextMeshProUGUI _txtTotalPollenAmount;
    [SerializeField] private TextMeshProUGUI _txtTotalBreathAmount;


    // Start is called before the first frame update
    void Start()
    {
        CloseGameOverScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController.GetState() is GameController.GameState.playing) {
            _breathCycleCounter.GetComponent<TextMeshProUGUI>().text = gameController.CycleCount.ToString();
            _nectarCounter.GetComponent<TextMeshProUGUI>().text = player.PollenCount.ToString();
        }
        
    }

    public void OpenGameOverScreen() {
        _txtTotalPollenAmount.text = player.TotalPollenCount.ToString();
        _txtTotalBreathAmount.text = gameController.CycleCount.ToString();
        _gameOverScreen.SetActive(true);
    }
    public void CloseGameOverScreen() {
        _gameOverScreen.SetActive(false);
    }
}
