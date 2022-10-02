using DG.Tweening;
using UnityEngine;
using Zenject;

public class BumblebeeController : MonoBehaviour
{
    [Header("References")] [Inject] private GameController gameController;

    [Header("Controls")] [SerializeField] private Rigidbody2D rigidbody;

    [Header("Vertical force")] [SerializeField]
    private bool useForceCurve;

    [SerializeField] private float forceFactor;
    [SerializeField] private AnimationCurve forceFactorCurve;
    [SerializeField] private float maxVerticalVelocity;

    // Upgrades
    private int _verticalForceUpgradeLevel = 0;
    private int _horizontalForceUpgradeLevel = 0;

    [Header("Score")]
    private int _pollenCount = 0;

    void Update()
    {
        if (gameController.GetState() is GameController.GameState.playing or GameController.GameState.idle)
        {
            //var verticalValue = Input.GetAxis("Vertical");
            var verticalValue = 0f;

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                verticalValue = 1f;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                verticalValue = -1f;
            }

            var horizontalValue = 0f;

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                horizontalValue = 1f;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                horizontalValue = -1f;
            }

            float horizontalForce;
            float verticalForce;

            if (useForceCurve)
            {
                verticalForce = forceFactorCurve.Evaluate(verticalValue);
                horizontalForce = forceFactorCurve.Evaluate(horizontalValue);
            }
            else
            {
                verticalForce = verticalValue * forceFactor;
                horizontalForce = horizontalValue * forceFactor;
                //Applying additional force from Upgrades
                verticalForce *= Upgrades.Instance.ExtraVerticalForce;
                horizontalForce *= Upgrades.Instance.ExtraHorizontalForce;
            }

            var force = Vector2.up * verticalForce + Vector2.right * horizontalForce;
            rigidbody.AddForce(force, ForceMode2D.Impulse);

            rigidbody.velocity =
                Vector2.up * Mathf.Clamp(rigidbody.velocity.y, -maxVerticalVelocity, maxVerticalVelocity) +
                Vector2.right * Mathf.Clamp(rigidbody.velocity.x, -maxVerticalVelocity, maxVerticalVelocity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StartFlower" && gameController.GetState() == GameController.GameState.idle)
        {
            collision.transform.parent.DOMoveX(-30, 40);

            gameController.StartGame();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject hitObject = collision.gameObject;
        if (hitObject.tag == "Flower") {
            _pollenCount += hitObject.GetComponentInParent<Flower>().Harvest();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) { }

    public int PollenCount => _pollenCount;
    public void BuyUpgrade(int price) {
        _pollenCount -= price;
    }

    public int VerticalForceUpgradeLevel {
        get { return _verticalForceUpgradeLevel; }
        set { _verticalForceUpgradeLevel = value; }
    }
    public int HorizontalForceUpgradeLevel {
        get { return _horizontalForceUpgradeLevel; }
        set { _horizontalForceUpgradeLevel = value; }
    }

    public void Reset () {
        ResetUpgrades();
    }

    private void ResetUpgrades() {
        _horizontalForceUpgradeLevel = 1;
        _verticalForceUpgradeLevel = 1;
    }
}