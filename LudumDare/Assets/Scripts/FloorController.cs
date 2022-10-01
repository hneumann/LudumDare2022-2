using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField] private float tileSpeed = 1;
    
    private List<SpriteRenderer> floorTiles;
    private float tileWidth => floorTiles.First().bounds.size.x;
    
    void Awake()
    {
        floorTiles = GetComponentsInChildren<SpriteRenderer>().ToList();
        UpdatePositions(0);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        UpdatePositions(Time.time);
    }

    private void UpdatePositions(float time)
    {
        var totalWidth = tileWidth * floorTiles.Count;
        
        for (var i = 0; i < floorTiles.Count; i++)
        {
            var tile = floorTiles[i];
            var screenPosIdx = i - Mathf.Floor(floorTiles.Count / 2);

            var verticalPosition = tileWidth * i + time * tileSpeed;
            verticalPosition = verticalPosition % (totalWidth ) * -1f;
            verticalPosition = verticalPosition + (totalWidth / 2);
            

            tile.transform.localPosition = Vector3.right * verticalPosition;
        }
    }
}
