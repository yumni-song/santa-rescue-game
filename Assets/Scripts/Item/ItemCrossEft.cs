using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Cross")]
public class ItemCrossEft : ItemEffect
{
    public float shieldDuration = 5f;
    public float shieldRadius = 7f;
    public GameObject shieldEffectPrefab;
    public float effectOffsetDistance = 2f;
    public float effectSizeMultiplier = 1.5f;
    public AudioClip shieldActivateSound;
    public AudioClip shieldLoopSound;
    [Range(0f, 1f)]
    public float shieldSoundVolume = 0.8f;

    public override bool ExecuteRole()
    {
        Debug.Log("십자가 사용 - 보호막 생성!");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다!");
            return false;
        }

        PlayerShield shield = player.GetComponent<PlayerShield>();
        if (shield == null)
        {
            shield = player.AddComponent<PlayerShield>();
        }

        shield.ActivateShield(shieldDuration, shieldRadius, shieldEffectPrefab,
                             effectOffsetDistance, effectSizeMultiplier,
                             shieldActivateSound, shieldLoopSound, shieldSoundVolume);

        return true;
    }
}