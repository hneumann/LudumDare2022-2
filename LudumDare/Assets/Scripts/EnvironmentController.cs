using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class EnvironmentController : MonoBehaviour
{
    [Inject] private GameController gameController;

    [SerializeField] private float tileSpeed = 1;
    [SerializeField] private Transform prefab;
    [SerializeField] private bool useWidthOverride;
    [SerializeField] private float overrideWidth;
    [SerializeField] private bool hideAtStart;

    private float actualTileSpeed = 1;
    private List<Transform> floorTiles = new List<Transform>(); 
    private float tileWidth;
 
    private float startTime;
 
 
    void Awake()
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float camHalfHeight = Camera.main.orthographicSize; 
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 2.0f * camHalfWidth;

        tileWidth = overrideWidth;
        
        if (!useWidthOverride && prefab.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            tileWidth = spriteRenderer.bounds.size.x;
        }
        
        SetupTiles(camWidth);

        UpdatePositions(0);
    }

    private void Start()
    {
        gameController.ScrollSpeedFactor.Subscribe(factor => actualTileSpeed = factor * tileSpeed).AddTo(this);
        gameController.ScrollSpeedFactor.Where(f => Mathf.Approximately(f, 1))
            .Take(1)
            .Subscribe(_ => startTime = Time.time);
        Observable.EveryUpdate().Subscribe(_ => MyUpdate()).AddTo(this);
    }


    void MyUpdate()
    {
        UpdatePositions(Time.time - startTime);
    }

    private void SetupTiles(float screenWidth)
    {
        floorTiles = GetComponentsInChildren<Transform>().ToList();
        floorTiles.Remove(transform);
        if(!floorTiles.Contains(prefab.transform))
        {
            floorTiles.Add(prefab.transform);
        }

        var spriteCount = screenWidth / tileWidth +2;

        if(spriteCount == floorTiles.Count)
            return;

        while (spriteCount > floorTiles.Count)
        {
            floorTiles.Add(Instantiate(prefab, transform).transform);
        }
        while (spriteCount < floorTiles.Count-1)
        {
            floorTiles.Remove(floorTiles.Last());
        }
        
        if(hideAtStart)
        {
            for (var i = Mathf.RoundToInt(floorTiles.Count/2f); i < floorTiles.Count; i++)
            {
                floorTiles[i].gameObject.SetActive(false);
            }
        }
    }
    
    private void UpdatePositions(float time)
    {
        var totalWidth = tileWidth * floorTiles.Count;

        for (var i = 0; i < floorTiles.Count; i++)
        {
            var tile = floorTiles[i];

            var verticalPosition = tileWidth * i + time * actualTileSpeed;
            verticalPosition = verticalPosition % (totalWidth) * -1f;
            verticalPosition += (totalWidth / 2);

            tile.transform.localPosition = Vector3.right * verticalPosition;

            if (hideAtStart && verticalPosition > 15)
            {
                tile.gameObject.SetActive(true);
            }
        }
    }
}