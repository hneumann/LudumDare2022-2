using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;

public class Upgrade
{
    public string name;
    public int cost;
}
public class ShopController : MonoBehaviour
{
    [Inject] private GameController _gameController;
    [Inject] private BumblebeeController _player;

    [Header("Left Button")]    
    [SerializeField] private Button leftButton;
    [SerializeField] private Image leftButtonImage;
    [SerializeField] private TMPro.TMP_Text leftButtonText;
    [SerializeField] private TMPro.TMP_Text leftPrice;
    [Header("Center Button")]    
    [SerializeField] private Button centerButton;
    [SerializeField] private Image centerButtonImage;
    [SerializeField] private TMPro.TMP_Text centerButtonText;
    [SerializeField] private TMPro.TMP_Text centerPrice;
    [Header("Right Button")]    
    [SerializeField] private Button rightButton;
    [SerializeField] private Image rightButtonImage;
    [SerializeField] private TMPro.TMP_Text rightButtonText;
    [SerializeField] private TMPro.TMP_Text rightPrice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake () {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenShopScreen(List<UpgradeOption> options)
    {
        SetupButton(leftButton, leftButtonImage, leftButtonText, leftPrice, options[0]);
        SetupButton(centerButton, centerButtonImage, centerButtonText, centerPrice, options[1]);
        SetupButton(rightButton, rightButtonImage, rightButtonText, rightPrice, options[2]);
    }

    private void SetupButton(Button button, Image image, TMPro.TMP_Text text, TMPro.TMP_Text price, UpgradeOption option)
    {
        //ButtonAnimation(button.gameObject);
        image.sprite = option.sprite;
        text.text = option.text;
        price.text = option.price.ToString();
        if(_player.PollenCount >= option.price) {
            button.interactable = true;
            button.onClick.AddListener(() => OnButtonClicked(option));
            button.onClick.AddListener(CloseShop);
        } else {
            button.interactable = false;
        }
    }
    
    private void OnButtonClicked(UpgradeOption option)
    {
        if(_player.PollenCount >= option.price) {
            _player.BuyUpgrade(option.price);
            option.onSelected();
            // happy shopkeeper noises

        } else {
            // nicht genug Pollen
            // Sad shopkeeper noises
        }
    }

    public void OpenShop() {
        this.gameObject.SetActive(true);
        var options = Upgrades.Instance.GetUpgradeOptions();
        OpenShopScreen(options);
    }

    public void CloseShop()
    {
        this.gameObject.SetActive(false);

        leftButton.onClick.RemoveAllListeners();
        centerButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        
        _gameController.StopShopping();
    }
}
