using System.Collections;
using UnityEngine;

namespace Eughc.Farm {
    [RequireComponent(typeof(Hose))]
    public class HoseVisual : MonoBehaviour {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private ParticleSystem hitParticles;
        [SerializeField] private Transform nozzleTip;
        [SerializeField] private Vector3 restingLocalEuler;
        [SerializeField] private float returnDuration = 0.3f;

        public Transform NozzleTip => nozzleTip;

        private Coroutine _returnCoroutine;

        public void OnEquip() {
            transform.localRotation = Quaternion.Euler(restingLocalEuler);
        }

        public void StartFlow() {
            lineRenderer.positionCount = 2;
            hitParticles.Play();
        }

        public void UpdateFlow(Vector3 impactPoint) {
            lineRenderer.SetPosition(0, nozzleTip.position);
            lineRenderer.SetPosition(1, impactPoint);
            transform.rotation = Quaternion.LookRotation((impactPoint - nozzleTip.position).normalized);
            hitParticles.transform.position = impactPoint;
        }

        public void StopFlow() {
            lineRenderer.positionCount = 0;
            hitParticles.Stop();
            hitParticles.Clear();
            if (_returnCoroutine != null) StopCoroutine(_returnCoroutine);
            _returnCoroutine = StartCoroutine(ReturnToResting());
        }

        private IEnumerator ReturnToResting() {
            Quaternion from = transform.localRotation;
            Quaternion to = Quaternion.Euler(restingLocalEuler);
            float t = 0f;
            while (t < 1f) {
                t += Time.deltaTime / returnDuration;
                transform.localRotation = Quaternion.Lerp(from, to, t);
                yield return null;
            }
            transform.localRotation = to;
            _returnCoroutine = null;
        }
    }
}