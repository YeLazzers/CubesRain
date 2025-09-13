using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private Bomb _prefab;

    private ShapePool<Bomb> _pool;

    private void Awake()
    {
        _pool = new ShapePool<Bomb>(_prefab, transform);
    }

    // private void Start()
    // {
    //     _pool.Get().Initialize(new Vector3(0, 1, 0));
    // }

    public void SpawnAtPosition(Vector3 position)
    {
        _pool.Get().Initialize(position);
    }
}
