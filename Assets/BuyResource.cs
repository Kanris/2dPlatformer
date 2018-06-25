using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyResource : MonoBehaviour
{
    [SerializeField]
    private Item item;

    // Use this for initialization
    void Start()
    {
        InitializeItemGUI();
    }

    private void InitializeItemGUI()
    {
        var priceText = transform.GetComponentInChildren<Text>();

        if (priceText != null)
            priceText.text = item.Price.ToString();
        else
            Debug.LogError("BuyItem: Can't find text in child");
    }

    public void Buy()
    {
        PlayerStats.SpendMoney(item.Price, item);
    }

    private IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
