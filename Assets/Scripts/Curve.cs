using UnityEngine;

public class Curve : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;

    private Vector3 gizmosPosition;

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.1f);
        }
    }

    public void GetControlPoints(out Vector3 p0, out Vector3 p1, out Vector3 p2, out Vector3 p3)
    {
        p0 = controlPoints[0].position;
        p1 = controlPoints[1].position;
        p2 = controlPoints[2].position;
        p3 = controlPoints[3].position;
    }

    public Vector3 GetControlPoint(int id)
    {
        return controlPoints[id].position;
    }

}
