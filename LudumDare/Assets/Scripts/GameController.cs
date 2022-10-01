using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private BumblebeeController player;
    [Inject] private ShopController shopUI;

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

    [Header("Flowers")]
    [SerializeField] private List<GameObject> flowerPrefabs;
    [SerializeField] private float _flowerSpawnTime;
    [SerializeField] private float _flowerSpawnTimeRangeModifier;
    private float _flowerSpawnTimer;

    [Header("Shop")]
    [SerializeField] private GameObject _shopPrefab;
    [SerializeField] private float _shopSpawnTime;
    private float _shopSpawnTimer;
    private ShopObjectController _shopObject;

    
    // Start is called before the first frame update
    void Start()
    {
        _state = GameState.playing;
        _shopSpawnTimer = _shopSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(_state == GameState.playing) {
            _gameTimer += Time.deltaTime;
            _flowerSpawnTimer -= Time.deltaTime;
            _shopSpawnTimer -= Time.deltaTime;
            _cycleCount = Mathf.FloorToInt(_gameTimer / _cycleTime);

            if(_flowerSpawnTimer <= 0f) {
                SpawnFlower();
            }
            if(_shopSpawnTimer <= 0f) {
                SpawnShop();
            }
        }
    }

    private void SpawnFlower () {
        _flowerSpawnTimer = _flowerSpawnTime + UnityEngine.Random.Range(-_flowerSpawnTimeRangeModifier, _flowerSpawnTimeRangeModifier);
        GameObject newFlower = Instantiate(flowerPrefabs[UnityEngine.Random.Range(0, flowerPrefabs.Count)]);
        newFlower.transform.localPosition = new Vector3(10f, UnityEngine.Random.Range(-4f, 0f), newFlower.transform.localPosition.z);
    }

    private void SpawnShop() {
        _shopSpawnTimer = _shopSpawnTime;
        GameObject shop = Instantiate(_shopPrefab);
        shop.transform.localPosition = new Vector3(10f, shop.transform.localPosition.y, shop.transform.localPosition.z);
    }

    public GameState GetState() {
        return _state;
    }

    private GameState State {
        get { return _state; }
        set { 
            _state = value;
            switch (_state) {
                case GameState.idle:
                    Time.timeScale = 1f;
                    break;
                case GameState.playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.shopping:
                    Time.timeScale = 0.01f;
                    break;
            }
        }
    }

    public int CycleCount => _cycleCount;
    public float gravityIncreasePerCycle => _grativyIncreasePerCycle;

    public void StartGame() {
        State = GameState.playing;
    }

    public void StartShopping(ShopObjectController shop) {
        State = GameState.shopping;
        _shopObject = shop;
        shopUI.gameObject.SetActive(true);
    }

    public void StopShopping() {
        State = GameState.playing;
        _shopObject.CloseShop();
        shopUI.gameObject.SetActive(false);
    }
}
