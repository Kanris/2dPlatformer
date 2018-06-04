using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStats {

    public Stats stats;

    private GameObject gameObject;
    private Canvas StatsUI;
    private Slider healthSlider;
    private Text healthText;

    public ObjectStats(GameObject thisGameObject, Stats stats)
    {
        stats.CurrentHealth = stats.MaxHealth;
        this.stats = stats;

        this.gameObject = thisGameObject;
        this.StatsUI = this.gameObject.GetComponentInChildren(typeof(Canvas)) as Canvas;
        this.healthSlider = this.gameObject.GetComponentInChildren(typeof(Slider)) as Slider;
        this.healthText = this.gameObject.GetComponentInChildren(typeof(Text)) as Text;

        if (healthSlider == null)
        {
            Debug.LogError("Can't find slider!");
        }

        if (this.healthSlider != null)
        {
            this.healthSlider.maxValue = this.stats.CurrentHealth;
        }

        if (healthText != null)
        {
            healthText.text = stats.CurrentHealth + "/" + stats.MaxHealth;
        }

        if (StatsUI != null)
        {
            StatsUI.enabled = true;
        }

        Debug.Log(thisGameObject.name + "-" + stats.MaxHealth + "/" + stats.CurrentHealth);
    }

    public void Damage(float damageFromSource)
    {
        stats.CurrentHealth -= damageFromSource;
        DisplayUIChanges(damageFromSource);

        if (stats.CurrentHealth <= 0)
        {
            GameMaster.KillObject(gameObject);
        }
    }

    private void DisplayUIChanges(float damageFromSource)
    {
        StatsUI.enabled = true;

        if (healthText != null)
            healthText.text = stats.CurrentHealth + "/" + stats.MaxHealth;

        if (healthSlider != null)
            healthSlider.value += damageFromSource;
    }
}
