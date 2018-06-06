using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stats {

    public float MaxHealth = 100f;
    private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, MaxHealth); }
    }

    public float speed = 300f;

    public string DeathSound = "Explosion";
    public string DamageSound;

    public GameObject gameObject;

    private Canvas StatsUI;
    private Slider healthSlider;
    private Text healthText;
    private AudioManager audioManager;

    public void Initialize(GameObject parentGameObject)
    {

        gameObject = parentGameObject;
        StatsUI = gameObject.GetComponentInChildren(typeof(Canvas)) as Canvas;
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

    public void Damage(float damageFromSource)
    {
        CurrentHealth -= damageFromSource;
        DisplayUIChanges(damageFromSource);

        if (CurrentHealth <= 0)
        {
            GameMaster.KillObject(gameObject);
        }

        if (DamageSound != string.Empty)
        {
            audioManager.PlaySound(DamageSound);
        }
    }

    private void DisplayUIChanges(float damageFromSource)
    {
        if (healthText != null)
            healthText.text = CurrentHealth + "/" + MaxHealth;

        if (healthSlider != null)
            healthSlider.value += damageFromSource;
    }
}

[System.Serializable]
public class EnemyStats : Stats
{
    public float damage = 20f;
    public bool isAttacking = false;
}


[System.Serializable]
public class RangeEnemyStats : EnemyStats
{
    public float AttackRange = 50f;
    public float AttackRate = 1f;
}
