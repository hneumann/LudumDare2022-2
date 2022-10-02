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
    private float harvestTimer = 0;

    [Inject] private GameController gameController;
    

    // Start is called before the first frame update
    void Start()
    {
        _currentYield = _nectarYield + Random.Range(-randomModifierRange, randomModifierRange);
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
}
