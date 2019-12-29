using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity : MonoBehaviour, IDamageable
{
    new private string name;
    private int level;
    private uint freeExp;

    [SerializeField]
    protected Attribute[] attributes;
    private Vital[] vitals;
    private Skill[] skills;
    private Characteristic[] characteristics;

    public int DamageMods;

    public float InvTime;

    public float KnockbackForce;

    public GameObject CombatTextPrefab;

    #region Getters and Setters
    public string Name { get => name; set => name = value; }
    public int Level { get => level; set => level = value; }
    public uint FreeExp { get => freeExp; set => freeExp = value; }
    #endregion

    #region Getters
    public Attribute GetAttribute(int index)
    {

        return attributes[index];
    }

    public Vital GetVital(int index)
    {

        return vitals[index];
    }

    public Skill GetSkill(int index)
    {

        return skills[index];
    }

    public Characteristic GetCharacteristic(int index)
    {
        return characteristics[index];
    }
    #endregion

    public bool IsAlive
    {
        get { return GetVital((int)VitalName.Life).CurrentValue > 0; }
        set => IsAlive = value;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected virtual void Awake()
    {
        name = string.Empty;
        level = 1;
        freeExp = 0;

        attributes = new Attribute[Enum.GetValues(typeof(AttributeName)).Length];
        vitals = new Vital[Enum.GetValues(typeof(VitalName)).Length];
        skills = new Skill[Enum.GetValues(typeof(SkillName)).Length];
        characteristics = new Characteristic[Enum.GetValues(typeof(CharacteristicName)).Length];

        SetupAttributes();
        SetupVitals();
        SetupSkills();
        SetupCharacteristics();

        StatUpdate();
        //OutputValues();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void AddExp(uint amount)
    {
        freeExp += amount;

        CalculateLevel();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //TODO: take avg of all skills and assign that as level
    public void CalculateLevel() { }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void StatUpdate()
    {
        for (int i = 0; i < vitals.Length; i++)
        {
            //int oldValue = vitals[i].AdjustedBaseValue;
            vitals[i].Update();
            //Debug.Log("Updating " + vitals[i].Name + " from " + oldValue + " to " + vitals[i].AdjustedBaseValue);
        }

        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].Update();
        }

        for (int i = 0; i < characteristics.Length; i++)
        {
            characteristics[i].Update();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected virtual void FixedUpdate()
    {
        if (!IsAlive)
            return;

        InvTime -= Time.deltaTime;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public virtual int TakeDamage(Entity attacker, Entity target)
    {
        int dmg = FormulaeHelper.CalculateMeleeDamage(attacker, target, DamageMods);

        //TODO: % Reduction = (Armor / ([85 * Enemy_Level] + Armor + 400)) * 100
        int armour = GetCharacteristic((int)CharacteristicName.Armour).AdjustedBaseValue;
        Debug.LogFormat("Armor reduced {0} damage by {1}.", dmg, armour);
        dmg -= armour;

        //TODO: add special behavior if no damage is taken
        if (dmg > 0)
        {
            Debug.Log(attacker.gameObject.name + " dealing " + dmg + " damage to " + target.gameObject.name);

            InvTime = 1.0f;

            GetVital((int)VitalName.Life).CurrentValue -= dmg;

            GameObject combatText = Instantiate(CombatTextPrefab, transform.position, Quaternion.identity);
            combatText.GetComponent<CombatText>().MainTarget = target.transform;
            combatText.GetComponent<CombatText>().Damage = dmg;

            Player playerAttacker = attacker as Player;

            if (playerAttacker != null)
            {
                Camera.main.GetComponent<CameraController>().CameraShake();
                combatText.GetComponent<CombatText>().IsPlayerDamaged = false;
            }
            else
            {
                GameManager.Instance.UIManager.UpdateHealthBar();
                combatText.GetComponent<CombatText>().IsPlayerDamaged = true;
            }

            StartCoroutine(GetComponent<EntityController>().Flash());
            StartCoroutine(GetComponent<EntityController>().Knockback(attacker, target, this.GetComponent<Entity>().KnockbackForce, 0.5f));

            if (!IsAlive)
            {
                StartCoroutine(Die(5.0f));
            }
        }

        return dmg;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public virtual IEnumerator Die(float despawnTimer)
    {
        transform.localScale += new Vector3(0.5f, 0, 0);

        yield return new WaitForSeconds(despawnTimer);

        Destroy(this.gameObject);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Kill()
    {
        IsAlive = false;
        Die(1.0f);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetupAttributes()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i] = new Attribute();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetupVitals()
    {
        for (int i = 0; i < vitals.Length; i++)
        {
            vitals[i] = new Vital();
        }

        SetupVitalModifiers();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetupSkills()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i] = new Skill();
        }

        SetupSkillModifiers();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetupCharacteristics()
    {
        for (int i = 0; i < characteristics.Length; i++)
        {
            characteristics[i] = new Characteristic();
        }

        SetupCharacteristicModifiers();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetupVitalModifiers()
    {
        //  Life
        GetVital((int)VitalName.Life).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Strength), 2.0f));
        GetVital((int)VitalName.Life).CurrentValue = GetVital((int)VitalName.Life).AdjustedBaseValue;

        //  Fatigue
        GetVital((int)VitalName.Fatigue).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Health)));
        GetVital((int)VitalName.Fatigue).CurrentValue = GetVital((int)VitalName.Fatigue).AdjustedBaseValue;

        //  Energy
        GetVital((int)VitalName.Energy).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Intelligence)));
        GetVital((int)VitalName.Energy).CurrentValue = GetVital((int)VitalName.Energy).AdjustedBaseValue;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetupSkillModifiers()
    {

        // melee offence

        GetSkill((int)SkillName.Melee_Offence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Strength)));
        //GetSkill((int)SkillName.Melee_Offence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Dexterity), 0.5f));

        // melee defence

        GetSkill((int)SkillName.Melee_Defence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Dexterity)));
        GetSkill((int)SkillName.Melee_Defence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Luck), 0.5f));

        // range offence

        GetSkill((int)SkillName.Ranged_Offence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Dexterity)));
        //GetSkill((int)SkillName.Ranged_Offence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Strength), 0.5f));

        // range defence

        GetSkill((int)SkillName.Ranged_Defence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Dexterity)));
        GetSkill((int)SkillName.Ranged_Defence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Luck), 0.5f));

        // magic offence

        GetSkill((int)SkillName.Magic_Offence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Intelligence)));
        //GetSkill((int)SkillName.Magic_Offence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Health), 0.5f));

        // magic defence

        GetSkill((int)SkillName.Magic_Defence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Intelligence)));
        GetSkill((int)SkillName.Magic_Defence).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Luck), 0.5f));

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetupCharacteristicModifiers()
    {
        //GetCharacteristic((int)CharacteristicName.Armour).AddModifier(new ModifyingAttribute(GetAttribute((int)AttributeName.Luck)));
        //GetCharacteristic((int)CharacteristicName.Armour).AddModifierValue(new ModifyingValue(100));
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected void OutputValues()
    {
        Debug.Log("ATTRIBUTES");
        for (int i = 0; i < attributes.Length; i++)
        {
            Debug.Log(this.gameObject.name + " " + attributes[i].Name + " " + attributes[i].AdjustedBaseValue);
        }

        Debug.Log("VITALS");
        for (int i = 0; i < vitals.Length; i++)
        {
            Debug.Log(this.gameObject.name + " " + vitals[i].Name + " " + vitals[i].AdjustedBaseValue);
        }

        Debug.Log("SKILLS");
        for (int i = 0; i < skills.Length; i++)
        {
            Debug.Log(this.gameObject.name + " " + skills[i].Name + " " + skills[i].AdjustedBaseValue);
        }

        Debug.Log("CHARACTERISTICS");
        for (int i = 0; i < characteristics.Length; i++)
        {
            Debug.Log(this.gameObject.name + " " + characteristics[i].Name + " " + characteristics[i].AdjustedBaseValue);
        }
    }
}
