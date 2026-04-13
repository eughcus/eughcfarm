using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eughc.Farm {
    [RequireComponent(typeof(FruitBasket))]
    public class FruitBasketVisual : MonoBehaviour {
        private FruitBasket _basket;
        private List<Coroutine> _coroutineQueue = new();

        private void Start() {
            _basket = GetComponent<FruitBasket>();
            _basket.OnFruitAdded += OnFruitAdded;
        }

        private void OnDestroy() {
            _basket.OnFruitAdded -= OnFruitAdded;
        }

        [SerializeField] private Transform fruitParent;

        private void OnFruitAdded(Fruit fruit) {
            fruit.transform.parent = fruitParent;

            int layer = fruitParent.gameObject.layer;
            foreach (var t in fruit.GetComponentsInChildren<Transform>(includeInactive: true))
                t.gameObject.layer = layer;

            Coroutine coroutine = null;
            coroutine = StartCoroutine(
                MoveToPosition(
                    target: fruit.transform,
                    duration: 0.5f,
                    OnComplete: () => {
                        _coroutineQueue.Remove(coroutine);
                    })
            );
            _coroutineQueue.Add(coroutine);
        }

        private IEnumerator MoveToPosition(Transform target, float duration, Action OnComplete) {
            yield return new WaitForSeconds(_coroutineQueue.Count * 0.2f);
            target.TryGetComponent(out Rigidbody rigidbody);
            if (rigidbody != null)
                rigidbody.isKinematic = false;

            Vector3 start = target.position;
            float elapsed = 0f;

            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                target.position = Vector3.Lerp(start, fruitParent.position, elapsed / duration);
                yield return null;
            }

            target.position = fruitParent.position;
            OnComplete?.Invoke();
        }
    }
}