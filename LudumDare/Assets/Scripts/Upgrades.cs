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
    public float ExtraVerticalForce => 1 + VerticalForceLevel * BaseForceMultiplier; 
    
    [Header("HorizontalFlightForce")]
    [SerializeField] private Sprite HorizontalForceUpgradeSprite;
    [SerializeField] private Sprite HorizontalForceUpgradeSpriteSecondary;
    public int horizontalForceUpgradeBasePrice;
    public int HorizontalForceLevel;
    public float ExtraHorizontalForce => 1 + HorizontalForceLevel * BaseForceMultiplier; 

    [Header("PollenHarvestSpeed")]
    [SerializeField] private Sprite HarvestSpeedUpgradeSprite;
    [SerializeField] private Sprite HarvestSpeedUpgradeSpriteSecondary;
    public int harvestSpeedUpgradeBasePrice;
    public int HarvestSpeedLevel;
    public float ExtraHarvestSpeed => 1 + HarvestSpeedLevel * BaseForceMultiplier; 

    [Header("BonusHarvestChance")]
    [SerializeField] private Sprite BonusHarvestUpgradeSprite;
    [SerializeField] private Sprite BonusHarvestUpgradeSpriteSecondary;
    public int bonusHarvestUpgradeBasePrice;
    public int BonusHarvestLevel;
    public float ExtraHarvestChance => BonusHarvestLevel * BaseForceMultiplier; 


    public void Reset()
    {
        VerticalForceLevel = 0;
        HorizontalForceLevel = 0;
        HarvestSpeedLevel = 0;
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
        AddHarvestSpeedOption(options);
        AddBonusHarvestOption(options);

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

    private void AddHarvestSpeedOption(List<UpgradeOption> options)
    {
        options.Add(new UpgradeOption{
            sprite = HarvestSpeedUpgradeSprite,
            spriteSecondary = HarvestSpeedUpgradeSpriteSecondary,
            onSelected = () => HarvestSpeedLevel += 1,
            currentLevel = HarvestSpeedLevel,
            text = "Harvest Speed Increase",
            price = harvestSpeedUpgradeBasePrice + (HarvestSpeedLevel * _priceIncreasePerLevel)
        });
    }

    private void AddBonusHarvestOption(List<UpgradeOption> options)
    {
        options.Add(new UpgradeOption{
            sprite = BonusHarvestUpgradeSprite,
            spriteSecondary = BonusHarvestUpgradeSpriteSecondary,
            onSelected = () => BonusHarvestLevel += 1,
            currentLevel = BonusHarvestLevel,
            text = "Bonus Harvest Chance",
            price = bonusHarvestUpgradeBasePrice + (BonusHarvestLevel * _priceIncreasePerLevel)
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