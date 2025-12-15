using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemAcquirer : MonoBehaviour
{
    public AudioClip itemPickupSound;
    [Range(0f, 1f)]
    public float pickupSoundVolume = 0.7f;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            if (fieldItems == null) return;

            Item item = fieldItems.GetItem();

            if (Inventory.instance == null)
            {
                Debug.LogError("Inventory.instance∞° null¿‘¥œ¥Ÿ! æ∆¿Ã≈€ Ω¿µÊ Ω«∆–.");
                return;
            }

            PlayPickupSound();

            if (item.itemType == ItemType.Present)
            {
                PlayerHealth playerHealth = GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(1);
                    Debug.Log($"Present »πµÊ! HP +1");
                }
                fieldItems.DestroyItem();
            }
            else
            {
                if (Inventory.instance.AddItem(item))
                {
                    fieldItems.DestroyItem();
                }
            }
        }
    }

    void PlayPickupSound()
    {
        if (itemPickupSound != null)
        {
            AudioSource.PlayClipAtPoint(itemPickupSound, transform.position, pickupSoundVolume);
        }
    }
}