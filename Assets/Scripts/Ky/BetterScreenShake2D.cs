using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ky
{
    public class BetterScreenShake2D : Singleton<BetterScreenShake2D>
    {
        [SerializeField, Tooltip("Controls the local position, non relatively.")] private Transform cameraTransform;
        [SerializeField] private float drainDuration = 1;
        [SerializeField] private float shakeCameraOffsetMax = 0.03f;
        [SerializeField] private float shakeExponent = 1.9f;
        [SerializeField] private float perlinSpeed = 13f;

        public static float MagnitudeMultSetting { get; set; } = 0.6f;

        private DrainingTime shakeTrauma = new DrainingTime(1, 0);
        private List<ShakeTraumaModifier> shakeTraumaModifiers = new();
        private float lockedShake = 0f;
        private float perlinPoint;

        private float Mag => Mathf.Pow(MagRaw, shakeExponent) * shakeCameraOffsetMax;

        private float MagRaw
        {
            get
            {
                var total = 0f;
                foreach (ShakeTraumaModifier modifier in shakeTraumaModifiers)
                    total += modifier.Current;
                total += shakeTrauma.Current;
                total += lockedShake;
                return total;
            }
        }

        private void Start()
        {
            RefreshSettings();
        }

        private void Update()
        {
            shakeTrauma.Update(Time.deltaTime / drainDuration);
            shakeTrauma.Clamp();
            perlinPoint += Time.deltaTime;
            AlterCamera();
        }

        public void Amplify(float ratio)
        {
            shakeTrauma.ClampedChange(ratio);
            AlterCamera();
        }

        public void Set(float shakeRatio, float delay)
        {
            StartCoroutine(Delayed());

            IEnumerator Delayed()
            {
                yield return new WaitForSeconds(delay);
                Set(shakeRatio);
            }
        }

        public static void RefreshSettings()
        {
            // MagnitudeMultSetting = Settings.Accessibility.ScreenShake;
        }

        [Button]
        public void Set(float shakeRatio)
        {
            shakeTrauma.Current = shakeRatio;
            AlterCamera();
        }

        [Button]
        public void Lock(float shakeRatio)
        {
            lockedShake = shakeRatio;
        }

        public void SetCurve(ShakeTraumaModifierMaker maker)
        {
            SetCurve(maker.curve, maker.duration, maker.delay);
        }

        public void SetCurve(AnimationCurve shakeCurve, float duration, float delay = 0)
        {
            StartCoroutine(CurveShake());

            IEnumerator CurveShake()
            {
                yield return new WaitForSeconds(delay);

                var mod = new ShakeTraumaModifier(shakeCurve, duration);
                shakeTraumaModifiers.Add(mod);
                DrainingTime timer = mod.curveTimer;
                while (!timer.IsDrained)
                {
                    timer.Update(Time.deltaTime);
                    yield return null;
                }

                shakeTraumaModifiers.Remove(mod);
            }
        }

        private void AlterCamera()
        {
            // Don't question the magic numbers in here.
            // I'd just offset the noise by 0.5f and mod with %10,(and some more) but for some reason it didn't work.
            // + 420.69f) %10 did tho. Dunno why, but it works, so it stays.
            var perlinNoiseX = 1f - 2 * Mathf.PerlinNoise((perlinPoint * perlinSpeed) % 10, 0.1f);
            var perlinNoiseY = 1f - 2 * Mathf.PerlinNoise((perlinPoint * perlinSpeed + 420.69f) % 10, 0.6f);
            var v3 = new Vector3(perlinNoiseX, perlinNoiseY, 0f);
            // Default MagMultSetting is 0.6f, so we multiply by 1.666f
            cameraTransform.localPosition = v3 * (Mag * MagnitudeMultSetting * 1.666f);
        }
    }

    [Serializable]
    public class ShakeTraumaModifierMaker
    {
        // I am not sure if this kinda class is okay, as in it makes sense or not.
        // But I'm making it cuz it's like ShakeTraumaModifier but split into two pieces.
        // Also very basic
        [SerializeField] public AnimationCurve curve;
        [Tooltip("If desired")] [SerializeField] public float delay;
        [SerializeField] public float duration = 1f;
    }

    internal class ShakeTraumaModifier
    {
        public float Current => curve.Evaluate(curveTimer.RatioTillDrain);

        public DrainingTime curveTimer;
        private AnimationCurve curve;

        public ShakeTraumaModifier(AnimationCurve shakeCurve, float duration)
        {
            curve = shakeCurve;
            curveTimer = new DrainingTime(duration);
        }
    }
}