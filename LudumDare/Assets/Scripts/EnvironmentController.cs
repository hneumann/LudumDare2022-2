using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private float tileSpeed = 1;
    [SerializeField] private SpriteRenderer prefab;
    [SerializeField] private bool useWidthOverride;
    [SerializeField] private float overrideWidth;
    

    private List<SpriteRenderer> floorTiles = new List<SpriteRenderer>(); 
    private float tileWidth => useWidthOverride ? overrideWidth : prefab.bounds.size.x;
 
    void Awake()
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float camHalfHeight = Camera.main.orthographicSize; 
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 2.0f * camHalfWidth;
        
        SetupTiles(camWidth);

        UpdatePositions(0);
    }

    private void Start()
    {
        Observable.EveryUpdate().Subscribe(_ => MyUpdate()).AddTo(this);
    }


    void MyUpdate()
    {
        UpdatePositions(Time.time);
    }

    private void SetupTiles(float screenWidth)
    {
        floorTiles = GetComponentsInChildren<SpriteRenderer>().ToList();
        if(!floorTiles.Contains(prefab))
        {
            floorTiles.Add(prefab);
        }

        var spriteCount = screenWidth/ prefab.bounds.size.x + 2;

        if(spriteCount == floorTiles.Count)
            return;

        while (spriteCount > floorTiles.Count)
        {
            floorTiles.Add(Instantiate(prefab, transform));
        }
        while (spriteCount < floorTiles.Count-1)
        {
            floorTiles.Remove(floorTiles.Last());
        }

    }
    
    private void UpdatePositions(float time)
    {
        var totalWidth = tileWidth * floorTiles.Count;

        for (var i = 0; i < floorTiles.Count; i++)
        {
            var tile = floorTiles[i];

            var verticalPosition = tileWidth * i + time * tileSpeed;
            verticalPosition = verticalPosition % (totalWidth) * -1f;
            verticalPosition += (totalWidth / 2);

            tile.transform.localPosition = Vector3.right * verticalPosition;
        }
    }
}