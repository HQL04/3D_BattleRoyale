using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RangedGuardBT : BehaviorTree.Tree
{
    public UnityEngine.Transform[] waypoints;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float acquireRange = 30f;
    public float loseRange    = 50f;  
    public static float speed = 7f;
    public static float rangedRange = 15f;

    protected override Node SetupTree()
    {
        var attackSequence = new Sequence(new List<Node>
        {
            new CheckFOVRange(transform, acquireRange, loseRange),
            new CheckEnemyInRangedRange(transform, rangedRange),
            new TaskRangedAttack(transform, projectilePrefab, projectileSpawnPoint)
        });

        var moveSequence = new Sequence(new List<Node>
        {
            new CheckHasTarget(),
            new TaskMoveToRangedDistance(transform, rangedRange, speed)
        });

        var patrol = new TaskPatrol(transform, waypoints);

        return new Selector(new List<Node> { attackSequence, moveSequence, patrol });
    }
}