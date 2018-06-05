using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChange : MonoBehaviour {
    
    public int currentWeaponEquiped = 0;

    public Transform[] weaponsToEquip;
    private int totalWeaponAmount = 0;

    public GridLayoutGroup weaponsPanel;
    public Transform weaponArm;

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
            weaponsPanel = FindObjectOfType(typeof(GridLayoutGroup)) as GridLayoutGroup;
        }
    }

    // Update is called once per frame
    void Update () {

        for (int keyValue = 0; keyValue < totalWeaponAmount & keyValue < keyCodes.Length; keyValue++)
        {
            if (Input.GetKeyDown(keyCodes[keyValue]))
            {
                EquipWeapon(keyValue);
                break;
            }
        }
	}

    void EquipWeapon(int weaponToEquip)
    {
        if (weaponToEquip != currentWeaponEquiped)
        {
            ChangeAlpha(false);

            currentWeaponEquiped = weaponToEquip;

            ChangeAlpha(true);

            Destroy(weaponArm.transform.GetChild(0).gameObject); //destroy equiped weapon 

            Instantiate(weaponsToEquip[currentWeaponEquiped], weaponArm.transform);
        }
    }

    void ChangeAlpha(bool isEquiped)
    {
        var equipedWeapon = weaponsPanel.gameObject.transform.GetChild(currentWeaponEquiped).GetComponent<Image>();

        var newColor = equipedWeapon.color;

        newColor.a = isEquiped ? 1f : 0.4f;

        equipedWeapon.color = newColor;
    }

    public void ResetWeaponGUI()
    {
        EquipWeapon(0);
    }
}
