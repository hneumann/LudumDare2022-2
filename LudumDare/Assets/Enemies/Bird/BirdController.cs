using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class BirdController : MonoBehaviour
{
    [Inject] private InvisibleWalls invisibleWalls;
    [Inject] private GameController gameController;
    [Inject] private BumblebeeController player;
    [Inject] private SoundController soundController;

    [SerializeField] private float targetXOffset;
    [SerializeField] private float targetYOffset;
    [SerializeField] private Vector3 lowestPointOffset;
    [SerializeField] private Transform schnabelAnchor;

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

        lastTween = transform.DOPath(waypoints, 3, PathType.CatmullRom, PathMode.Sidescroller2D).OnWaypointChange(WaypointCallback);
    }

    void WaypointCallback(int waypointIndex) {
        if(waypointIndex == 2) {
            soundController.PlayBirdSound();
        }
    }

    private void OnCollisionEnter2D ( Collision2D collision ) {
        if(collision.collider.gameObject.tag == "Player") {            
            collision.collider.gameObject.transform.SetParent(schnabelAnchor);
            player.Die();
            gameController.GameOver();
        }
    }
}