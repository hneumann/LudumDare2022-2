using System;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEditor;
using UnityEngine;

public class HedgehogController : MonoBehaviour
{
    [SerializeField] private Transform jaw;
    [SerializeField] private float maxJawAngle;
    [SerializeField] private float animationSpeed;
    [SerializeField] private Vector3 biteAnimationStartOffset;

    private CancellationTokenSource cancellationTokenSource = new();
    private float animationStartTime;
    private float animationStartAngle;
  
    private void OnEnable()
    {
        OpenJaw();
    }

    public void BiteAt(Vector3 screenPosition)
    {
        var biteAnimationStart = screenPosition - biteAnimationStartOffset;
        
        
    }

    public async Task BiteAtInternal(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;

        while (transform.position != targetPosition)
        {
            
        }
    }
    
    public async Task CloseJaw()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        animationStartTime = Time.time;
        animationStartAngle = jaw.localRotation.eulerAngles.z;

        cancellationTokenSource = new CancellationTokenSource();

        await CloseJawInternal(cancellationTokenSource.Token);
    }
    
    public async Task OpenJaw()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        
        animationStartTime = Time.time;
        animationStartAngle = jaw.localRotation.eulerAngles.z;

        cancellationTokenSource = new CancellationTokenSource();

        await OpenJawInternal(cancellationTokenSource.Token);
    }


    private async Task OpenJawInternal(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var timeDiff = Time.time - animationStartTime;

            var angle = timeDiff * animationSpeed + animationStartAngle;
            jaw.localRotation = Quaternion.Euler(0,0, angle);

            if (angle > maxJawAngle)
            {
                jaw.localRotation = Quaternion.Euler(0,0, maxJawAngle);
                return;
            }

            await Task.Yield();
        }
    }
    
    private async Task  CloseJawInternal(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var timeDiff = Time.time - animationStartTime;

            var angle = animationStartAngle - timeDiff * animationSpeed;
            jaw.localRotation = Quaternion.Euler(0,0, angle);

            if (angle < 0)
            {
                jaw.localRotation = Quaternion.Euler(0,0, 0);
                return;
            }

            await Task.Yield();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(HedgehogController))] 
public class HedgehogControllerEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        var controller = target as HedgehogController;

        DrawDefaultInspector();
        
        if (GUILayout.Button("Open Jaw"))
        {
            controller.OpenJaw();
        }
        
        if (GUILayout.Button("Close Jaw"))
        {
            controller.CloseJaw();
        }
    }
}
#endif