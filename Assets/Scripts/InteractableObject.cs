using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float radius = 3f;

    bool focused = false;

    bool interacted = false;

    Transform player;

    public Transform interactionTransform;


    public virtual void Interact() 
    {
        
    }


    private void Update()
    {
        if (focused && !interacted)
        {
            float dist = Vector3.Distance(player.position, interactionTransform.position);
            if(dist <= radius)
            {
                Interact();
                interacted= true;
            }
        }
    }


    public void OnFocused(Transform playerTransform)
    {
        focused = true;
        player = playerTransform;
        interacted= false;
    }

    public void OnNotFocused()
    {
        focused = false;
        player = null;
        interacted= false;
    }

    private void OnDrawGizmosSelected()
    {

        if (interactionTransform == null)
            interactionTransform= transform;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}
