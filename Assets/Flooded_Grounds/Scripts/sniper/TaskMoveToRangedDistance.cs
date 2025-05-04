using UnityEngine;
using BehaviorTree;

public class TaskMoveToRangedDistance : Node
{
    private Transform _transform;
    private float _rangedDistance;
    private float _moveSpeed;

    public TaskMoveToRangedDistance(Transform transform, float rangedDistance, float moveSpeed)
    {
        _transform = transform;
        _rangedDistance = rangedDistance;
        _moveSpeed = moveSpeed;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
            return state = NodeState.FAILURE;

        Transform _player = (Transform)t;        

        float distance = Vector3.Distance(_transform.position, _player.position);
        if (distance > _rangedDistance)
        {
            _transform.position = Vector3.MoveTowards(
                _transform.position,
                _player.position,
                _moveSpeed * Time.deltaTime);
            return state = NodeState.RUNNING;
        }
        return state = NodeState.FAILURE;
    }
}