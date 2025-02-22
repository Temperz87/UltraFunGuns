﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraFunGuns
{
    //Base class for UFG weapons.
    public abstract class UltraFunGunBase : MonoBehaviour
    {
        public Dictionary<string, ActionCooldown> actionCooldowns;
        public Dictionary<string, AudioSource> soundEffects;

        public Transform mainCam, firePoint;
        public OptionsManager om;
        public NewMovement player;
        public WeaponIcon weaponIcon;
        public Animator animator;

        public string registryName;

        public abstract void DoAnimations();

        private void Awake()
        {
            registryName = gameObject.name;
            if (registryName.Contains("(Clone)"))
            {
                registryName = registryName.Replace("(Clone)", "");
            }
            actionCooldowns = SetActionCooldowns();
            mainCam = MonoSingleton<CameraController>.Instance.transform;
            om = MonoSingleton<OptionsManager>.Instance;
            player = MonoSingleton<NewMovement>.Instance;
            animator = GetComponent<Animator>();
            weaponIcon = GetComponent<WeaponIcon>();
            foreach (Transform transf in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (transf.name == "firePoint")
                {
                    firePoint = transf;
                    break;
                }
            }

            HydraLoader.dataRegistry.TryGetValue(String.Format("{0}_weaponIcon", registryName), out UnityEngine.Object weapon_weaponIcon);
            weaponIcon.weaponIcon = (Sprite) weapon_weaponIcon;

            HydraLoader.dataRegistry.TryGetValue(String.Format("{0}_glowIcon", registryName), out UnityEngine.Object weapon_glowIcon);
            weaponIcon.glowIcon = (Sprite) weapon_glowIcon;

            weaponIcon.variationColor = 0; //TODO find a way to fix this UPDATE: Its aight for now.

            if (weaponIcon.weaponIcon == null)
            {
                HydraLoader.dataRegistry.TryGetValue("debug_weaponIcon", out UnityEngine.Object debug_weaponIcon);
                weaponIcon.weaponIcon = (Sprite)debug_weaponIcon;   
            }

            if(weaponIcon.glowIcon == null)
            {
                HydraLoader.dataRegistry.TryGetValue("debug_glowIcon", out UnityEngine.Object debug_glowIcon);
                weaponIcon.glowIcon = (Sprite)debug_glowIcon;
            }

            OnAwakeFinished();
        }
        
        public virtual void OnAwakeFinished() {}

        private void Update()
        {
            GetInput();
            DoAnimations();
        }

        //Example input function call this in update.
        public virtual void GetInput()
        {
            if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && actionCooldowns["primaryFire"].CanFire() && !om.paused)
            {
                actionCooldowns["primaryFire"].AddCooldown();
                FirePrimary();
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && actionCooldowns["secondaryFire"].CanFire() && !om.paused)
            {
                actionCooldowns["secondaryFire"].AddCooldown();
                FireSecondary();
            }
        }

        //Implement the cooldowns here.
        public virtual Dictionary<string, ActionCooldown> SetActionCooldowns()
        {
            Dictionary<string, ActionCooldown> cooldowns = new Dictionary<string, ActionCooldown>();
            cooldowns.Add("primaryFire", new ActionCooldown(1.0f));
            cooldowns.Add("secondaryFire", new ActionCooldown(1.0f));
            return cooldowns;
        }

        public virtual void FirePrimary()
        {
            Debug.Log("Fired Primary! (not implemented)");
        }

        public virtual void FireSecondary()
        {
            Debug.Log("Fired Secondary! (not implemented)");
        }

        public class ActionCooldown
        {
            public float timeToFire;
            public float fireDelay;
            public bool noCooldown;

            public ActionCooldown(float delay = 1f)
            {
                timeToFire = 0.0f;
                this.noCooldown = (delay <= 0.0f);
                this.fireDelay = delay;
            }            

            public void AddCooldown()
            {
                timeToFire = fireDelay + Time.time;
            }

            public void AddCooldown(float delayInSeconds)
            {
                timeToFire = delayInSeconds + Time.time;
            }

            public bool CanFire()
            {
                if(timeToFire < Time.time || noCooldown)
                {
                    return true;
                }
                return false;
            }
        }

    }
}
