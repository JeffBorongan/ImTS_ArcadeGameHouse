using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New EnemySpawn Action", menuName = "Action/Enemy Spawn", order = 2)]
public class SpawnEnemyAction : Action
{
    public Side main = Side.Middle;
    public Side secondary = Side.Middle;
    public float enemySpeed = 0;

    public override void ExecuteAction(TutorialActor actor, UnityAction OnEndAction)
    {
        SpawnManager.Instance.SpawnEnemy(main, secondary, this, OnEndAction, OnEndAction);
    }
}
