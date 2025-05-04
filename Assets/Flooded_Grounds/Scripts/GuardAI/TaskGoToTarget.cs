using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform _transform;
    private TeleportSkill _teleport;

    private float _teleportThreshold = 3f;

    public TaskGoToTarget(Transform transform)
    {
        _transform = transform;
        _teleport  = transform.GetComponent<TeleportSkill>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        float dist = Vector3.Distance(_transform.position, target.position);

        if (_teleport != null && _teleport.IsReady && dist > _teleportThreshold)
        {
            _teleport.TeleportBehind(target);
            Animator a = _transform.GetComponent<Animator>();
            if (a != null)
            {
                a.SetTrigger("Teleport");
            }
            state = NodeState.SUCCESS;
            Debug.Log("hehe");
            return state;
        }

        if (dist > 0.01f)
        {
            _transform.position = Vector3.MoveTowards(
                _transform.position,
                target.position,
                GuardBT.speed * Time.deltaTime);
            _transform.LookAt(target.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
}
