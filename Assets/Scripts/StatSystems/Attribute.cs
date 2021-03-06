﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute : BaseStat
{
    new public AttributeName Name;

    new public const int STARTING_EXP_COST = 100;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public Attribute()
    {
        ExpToLevel = STARTING_EXP_COST;
        LevelModifier = 1.5f;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public enum AttributeName
{
    Strength,
    Dexterity,
    Intelligence,
    Health,
    Luck
}
