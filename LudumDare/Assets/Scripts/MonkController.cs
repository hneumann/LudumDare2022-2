
using System;
using UnityEngine;

public class MonkController : MonoBehaviour
{
    [SerializeField] private GameObject idleMonks;
    [SerializeField] private GameObject inMonks;
    [SerializeField] private GameObject outMonks;

    void Start()
    {
        outMonks.SetActive(true);
        idleMonks.SetActive(false);
        inMonks.SetActive(false);
    }

    //TODO switch monks really correctly
    private void Update()
    {
        var ones = Mathf.RoundToInt(Time.time) % 10;

        if (ones % 5 is 0 or 5)
        {
            idleMonks.SetActive(false);
            outMonks.SetActive(false);
            inMonks.SetActive(true);
            return;
        }

        idleMonks.SetActive(false);
        inMonks.SetActive(ones > 5);
        outMonks.SetActive(ones < 5);
        
    }
    
}
