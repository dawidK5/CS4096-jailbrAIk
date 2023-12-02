using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float radius = 2f, agentColliderSize = 0.6f;

    [SerializeField]
    private bool showGizmo = true;

    //gizmo parameters
    float[] dangersResultTemp = null;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        foreach (Collider obstacleCollider in aiData.obstacles)
        {
            Vector3 directionToObstacle
                = obstacleCollider.ClosestPoint(transform.position) - (Vector3)transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            //calculate weight based on the distance Enemy<--->Obstacle
            // This line of code is using a ternary operator to assign a value to the weight variable based on the condition distanceToObstacle <= agentColliderSize.

                // Here's a breakdown:

                // If distanceToObstacle is less than or equal to agentColliderSize, then weight is assigned a value of 1.
                // If distanceToObstacle is greater than agentColliderSize, then weight is assigned the value of (radius - distanceToObstacle) / radius.
                // The (radius - distanceToObstacle) / radius part calculates what fraction of the radius the distanceToObstacle is. If distanceToObstacle is equal to radius, then weight will be 0. If distanceToObstacle is less than radius, then weight will be greater than 0 and less than 1.

            float weight
                = distanceToObstacle <= agentColliderSize
                ? 1
                : (radius - distanceToObstacle) / radius;

                
            Vector3 directionToObstacleNormalized = directionToObstacle.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float result = Vector3.Dot(directionToObstacleNormalized, Directions.eightDirections[i]);

                float valueToPutIn = result * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        dangersResultTemp = danger;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        if (Application.isPlaying && dangersResultTemp != null)
        {
            if (dangersResultTemp != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < dangersResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(
                        transform.position,
                        Directions.eightDirections[i] * dangersResultTemp[i]*2
                        );
                }
            }
        }

    }
}

public static class Directions
{
    public static List<Vector3> eightDirections = new List<Vector3>{
            new Vector3(0,0,1).normalized,
            new Vector3(1,0,1).normalized,
            new Vector3(1,0,0).normalized,
            new Vector3(1,0,-1).normalized,
            new Vector3(0,0,-1).normalized,
            new Vector3(-1,0,-1).normalized,
            new Vector3(-1,0,0).normalized,
            new Vector3(-1,0,1).normalized
        };
}
