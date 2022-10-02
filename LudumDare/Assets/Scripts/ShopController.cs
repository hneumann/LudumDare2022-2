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

    [Header("Left Button")]    
    [SerializeField] private Button leftButton;
    [SerializeField] private Image leftButtonImage;
    [SerializeField] private TMPro.TMP_Text leftButtonText;
    [Header("Center Button")]    
    [SerializeField] private Button centerButton;
    [SerializeField] private Image centerButtonImage;
    [SerializeField] private TMPro.TMP_Text centerButtonText;
    [Header("Right Button")]    
    [SerializeField] private Button rightButton;
    [SerializeField] private Image rightButtonImage;
    [SerializeField] private TMPro.TMP_Text rightButtonText;

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
        SetupButton(leftButton, leftButtonImage, leftButtonText, options[0]);
        SetupButton(centerButton, centerButtonImage, centerButtonText, options[1]);
        SetupButton(rightButton, rightButtonImage, rightButtonText, options[2]);
    }

    private void SetupButton(Button button, Image image, TMPro.TMP_Text text, UpgradeOption option)
    {
        //ButtonAnimation(button.gameObject);
        image.sprite = option.sprite;
        text.text = option.text;
        button.onClick.AddListener(() => OnButtonClicked(option));
        button.onClick.AddListener(CloseShop);
    }
    
    private void OnButtonClicked(UpgradeOption option)
    {
        option.onSelected();
        /*var icon = Instantiate(skillLearnedPrefab, null, true);
        icon.GetComponent<UpgradeIcon>().SetSprite(option.spriteSecondary);
        icon.transform.SetParent(skillLearnedParent);
        selectedUpgrades.Add(icon); */
    }

    public void OpenShop() {
        this.gameObject.SetActive(true);
        var options = Upgrades.Instance.GetUpgradeOptions();
        OpenShopScreen(options);
    }

    private void CloseShop()
    {
        this.gameObject.SetActive(false);

        leftButton.onClick.RemoveAllListeners();
        centerButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        
        _gameController.StopShopping();
    }
}
