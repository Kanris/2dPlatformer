using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem : MonoBehaviour
{

    public int Price = 50;
    public Item item;

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