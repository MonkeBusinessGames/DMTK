using System;
using System.Collections.Generic;

[Serializable]
public class Open5EResponse
{
    public int? count;
    public string next;
    public string previous;
    public List<Open5EResult> results;
}

[Serializable]
public class Open5EResult
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
    public SpeedData speed_all;
    public AbilityScoresData ability_scores;
    public int? normalSightRange;
    public int? darkvisionRange;
    public int? blindsightRange;
    public int? tremorsenseRange;
    public int? truesightRange;
    public ResistanceData resistances_and_immunities;
    public LanguageData languages;

    // Abilities
    public List<Trait> traits = new();
    public List<Action> actions = new();
}