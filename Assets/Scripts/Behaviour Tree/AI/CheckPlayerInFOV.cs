using UnityEngine;

using BehaviourTree;
public class CheckPlayerInFOV : Node
{
    // Start is called before the first frame update
    private Transform _transform;
    public FieldOfView fov;
    private static int _playerLayerMask = 1 << 3;
    public bool playerInTrigger = false;
    public float timeToDisengage = 3f;

    public CheckPlayerInFOV(Transform transform, FieldOfView fov)
    {
        _transform = transform;
        this.fov = fov;
    }

    public override NodeState Evaluate()
    {
        object t = getData("target");

        // Clearing target and returning fail to stop following player after period of time not in trigger.
        if (fov.canSeePlayer == false && fov.timePlayerLastSeen < Time.fixedTime - timeToDisengage)
        {
            parent.parent.setData("target", null);
            return NodeState.FAILURE;
        }
        else if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, SpotLightBT.fovRange, _playerLayerMask);

            if (fov.canSeePlayer == true)
            {
                Debug.Log("seeingTarget");
                parent.parent.setData("target", colliders[0].transform);

                state = NodeState.SUCCESS;
                return state;
            }


            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }


}

