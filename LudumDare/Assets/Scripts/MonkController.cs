using UniRx;
using UnityEngine;
using Zenject;

public class MonkController : MonoBehaviour
{
    [Inject] private GravityController gravityController;
    
    [SerializeField] private GameObject idleMonks;
    [SerializeField] private GameObject inMonks;
    [SerializeField] private GameObject outMonks;
    [SerializeField] private ParticleSystem particlesBreathOut;    
    [SerializeField] private ParticleSystem particlesBreathIn;

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
            //particlesBreathIn.Play();
            return;
        }

        idleMonks.SetActive(false);
        inMonks.SetActive(ones > 5);
        outMonks.SetActive(ones < 5);
        //particlesBreathOut.Play();
    }
    
}
