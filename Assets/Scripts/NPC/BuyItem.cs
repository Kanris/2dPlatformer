using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{

    public int Price = 50;
    public Item item;

    public void Start()
    {
        var priceText = transform.GetComponentInChildren<Text>();

        if (priceText != null)
            priceText.text = Price.ToString();
        else
            Debug.LogError("BuyItem: Can't find text in child");
    }

    public void Buy()
    {
        var playerBoughtItem = PlayerStats.SpendMoney(Price, item);
        //StartCoroutine(DestroyItem());

        if (playerBoughtItem)
            Destroy(gameObject);
    }

    private IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}

[System.Serializable]
public class Item
{
    public string Name;
    public float BuffAmount = 1f;

    public ItemType itemType;
}

public enum ItemType { Damage, Resistance };