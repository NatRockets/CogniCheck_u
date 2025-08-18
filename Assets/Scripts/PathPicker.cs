using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathPicker : MonoBehaviour
{
    [SerializeField] private CurvedPath[] bezierTrajs;
    [SerializeField] private UpdateWrapper updateWrapper;

    private CurvedPath currentTraj;
    private float currentSpeed = 0.1f;
    
    public void BindTracks(Transform trackedObj, Action callback)
    {
        foreach (CurvedPath traj in bezierTrajs)
        {
            traj.InitMono(updateWrapper);
            traj.SetTarget(trackedObj);
            traj.SetCallback(callback);
            traj.SetSpeed(currentSpeed);
        }
    }

    public void PickRandomTrajectory()
    {
        currentTraj?.UnfollowPath();

        currentTraj = bezierTrajs[Random.Range(0, bezierTrajs.Length)];
        
        currentTraj.ResetPath();
        currentTraj.FollowPath();
    }

    public void StopTracking()
    {
        currentTraj?.UnfollowPath();
    }
}
