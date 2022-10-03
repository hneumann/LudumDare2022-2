using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOutput : MonoBehaviour
{
    private AudioSource _source;
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_source.isPlaying == false) {
            Destroy(this.gameObject);
        }
    }
}
