using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InvisibleWalls : MonoBehaviour
{
    [SerializeField] private BoxCollider2D ceiling;
    [SerializeField] private BoxCollider2D floor;
    [SerializeField] private BoxCollider2D left;
    [SerializeField] private BoxCollider2D right;

    private ReactiveCommand<Vector2> targetCollideBottom = new ReactiveCommand<Vector2>();
    public IObservable<Vector2> TargetCollideBottom => targetCollideBottom;

    // Start is called before the first frame update
    void Start()
    {
        SetToScreenSize();

        floor.OnCollisionEnter2DAsObservable().Subscribe(OnCollide).AddTo(this);
    }

    private void OnCollide(Collision2D collision)
    {
        var meanPoint = collision.contacts.Select(c => c.point).Aggregate((vec1, vec2) => vec1 + vec2) / collision.contacts.Length;
        targetCollideBottom.Execute(meanPoint);
    }
    
    // Update is called once per frame
    private void SetToScreenSize()
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float camHalfHeight = Camera.main.orthographicSize; 
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 2.0f * camHalfWidth;

        ceiling.size = new Vector2(camWidth+2, floor.size.y);
        floor.size = new Vector2(camWidth+2, ceiling.size.y);

        left.transform.localPosition = new Vector3(-camHalfWidth - 1f, left.transform.localPosition.y, left.transform.localPosition.z);
        right.transform.localPosition = new Vector3(camHalfWidth + 1f, right.transform.localPosition.y, right.transform.localPosition.z);
    }
}
