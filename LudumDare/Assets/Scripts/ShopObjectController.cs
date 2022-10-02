using UnityEngine;
using Zenject;

public class ShopObjectController : MonoBehaviour
{
    [Inject] private GameController _gameController;
    [Inject] private ShopController _shopController;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private GameObject _shopKeeper;
    [SerializeField] private GameObject _closedSign;
    private bool _shopOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake () {
        _shopKeeper.SetActive(true);
        _closedSign.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController.GetState() == GameController.GameState.playing) {
            transform.localPosition = new Vector3(transform.localPosition.x - _movementSpeed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
            if(transform.localPosition.x <= -15f) {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D collider) {
        if (_shopOpen) {
            GameObject hitObject = collider.gameObject;
            if (hitObject.tag == "Player") {
                _gameController.StartShopping(this);
            }
        }
    }

    public void CloseShop() {
        _shopOpen = false;
        _shopKeeper.SetActive(false);
        _closedSign.SetActive(true);
    }
}
