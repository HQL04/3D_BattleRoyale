using UnityEngine;
using BehaviorTree;

public class CheckEnemyInRangedRange : Node
{
    private Transform _transform;
    private float _rangedRange;

    public CheckEnemyInRangedRange(Transform transform, float rangedRange)
    {
        _transform = transform;
        _rangedRange = rangedRange;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Debug.Log("[InRangedRange] Không có target.");
            return state = NodeState.FAILURE;
        }

        Transform target = (Transform)t;
        float distance = Vector3.Distance(_transform.position, target.position);
        bool inRange = distance <= _rangedRange;
        Debug.Log($"[InRangedRange] dist={distance:F1} <= {_rangedRange}? {inRange}");
        return state = inRange 
            ? state = NodeState.SUCCESS 
            : state = NodeState.FAILURE;
    }
}