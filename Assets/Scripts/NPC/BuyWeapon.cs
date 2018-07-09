using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWeapon : MonoBehaviour {

    public PlayerWeapon item;

    private void Start()
    {
        InitializeItemGUI();
        CheckIsItemAlreadyPurchased();
    }

    private void InitializeItemGUI()
    {
        var priceText = transform.GetComponentInChildren<Text>();

        if (priceText != null)
            priceText.text = item.Price.ToString();
        else
            Debug.LogError("BuyItem: Can't find text in child");
    }

    private void CheckIsItemAlreadyPurchased()
    {
        if (isAlreadyPurchased())
        {
            Destroy(gameObject);
        }
    }

    public void Buy()
    {
        var playerBoughtItem = PlayerStats.SpendMoney(item.Price, item);

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
        return PlayerStats.Equipment[equipmentIndex] == item.Name;
    }
}

[System.Serializable]
public class PlayerWeapon : Item
{
    public WeaponType itemType;
}

public enum WeaponType { Handgun, Automatic, ItemTypeCount };
