using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameController gameController;

    [Header("HUD Components")]
    [SerializeField] private GameObject _breathCycleCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _breathCycleCounter.GetComponent<TextMeshProUGUI>().text = gameController.CycleCount.ToString();
    }
}
