using System;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEditor;
using UnityEngine;
using Zenject;

public class HedgehogController : MonoBehaviour
{
    [Inject] private InvisibleWalls invisibleWalls;
    [Inject] private GameController gameController;
    [Inject] private BumblebeeController player;

    [SerializeField] private Transform jaw;
    [SerializeField] private float maxJawAngle;
    [SerializeField] private float animationSpeed;
    [SerializeField] private float biteDuration;
    [SerializeField] private Vector3 biteAnimationStartOffset;
    [SerializeField] private Transform mäulchenAnchor;

    private CancellationTokenSource cancellationTokenSource = new();
    private float animationStartTime;
    private float animationStartAngle;

    public bool isBiting;

    private void Start()
    {
        invisibleWalls.TargetCollideBottom.Where(_ => gameController.GetState() == GameController.GameState.playing)
            .Subscribe(vec => BiteAt(new Vector3(vec.x, vec.y, transform.position.z)))
            .AddTo(this);
    }

    public void BiteAt(Vector3 screenPosition)
    {
        if (isBiting)
        {
            return;
        }

        isBiting = true;
        var startPosition = screenPosition - biteAnimationStartOffset;
        BiteAtInternal(startPosition, screenPosition);
    }
    
    private void OnCollisionEnter2D ( Collision2D collision ) {
        if(collision.collider.gameObject.tag == "Player") {            
            collision.collider.gameObject.transform.SetParent(mäulchenAnchor);
            player.Die();
            gameController.GameOver();
        }
    }

    private async Task BiteAtInternal(Vector3 startPosition, Vector3 targetPosition)
    {
        jaw.localRotation = Quaternion.Euler(0, 0, maxJawAngle);
        transform.position = startPosition;

        var biteStartTime = Time.time;

        while (transform.position != targetPosition)
        {
            var timeSinceAnimationStart = Time.time - biteStartTime;

            transform.position = Vector3.Lerp(startPosition, targetPosition, timeSinceAnimationStart / biteDuration);

            await Task.Yield();
        }

        await CloseJaw();

        biteStartTime = Time.time;

        while (transform.position != startPosition)
        {
            var timeSinceAnimationStart = Time.time - biteStartTime;

            transform.position = Vector3.Lerp(targetPosition, startPosition, timeSinceAnimationStart / biteDuration);

            await Task.Yield();
        }

        isBiting = false;
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
            jaw.localRotation = Quaternion.Euler(0, 0, angle);

            if (angle > maxJawAngle)
            {
                jaw.localRotation = Quaternion.Euler(0, 0, maxJawAngle);

                return;
            }

            await Task.Yield();
        }
    }

    private async Task CloseJawInternal(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var timeDiff = Time.time - animationStartTime;

            var angle = animationStartAngle - timeDiff * animationSpeed;
            jaw.localRotation = Quaternion.Euler(0, 0, angle);

            if (angle < 0)
            {
                jaw.localRotation = Quaternion.Euler(0, 0, 0);

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

        if (GUILayout.Button("Bite"))
        {
            controller.BiteAt(new Vector3(0f, -4.62f, -0.5f));
        }
    }
}
#endif