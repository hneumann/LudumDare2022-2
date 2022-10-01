using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private int _nectarYield;
    [SerializeField] private int randomModifierRange;
    [SerializeField] private int _currentYield;
    [SerializeField] private float harvestTimeRequired;
    [SerializeField] private int _nectarPerHarvest;
    private float harvestTimer = 0;

    [SerializeField] private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        _currentYield = _nectarYield + Random.Range(-randomModifierRange, randomModifierRange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Harvest() {
        if (gameController.GetState() == GameController.GameState.playing) {
            if(_currentYield == 0) {
                return 0;
            }
            harvestTimer += Time.deltaTime;
            if(harvestTimer >= harvestTimeRequired) {
                harvestTimer = 0f;
                _currentYield -= 1;
                return 1;
            }
        }
        return 0;
    }
}
