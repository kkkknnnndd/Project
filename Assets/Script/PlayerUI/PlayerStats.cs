using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region StatsVariable
    [Header("Character Level")]
    public int playerLevel = 1;

    [Header("Stat Levels")]
    public int healthLevel = 10;
    public int staminaLevel = 10;

    [Header("Stats")]
    public int maxHealth;
    public int currentHealth;
    public float maxStamina;
    public float currentStamina;

    
    public int attackDamage = 10;
    public static int _attackD;
    public int currentSouls = 1000;

    public float staminaRegenRate = 20f; // Tốc độ hồi phục Stamina mỗi giây
    private float staminaRegenTimer = 2f; // Thời gian delay sau mỗi hành động animation
    public BoxCollider bxcollid;
    [SerializeField] private AttackSO _impactAni;
    public AudioClip HitSFX;
    private AudioSource audioFx;

    private Coroutine regenCoroutine;

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    
    private Animator ani;
    private bool isDodging;
    private float isRunning;
    private bool isAttacking;
    private CharacterController _characterController;
    private LevelUpUI _levelUpUI;
    public bool _isLoadCurrentHealth = false;
    private GameObject tempChar;
    private int _soulsRequireLevelUp;
    public bool _isLoadingyposition = false;
    #endregion
    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        _levelUpUI = FindObjectOfType<LevelUpUI>();
        _soulsRequireLevelUp = _levelUpUI.soulsRequiredToLevelUp;
        WorldSaveGameManager.instance.player = this;
    }
    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currentStamina);
        _attackD = attackDamage;
        bxcollid.enabled = false;

        // Bắt đầu Coroutine để hồi phục Stamina mượt mà
        regenCoroutine = StartCoroutine(RegenerateStamina());

        _characterController = gameObject.GetComponent<CharacterController>();
        tempChar = GameObject.FindGameObjectWithTag("Player");
        ani = tempChar.GetComponentInChildren<Animator>();
        audioFx = tempChar.GetComponent<CharacterController>().GetComponent<AudioSource>();
    }
    private void Update()
    {
        isDodging = ani.GetBool("Dodge");
        isAttacking = ani.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        isRunning = ani.GetFloat("Ani_Transition");
    }   
    public int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    public float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        ani.runtimeAnimatorController = _impactAni.aniOV;
        ani.Play("Impact", 0, 0.05f);
        audioFx.PlayOneShot(HitSFX);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }
    public void HealHP()
    {
        currentHealth = maxHealth;
        healthBar.SetCurrentHealth(currentHealth);
    }
    public void TakeStaminaDamage(int damage)
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentStamina -= damage * Time.deltaTime;
        }
        else
        {
            currentStamina = Mathf.Min(currentStamina - damage, maxStamina);
        }
        staminaBar.SetCurrentStamina(currentStamina);
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
    }
    IEnumerator RegenerateStamina()
    {
        while (true)
        {
            if(isDodging || isAttacking || isRunning >= 0.9f)
            {
                staminaRegenTimer = 0;
            }
            // Hồi phục Stamina mỗi giây
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 0.7f)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                    staminaBar.SetCurrentStamina(currentStamina);
                }
            }
            yield return null;
        }
    }
    public void AddSouls(int amount)
    {
        currentSouls += amount + Mathf.RoundToInt(_soulsRequireLevelUp / 2);
        _levelUpUI.UpdateUI();
    }
    public void DeductSouls(int amount)
    {
        currentSouls -= amount;
        if (currentSouls < 0)
        {
            currentSouls = 0;
        }
    }
    public void IncreaseAttackDamage(int amount)
    {
        attackDamage += amount;
    }
    public void DecreaseAttackDamage(int amount)
    {
        attackDamage -= amount;
        if (attackDamage < 0)
        {
            attackDamage = 0;
        }
    }
    public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
    {
        currentCharacterSaveData.characterLevel = playerLevel;

        currentCharacterSaveData.currentHealth = maxHealth; // 
        currentCharacterSaveData.maxHealth = maxHealth;
        currentCharacterSaveData.healthLevel = healthLevel;
        currentCharacterSaveData.currentStamina = maxStamina; //
        currentCharacterSaveData.maxStamina = maxStamina;
        currentCharacterSaveData.staminaLevel = staminaLevel;
        currentCharacterSaveData.currentAttackDamage = attackDamage;
        currentCharacterSaveData.currentSouls = currentSouls;

        currentCharacterSaveData.healthSliderValue = maxHealth;// healthBar.slider.value;
        currentCharacterSaveData.staminaSliderValue = maxStamina; // staminaBar.slider.value;

        currentCharacterSaveData.xPosition = tempChar.transform.position.x;
        currentCharacterSaveData.yPosition = tempChar.transform.position.y;
        currentCharacterSaveData.zPosition = tempChar.transform.position.z;
        currentCharacterSaveData.yRotation = tempChar.transform.rotation.y;
        _isLoadingyposition = true;
    }
    public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
    {
        playerLevel = currentCharacterSaveData.characterLevel;

        currentHealth = currentCharacterSaveData.maxHealth;
        maxHealth = currentCharacterSaveData.maxHealth;
        healthLevel = currentCharacterSaveData.healthLevel;
        currentStamina = currentCharacterSaveData.maxStamina;
        maxStamina = currentCharacterSaveData.maxStamina;
        staminaLevel = currentCharacterSaveData.staminaLevel;
        attackDamage = currentCharacterSaveData.currentAttackDamage;
        currentSouls = currentCharacterSaveData.currentSouls;
        _isLoadCurrentHealth = false;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(maxHealth);
        healthBar.slider.value = currentCharacterSaveData.healthSliderValue;

        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(maxStamina);
        staminaBar.slider.value = currentCharacterSaveData.staminaSliderValue;

        _levelUpUI.UpdateUI();
        _isLoadCurrentHealth = false;
        _characterController.enabled = false;
        transform.position = new Vector3(currentCharacterSaveData.xPosition, 
            6f, currentCharacterSaveData.zPosition);
        transform.localEulerAngles = new Vector3(0f, currentCharacterSaveData.yRotation, 0f);
        DoiHuong._dhChangeScene = true;
        _characterController.enabled = true;
    }
    public void enableColliSword(float num)
    {
        if (num == 1)
        {
            bxcollid.enabled = true;
        }
        else
        {
            bxcollid.enabled = false;
        }
    }
}