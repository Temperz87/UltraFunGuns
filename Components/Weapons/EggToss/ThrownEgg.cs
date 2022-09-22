﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraFunGuns
{
    public class ThrownEgg : MonoBehaviour
    {
        public GameObject impactFX;
        public GameObject eggsplosionPrefab;

        private Rigidbody rb;
        private CapsuleCollider eggCollider;
        private Vector3 oldVelocity;
        private float invicibleTimer = 0.015f;
        private bool canImpact = false;
        private bool impacted = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            eggCollider = GetComponent<CapsuleCollider>();
        }

        private void Start()
        {
            float randomAngle = UnityEngine.Random.Range(0, 360);
            transform.rotation = Quaternion.AngleAxis(randomAngle, transform.forward);
        }

        private void FixedUpdate()
        {
            invicibleTimer -= Time.fixedDeltaTime;
            if(invicibleTimer < 0.0f)
            {
                canImpact = true;
            }
        }

        private void LateUpdate()
        {
            oldVelocity = rb.velocity;
        }

        //TODO call when egg is shot eggsplosion hehe
        public void Explode()
        {
            Debug.Log("EggSplosion not yet implemented.");
            Destroy(this);
        }

        //TODO Call when player grapples the egg should heal player for 10 hp or something cringe idk
        private void Cracked()
        {
            
        }

        //TODO add code for shooting it and habving it explode and also grapple thing do the overload too.
        private void Collide(Collision col)
        {
            if(!impacted)
            {
                impacted = true;

                EnemyIdentifier enemy = null;
                GameObject impact = GameObject.Instantiate<GameObject>(impactFX, col.GetContact(0).point, Quaternion.identity);
                impact.transform.up = col.GetContact(0).normal;
                impact.transform.parent = col.transform;
                float damage = rb.velocity.magnitude * 0.035f; //Scales damage from speed of egg

                if (col.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out EnemyIdentifierIdentifier enemyPart))
                {
                    enemy = enemyPart.eid;
                }

                if (enemy != null)
                {
                    if (!enemy.dead)
                    {
                        enemy.DeliverDamage(enemy.gameObject, oldVelocity, col.GetContact(0).point, damage, false);
                        MonoSingleton<StyleHUD>.Instance.AddPoints(150, "hydraxous.ultrafunguns.egged");
                    }
                    else
                    {
                        UnityEngine.Physics.IgnoreCollision(eggCollider, col.collider, true);
                        impacted = false;
                    }
                }

            }
            if (impacted)
            {
                Destroy(gameObject);
            }

        }

        private void Collide(Collider col)
        {
            if (!impacted)
            {
                impacted = true;
                
                EnemyIdentifier enemy = null;
                GameObject impact = GameObject.Instantiate<GameObject>(impactFX, transform.position, Quaternion.identity);
                impact.transform.Find("impactSpotQuad").gameObject.SetActive(false);
                Vector3 collisionNormalGuess = MonoSingleton<NewMovement>.Instance.transform.position - transform.position;
                impact.transform.up = collisionNormalGuess;
                impact.transform.parent = col.transform;
                float damage = rb.velocity.magnitude * 0.018f; //Scales damage from speed of egg :)

                if (col.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out EnemyIdentifierIdentifier enemyPart))
                {
                    enemy = enemyPart.eid;
                }

                if (enemy != null)
                {
                    if (!enemy.dead)
                    {
                        enemy.DeliverDamage(enemy.gameObject, oldVelocity, transform.position, damage, false);
                        MonoSingleton<StyleHUD>.Instance.AddPoints(150, "hydraxous.ultrafunguns.egged");
                    }
                    else
                    {
                        UnityEngine.Physics.IgnoreCollision(eggCollider, col, true);
                        impacted = false;
                    }
                }

                
            }
            if (impacted)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision col)
        {
            if (canImpact)
            {
                Collide(col);
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (canImpact && col.gameObject.layer != 20 && (col.gameObject.layer == 10 || col.gameObject.layer == 11) && !col.isTrigger)
            {
                Collide(col);
            }
        }
    }
}