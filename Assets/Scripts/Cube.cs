using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private IObjectPool<Cube> _pool;
    private bool _isHit;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Initialize(Vector3 position, IObjectPool<Cube> pool)
    {
        _pool = pool;

        _isHit = false;
        transform.position = position;
        _renderer.material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Platform platform) && !_isHit)
        {
            _isHit = true;
            _renderer.material.color = Random.ColorHSV(0.3f, 0.3f);

            StartCoroutine(DestroyDelayed(Random.Range(2f, 5f)));
        }
    }

    private IEnumerator DestroyDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        _pool.Release(this);
    }
}
