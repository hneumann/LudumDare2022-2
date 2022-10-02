using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Flower : MonoBehaviour
{
    [SerializeField] private int _nectarYield;
    [SerializeField] private int randomModifierRange;
    [SerializeField] private int _currentYield;
    [SerializeField] private float harvestTimeRequired;
    [SerializeField] private int _nectarPerHarvest;
    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private List<Transform> pollenAnchors;
    [SerializeField] private List<GameObject> pollen;
    [SerializeField] private List<SpriteRenderer> pollenPrefabs;
    [SerializeField] private float pollenSpawnOffsetRange;
    [SerializeField] private Animator animator;

    private float harvestTimer = 0;

    private float _pollenSpawnRange = 0.05f;

    [Inject] private GameController gameController;
    

    // Start is called before the first frame update
    void Start()
    {
        _currentYield = _nectarYield + Random.Range(-randomModifierRange, randomModifierRange);

        for (int i = 0; i < _currentYield; i++)
        {
            var spawnOffset = (Vector3)Random.insideUnitCircle * pollenSpawnOffsetRange;
            var spawnRotation = Quaternion.Euler(0, 0, Random.Range(0,360));
            var pollenPrefabIdx = Random.Range(0, pollenPrefabs.Count);

            var anchor = pollenAnchors[Random.Range(0, pollenAnchors.Count)];
            
            var newPollen = Instantiate(pollenPrefabs[pollenPrefabIdx], spawnOffset + anchor.position, spawnRotation, anchor);
            newPollen.sortingOrder = 3;
            pollen.Add(newPollen.gameObject);
        }
        StopAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.GetState() == GameController.GameState.playing) {
            transform.localPosition = new Vector3(transform.localPosition.x - _movementSpeed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
            if(transform.localPosition.x <= -15f) {
                Destroy(this.gameObject);
            }
        }
    }

    public int Harvest() {
        if (gameController.GetState() == GameController.GameState.playing) {
            if (_currentYield > 0) {
                int harvestAmount = 0;
                harvestTimer += (Time.deltaTime * Upgrades.Instance.ExtraHarvestSpeed);
                if (harvestTimer >= harvestTimeRequired) {
                    harvestTimer = 0f;
                    _currentYield -= 1;
                    RemovePollen();
                    harvestAmount += 1;
                    float rand = Random.Range(0f, 1f);
                    if (rand <= Upgrades.Instance.ExtraHarvestChance) {
                        harvestAmount += 1;
                    }
                }
                return harvestAmount;
            }
        }
        return 0;
    }

    public void StartAnimation()
    {
        animator.enabled = true;
    }
    
    public void StopAnimation()
    {
        animator.enabled = false;
    }

    private void RemovePollen()
    {
        if(!pollen.Any()) return;

        var rndIdx = Random.Range(0, pollen.Count);
        
        Destroy(pollen[rndIdx]);
        
        pollen.RemoveAt(rndIdx);
    }
}
