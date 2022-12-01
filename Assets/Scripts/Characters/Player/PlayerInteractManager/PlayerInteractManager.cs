using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum InteractSelectionMode
{
    First,
    Last,
    Closest
}

public class PlayerInteractManager : MonoBehaviour
{
    public PlayerManager player;

    public InteractSelectionMode selectMode = InteractSelectionMode.Closest;

    private LinkedList<GameObject> interactables;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(player);

        interactables = new();
    }

    // Interact with the next interactable based on the selection mode
    public void Interact()
    {
        if (interactables.Count == 0)
        {
            return;
        }

        LinkedListNode<GameObject> interaction = GetInteraction();
        if (interaction == null)
        {
            return;
        }

        // Do action according to interaction tag
        // Expand list as more interactions are created
        switch (interaction.Value.tag)
        {
            case "Weapon":
                GameObject weaponRoot = interaction.Value.transform.parent.gameObject;
                interactables.Remove(interaction);
                player.WeaponInventory.AddWeapon(weaponRoot);
                return;

            case "Interactor":
                GameObject target = interaction.Value;
                //interactables.Remove(interaction);
                target.GetComponent<Interactor>().Interact();
                return;

            default:
                break;
        }

        Debug.Log("Unknown interaction of tag " + interaction.Value.tag);
        interactables.Remove(interaction);
    }

    // Add an interaction to the interactables list
    public void AddInteractable(GameObject interaction)
    {
        interactables.AddLast(interaction);
    }

    // Remove an interaction from the interactables list
    public void RemoveInteractable(GameObject interaction)
    {
        interactables.Remove(interaction);
    }

    // Get the next interaction based on the selection mode
    private LinkedListNode<GameObject> GetInteraction()
    {
        if (interactables.Count == 0)
        {
            return null;
        }

        switch(selectMode)
        {
            case InteractSelectionMode.Closest:
                LinkedListNode<GameObject> traversal = interactables.First;
                LinkedListNode<GameObject> closestNode = traversal;
                float closestDist = Vector3.Distance(player.transform.position, closestNode.Value.transform.position);
                traversal = traversal.Next;

                // Loop through interactables to get closest interaction
                for (; traversal != null; traversal = traversal.Next)
                {
                    float dist = Vector3.Distance(player.transform.position, traversal.Value.transform.position);
                    if (dist < closestDist)
                    {
                        closestNode = traversal;
                        closestDist = dist;
                    }
                }
                return closestNode;

            case InteractSelectionMode.First:
                return interactables.First;

            case InteractSelectionMode.Last:
                return interactables.Last;

            default:
                return null;
        }
    }
}
