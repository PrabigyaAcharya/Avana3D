using UnityEngine;

public class ItemPickup : InteractableObject
{

    public override void Interact()
    {
        PickUp();
    }

    void PickUp()
    {
        Destroy(gameObject);
    }


}
