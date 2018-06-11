using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChange : MonoBehaviour {
    
    public int currentWeaponEquiped = 0;

    public Transform[] weaponsToEquip;
    private int totalWeaponAmount = 0;

    private GridLayoutGroup weaponsPanel;

    public bool AllowToChageWeapon;

    private PauseMenu pauseMenu;

    #region keycodes
    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };
    #endregion

    private void Start()
    {
        totalWeaponAmount = 2; //TODO : CHANGE TO ARRAY LENGTH

        if (weaponsPanel == null)
        {
            weaponsPanel = GameObject.FindGameObjectWithTag("WeaponsPanel").GetComponent<GridLayoutGroup>();
        }

        pauseMenu = PauseMenu.pm;
    }

    // Update is called once per frame
    void Update () {
        if (AllowToChageWeapon)
        {
            if (!pauseMenu.IsGamePause)
            {
                for (int keyValue = 0; keyValue < totalWeaponAmount & keyValue < keyCodes.Length; keyValue++)
                {
                    if (Input.GetKeyDown(keyCodes[keyValue]))
                    {
                        EquipWeapon(keyValue);
                        break;
                    }
                }    
            }
        }
	}

    public void EquipWeapon(int weaponToEquip)
    {
        if (weaponToEquip != currentWeaponEquiped)
        {
            ChangeAlpha(false);

            currentWeaponEquiped = weaponToEquip;

            ChangeAlpha(true);

            Destroy(gameObject.transform.GetChild(0).gameObject); //destroy equiped weapon 

            Instantiate(weaponsToEquip[currentWeaponEquiped], gameObject.transform);
        }
    }

    void ChangeAlpha(bool isEquiped)
    {
        if (weaponsPanel != null)
        {
            var equipedWeapon = weaponsPanel.gameObject.transform.GetChild(currentWeaponEquiped).GetComponent<Image>();

            var newColor = equipedWeapon.color;

            newColor.a = isEquiped ? 1f : 0.4f;

            equipedWeapon.color = newColor;
        }
    }

    public void ResetWeaponGUI()
    {
        EquipWeapon(0);
    }
}
