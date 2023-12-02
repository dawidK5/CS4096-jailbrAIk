using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRange = 5;

    [SerializeField]
    private LayerMask obstaclesLayerMask, playerLayerMask;

    [SerializeField]
    private bool showGizmos = false;

    //gizmo parameters
    private List<Transform> colliders;
    Collider playerCollider;

    public override void Detect(AIData aiData)
    {
        //Find out if player is near
        Collider[] playerColliders = 
            Physics.OverlapSphere(transform.position, targetDetectionRange, playerLayerMask);
        if (playerColliders.Length != 0)
        {
            playerCollider = playerColliders[0];
            Debug.Log("Player detected");
        }
        else
        {
            Debug.Log("Player not detected");
        }
        string playerLayerName = LayerMask.LayerToName(playerLayerMask);
        Debug.Log(playerLayerName);

        if (playerCollider != null)
        {
            //Check if you see the player
            Vector3 direction = (playerCollider.transform.position - transform.position).normalized;
            // RaycastHit hit = 
            //     Physics.Raycast(transform.position, direction, out hit, targetDetectionRange, obstaclesLayerMask);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, targetDetectionRange, obstaclesLayerMask))
            {
                if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                    colliders = new List<Transform>() { playerCollider.transform };
                }
                
            }
            //Make sure that the collider we see is on the "Player" layer
            
            else
            {
                colliders = null;
            }
        }
        else
        {
            //Enemy doesn't see the player
            colliders = null;
        }
        aiData.targets = colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
