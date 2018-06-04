using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stats {

    public float MaxHealth = 100f;
    public float CurrentHealth = 100f;
    public float speed = 300f;

    private GameObject gameObject;
    private Canvas StatsUI;
    private Slider healthSlider;
    private Text healthText;

    private SomeMono mono;

    public Stats(GameObject thisGameObject)
    {
        this.gameObject = thisGameObject;
        this.StatsUI = this.gameObject.GetComponentInChildren(typeof(Canvas)) as Canvas;
        this.healthSlider = this.gameObject.GetComponentInChildren(typeof(Slider)) as Slider;
        this.healthText = this.gameObject.GetComponentInChildren(typeof(Text)) as Text;

        if (this.healthSlider != null)
        {
            this.healthSlider.maxValue = CurrentHealth;
        }

        if (StatsUI != null)
        {
            StatsUI.enabled = false;
        }

        mono = new SomeMono();
    }

    public void Damage(float damageFromSource)
    {
        CurrentHealth -= damageFromSource;
        DisplayUIChanges(damageFromSource);

        if (CurrentHealth <= 0)
        {
            GameMaster.KillObject(gameObject);
        }
    }

    private void DisplayUIChanges(float damageFromSource)
    {
        StatsUI.enabled = true;

        if (healthText != null)
            healthText.text = CurrentHealth + "/" + MaxHealth;

        if (healthSlider != null)
            healthSlider.value += damageFromSource;
    }
}

public class SomeMono : MonoBehaviour
{
    public IEnumerator DoCoroutine(IEnumerator cor)
    {
        while (cor.MoveNext())
            yield return cor.Current;
    }
}
