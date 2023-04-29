using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{

    public LayerMask moveMask;

    public InteractableObject focus;

    Camera cam;

    PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, moveMask))
            {
                movement.MoveTo(hit.point);
                

                //stop focus
                StopFocus();
            }
        }




        if (Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                InteractableObject interactable  = hit.collider.GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    FocusOn(interactable);
                }


            }
        }
    }

    void FocusOn(InteractableObject focused)
    {
        if (focused != focus)
        {
            if (focus != null)
                focus.OnNotFocused();
            focus = focused;
            movement.HomingTarget(focused);
            
        }

        focused.OnFocused(transform);

    }

    void StopFocus()
    {
        if (focus!=null)
            focus.OnNotFocused();
        focus = null;
        movement.StopTrackingTarget();
    }

}
