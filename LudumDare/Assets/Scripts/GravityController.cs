using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] private float _cycleTime = 0f;
    [SerializeField] [Range(0f, 20f)] private float _breatheForce = 0f;
    [SerializeField] private AnimationCurve _breatheRythm;

    private float _timer;
    private float _gravityForce;

    private bool _playing = false;

    [SerializeField] private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        ModifyGravity(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController.GetState() == GameController.GameState.playing) {
            _gravityForce = _breatheRythm.Evaluate((_timer % _cycleTime)/_cycleTime) * _breatheForce;
            _timer += Time.deltaTime;
            ModifyGravity(_gravityForce);
        }
    }

    private void ModifyGravity(float modifier) {
        Physics2D.gravity = new Vector2(0f , modifier);
        Physics.gravity = new Vector3(0f , modifier, 0f);
    }
}
