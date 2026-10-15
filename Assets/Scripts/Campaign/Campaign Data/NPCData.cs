using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NPCData : CreatureData
{
    public bool customIcon;
    public Sprite sprite;

    public NPCData(string newName)
    {
        name = newName;
        customIcon = false;
        sprite = null;
    }

    public NPCData(CreatureData data)
    {
        key = data.key;
        name = data.name;
        document = data.document;
        category = data.category;
        subcategory = data.subcategory;
        alignment = data.alignment;
        type = data.type;
        size = data.size;
        challenge_rating = data.challenge_rating;
        proficiency_bonus = data.proficiency_bonus;
        experience_points = data.experience_points;
        armor_class = data.armor_class;
        armor_detail = data.armor_detail;
        hit_points = data.hit_points;
        hit_dice = data.hit_dice;
        initiative_bonus = data.initiative_bonus;
        speed = data.speed;
        ability_scores = data.ability_scores;
        senses = data.senses;
        resistances = data.resistances;
        languages = data.languages;
        traits = data.traits;
        actions = data.actions;
        customIcon = false;
        sprite = null;


    }
}
