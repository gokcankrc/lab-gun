using System;
using Ky.SoundSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ky.Juice
{
    [Serializable]
    public class Juice
    {
        [NonSerialized] public Hash128 hash = HashMaker.GenerateHash();
        [NonSerialized] public bool isRunning = false;

        public Transform targetTransform;
        public float duration = 0.2f;

        public bool affectsTransform = false;

        [ShowIfGroup("AffectsTransform")] [BoxGroup("AffectsTransform/Transform Curve Settings")] public JuiceCurves juiceCurves;

        public bool spawnFx = false;

        [ShowIfGroup("SpawnFx")] [BoxGroup("SpawnFx/Spawn Fx Settings")] public ParentType fxParentType;
        [ShowIfGroup("SpawnFx")] [BoxGroup("SpawnFx/Spawn Fx Settings")] public GameObject startFx;
        [ShowIfGroup("SpawnFx")] [BoxGroup("SpawnFx/Spawn Fx Settings")] public GameObject endFx;

        public bool spawnSound = false;

        [ShowIfGroup("SpawnSound")] [BoxGroup("SpawnSound/Spawn Sound Settings")] public ParentType soundParentType;
        [ShowIfGroup("SpawnSound")] [BoxGroup("SpawnSound/Spawn Sound Settings")] public Sound startSound;
        [ShowIfGroup("SpawnSound")] [BoxGroup("SpawnSound/Spawn Sound Settings")] public Sound endSound;

        public void Run()
        {
            JuiceManager.I.Run(this);
        }

        public void Stop()
        {
            JuiceManager.I.Stop(this);
        }

        public Juice Copy()
        {
            Juice copy = new()
            {
                hash = HashMaker.GenerateHash(),
                targetTransform = targetTransform,
                juiceCurves = juiceCurves,
                duration = duration,

                spawnFx = spawnFx,
                fxParentType = fxParentType,
                startFx = startFx,
                endFx = endFx,

                spawnSound = spawnSound,
                soundParentType = soundParentType,
                startSound = startSound,
                endSound = endSound,
            };
            return copy;
        }

        public enum ParentType
        {
            This,
            WorldSpace
        }

        public bool Validate()
        {
            var isValid = true;
            if (spawnFx || spawnSound || affectsTransform)
            {
                isValid &= targetTransform;
            }

            if (affectsTransform)
            {
                isValid &= juiceCurves;
            }

            return isValid;
        }
    }
}