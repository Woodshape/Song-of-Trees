﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiedStat : BaseStat
{
    private List<ModifyingAttribute> mods;
    private List<ModifyingValue> values;
    private int modValue;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public ModifiedStat()
    {
        mods = new List<ModifyingAttribute>();
        values = new List<ModifyingValue>();
        modValue = 0;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void AddModifier(ModifyingAttribute mod)
    {
        mods.Add(mod);

        Update();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void AddModifierValue(ModifyingValue val)
    {
        values.Add(val);

        Update();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Update()
    {
        CalculateModValues();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public new int AdjustedBaseValue
    {
        get => (BaseValue + BuffValue + modValue);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void CalculateModValues()
    {
        modValue = 0;

        if (mods.Count > 0)
        {
            foreach (ModifyingAttribute att in mods)
            {
                modValue += (int)(att.ModAttribute.AdjustedBaseValue * att.Ratio);
            }
        }

        if (values.Count > 0)
        {
            foreach (ModifyingValue val in values)
            {
                modValue += (int)(val.Value * val.Ratio);
            }
        }
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public struct ModifyingAttribute
{
    public Attribute ModAttribute;
    public float Ratio;

    public ModifyingAttribute(Attribute att, float rat = 1.0f)
    {
        ModAttribute = att;
        Ratio = rat;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public struct ModifyingValue
{
    public float Value;
    public float Ratio;

    public ModifyingValue(int val, float rat = 1.0f)
    {
        Value = val;
        Ratio = rat;
    }
}