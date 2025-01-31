using System;
using Ky;
using UnityEngine;
using System.Collections.Generic;

public class Player : Singleton<Player>, TaggedObject
{
    public PlayerMovement movement;
    public List<PlayerTag> tagList = new List<PlayerTag>();
    public Transform tagIndicatorsParent;
    public ParticleSystem tagIndicatorPrefab;
    public int health;
    [SerializeField] private GeneralVfxSpawner bloodTrails;
    [SerializeField] private SerializableDictionary<string, ParticleSystem.MinMaxGradient> playerTagColors;
    public Vector3 Pos => transform.position;

    void Update()
    {
        List<string> removeList = new List<string>();
        foreach (PlayerTag pt in tagList)
        {
            if (pt.ProcessResetCondition(Tag.ResetsOn.OnTimePassed, Time.deltaTime))
            {
                removeList.Add(pt.tagName);
            }

            if (pt.ProcessResetCondition(Tag.ResetsOn.OnSpeedReached | Tag.ResetsOn.OnSpeedNotReached,
                    transform.GetComponent<Rigidbody2D>().velocity.magnitude))
            {
                removeList.Add(pt.tagName);
            }
        }

        foreach (string tagName in removeList)
        {
            print("Removed " + tagName);
            PlayerTag.ResetValue(tagList, tagName);
            PlayerTagsChanged();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        List<string> removeList = new List<string>();
        foreach (PlayerTag pt in tagList)
        {
            SpecialCollisionForTag special = other.transform.GetComponent<SpecialCollisionForTag>();
            if (special != null && special.IgnoreForCollision())
            {
                return;
            }

            if (pt.ProcessResetCondition(Tag.ResetsOn.OnPlayerCollide, 1f))
            {
                removeList.Add(pt.tagName);
            }
        }

        foreach (string tagName in removeList)
        {
            PlayerTag.ResetValue(tagList, tagName);
            PlayerTagsChanged();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Debug.Log($"Took damage, other: {other.name} health: {health - 1}");
            TakeDamage();
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage()
    {
        health -= 1;
        bloodTrails.gameObject.SetActive(true);
        bloodTrails.distanceInterval /= 2f;
        bloodTrails.interval /= 2f;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        movement.ForceReset();
    }

    public List<PlayerTag> GetTagList()
    {
        return tagList;
    }

    public void PlayerTagsChanged()
    {
        foreach (Transform tr in tagIndicatorsParent)
        {
            tr.GetComponent<ParticleSystem>().Stop();
            Destroy(tr.gameObject, 5f);
        }

        foreach (var playerTag in tagList)
        {
            if (!playerTagColors.TryGetValue(playerTag.tagName, out var gradient)) continue;
            var partSys = Instantiate(tagIndicatorPrefab, Pos, Quaternion.identity, tagIndicatorsParent);
            ParticleSystem.MainModule main = partSys.main;
            var c = gradient.color;
            c.a = 90 / 255f;
            main.startColor = c;
            var emis = partSys.emission;
            var val = Mathf.Max(playerTag.value, 1);
            emis.rateOverTimeMultiplier *= val;
            emis.rateOverDistanceMultiplier *= val;
        }

        var log = $"Tags count:  {tagList.Count};";
        foreach (var playerTag in tagList)
        {
            log += $"{playerTag.tagName}, {playerTag.value}";
        }

        Debug.Log(log);
    }
}