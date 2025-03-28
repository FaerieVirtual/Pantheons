using System.Collections;
using UnityEngine;

public class DeathState : EnemyState
{
    public DeathState(EnemyBase enemy, EnemyStateMachine machine) : base(enemy, machine)
    {
        this.machine = machine;
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        enemy.RigidBody.velocity = Vector3.zero;
        enemy.SpawnCoin(enemy.CoinCount + PlayerManager.Instance.GatherGoldBoost);
        PlayerManager.Instance.AddMana(enemy.ManaCount + PlayerManager.Instance.GatherManaBoost);

        enemy.StartCoroutine(AnimateAndDisable());
    }

    private IEnumerator AnimateAndDisable()
    {
        enemy.Animator.Play("Death");
        AnimatorStateInfo info = enemy.Animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(info.length);
        enemy.gameObject.SetActive(false);
    }
}
