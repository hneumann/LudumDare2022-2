using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShopController : MonoBehaviour
{
    [Inject] private GameController _gameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShopOption(string option) {
        Debug.Log("option: " + option);
        _gameController.StopShopping();
    }
}
