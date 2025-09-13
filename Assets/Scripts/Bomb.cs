using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour, IPoolable<Bomb>
{
    [SerializeField] private float _minLifetime;
    [SerializeField] private float _maxLifetime;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    private Renderer _renderer;


    public event System.Action<Bomb> OnExpired;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public Bomb Initialize(Vector3 position)
    {
        transform.position = position;

        StartCoroutine(DelayedExplosion(Random.Range(2f, 5f)));

        return this;
    }

    private IEnumerator DelayedExplosion(float time)
    {
        float elapsed = 0;
        float fadeMaxDelta = 1 / time * Time.deltaTime;

        while (elapsed < time)
        {

            Color c = _renderer.material.color;
            c.a = Mathf.MoveTowards(c.a, 0, fadeMaxDelta);
            _renderer.material.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Explode();

        OnExpired?.Invoke(this);
    }

    private void Explode()
    {
        // Debug.Log("Exploded");
        foreach (var collider in Physics.OverlapSphere(transform.position, _explosionRadius))
        {
            if (collider.attachedRigidbody != null)
            {
                collider.attachedRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }
}