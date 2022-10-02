using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpawnFlowingPollen : MonoBehaviour
{
    [Inject] private GameController gameController;
    [Inject] private BumblebeeController bumblebeeController;
    [SerializeField] private List<Collider2D> pollenPrefabs;
    [SerializeField] private float minSpeed = 2.5f;
    [SerializeField] private float maxSeed = 4.5f;
    [SerializeField] private float maxAngularVelocity;
    [SerializeField] private float spawnInterval;

    private readonly CompositeDisposable disposable = new();

    private void Start()
    {
        disposable.AddTo(this);
        RebindSpawnTimer(1);

        Upgrades.Instance.FloatingPollenIntervallFactor.Subscribe(RebindSpawnTimer).AddTo(this);
    }

    private void RebindSpawnTimer(float factor)
    {
        disposable.Clear();
        
        Observable.Interval(TimeSpan.FromSeconds(spawnInterval * factor))
            .Where(_ => gameController.GetState() == GameController.GameState.playing)
            .Subscribe(_ => SpawnSinglePollen())
            .AddTo(disposable);
    }


    private void SpawnSinglePollen()
    {
        var startPosition = (Vector3) Random.insideUnitCircle * 7;
        startPosition.Set(15, startPosition.y, 0);
        
        var endPosition = (Vector3) Random.insideUnitCircle * 7;
        endPosition.Set(-15, endPosition.y, 0);

        var direction = endPosition - startPosition;
        var speed = Random.Range(minSpeed, maxSeed);
        var lifetime = direction.magnitude / speed;
        
        var prefabIdx = Random.Range(0, pollenPrefabs.Count);

        var pollen = Instantiate(pollenPrefabs[prefabIdx], startPosition, Quaternion.identity, transform);
        var pollenRigidbody = pollen.attachedRigidbody;

        pollenRigidbody.velocity = direction.normalized * speed;
        pollenRigidbody.angularVelocity = Random.Range(-maxAngularVelocity, maxAngularVelocity);

        pollen.OnTriggerEnter2DAsObservable().Subscribe(collision => HandleCollision(collision, pollen.gameObject)).AddTo(pollen);

        Observable.ReturnUnit().Delay(TimeSpan.FromSeconds(lifetime)).Where(_ => pollen != null && pollen.gameObject != null).Subscribe(_ => Destroy(pollen.gameObject)).AddTo(this);
    }

    private void HandleCollision(Collider2D collider, GameObject pollen)
    {
        if (collider.gameObject.tag != "Player")
        {
            return;
        }
        bumblebeeController.FloatingPollenCollected();
        Destroy(pollen);
    }
}
