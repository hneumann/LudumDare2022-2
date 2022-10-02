using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class MonkController : MonoBehaviour
{
    [Inject] private GravityController gravityController;

    [SerializeField] private GameObject idleMonks;
    [SerializeField] private GameObject inMonks;
    [SerializeField] private GameObject outMonks;
    [SerializeField] private GameObject particlesBreathOut;
    [SerializeField] private GameObject particlesBreathIn;

    private ParticleSystem[] particleSystemsIn;
    private ParticleSystem[] particleSystemsOut;

    void Start()
    {
        CollectParticleSystems();

        outMonks.SetActive(false);
        idleMonks.SetActive(true);
        inMonks.SetActive(false);

        gravityController.TimeInCycle.Subscribe(SetMonks).AddTo(this);
    }

    private void CollectParticleSystems()
    {
        particleSystemsIn = particlesBreathIn.GetComponentsInChildren<ParticleSystem>();
        particleSystemsOut = particlesBreathOut.GetComponentsInChildren<ParticleSystem>();

        gravityController.TimeInCycle.SkipLatestValueOnSubscribe()
            .Select(time => Mathf.RoundToInt(time * 10) > 5)
            .Pairwise()
            .Where(pair => pair.Current != pair.Previous)
            .Select(pair => pair.Current)
            .Subscribe(SpawnParticles)
            .AddTo(this);
    }

    private void SpawnParticles(bool inParticlesActive)
    {
        Debug.Log($"InParticles: {inParticlesActive}");

        var particlesToPlay = inParticlesActive ? particleSystemsIn : particleSystemsOut;
        var particlesToStop = inParticlesActive ? particleSystemsOut : particleSystemsIn;

        foreach (var system in particlesToPlay)
        {
            system.Play();
        }

        foreach (var system in particlesToStop)
        {
            system.Stop();
        }
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