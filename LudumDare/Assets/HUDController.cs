using UnityEngine;
using TMPro;
using Zenject;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [Inject] private GameController gameController;

    [Header("HUD Components")]
    [SerializeField] private TextMeshProUGUI _breathCycleCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _breathCycleCounter.text = gameController.CycleCount.ToString();
    }
}
