using System;
using Ky;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneralVfxSpawner : MonoBehaviour
{
    public GameObject vfxPrefab;
    public SpawnObjectSettings settings;
    [FoldoutGroup("When?")] public bool spawnOnStart;
    [FoldoutGroup("When?")] public bool spawnOnEnable;
    [FoldoutGroup("When?")] public bool spawnOnDisable;
    [FoldoutGroup("When?")] public bool spawnOnDestroy;
    [FoldoutGroup("When?")] public bool spawnOverTime;
    [ShowIf("spawnOverTime")] public float interval;
    public Vector3 globalOffset;
    public bool spawnLocally;
    public float destroyAfter = 10f;

    private float timer;

    private void Start()
    {
        Spawn(spawnOnStart);
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
        Spawn(spawnOnDestroy);
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
    }

    public void Spawn(bool active)
    {
        if (!active) return;
        var parent = spawnLocally ? transform : GeneralVfxParent.I;
        var vfx = Instantiate(vfxPrefab, transform.position, Quaternion.identity, parent);
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