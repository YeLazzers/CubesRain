using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _spawnDelay = 1.0f;

    private ShapePool<Cube> _pool;
    private BoxCollider _spawnArea;
    private WaitForSeconds _wfsDelay;

    private void Awake()
    {
        _spawnArea = GetComponent<BoxCollider>();
        _pool = new ShapePool<Cube>(_prefab, transform);
        _wfsDelay = new WaitForSeconds(_spawnDelay);
    }

    private void OnEnable()
    {
        _pool.Released += OnReleased;
    }

    private void OnDisable()
    {
        _pool.Released -= OnReleased;
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes()
    {
        while (enabled)
        {
            Cube cube = _pool.Get();
            cube.Initialize(GetRandomPosition());

            yield return _wfsDelay;
        }
    }

    private void OnReleased(Cube cube)
    {
        _bombSpawner.SpawnAtPosition(cube.transform.position);
    }

    private Vector3 GetRandomPosition()
    {
        float xPosition = Random.Range(-1 * _spawnArea.size.x / 2, _spawnArea.size.x / 2);
        return new Vector3(xPosition, transform.position.y, transform.position.z);
    }
}
