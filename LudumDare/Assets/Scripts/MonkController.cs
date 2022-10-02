using UniRx;
using UnityEngine;
using Zenject;

public class MonkController : MonoBehaviour
{
    [Inject] private GravityController gravityController;
    
    [SerializeField] private GameObject idleMonks;
    [SerializeField] private GameObject inMonks;
    [SerializeField] private GameObject outMonks;

    void Start()
    {
        outMonks.SetActive(false);
        idleMonks.SetActive(true);
        inMonks.SetActive(false);

        gravityController.TimeInCycle.Subscribe(SetMonks).AddTo(this);
    }

    private void SetMonks(float time)
    {
        time = Mathf.RoundToInt(time * 10);
        
        if (time % 10 is 0 or 5)
        {
            idleMonks.SetActive(true);
            outMonks.SetActive(false);
            inMonks.SetActive(false);
            return;
        }

        idleMonks.SetActive(false);
        inMonks.SetActive(time > 5);
        outMonks.SetActive(time < 5);
        
    }
    
}
