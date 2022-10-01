
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private BumblebeeController player;
    public enum GameState
    {
        playing,
        shopping,
        idle
    }
    private GameState _state;

    [SerializeField] private float _cycleTime;
    [SerializeField] private float _grativyIncreasePerCycle;
    private float _gameTimer;
    private int _cycleCount;

    // Start is called before the first frame update
    void Start()
    {
        _state = GameState.playing;
    }

    // Update is called once per frame
    void Update()
    {
        if(_state == GameState.playing) {
            _gameTimer += Time.deltaTime;
            _cycleCount = Mathf.FloorToInt(_gameTimer / _cycleTime);
        }
    }

    public GameState GetState() {
        return _state;
    }

    public int CycleCount => _cycleCount;

    public void StartGame() {
        _state = GameState.playing;
    }
}
