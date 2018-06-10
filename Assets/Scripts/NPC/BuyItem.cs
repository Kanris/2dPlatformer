using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BuyItem : MonoBehaviour
{

    public int Price = 50;
    public Item item;

    private void Start()
    {
        if (isAlreadyPurchased())
        {
            Debug.LogError("Item is already purchased.");
            Destroy(gameObject);
        }

        var priceText = transform.GetComponentInChildren<Text>();

        if (priceText != null)
            priceText.text = Price.ToString();
        else
            Debug.LogError("BuyItem: Can't find text in child");
    }

    private void Buy()
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

    private bool isAlreadyPurchased()
    {
        var equipmentIndex = (int)item.itemType;
        Debug.LogError("Index - " + equipmentIndex + "(" + PlayerStats.Equipment[equipmentIndex] + ")");
        return PlayerStats.Equipment[equipmentIndex] == item.Name;
    }

}

[System.Serializable]
public class Item
{
    public string Name;
    public float BuffAmount = 1f;

    public ItemType itemType;
}

public enum ItemType { Damage, Resistance, ItemTypeCount };