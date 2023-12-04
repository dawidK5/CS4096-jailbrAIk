using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    public float radiusOfSmell;
    public float radiusOfHearing;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    
    public LayerMask targetMask;
    public LayerMask smellMask;
    public LayerMask DogLayer;
    public LayerMask obstructionMask;
    private Rigidbody rb;
    public float speed;
    public Vector3 playerLastSeenPostion;
    public Vector3 lastSmelledPosition;
    public Transform head;
    public float timePlayerLastSeen = 0f;
    public bool canSeePlayer;
    
    public bool canSmellPlayer;

    public bool canHearOtherEnemy;
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public float meshResolution;
    public float edgeDstThreshold;
    public int edgeResolveIterations;
    public float playerLastLocatedTime = 0.0f;
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        if (name.Contains("Dog"))
        {
          StartCoroutine(FOVRoutine());
        }
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
            FieldOfSmellCheck();
            PlayerDetected();
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(angle * meshResolution);
        float stepAngleSize = angle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle1 = transform.eulerAngles.y - angle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle1);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, radius, obstructionMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * radius, radius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    private void chase()
    {
        //transform.LookAt(playerRef.transform);
        //float distanceToTarget = Vector3.Distance(transform.position, playerRef.transform.position);
        //Vector3 directionToTarget = playerRef.transform.position - transform.position;

        //transform.position = Vector3.MoveTowards(this.transform.position, playerRef.transform.position, speed * Time.deltaTime);
    }

    private bool FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    //Debug.Log("can see Path");
                    canSeePlayer = true;
                    timePlayerLastSeen = Time.fixedTime;
                    playerLastSeenPostion = target.position;
                    playerLastLocatedTime = Time.time;
                }
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;

        return canSeePlayer;
    }

    private bool FieldOfSmellCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radiusOfSmell, smellMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    Debug.Log("can smell Player");
                    canSmellPlayer = true;
                    lastSmelledPosition = target.position;
                    playerLastLocatedTime = Time.time;
                }
                //else
                //    canSmellPlayer = false;
             
        }
        else if (canSmellPlayer)
            canSmellPlayer = false;

        return canSmellPlayer;
    }

    private bool PlayerDetected()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, radiusOfHearing, DogLayer);

        if (collider.Length != 0)
        {
            foreach (Collider c in collider)
            {
                if (c.gameObject != gameObject) { 
                    canHearOtherEnemy = true;
                }
                else
                {
                    canHearOtherEnemy = false;
                }
            }
            
        }
        

        return canHearOtherEnemy;
    }


}