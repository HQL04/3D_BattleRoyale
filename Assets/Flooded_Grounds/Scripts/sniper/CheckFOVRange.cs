using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckFOVRange : Node
{
    private static int _enemyLayerMask = 1 << 6;
    private Transform _transform;
    private Animator _animator;
    private float _acquireRange;
    private float _loseRange;

    public CheckFOVRange(Transform transform, float acquireRange, float loseRange)
    {
        _transform    = transform;
        _animator     = transform.GetComponent<Animator>();
        _acquireRange = acquireRange;
        _loseRange    = loseRange;
    }

    public override NodeState Evaluate()
    {
        Debug.Log($"[FOV] hasTarget={(GetData("target")!=null)}");
        
        object t = GetData("target");

        // 1) Nếu chưa có target → thử acquire
        if (t == null)
        {
            Collider[] hits = Physics.OverlapSphere(
                _transform.position,
                _acquireRange,
                _enemyLayerMask);

            if (hits.Length > 0)
            {
                parent.SetData("target", hits[0].transform);
                _animator.SetBool("Walking", true);
                Debug.Log("[FOV] Acquired target");
                return state = NodeState.SUCCESS;
            }
            return state = NodeState.FAILURE;
        }

        // 2) Nếu đã có target → giữ nếu vẫn trong loseRange
        Transform target = (Transform)t;
        float d = Vector3.Distance(_transform.position, target.position);
        if (d <= _loseRange)
        {
            return state = NodeState.SUCCESS;
        }

        // 3) Nếu target quá xa – quên target
        ClearData("target");
        _animator.SetBool("Walking", false);
        Debug.Log("[FOV] Lost target");
        return state = NodeState.FAILURE;
    }
}

public class CheckHasTarget : Node {
    public override NodeState Evaluate() {
        return (GetData("target") != null)
            ? NodeState.SUCCESS
            : NodeState.FAILURE;
    }
}