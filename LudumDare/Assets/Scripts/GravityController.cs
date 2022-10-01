using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] private float _breatheInTime = 0f;
    [SerializeField] private float _breatheOutTime = 0f;
    [SerializeField] private float _breatheIdleTime = 0f;

    private float _timer;
    private float _gravityForce;

    private enum breatheState
    {
        In,
        Out,
        Idle 
    }
    private breatheState _state = breatheState.Idle;
    private breatheState _prevState = breatheState.Idle;


    private bool _playing = false;
    [SerializeField] private float _breatheInForce = 2f;
    [SerializeField] private float _breatheOutForce = 0f;
    [SerializeField] private float _breatheIdleForce = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_playing) {
            if (_timer <= 0f) {
                _state = NextState(_prevState);
                switch (_state) {
                    case breatheState.In:
                        BreatheIn();
                        break;
                    case breatheState.Out:
                        BreatheOut();
                        break;
                    case breatheState.Idle:
                        BreatheIdle();
                        break;
                }
                ModifyGravity();
            }
            _timer -= Time.deltaTime;
            Debug.Log("Timer: " + _timer + " | State: " + _state + " | GravityForce: " + _gravityForce);
            //Debug.Log("");
        }
    }

    private void BreatheIn() {
        _timer = _breatheInTime;
        _gravityForce = _breatheInForce;
        _prevState = breatheState.In;
    }
    private void BreatheOut() {
        _timer = _breatheOutTime;
        _gravityForce = _breatheOutForce;
        _prevState = breatheState.Out;
    }
    private void BreatheIdle() {
        _timer = _breatheIdleTime;
        _gravityForce = _breatheIdleForce;
        _prevState = breatheState.Idle;
    }

    private breatheState NextState(breatheState prevState) {
        breatheState nextState = breatheState.Idle;
        switch (prevState) {
            case breatheState.In:
                nextState = breatheState.Out;
                break;
            case breatheState.Out:
                nextState = breatheState.In;
                break;
            case breatheState.Idle:
                nextState = breatheState.In;
                break;
        }
        return nextState;
    }

    private void ModifyGravity() {
        Physics2D.gravity = new Vector2(0f , _gravityForce);
    }

}
