using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _hedgehogMonch;
    [SerializeField] private AudioClip _birdMonch;
    [SerializeField] private AudioClip _flyingNoise;
    [SerializeField] private AudioClip _getPollen;
    [SerializeField] private AudioClip _buying;
    [SerializeField] private GameObject _audioOutputPrefab;

    private List<GameObject> audioOutputObjects = new List<GameObject>();
    private GameObject _musicOutput;
    private GameObject _flyingNoiseOutput;
    private GameObject _hedgehogMonchOutput;
    private GameObject _birdMonchOutput;

    private bool _muted = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayBackgroundMusic();
        PlayFlyingSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMute() {
        _muted = !_muted;
        if(_muted) {
            StopFlyingSound();
            StopBackgroundMusic();

        } else {
            PlayFlyingSound();
        PlayBackgroundMusic();
        }
        
    }

    private void PlayBackgroundMusic() {
        if(!_muted) {
            if(_musicOutput == null) {
                _musicOutput = Instantiate(_audioOutputPrefab);
                _musicOutput.transform.SetParent(this.transform);
                _musicOutput.GetComponent<AudioSource>().clip = _backgroundMusic;
                _musicOutput.GetComponent<AudioSource>().loop = true;
                _musicOutput.GetComponent<AudioSource>().volume = 0.5f;
            }
                _musicOutput.GetComponent<AudioSource>().Play();
        }
    }

    private void StopBackgroundMusic() {
        if (_musicOutput != null) {
                _musicOutput.GetComponent<AudioSource>().Stop();
            }
    }

    public void PlayFlyingSound() {
        if(!_muted) {
            if(_flyingNoiseOutput == null) {
                _flyingNoiseOutput = Instantiate(_audioOutputPrefab);
                _flyingNoiseOutput.transform.SetParent(this.transform);
                _flyingNoiseOutput.GetComponent<AudioSource>().clip = _flyingNoise;
                _flyingNoiseOutput.GetComponent<AudioSource>().loop = true;
                _flyingNoiseOutput.GetComponent<AudioSource>().volume = 0.5f;
            }
            _flyingNoiseOutput.GetComponent<AudioSource>().Play();
        } else {
            if (_flyingNoiseOutput != null) {
                _flyingNoiseOutput.GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void StopFlyingSound() {
        if (_flyingNoiseOutput != null) {
                _flyingNoiseOutput.GetComponent<AudioSource>().Stop();
            }
    }

    public void PlayHedgehogSound() {
        if(!_muted) {
            _hedgehogMonchOutput = PlaySound(_hedgehogMonch); 
        }
    }
    public void PlayBirdSound() {
        if(!_muted) {
            _birdMonchOutput = PlaySound(_birdMonch); 
        }
    }

    public void PlayPollenSound() {
        if(!_muted) {
            PlaySound(_getPollen, 0.6f); 
        }
    }

    public void PlayBuyingSound() {
        if(!_muted) {
            PlaySound(_buying); 
        }
    }
    
    private GameObject PlaySound ( AudioClip clip, float volume = 1f) {
        GameObject outputObject = Instantiate(_audioOutputPrefab);
        outputObject.transform.SetParent(this.transform);
        outputObject.GetComponent<AudioSource>().clip = clip;
        outputObject.GetComponent<AudioSource>().volume = volume;
        outputObject.GetComponent<AudioSource>().Play();
        return outputObject;
    }

    private void StopSound(GameObject output) {
        if(output != null) output.GetComponent<AudioSource>().Stop();
    }
}
