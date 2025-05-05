using UnityEngine;
using BehaviorTree;

public class TaskRangedAttack : Node
{
    Transform _transform;
    GameObject _projectilePrefab;
    Transform _spawnPoint;
    float _lastFireTime;
    public float fireRate = 0.5f;

    public TaskRangedAttack(Transform transform, GameObject prefab, Transform spawnPoint)
    {
        _transform = transform;
        _projectilePrefab = prefab;
        _spawnPoint = spawnPoint;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("[Attack] Evaluate()");
        object t = GetData("target");
        if (t == null)
            return state = NodeState.FAILURE;

        if (Time.time < _lastFireTime + 1f/fireRate)
            return state = NodeState.RUNNING;

        Transform _player = (Transform)t;

        Vector3 toPlayer = _player.position - _transform.position;
        toPlayer.y = 0;
        if (toPlayer.sqrMagnitude > 0.001f) _transform.rotation = Quaternion.LookRotation(toPlayer);

        GameObject proj = Object.Instantiate(_projectilePrefab,
                                             _spawnPoint.position,
                                             _spawnPoint.rotation);

        Collider bulletCol = proj.GetComponent<Collider>();
        Collider shooterCol = _transform.GetComponent<Collider>();  
        if (bulletCol != null && shooterCol != null)
            Physics.IgnoreCollision(bulletCol, shooterCol);
        
        Debug.Log("[Attack] Fired!");
        // No need set velocity here if Projectile.Start() đã làm
        state = NodeState.SUCCESS;
        return state;
    }
}