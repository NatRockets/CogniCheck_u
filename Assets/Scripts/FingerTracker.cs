using UnityEngine;
using UnityEngine.UI;

public class FingerTracker : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private UpdateWrapper updateHandler;
    [SerializeField] private Transform trackedObj;
    [SerializeField] private Text condition;

    private float timeTotal;
    private float timeGood;
    private float timeOk;
    
    private float distTreshold = 1.5f;//circle radius
    
    private void OnDisable()
    {
        SwitchInput(false);
    }

    private void CalculateAccuracy(Vector3 hit)
    {
        float distance = Vector3.Distance(trackedObj.position, hit);
        if (distance < distTreshold / 2f)
        {
            timeGood += Time.deltaTime;
            condition.text = "EXCELLENT!";
            return;
        }
        
        if (distance < distTreshold)
        {
            timeOk += Time.deltaTime;
            condition.text = "GOOD!";
            return;
        }
        
        condition.text = "BAD!";
    }

    private void CheckHit(Vector3 inputPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPos);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent<Cast>(out var target))
        {
            CalculateAccuracy(hit.point);
        }
    }

    private void LocalUpdate()
    {
        timeTotal += Time.deltaTime;
#if !UNITY_EDITOR
        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                CheckHit(Input.GetTouch(0).position);
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                condition.text = "BAD!";
            }
        }
#elif UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            CheckHit(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            condition.text = "BAD!";
        }
#endif
    }

    public void SetResult(out int good, out int bad, out int ok)
    {
        good = (int) (timeGood/timeTotal * 100f);
        ok = (int) (timeOk/timeTotal * 100f);
        bad = 100 - good - ok;
    }

    public void SwitchInput(bool activate)
    {
        if (activate)
        {
            timeGood = timeOk = timeTotal = 0f;
            updateHandler.UpdateEvent += LocalUpdate;
            condition.enabled = true;
        }
        else
        {
            condition.enabled = false;
            updateHandler.UpdateEvent -= LocalUpdate;
        }
    }
}
