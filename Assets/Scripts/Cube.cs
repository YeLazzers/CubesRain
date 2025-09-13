using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, IPoolable<Cube>
{
    private Renderer _renderer;
    private bool _isHit;

    public event System.Action<Cube> OnExpired;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public Cube Initialize(Vector3 position)
    {
        _isHit = false;
        transform.position = position;
        _renderer.material.color = Color.white;

        return this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Platform platform) && !_isHit)
        {
            _isHit = true;
            _renderer.material.color = Random.ColorHSV();

            StartCoroutine(DestroyDelayed(Random.Range(2f, 5f)));
        }
    }

    private IEnumerator DestroyDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnExpired?.Invoke(this);
    }
}
