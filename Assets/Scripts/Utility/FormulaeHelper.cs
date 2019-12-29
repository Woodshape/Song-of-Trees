using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormulaeHelper
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static int CalculateMeleeDamage(Entity attacker, Entity target, int damageMods = 0)
    {
        if (attacker == null || target == null)
            return 0;

        float damageLow = 0;
        float damageHigh = 0;
        int baseDamage = 0;
        int damageResult = 0;
        float chanceToHitMod = 0.0f;

        damageLow = Mathf.Pow(attacker.GetAttribute((int)AttributeName.Strength).AdjustedBaseValue * 0.5f, 2);
        damageHigh = Mathf.Pow(attacker.GetAttribute((int)AttributeName.Strength).AdjustedBaseValue * 1.5f, 2);
        //chanceToHitMod = attacker.GetSkill((int)SkillName.Melee_Offence).AdjustedBaseValue / 2;

        baseDamage = Random.Range((Mathf.RoundToInt(damageLow)), Mathf.RoundToInt(damageHigh));

        //TODO: implement damage modifiers for race, vulnerabilities etc.
        int damageModifiers = damageMods;

        if (CalculateMeleeHit(attacker, target, chanceToHitMod))
        {
            damageResult = Mathf.Max(0, (baseDamage + damageModifiers));
            damageResult = Mathf.RoundToInt((damageResult / target.GetAttribute((int)AttributeName.Strength).AdjustedBaseValue) * CalculateLevelFactor(attacker, target));
        }

        return damageResult;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static int CalculateMagicalDamage(Entity attacker, Entity target)
    // {
    //     if (attacker == null || target == null)
    //         return 0;

    //     float damageLow = 0;
    //     float damageHigh = 0;
    //     int damageModifiers = 0;
    //     int baseDamage = 0;
    //     int damageResult = 0;
    //     float chanceToHitMod = 0.0f;

    //     return damageResult;
    // }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool CalculateMeleeHit(Entity attacker, Entity target, float chanceToHitMod)
    {
        if (attacker == null || target == null)
            return false;

        float hit = chanceToHitMod;

        float levelFactor = CalculateLevelFactor(attacker, target);

        float compareDefence = (float)attacker.GetSkill((int)SkillName.Melee_Offence).AdjustedBaseValue /
                                target.GetSkill((int)SkillName.Melee_Defence).AdjustedBaseValue;

        hit += compareDefence * 75 * levelFactor;

        //  we always want a small amount of chance to hit or miss
        hit = Mathf.Clamp(hit, 3, 97);

        Debug.Log(attacker.gameObject.name + " CHANCE TO HIT: " + hit + " against " + target.gameObject.name);

        int roll = Random.Range(0, 101);

        if (roll <= hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static float CalculateLUKChance(Entity attacker, Entity target)
    {
        if (attacker == null || target == null)
            return 0;

        float luk = 0.0f;

        float levelFactor = CalculateLevelFactor(attacker, target);

        float compareLUK = (float)attacker.GetAttribute((int)AttributeName.Luck).AdjustedBaseValue /
                            target.GetAttribute((int)AttributeName.Luck).AdjustedBaseValue;

        luk += compareLUK * 5 * levelFactor;

        return luk;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static float CalculateLevelFactor(Entity attacker, Entity target)
    {
        if (attacker == null || target == null)
            return 0;

        float levelFactor = 0.0f;

        //  Assuming the MAX LEVEL is 99, then division by 50 will give us
        //  a clean +10% LEVEL FACTOR per 5 LEVELS
        levelFactor += (((float)attacker.Level - target.Level)
                        / 50)
                        + 1;

        return levelFactor;
    }
}

