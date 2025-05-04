using UnityEngine;
using BehaviorTree;

public class CheckEnemyNotInRangedRange : Node
{
    private Transform _transform;
    private float _rangedRange = 10f;

    public CheckEnemyNotInRangedRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object target = GetData("target");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform targetTransform = (Transform)target;
        float distance = Vector3.Distance(_transform.position, targetTransform.position);
        state = distance > _rangedRange ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}