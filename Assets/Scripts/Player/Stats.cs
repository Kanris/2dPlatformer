using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stats {

#region Health
    public float MaxHealth = 100f; //max object health
    private float m_currentHealth; //current object health
    public float CurrentHealth
    {
        get { return m_currentHealth; }
        set { m_currentHealth = Mathf.Clamp(value, 0, MaxHealth); } //current health can't be less than thero and greter than MaxHealth
    }
#endregion

    public float Speed = 300f; //object movement speed
    public string DeathSound = "Explosion"; //death sound
    public string DamageSound; //sound when object takes some damage

#region GameObjects
    private GameObject m_gameObject; //gameobject where this Stats attached
    private Slider m_healthSlider; //object health UI
    private Text m_healthText; //object text health UI
    private AudioManager m_audioManager; //audio manager
    private Transform m_deathPrefab;
#endregion

    public virtual void Initialize(GameObject parentGameObject)
    {
        m_gameObject = parentGameObject;

        m_audioManager = AudioManager.instance;

        InitializeHealth();
        InitializeDeathPrefab();
    }

    private void InitializeHealth()
    {
        m_healthSlider = m_gameObject.GetComponentInChildren(typeof(Slider)) as Slider;

        CurrentHealth = MaxHealth;

        if (m_healthSlider == null)
            Debug.LogError("Stats: Can't find slider!");
        else
            m_healthSlider.maxValue = CurrentHealth;

        m_healthText = m_gameObject.GetComponentInChildren(typeof(Text)) as Text;

        if (m_healthText != null)
            m_healthText.text = CurrentHealth + "/" + MaxHealth;
        else
            Debug.LogError("Stats: Can't find health text");
    }

    private void InitializeDeathPrefab()
    {
        var deathPrefabGO = Resources.Load("Effects/SpawnParticles") as GameObject;

        if (deathPrefabGO != null)
            m_deathPrefab = deathPrefabGO.transform;
        else
            Debug.LogError("Stats: Can't find death prefab particles");
    }

    public virtual void Damage(float damageFromSource)
    {
        CurrentHealth -= damageFromSource;
        DisplayUIChanges(damageFromSource);

        if (CurrentHealth <= 0)
        {
            KillObject();
        }

        PlayDamageSound();
    }

    protected virtual void KillObject()
    {
        PlayDeathSound();

        ShowDeathAnimation();

        GameObject.Destroy(m_gameObject); 
    }

    private void ShowDeathAnimation()
    {
        if (m_deathPrefab != null)
        {
            var deathEffect = GameObject.Instantiate
                (m_deathPrefab, m_gameObject.transform.position, m_gameObject.transform.rotation).gameObject;
            GameObject.Destroy(deathEffect, 2.5f);
        }

    }

    protected virtual void DisplayUIChanges(float damageFromSource)
    {
        if (m_healthText != null)
            m_healthText.text = CurrentHealth + "/" + MaxHealth;

        if (m_healthSlider != null)
            m_healthSlider.value += damageFromSource;
    }

    private void PlayDamageSound()
    {
        if (DamageSound != string.Empty)
        {
            m_audioManager.PlaySound(DamageSound);
        }
    }

    private void PlayDeathSound()
    {
        if (DeathSound != string.Empty)
        {
            m_audioManager.PlaySound(DeathSound);
        }
    }
}

[System.Serializable]
public class PlayerStats : Stats
{
    public static int Coins = 2000;
    public static float AdditionalDamage = 0f;
    public static float DamageResistance = 0f;
    public static int ResurectionStones = 0;
    public static string[] Equipment;

    private static Text m_coinsText;
    private delegate void VoidDelegate();

    public override void Initialize(GameObject parentGameObject)
    {
        InitializeCoinsText();

        InitializeEquipment();

        base.Initialize(parentGameObject);
    }

    private void InitializeCoinsText()
    {
        m_coinsText = GameObject.Find("CoinsUI").GetComponent<Text>();
        m_coinsText.text = "Coins:" + Coins.ToString();
    }

    private void InitializeEquipment()
    {
        if (Equipment == null)
        {
            var equipmentLength = (int)ItemType.ItemTypeCount;
            Equipment = new string[equipmentLength];
        }
    }

    protected override void KillObject()
    {
        var weaponChange = GameObject.FindObjectOfType(typeof(WeaponManager)) as WeaponManager;

        if (weaponChange != null)
            weaponChange.ResetWeaponGUI(); //reset player weapon GUI
        
        GameMaster.gm.PlayerIsDead(); //notify game manager that player is dead
        base.KillObject();
    }

    public static bool SpendMoney(int amount, Equipment item)
    {
        return SpendMoney(amount, item, () =>
        {
            ChangeCoinsAmount(amount);
            ApplyBuff(item, item.BuffAmount);

            int equipmentIndex = (int)item.itemType;
            Equipment[equipmentIndex] = item.Name;
        });
    }

    public static bool SpendMoney(int amount, Item item)
    {
        return SpendMoney(amount, item, () =>
        {
            ChangeCoinsAmount(amount);
            ResurectionStones++;
            DisplayAnnouncerMessage("Resurection stone amount - " + ResurectionStones, 2f);
        });
    }

    private static bool SpendMoney(int amount, Item item, VoidDelegate logic)
    {
        if (CheckMoney(amount, item))
        {
            logic();
            return true;
        }

        return false;
    }

    private static bool CheckMoney(int amount, Item item)
    {
        if (amount > Coins)
        {
            var announcerMessage = "Not enough money to buy - " + item.Name;
            DisplayAnnouncerMessage(announcerMessage, 4f);

            return false;
        }

        return true;
    }

    public static void ChangeCoinsAmount(int amount)
    {
        Coins -= amount;
        m_coinsText.text = "Coins:" + Coins.ToString(); 
    }

    private static void DisplayAnnouncerMessage(string message, float time)
    {
        AnnouncerManager.instance.StartCoroutine(
            AnnouncerManager.instance.DisplayAnnouncerMessage(message, time));
    }

    private static void ApplyBuff(Equipment item,  float buffAmount)
    {
        switch (item.itemType)
        {
            case ItemType.Damage:
                AdditionalDamage += buffAmount;
                break;
            case ItemType.Resistance:
                DamageResistance += buffAmount;
                break;
        }      

        var announcerMessage = item.itemType.ToString() + " increased by " + buffAmount;
        DisplayAnnouncerMessage(announcerMessage, 2f);
    }

    public override void Damage(float damageFromSource)
    {
        damageFromSource -= DamageResistance;

        if (damageFromSource <= 0)
            damageFromSource = 1;

        base.Damage(damageFromSource);
    }
}

[System.Serializable]
public class EnemyStats : Stats
{
    public float OutputDamage = 20f;
    public int EnemyDeathCost = 10;
    public bool isAttacking = false;

    protected override void KillObject()
    {
        PlayerStats.ChangeCoinsAmount(-EnemyDeathCost);
        base.KillObject();
    }
}


[System.Serializable]
public class RangeEnemyStats : EnemyStats
{
    public float AttackRange = 50f;
    public float AttackRate = 1f;

    [HideInInspector]
    public bool ShotPreparing = false;

    public Transform firePoint;
}
