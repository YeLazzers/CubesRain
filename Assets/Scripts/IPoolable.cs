using System;
using UnityEngine;

public interface IPoolable<T>
{
    event Action<T> OnExpired;
    T Initialize(Vector3 position);
}