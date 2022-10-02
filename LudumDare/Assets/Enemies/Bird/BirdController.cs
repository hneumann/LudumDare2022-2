using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class BirdController : MonoBehaviour
{
    [Inject] private InvisibleWalls invisibleWalls;
    [Inject] private GameController gameController;

    [SerializeField] private float targetXOffset;
    [SerializeField] private float targetYOffset;
    [SerializeField] private Vector3 lowestPointOffset;

    private Tween lastTween;
    
    private void Start()
    {
        invisibleWalls.TargetCollideCeiling.Where(_ => gameController.GetState() == GameController.GameState.playing).Subscribe(vec => DoDoTween(new Vector3(vec.x, vec.y, 0))).AddTo(this);
    }

    private void DoDoTween(Vector3 targetPosition)
    {
        if (lastTween != null && lastTween.IsPlaying())
        {
            return;
        }
        transform.position = new Vector3(-12, 4.5f, 0);

        Vector3[] waypoints = new[]
        {
            new Vector3(targetPosition.x - targetXOffset, targetPosition.y + targetYOffset, 0), 
            targetPosition - lowestPointOffset,
            targetPosition,
            new Vector3(targetPosition.x + targetXOffset, targetPosition.y + targetYOffset, 0),
        };

        lastTween = transform.DOPath(waypoints, 3, PathType.CatmullRom, PathMode.Sidescroller2D);
    }
}