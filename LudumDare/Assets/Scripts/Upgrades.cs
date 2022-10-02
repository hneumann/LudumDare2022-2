using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    private static Upgrades instance;
    public static Upgrades Instance {
        get
        {
            return instance;
        }
    }

    [SerializeField] private float BaseForceMultiplier; // BaseForceMultiplier should be 0.1f
    [SerializeField] private int _priceIncreasePerLevel;

    [Header("VerticalFlightForce")]
    [SerializeField] private Sprite VerticalForceUpgradeSprite;
    [SerializeField] private Sprite VerticalForceUpgradeSpriteSecondary;
    public int verticalForceUpgradeBasePrice;
    public int VerticalForceLevel;
    public float ExtraVerticalForce => 1 + (VerticalForceLevel-1) * BaseForceMultiplier; 
    
    [Header("HorizontalFlightForce")]
    [SerializeField] private Sprite HorizontalForceUpgradeSprite;
    [SerializeField] private Sprite HorizontalForceUpgradeSpriteSecondary;
    public int horizontalForceUpgradeBasePrice;
    public int HorizontalForceLevel;
    public float ExtraHorizontalForce => 1 + (HorizontalForceLevel-1) * BaseForceMultiplier; 


    public void Reset()
    {
        VerticalForceLevel = 1;
        HorizontalForceLevel = 1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Reset();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            VerticalForceLevel++;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            HorizontalForceLevel++;
        }
    }

    void Awake()
    {
        instance = this;   
    }

    public List<UpgradeOption> GetUpgradeOptions()
    {
        var options = new List<UpgradeOption>();
        AddHorizontalForceOption(options);
        AddVerticalForceOption(options);
        AddVerticalForceOption(options);

        Helper.Shuffle(options);
        return options.Take(3).ToList();
    }

    private void AddVerticalForceOption(List<UpgradeOption> options)
    {
        options.Add(new UpgradeOption{
            sprite = VerticalForceUpgradeSprite,
            spriteSecondary = VerticalForceUpgradeSpriteSecondary,
            onSelected = () => VerticalForceLevel += 1,
            currentLevel = VerticalForceLevel,
            text = "Vertical Flight Power",
            price = verticalForceUpgradeBasePrice + (VerticalForceLevel * _priceIncreasePerLevel)
        });
    }

    private void AddHorizontalForceOption(List<UpgradeOption> options)
    {
        options.Add(new UpgradeOption{
            sprite = HorizontalForceUpgradeSprite,
            spriteSecondary = HorizontalForceUpgradeSpriteSecondary,
            onSelected = () => HorizontalForceLevel += 1,
            currentLevel = HorizontalForceLevel,
            text = "Horizontal Flight Power",
            price = horizontalForceUpgradeBasePrice + (HorizontalForceLevel * _priceIncreasePerLevel)
        });
    }


}

public class UpgradeOption
{
    public Sprite sprite;
    public Sprite spriteSecondary;
    public Action onSelected;
    public int currentLevel;
    public string text;
    public int price;
}