using UnityEngine;
using UnityEngine.UI;

public class SimpleQuestion : NumericQuestion
{
    [SerializeField] private Text description;
    [SerializeField] private string textDesc;
    [SerializeField] private string[] answers;
    
    public override void SetView(out string rightAns, out string wrongAns1, out string wrongAns2)
    {
        description.text = textDesc;
        rightAns = answers[0];
        wrongAns1 = answers[1];
        wrongAns2 = answers[2];
        
        ui.SetActive(true);
    }
}
