using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        DoDoTween(new Vector3(0,0,0));
    }

    private void DoDoTween(Vector3 targetPosition)
    {
        // TODO better animation here
        transform.DOMove(targetPosition, 3);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
