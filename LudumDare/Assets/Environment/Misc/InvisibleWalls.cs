using UnityEngine;

public class InvisibleWalls : MonoBehaviour
{
    [SerializeField] private BoxCollider2D ceiling;
    [SerializeField] private BoxCollider2D left;
    [SerializeField] private BoxCollider2D right;
    
    // Start is called before the first frame update
    void Start()
    {
        SetToScreenSize();
    }

    // Update is called once per frame
    void SetToScreenSize()
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float camHalfHeight = Camera.main.orthographicSize; 
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 2.0f * camHalfWidth;

        ceiling.size = new Vector2(camWidth+2, ceiling.size.y);

        left.transform.localPosition = new Vector3(-camHalfWidth - 1f, left.transform.localPosition.y, left.transform.localPosition.z);
        right.transform.localPosition = new Vector3(camHalfWidth + 1f, right.transform.localPosition.y, right.transform.localPosition.z);
    }
}
