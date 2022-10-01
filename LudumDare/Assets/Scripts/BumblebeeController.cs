using UnityEngine;
using Zenject;

public class BumblebeeController : MonoBehaviour
{
    [Header("References")]
    [Inject] private GameController gameController;

    [Header("Controls")]
    [SerializeField] private Rigidbody2D rigidbody;

    [Header("Vertical force")] 
    [SerializeField] private bool useForceCurve;
    [SerializeField] private float forceFactor;
    [SerializeField] private AnimationCurve forceFactorCurve;
    [SerializeField] private float maxVerticalVelocity;

    [Header("Score")]
    private int _nectarCount = 0;

    void Update()
    {
        if (gameController.GetState() == GameController.GameState.playing) {
            //var verticalValue = Input.GetAxis("Vertical");
            var verticalValue = 0f;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                verticalValue = 1f;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                verticalValue = -1f;
            }

            var horizontalValue = 0f;
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                horizontalValue = 1f;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                horizontalValue = -1f;
            }

            float horizontalForce;
            float verticalForce;

            if (useForceCurve) {
                verticalForce = forceFactorCurve.Evaluate(verticalValue);
                horizontalForce = forceFactorCurve.Evaluate(horizontalValue);
            } else {
                verticalForce = verticalValue * forceFactor;
                horizontalForce = horizontalValue * forceFactor;
            }

            var force = Vector2.up * verticalForce + Vector2.right * horizontalForce;
            rigidbody.AddForce(force, ForceMode2D.Impulse);

            rigidbody.velocity = Vector2.up * Mathf.Clamp(rigidbody.velocity.y, -maxVerticalVelocity, maxVerticalVelocity) + Vector2.right * Mathf.Clamp(rigidbody.velocity.x, -maxVerticalVelocity, maxVerticalVelocity);
        }
    }

    private void OnTriggerEnter2D ( Collider2D collision ) {
        
    }

    private void OnTriggerStay2D (Collider2D collision) {
        GameObject hitObject = collision.gameObject;
        if(hitObject.tag == "Flower") {
            _nectarCount += hitObject.GetComponentInParent<Flower>().Harvest();
        }
    }

    private void OnTriggerExit2D ( Collider2D collision ) {
        
    }

    public int NectarCount => _nectarCount;
}