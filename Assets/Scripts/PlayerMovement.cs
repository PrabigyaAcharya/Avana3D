using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class PlayerMovement : MonoBehaviour
{
    public AudioSource footsteps;

    Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent= GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target !=null) 
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }

        if (agent.velocity.magnitude >0)
        {
            footsteps.enabled= true;
        }
        else
        {
            footsteps.enabled= false;   
        }
    }

    public void MoveTo(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void HomingTarget(InteractableObject newTarget)
    {
        agent.stoppingDistance = newTarget.radius * .8f;
        agent.updateRotation = false;
        target = newTarget.interactionTransform;
    }

    public void StopTrackingTarget() 
    { 
        agent.stoppingDistance=0;
        agent.updateRotation = true;
        target = null; 
    }

    void FaceTarget()
    {
        Vector3 lookingDir = (target.position - transform.position).normalized;
        Quaternion lrotation = Quaternion.LookRotation(new Vector3(lookingDir.x, 0f, lookingDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lrotation, Time.deltaTime * 5f);
    }
}
