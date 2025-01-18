using System;
using System.Collections;
using System.Collections.Generic;
using Ky.SoundSystem;
using UnityEngine;

namespace Ky.Juice
{
    public class JuiceManager : Singleton<JuiceManager>
    {
        private Dictionary<Hash128, Coroutine> juiceCoroutines = new Dictionary<Hash128, Coroutine>();

        public void Run(Juice juiceToRun)
        {
            if (juiceToRun.isRunning) return;

            BruteRun(juiceToRun);
        }

        private void BruteRun(Juice juiceToRun)
        {
            if (!juiceToRun.Validate())
                throw new Exception("Juice set up is incorrect or missing has information!");

            Coroutine newCoroutine = StartCoroutine(JuiceCoroutine(juiceToRun));
            juiceCoroutines.Add(juiceToRun.hash, newCoroutine);
            juiceToRun.isRunning = true;
        }

        private IEnumerator JuiceCoroutine(Juice juice)
        {
            // Start
            float t = 0;
            float duration = juice.duration;
            float ratio = 0;
            MakeEffects(juice, juice.startFx);
            MakeSound(juice, juice.startSound);

            // Run
            while (ratio < 1)
            {
                // At start so, just in case (juice.targetTransform == null) at the frame (ratio >= 1)
                // null transform will be catched.
                yield return null;

                t += Time.deltaTime;
                ratio = t / duration;

                if (juice.affectsTransform)
                {
                    if (!juice.targetTransform)
                    {
                        // Debug.LogWarning("Juice's target transform returned null. Aborting juice.");
                        BruteStop(juice);
                        yield break;
                    }

                    EvaluateValues(juice, ratio);
                }
            }

            // End
            MakeEffects(juice, juice.endFx);
            MakeSound(juice, juice.endSound);
            EvaluateValues(juice, 1);
            BruteStop(juice);
        }

        public void Stop(Juice juice)
        {
            // if (!juiceCoroutines.ContainsKey(juice.Hash)) return;
            if (!juice.isRunning) return;

            EvaluateValues(juice, 1);
            BruteStop(juice);
        }

        private void BruteStop(Juice juice)
        {
            StopCoroutine(juiceCoroutines[juice.hash]);
            juiceCoroutines.Remove(juice.hash);
            juice.isRunning = false;
        }

        private void EvaluateValues(Juice juice, float ratio)
        {
            if (!juice.affectsTransform) return;

            Transform target = juice.targetTransform;
            JuiceCurves juiceCurves = juice.juiceCurves;
            float x, y, z;

            if (juiceCurves.effectsPosition)
            {
                x = juiceCurves.xPosCurve.Evaluate(ratio);
                y = juiceCurves.yPosCurve.Evaluate(ratio);
                z = juiceCurves.zPosCurve.Evaluate(ratio);
                target.localPosition = new Vector3(x, y, z);
            }

            if (juiceCurves.effectsScale)
            {
                x = juiceCurves.xScaleCurve.Evaluate(ratio);
                y = juiceCurves.yScaleCurve.Evaluate(ratio);
                z = juiceCurves.zScaleCurve.Evaluate(ratio);
                target.localScale = new Vector3(x, y, z);
            }

            if (juiceCurves.effectsRotation)
            {
                x = juiceCurves.xRotationCurve.Evaluate(ratio);
                y = juiceCurves.yRotationCurve.Evaluate(ratio);
                z = juiceCurves.zRotationCurve.Evaluate(ratio);
                target.localEulerAngles = new Vector3(x, y, z);
            }
        }

        private void MakeEffects(Juice j, GameObject fx)
        {
            if (!j.spawnFx) return;
            if (!fx) return;

            GameObject spawnedFx = Instantiate(fx, j.targetTransform.position, j.targetTransform.rotation);

            if (j.fxParentType == Juice.ParentType.This)
                spawnedFx.transform.SetParent(j.targetTransform);
            else if (j.fxParentType == Juice.ParentType.WorldSpace)
                spawnedFx.transform.SetParent(transform);
        }

        private void MakeSound(Juice j, Sound sound)
        {
            if (!j.spawnSound) return;
            if (!sound) return;

            Sound instantiatedSound = Instantiate(sound, j.targetTransform.position, j.targetTransform.rotation);

            if (j.soundParentType == Juice.ParentType.This)
                instantiatedSound.transform.SetParent(j.targetTransform);
            else if (j.soundParentType == Juice.ParentType.WorldSpace)
                instantiatedSound.transform.SetParent(transform);

            instantiatedSound.PlayAndDestroy();
        }

        private void MakeEffects(GameObject fx, Transform parentTransform, Juice.ParentType parent)
        {
            if (!fx) return; // Less optimized

            if (parent == Juice.ParentType.This)
                Instantiate(fx, parentTransform.position, parentTransform.rotation, parentTransform);
            else if (parent == Juice.ParentType.WorldSpace)
                Instantiate(fx, parentTransform.position, parentTransform.rotation, transform);
        }
    }
}