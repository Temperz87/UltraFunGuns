﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace UltraFunGuns
{
    public class UltraFunGunsPatch : MonoBehaviour
    {
        GunControl gc;
        List<List<string>> weaponKeySlots = new List<List<string>>() {
            new List<string> {"SonicReverberator"},
            new List<string> { },
            new List<string> { },
            new List<string> { }
        };

        List<List<GameObject>> customSlots = new List<List<GameObject>>()
        {
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>()
        };


        private void Start()
        {
            gc = GetComponent<GunControl>();
            NewStyleItem("vaporized", "<color=cyan>VAPORIZED</color>");
            NewStyleItem("vibecheck","VIBE-CHECKED");
            NewStyleItem("v2kill", "<color=#ff33001>OXIDIZED</color>");
            NewStyleItem("gabrielkill", "<color=#ff0051>L-DISTRIBUTED</color>");
            NewStyleItem("wickedkill", "<color=#919191>NOT WICKED ENOUGH</color>");
            NewStyleItem("minoskill", "<color=#03ffa7>JUDGED</color>");


            FetchWeapons();
        }

        //Gets weapon prefabs from the Data loader and instantiates them into the world and adds them to the gun controllers lists.
        private void FetchWeapons()
        {
            try
            {
                for (int i = 0; i < weaponKeySlots.Count;i++)
                {
                    if (weaponKeySlots[i].Count > 0)
                    {
                        foreach (string weaponKey in weaponKeySlots[i])
                        {
                            HydraLoader.prefabRegistry.TryGetValue(weaponKey, out GameObject weaponPrefab);
                            weaponPrefab.layer = 13;
                            Transform[] childs = weaponPrefab.GetComponentsInChildren<Transform>();
                            foreach (Transform child in childs)
                            {
                                child.gameObject.layer = 13;
                            }
                            customSlots[i].Add(GameObject.Instantiate<GameObject>(weaponPrefab, this.transform));
                        }
                    }
                }
                AddWeapons();
            }
            catch(System.Exception e)
            {
                Debug.Log("GunControl patcher componenet couldn't fetch weapons.");
                Debug.Log(e.Message);
            }
        }

        private void AddWeapons()
        {
            for (int i = 0; i < customSlots.Count; i++)
            {
                if (customSlots[i].Count > 0)
                {
                    gc.slots.Add(customSlots[i]);
                    foreach (GameObject wep in customSlots[i])
                    {
                        if (!gc.allWeapons.Contains(wep))
                        {
                            gc.allWeapons.Add(wep);
                        }
                    }
                }
            }
            

        }

        private void Update()
        {
            if (MonoSingleton<InputManager>.Instance.InputSource.Slot7.WasPerformedThisFrame && (customSlots[0].Count > 1 || gc.currentSlot != 7))
            {
                if (customSlots[0].Count > 0 && customSlots[0][0] != null)
                {
                    gc.SwitchWeapon(7, customSlots[0], false, false);
                }
            }else if (MonoSingleton<InputManager>.Instance.InputSource.Slot8.WasPerformedThisFrame && (customSlots[1].Count > 1 || gc.currentSlot != 8))
            {
                if (customSlots[1].Count > 1 && customSlots[1][0] != null)
                {
                    gc.SwitchWeapon(8, customSlots[1], false, false);
                }
            }
            else if(MonoSingleton<InputManager>.Instance.InputSource.Slot9.WasPerformedThisFrame && (customSlots[2].Count > 1 || gc.currentSlot != 9))
            {
                if (customSlots[2].Count > 0 && customSlots[2][0] != null)
                {
                    gc.SwitchWeapon(9, customSlots[2], false, false);
                }
            }
            else if(MonoSingleton<InputManager>.Instance.InputSource.Slot0.WasPerformedThisFrame && (customSlots[3].Count > 1 || gc.currentSlot != 10))
            {
                if (customSlots[3].Count > 0 && customSlots[3][0] != null)
                {
                    gc.SwitchWeapon(10, customSlots[3], false, false);
                }
            }
        }

        private void NewStyleItem(string name, string text)
        {
            if (MonoSingleton<StyleHUD>.Instance.GetLocalizedName("hydraxous.ultrafunguns."+ name) == "hydraxous.ultrafunguns." + name)
            {
                MonoSingleton<StyleHUD>.Instance.RegisterStyleItem("hydraxous.ultrafunguns." + name, text);
            }
        }
    }
}