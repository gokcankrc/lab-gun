using System;
using System.Collections;
using System.Collections.Generic;
using Ky;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneralVfxSpawner : MonoBehaviour
{
    [ShowIf("@vfxesRandomlySelectedFrom.Count == 0")] public GameObject vfxPrefab;
    public List<GameObject> vfxesRandomlySelectedFrom = new();
    public SpawnObjectSettings settings;
    [FoldoutGroup("When?")] public bool spawnOnStart;
    [FoldoutGroup("When?")] public bool spawnOnEnable;
    [FoldoutGroup("When?")] public bool spawnOnDisable;
    [FoldoutGroup("When?")] public bool spawnOnDestroy;
    [FoldoutGroup("When?")] public bool spawnOverTime;
    [FoldoutGroup("When?")] public bool spawnOverDistance;
    [ShowIf("spawnOverTime")] public float interval;
    [ShowIf("spawnOverDistance")] public float distanceInterval;
    public Vector3 globalOffset;
    public bool spawnLocally;
    public static bool canSpawnVFX;
    public float destroyAfter = 10f;

    private float timer;
    private Vector3 lastPos;

    private void Start()
    {
        Spawn(spawnOnStart);
        lastPos = transform.position;
    }

    private void OnEnable()
    {
        Spawn(spawnOnEnable);
    }

    private void OnDisable()
    {
        Spawn(spawnOnDisable);
    }

    private void OnDestroy()
    {
        // temporary potential error fix -> not really, if anything the spawning stopped working - Leo
        //GeneralVfxParent.I.GetComponent<GeneralVfxParent>()?.StartCoroutine(SpawnOneFrameDelay(spawnOnDestroy));
        if (canSpawnVFX) // canSpawnVFX is being handeled by playermovement, which knows when the game is about to be reset
        {
            Spawn(spawnOnDestroy);
        }
    }

    private void Update()
    {
        if (spawnOverTime)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer += interval;
                Spawn(true);
            }
        }

        if (spawnOverDistance)
        {
            var pos = transform.position;
            if ((lastPos - pos).sqrMagnitude > distanceInterval * distanceInterval)
            {
                lastPos = pos;
                Spawn(true);
            }
        }
    }

    private IEnumerator SpawnOneFrameDelay(bool active)
    {
        if (!active) yield break;
        yield return null;
        Spawn(true);
    }

    public void Spawn(bool active)
    {
        if (!active) return;
        var parent = spawnLocally ? transform : GeneralVfxParent.I;
        var selectedVfx = vfxesRandomlySelectedFrom.Count > 0
            ? vfxesRandomlySelectedFrom[Random.Range(0, vfxesRandomlySelectedFrom.Count)]
            : vfxPrefab;
        var vfx = Instantiate(selectedVfx, transform.position, Quaternion.identity, parent);
        rotate();
        vfx.transform.position += globalOffset;
        if (destroyAfter > 0)
            Destroy(vfx, destroyAfter);
        return;

        void rotate()
        {
            switch (settings.rotation)
            {
                case SpawnObjectSettings.Rotation.RandomRotation:
                    vfx.transform.Rotate(0, 0, Random.Range(0, 360f), Space.World);
                    break;
                case SpawnObjectSettings.Rotation.Backwards:
                case SpawnObjectSettings.Rotation.Forwards:
                    var v = getRotationVector();
                    if (settings.rotation == SpawnObjectSettings.Rotation.Backwards)
                    {
                        v = -v;
                    }

                    vfx.transform.forward = v;
                    break;
                case SpawnObjectSettings.Rotation.NoChange:
                default:
                    break;
            }
        }

        Vector3 getRotationVector()
        {
            return settings.direction switch
            {
                SpawnObjectSettings.Direction.RigidbodyVelocity => GetComponent<Rigidbody2D>().velocity,
                SpawnObjectSettings.Direction.ExternallyGiven => settings.directionVector,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    [Serializable]
    public class SpawnObjectSettings
    {
        public Rotation rotation;
        [ShowIf("@showDirectionVectorFlag")] public Direction direction;
        [ShowIf("@direction == Direction.ExternallyGiven && showDirectionVectorFlag")] public Vector3 directionVector;

        private bool showDirectionVectorFlag => rotation is Rotation.Forwards or Rotation.Backwards;

        public enum Rotation
        {
            RandomRotation,
            Backwards,
            Forwards,
            NoChange
        }

        public enum Direction
        {
            RigidbodyVelocity,
            ExternallyGiven,
        }
    }
}