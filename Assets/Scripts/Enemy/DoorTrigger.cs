using UnityEngine;
using UnityEngine.AI;

public class DoorTrigger : MonoBehaviour
{
    public bool open = false;
    public DoorState Door;
    private int AgentsInRange = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {

            AgentsInRange++;
            if (!Door.open)
            {
                Door.Open();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            // if you do not want to automatically close doors, do not implement this method
            AgentsInRange--;
            if (Door.open && AgentsInRange == 0)
            {
                Door.Close();
            }
        }
    }
}