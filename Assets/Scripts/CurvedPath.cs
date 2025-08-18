using UnityEngine;

public class CurvedPath : MonoBehaviour
{
    [SerializeField] private Curve[] curves;
    [SerializeField] private GameObject visuals;
    [SerializeField] private bool looped;

    private int curveId;

    private float tParam;

    private Vector3 targetPosition;
    private float targetSpeed;
    private Transform targetTransform;
    private Vector3 targetDirection;

    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    private bool isFollowing;
    private System.Action PathCallback;

    private UpdateWrapper globalUpdate;

    private void PathUpdate()
    {      
        if (tParam < 1)
        {
            tParam += Time.deltaTime * targetSpeed;
            targetPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1
                + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            targetDirection = targetPosition - targetTransform.position;
            //targetTransform.forward = targetDirection;

            targetTransform.position = targetPosition;
        }
        else
        {
            globalUpdate.UpdateEvent -= PathUpdate;
            CompleteCurve();
        }        
    }

    private void InitCurve(int id)
    {
        curves[id].GetControlPoints(out p0, out p1, out p2, out p3);//order bool
    }

    private void CompleteCurve()
    {
        if (!isFollowing)
        {
            return;
        }

        tParam = 0;
        curveId += 1;

        if (curveId >= curves.Length)
        {
            if (looped)
            {
                curveId = 0;
                InitCurve(curveId);
                globalUpdate.UpdateEvent += PathUpdate;
            }
            else
            {
                isFollowing = false;
                visuals.SetActive(false);
                if (PathCallback != null)
                {
                    PathCallback();
                }                
            }
        }
        else
        {
            InitCurve(curveId);
            globalUpdate.UpdateEvent += PathUpdate;
        }
    }

    public void FollowPath()
    {
        if (isFollowing)
        {
            return;
        }

        isFollowing = true;
        visuals.SetActive(true);
        globalUpdate.UpdateEvent += PathUpdate;
    }

    public void UnfollowPath()
    {
        if (!isFollowing)
        {
            return;
        }

        isFollowing = false;
        visuals.SetActive(false);
        globalUpdate.UpdateEvent -= PathUpdate;
    }

    public void ResetPath()//curve order bool
    {
        curveId = 0;
        InitCurve(curveId);
        tParam = 0f;

        targetTransform.position = curves[0].GetControlPoint(0);
        //targetTransform.rotation = Quaternion.Euler(Vector3.forward);
    }

    public void SetTarget(Transform targetT)
    {
        targetTransform = targetT;        
    }

    public void SetSpeed(float targetS)
    {
        targetSpeed = targetS;
    }

    public void SetCallback(System.Action callback)
    {
        PathCallback = callback;
    }

    public void InitMono(UpdateWrapper update)
    {
        globalUpdate = update;
    }
}
