using UnityEngine;
using UnityEngine.UI;

public class MotorTest : MonoBehaviour
{
    [SerializeField] private PathPicker pathPicker;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private FingerTracker fingerTracker;
    [SerializeField] private GameObject startObject;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject finishObject;
    [SerializeField] private Button finishButton;
    [SerializeField] private Text goodText;
    [SerializeField] private Text okText;
    [SerializeField] private Text badText;
    [SerializeField] private Text cText;

    private SpriteRenderer targetRend;
    
    private void Awake()
    {
        targetRend = targetTransform.GetChild(0).GetComponent<SpriteRenderer>();
        pathPicker.BindTracks(targetTransform, OnTrajectoryFinished);
        
        startButton.onClick.AddListener(StartTest);
        finishButton.onClick.AddListener(FinishTest);
    }

    private void OnEnable()
    {
        cText.enabled = false;
        targetRend.enabled = false;
        startObject.SetActive(true);
        finishObject.SetActive(false);
    }

    private void OnDisable()
    {
        pathPicker.StopTracking();
        fingerTracker.SwitchInput(false);
    }

    private void StartTest()
    {
        startObject.SetActive(false);
        
        pathPicker.PickRandomTrajectory();
        targetRend.enabled = true;
        fingerTracker.SwitchInput(true);
        cText.enabled = true;
    }

    private void OnTrajectoryFinished()
    {
        fingerTracker.SwitchInput(false);
        targetRend.enabled = false;
        fingerTracker.SetResult(out var good, out var bad, out var ok);
        
        goodText.text = good + "%";
        badText.text = bad + "%";
        okText.text = ok + "%";
        
        finishObject.SetActive(true);
    }

    private void FinishTest()
    {
        finishObject.SetActive(false);
        startObject.SetActive(true);
        cText.enabled = false;
    }
}
