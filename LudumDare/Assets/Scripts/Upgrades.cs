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

    [SerializeField] private Sprite HorizontalForceUpgradeSprite;
    [SerializeField] private Sprite VerticalForceUpgradeSprite;
    
    [SerializeField] private Sprite HorizontalForceUpgradeSpriteSecondary;
    [SerializeField] private Sprite VerticalForceUpgradeSpriteSecondary;

    public float ExtraHorizontalForce => HorizontalForceLevel * BaseForceMultiplier; 
    public float ExtraVerticalForce => VerticalForceLevel * BaseForceMultiplier; 
    

    public int VerticalForceLevel;
    public int HorizontalForceLevel;
    // TODO moar upgrades

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
            text = "Vertical Flight Power"
        });
    }

    private void AddHorizontalForceOption(List<UpgradeOption> options)
    {
        options.Add(new UpgradeOption{
            sprite = HorizontalForceUpgradeSprite,
            spriteSecondary = HorizontalForceUpgradeSpriteSecondary,
            onSelected = () => HorizontalForceLevel += 1,
            currentLevel = HorizontalForceLevel,
            text = "Horizontal Flight Power"
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
}