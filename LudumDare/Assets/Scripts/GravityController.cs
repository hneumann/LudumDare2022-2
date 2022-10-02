using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class GravityController : MonoBehaviour
{
    [SerializeField] private float _cycleTime = 0f;
    [SerializeField] [Range(0f, 20f)] private float _breatheForce = 0f;
    [SerializeField] private AnimationCurve _breatheRythm;

    private float _timer;
    private float _gravityForce;
    public ReadOnlyReactiveProperty<float> TimeInCycle => _timeInCycle.ToReadOnlyReactiveProperty();
    private ReactiveProperty<float> _timeInCycle = new ReactiveProperty<float>();

    private bool _playing = false;

    [Inject] private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        ModifyGravity(-9.81f);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController.GetState() == GameController.GameState.playing)
        {
            _timeInCycle.Value = (_timer % _cycleTime) / _cycleTime;
            _gravityForce = _breatheRythm.Evaluate(_timeInCycle.Value) * _breatheForce;
            _gravityForce += gameController.CycleCount * gameController.gravityIncreasePerCycle;
            _timer += Time.deltaTime;
            ModifyGravity(_gravityForce);
        }
    }

    private void ModifyGravity(float modifier) {
        Physics2D.gravity = new Vector2(0f , modifier);
        Physics.gravity = new Vector3(0f , modifier, 0f);
    }
}
