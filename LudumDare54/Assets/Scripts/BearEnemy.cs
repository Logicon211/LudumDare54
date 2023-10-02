using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearEnemy : EnemyAi
{
    // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
    private bool isAttacking = false;

    public override void SpawnAttack(){

        // Debug.Log("attak spawned.");

        GameObject projectile = Instantiate(attacks[Random.Range(0, attacks.Count)], attackLocation.position, Quaternion.identity);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = (sprite.sortingOrder + 1);
    }

    public void StartAttackSound() {
        if(!AS.isPlaying) {
            AS.Play();
        }
        isAttacking = true;
    }

    public void StopAttackSound() {
        if(AS) {
            AS.Stop();
        }
        IEnumerator coroutine = DisableAttackLock(0.5f);
        StartCoroutine(coroutine);
    }

    protected override void Move(Tile playerTile, int alignment)
    {
        if(!isAttacking){
            base.Move(playerTile, alignment);
        }
    }

    private IEnumerator DisableAttackLock(float waitTime)
    {
		while (true)
        {
			yield return new WaitForSeconds(waitTime);
			isAttacking = false;
			yield break;
		}
	}
}
