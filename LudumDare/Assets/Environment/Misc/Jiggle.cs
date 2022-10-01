using UnityEngine;

public class Jiggle : MonoBehaviour
{
    [SerializeField] private float degreeChangePerFrame;
    [SerializeField] private float maxDregreeRotation;
    
    private Quaternion initialRotation;
    
    
    private float rotDegOffset;
    private float rotDegTarget;
    
    private void Awake()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (Mathf.Abs(rotDegOffset - rotDegTarget)< degreeChangePerFrame)
        {
            rotDegTarget = Random.Range(-maxDregreeRotation, maxDregreeRotation);
        }

        if (rotDegTarget > rotDegOffset)
        {
            rotDegOffset += degreeChangePerFrame;
        }
        else
        {
            rotDegOffset -= degreeChangePerFrame;

        }

        transform.localRotation = initialRotation * Quaternion.Euler(0, 0, rotDegOffset);
    }
}
