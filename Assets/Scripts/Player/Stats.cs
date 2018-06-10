using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stats {

    public float MaxHealth = 100f; //max object health
    private float _currentHealth; //current object health
    public float CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, MaxHealth); } //current health can't be less than thero and greter than MaxHealth
    }

    public float speed = 300f; //object movement speed

    public string DeathSound = "Explosion"; //death sound
    public string DamageSound; //sound when object takes some damage

    private GameObject gameObject; //gameobject where this Stats attached
    private Slider healthSlider; //object health UI
    private Text healthText; //object text health UI
    private AudioManager audioManager; //audio manager

    public Transform deathPrefab;

    public virtual void Initialize(GameObject parentGameObject)
    {
        gameObject = parentGameObject;
        healthSlider = gameObject.GetComponentInChildren(typeof(Slider)) as Slider;
        healthText = gameObject.GetComponentInChildren(typeof(Text)) as Text;

        audioManager = AudioManager.instance;

        _currentHealth = MaxHealth;

        if (healthSlider == null)
        {
            Debug.LogError("Can't find slider!");
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = CurrentHealth;
        }

        if (healthText != null)
        {
            healthText.text = CurrentHealth + "/" + MaxHealth;
        }
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

        GameObject.Destroy(gameObject); 
    }

    private void ShowDeathAnimation()
    {
        if (deathPrefab != null)
        {
            var deathEffect = GameObject.Instantiate(deathPrefab, gameObject.transform.position, gameObject.transform.rotation).gameObject;
            GameObject.Destroy(deathEffect, 2.5f);
        }

    }

    protected virtual void DisplayUIChanges(float damageFromSource)
    {
        if (healthText != null)
            healthText.text = CurrentHealth + "/" + MaxHealth;

        if (healthSlider != null)
            healthSlider.value += damageFromSource;
    }

    private void PlayDamageSound()
    {
        if (DamageSound != string.Empty)
        {
            audioManager.PlaySound(DamageSound);
        }
    }

    private void PlayDeathSound()
    {
        if (DeathSound != string.Empty)
        {
            audioManager.PlaySound(DeathSound);
        }
    }
}

[System.Serializable]
public class PlayerStats : Stats
{
    public static int Coins = 100;
    public static float AdditionalDamage = 0f;
    public static float DamageResistance = 0f;

    private static Text CoinsText;

    public static string[] Equipment;

    public override void Initialize(GameObject parentGameObject)
    {
        CoinsText = GameObject.Find("CoinsUI").GetComponent<Text>();
        CoinsText.text = "Coins:" + Coins.ToString();


        var equipmentLength = (int)ItemType.ItemTypeCount;
        Equipment = new string[equipmentLength];

        base.Initialize(parentGameObject);
    }

    protected override void KillObject()
    {
        var weaponChange = GameObject.FindObjectOfType(typeof(WeaponChange)) as WeaponChange;

        if (weaponChange != null)
            weaponChange.ResetWeaponGUI(); //reset player weapon GUI
        
        GameMaster.gm.PlayerIsDead(); //notify game manager that player is dead
        base.KillObject();
    }

    public static bool SpendMoney(int amount, Item item)
    {
        if (amount > Coins)
        {
            var announcerMessage = "Not enough money to buy - " + item.Name;
            GameMaster.gm.StartCoroutine(
                GameMaster.gm.DisplayAnnouncerMessage(announcerMessage, 2f));

            return false;
        }
        else
        {
            Coins -= amount;
            CoinsText.text = "Coins:" + Coins.ToString();
            var announcerMessage = item.itemType.ToString() + " increased by " + item.BuffAmount;
            GameMaster.gm.StartCoroutine(
                GameMaster.gm.DisplayAnnouncerMessage(announcerMessage, 2f));

            if (item.itemType == ItemType.Damage)
                AdditionalDamage += item.BuffAmount;
            else
                DamageResistance += item.BuffAmount;

            int equipmentIndex = (int)item.itemType;
            Equipment[equipmentIndex] = item.Name;

            Debug.LogError(Equipment[equipmentIndex]);

            return true;
        }
    }

    public override void Damage(float damageFromSource)
    {
        damageFromSource -= DamageResistance;

        if (damageFromSource < 0)
            damageFromSource = 1;

        base.Damage(damageFromSource);
    }
}

[System.Serializable]
public class EnemyStats : Stats
{
    public float damage = 20f;
    public bool isAttacking = false;

    public int EnemyDeathCost = 10;

    private Text CoinsText;

    public override void Initialize(GameObject parentGameObject)
    {
        CoinsText = GameObject.Find("CoinsUI").GetComponent<Text>();
        base.Initialize(parentGameObject);
    }

    protected override void KillObject()
    {
        PlayerStats.Coins += EnemyDeathCost;
        CoinsText.text = "Coins:" + PlayerStats.Coins.ToString();
        base.KillObject();
    }
}


[System.Serializable]
public class RangeEnemyStats : EnemyStats
{
    public float AttackRange = 50f;
    public float AttackRate = 1f;
    //[HideInInspector]
    public bool shotPreparing = false;

    public Transform firePoint;
}
