using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Since we want to reference this data for every save file, this script is not a monobehaviour and is instead serializable
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName;

    [Header("Character Level")]
    public int characterLevel;

    //Chỉ có thể save data đối với biến kiểu "basic", không dùng được Vector3 để lưu
    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
    public float yRotation;
    [Header("SceneNow")]
    public string _sceneNow;

    [Header("Stats")]
    public int currentHealth;
    public int maxHealth;
    public int healthLevel;
    public float currentStamina;
    public float maxStamina;
    public int staminaLevel;
    public int currentAttackDamage;
    public int currentSouls;

    public float healthSliderValue;
    public float staminaSliderValue;
}
