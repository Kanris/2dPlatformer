using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrader : MonoBehaviour {

    public GameObject interactionButton;
    public GameObject npcInventory;

    private bool isPlayerNearTrader = false;

    private void Update()
    {
        if (isPlayerNearTrader)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                ShowTraderMenu(interactionButton.activeSelf);
                interactionButton.SetActive(!interactionButton.activeSelf);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionButton.SetActive(true);
            isPlayerNearTrader = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var isActive = false;
            isPlayerNearTrader = false;
            interactionButton.SetActive(isActive);
            ShowTraderMenu(isActive);
        }
    }

    private void ShowTraderMenu(bool isActive)
    {
        npcInventory.SetActive(isActive);
    }
}
