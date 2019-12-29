using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat
{
    public const int STARTING_EXP_COST = 100;   //  constant value for all base stats to start

    private string name;

    private int baseValue;
    private int buffValue;
    private int expToLevel;
    private float levelModifier;

    #region Setters and Getters
    public string Name { get => name; set => name = value; }
    public int BaseValue { get => baseValue; set => baseValue = value; }
    public int BuffValue { get => buffValue; set => buffValue = value; }
    public int ExpToLevel { get => expToLevel; set => expToLevel = value; }
    public float LevelModifier { get => levelModifier; set => levelModifier = value; }
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public BaseStat()
    {
        name = "";
        baseValue = 10;
        buffValue = 0;
        expToLevel = STARTING_EXP_COST;
        levelModifier = 1.1f;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public int AdjustedBaseValue
    {
        get => baseValue + buffValue;
        set => baseValue = value;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private int CalculateExpToLevel()
    {
        return (int)(expToLevel * levelModifier);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void LevelUp()
    {
        expToLevel = CalculateExpToLevel();
        baseValue++;
    }
}
