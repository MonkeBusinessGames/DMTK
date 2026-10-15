using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class CreatureData
{
    // Identity
    public string key;
    public string name;
    public DocumentData document;

    // Classification
    public string category;
    public string subcategory;
    public string alignment;
    public ReferenceData type;
    public ReferenceData size;

    // Combat
    public float? challenge_rating;
    public int? proficiency_bonus;
    public int? experience_points;
    public int? armor_class;
    public string armor_detail;
    public int? hit_points;
    public string hit_dice;
    public int? initiative_bonus;

    // Statistics
    public SpeedData speed;
    public AbilityScoresData ability_scores;
    public SensesData senses;
    public ResistanceData resistances;
    public LanguageData languages;

    // Abilities
    public List<Trait> traits = new();
    public List<Action> actions = new();

}
[System.Serializable]
public class DocumentData
{
    public string name;
    public string key;
    public string type;
    public string display_name;
    public ReferenceData publisher;
    public ReferenceData gamesystem;
    public string permalink;
}

[System.Serializable]
public class ReferenceData
{
    public string name;
    public string key;
}

[System.Serializable]
public class LanguageData
{
    public string as_string;
    public List<LanguageDetails> data;
}

public class LanguageDetails
{
    public string name;
    public string key;
    public string desc;

}

[System.Serializable]
public class SpeedData
{
    public string unit;
    public int? walk;
    public int? crawl;
    public bool hover;
    public int? fly;
    public int? burrow;
    public int? climb;
    public int? swim;
}

[System.Serializable]
public class Action
{
    public string name;
    public string desc;
    public string action_type;
    public int? order_in_statblock;
    public int? legendary_action_cost;
    public string limited_to_form;
    public List<Attack> attacks = new();
}

[System.Serializable]
public class Attack
{
    public string name;
    public string attack_type;
    public int? to_hit_mod;

    public int? reach;
    public int? range;
    public int? long_range;

    public bool target_creature_only;

    public int? damage_die_count;
    public string damage_die_type;
    public int? damage_bonus;
    public DamageType damage_type;

    public int? extra_damage_die_count;
    public string extra_damage_die_type;
    public int? extra_damage_bonus;
    public DamageType extra_damage_type;

    public string distance_unit;
}

[System.Serializable]
public class DamageType
{
    public string name;
    public string key;
}

[System.Serializable]
public class Trait
{
    public string name;
    public string desc;
}
[System.Serializable]
public class AbilityScoresData
{
    public int? strength;
    public int? dexterity;
    public int? constitution;
    public int? intelligence;
    public int? wisdom;
    public int? charisma;

    public int? GetModifier(int? score)
    {
        if (score == null)
            return null;
        return (int?) Mathf.FloorToInt((((float) score.Value) - 10f) / 2.0f);
    }

    public int? StrengthModifier => GetModifier(strength);
    public int? DexterityModifier => GetModifier(dexterity);    
    public int? ConstitutionModifier => GetModifier(constitution);
    public int? IntelligenceModifier => GetModifier(intelligence);
    public int? WisdomModifier => GetModifier(wisdom);
    public int? CharismaModifier => GetModifier(charisma);
}

[Serializable]
public class SensesData
{
    public int? normalSightRange;
    public int? darkvisionRange;
    public int? blindsightRange;
    public int? tremorsenseRange;
    public int? truesightRange;
}

[Serializable]
public class ResistanceData
{
    public string damageImmunities;
    public string damageResistances;
    public string damageVulnerabilities;
    public string conditionImmunities;
}