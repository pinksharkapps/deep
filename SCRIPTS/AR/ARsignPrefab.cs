using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AR
{


    public class ARsignPrefab : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explsion;
        [SerializeField] private ParticleSystem _sparkle;

        private void Awake()
        {
            _explsion.Stop();
            _sparkle.Stop();
        }

        private void OnEnable()
        {
            StartCoroutine(ExplodeAterPause());
        }

        private IEnumerator ExplodeAterPause()
        {
            yield return new WaitForSeconds(0.3f);
            _explsion.Play();
            yield return new WaitForSeconds(0.6f);
            _sparkle.Play();
        }
    }
}
