using System;
using System.Collections.Generic;
using UniRx;
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
        idle,
        gameOver
    }
    private GameState _state;
    [SerializeField] private HUDController _hudController;
    
    [SerializeField] private float _cycleTime;
    [SerializeField] private float _grativyIncreasePerCycle;
    private float _gameTimer;
    private int _cycleCount;

    [Header("Flowers")]
    [SerializeField] private List<GameObject> flowerPrefabs;
    [SerializeField] private float _flowerSpawnTime;
    [SerializeField] private float _flowerSpawnTimeRangeModifier;
    [SerializeField] private List<GameObject> flowers = new List<GameObject>();
    [SerializeField] private List<GameObject> shops = new List<GameObject>();
    private float _flowerSpawnTimer;

    [Header("Shop")]
    [SerializeField] private GameObject _shopPrefab;
    [SerializeField] private float _shopSpawnTime;

    public ReadOnlyReactiveProperty<float> ScrollSpeedFactor => scrollSpeedFactor.ToReadOnlyReactiveProperty();
    private ReactiveProperty<float> scrollSpeedFactor = new ReactiveProperty<float>();
    

    private float _shopSpawnTimer;
    private ShopObjectController _shopObject;

    
    // Start is called before the first frame update
    void Start()
    {
        _shopSpawnTimer = _shopSpawnTime;
        // StartGame();
        State = GameState.idle;
        _hudController.CloseGameOverScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartGame();
        }
        
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

        newFlower.transform.localPosition = new Vector3(10f, UnityEngine.Random.Range(-7f, -5f), newFlower.transform.localPosition.z);
        flowers.Add(newFlower);

    }

    private void SpawnShop() {
        _shopSpawnTimer = _shopSpawnTime;
        GameObject shop = Instantiate(_shopPrefab);
        shop.transform.localPosition = new Vector3(12f, shop.transform.localPosition.y, shop.transform.localPosition.z);
        shops.Add(shop);
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
                    scrollSpeedFactor.Value = 0;
                    shopUI.gameObject.SetActive(false);
                    break;
                case GameState.playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.shopping:
                    Time.timeScale = 0.01f;
                    break;
                case GameState.gameOver:
                    Time.timeScale = 1f;
                    break;
            }
        }
    }

    public int CycleCount => _cycleCount;
    public float gravityIncreasePerCycle => _grativyIncreasePerCycle;

    public void StartGame () {
        scrollSpeedFactor.Value = 1;
        ClearScene();
        ResetValues();
        player.Reset();
        _hudController.CloseGameOverScreen();
        State = GameState.playing;
    }

    public void ResetValues() {
        _flowerSpawnTimer = _flowerSpawnTime;
        _shopSpawnTimer = _shopSpawnTime;
        _gameTimer = 0;
        Upgrades.Instance.Reset();
    }
    public void ClearScene() {
        //Remove Flowers
        for (int i = 0; i < flowers.Count; i++) {
            Destroy(flowers[i]);
        }
        flowers.Clear();
        //Remove Shops
        for (int i = 0; i < shops.Count; i++) {
            Destroy(shops[i]);
        }
        shops.Clear();
    }

    public void GameOver () {
        State = GameState.gameOver;
        _hudController.OpenGameOverScreen();
        shopUI.gameObject.SetActive(false);
    }

    public void StartShopping(ShopObjectController shop) {
        State = GameState.shopping;
        shopUI.OpenShop();
        _shopObject = shop;
    }

    public void StopShopping() {
        shopUI.gameObject.SetActive(false);
        _shopObject.CloseShop();
        State = GameState.playing;
    }
}
