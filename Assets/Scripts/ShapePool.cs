using System;
using UnityEngine;
using UnityEngine.Pool;

public class ShapePool<T> where T : MonoBehaviour, IPoolable<T>
{
    private T _prefab;
    private Transform _parent;
    private ObjectPool<T> _pool;

    public ShapePool(T prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;

        _pool = new ObjectPool<T>(
            CreateFunc,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy
        );
    }

    public event Action<T> Getted;
    public event Action<T> Released;

    public T Get() =>
        _pool.Get();


    private T CreateFunc()
    {
        T shape = GameObject.Instantiate(_prefab, _parent);
        shape.OnExpired += _pool.Release;

        return shape;
    }

    private void ActionOnGet(T shape)
    {
        shape.gameObject.SetActive(true);
        Getted?.Invoke(shape);
    }

    private void ActionOnRelease(T shape)
    {
        shape.gameObject.SetActive(false);
        Released?.Invoke(shape);
    }

    private void ActionOnDestroy(T shape)
    {
        shape.OnExpired -= _pool.Release;
        GameObject.Destroy(shape.gameObject);
    }
}