using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float visibleRadius = 10f;

    public AudioSource footsteps;

    Transform target;

    NavMeshAgent agent;

    Combat combat;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.Player.transform;
        combat = GetComponent<Combat>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= visibleRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                CharacterStat targetStat = target.GetComponent<CharacterStat>();
                if (targetStat != null)
                {
                    combat.Attack(targetStat);

                }
                FaceTarget();
            }
        }

        if (agent.velocity.magnitude > 0f)
        {
            footsteps.enabled= true;
        }
        else
        {
            footsteps.enabled= false;
        }
    }

    void FaceTarget()
    {
        Vector3 lookingDir = (target.position - transform.position).normalized;
        Quaternion lrotation = Quaternion.LookRotation(new Vector3(lookingDir.x, 0f, lookingDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lrotation, Time.deltaTime * 5f);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visibleRadius);
    }
}
