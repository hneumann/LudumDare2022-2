using System;
using System.Collections.Generic;
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
    [SerializeField] private List<GameObject> flowerPrefabs;
    [SerializeField] private float _flowerSpawnTime;
    [SerializeField] private float _flowerSpawnTimeRangeModifier;
    
    private float _flowerSpawnTimer;
    

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
            _flowerSpawnTimer -= Time.deltaTime;
            _cycleCount = Mathf.FloorToInt(_gameTimer / _cycleTime);

            if(_flowerSpawnTimer <= 0f) {
                SpawnFlower();
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            Time.timeScale = 0.01f;
        }

        if(Input.GetKeyDown(KeyCode.N)) {
            Time.timeScale = 1f;
        }
    }

    private void SpawnFlower () {
        _flowerSpawnTimer = _flowerSpawnTime + UnityEngine.Random.Range(-_flowerSpawnTimeRangeModifier, _flowerSpawnTimeRangeModifier);
        GameObject newFlower = Instantiate(flowerPrefabs[UnityEngine.Random.Range(0, flowerPrefabs.Count)]);
        newFlower.transform.localPosition = new Vector3(10f, UnityEngine.Random.Range(-4f, 0f), newFlower.transform.localPosition.z);
    }

    public GameState GetState() {
        return _state;
    }

    public int CycleCount => _cycleCount;
    public float gravityIncreasePerCycle => _grativyIncreasePerCycle;

    public void StartGame() {
        _state = GameState.playing;
    }
}
