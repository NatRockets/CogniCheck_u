using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DIagramQuestion : NumericQuestion
{
    [Header("UI")]
    [SerializeField] private Text[] paramTexts;
    [SerializeField] private Image[] pillars;

    [Header("Params")]
    [SerializeField] private float paramValueMax;
    [SerializeField] private float paramValueMin;
    [SerializeField] private float delta;

    [SerializeField] private bool valueNotIncrease;

    private float CalculateAverageIncrease(List<float> values)
    {
        float sum = 0;
        for (int i = 1; i < values.Count; i++)
        {
            sum += (values[i] - values[i - 1]);
        }
        return sum / values.Count;
    }

    private float CalculateAverageValue(List<float> values)
    {
        float sum = 0;
        for (int i = 1; i < values.Count; i++)
        {
            sum += (values[i] + values[i - 1]) / 2;
        }
        return sum / values[0];
    }
    
    public override void SetView(out string rightAns, out string wrongAns1, out string wrongAns2)
    {
        List<float> values = new List<float>();
        values.Add(Random.Range(paramValueMin, paramValueMax));
        paramTexts[0].text = values[0].ToString("0");
        pillars[0].fillAmount = valueNotIncrease ? 1f : 0.5f;
        for (int i = 1; i < paramTexts.Length; i++)
        {
            values.Add(values[i - 1] + Random.Range(delta / 2f, delta));
            paramTexts[i].text = values[i].ToString("0");
            pillars[i].fillAmount = pillars[i - 1].fillAmount * values[i] / values[i - 1];
        }
        
        float val = valueNotIncrease ? CalculateAverageValue(values) : CalculateAverageIncrease(values);
        string end = valueNotIncrease ? " years" : " units";
        rightAns = val.ToString("0.0") + end;
        
        do
        {
            wrongAns1 = (val + Random.Range(-val / 2f, val / 2f)).ToString("0.0") + end;
            wrongAns2 = (val + Random.Range(-val / 2f, val / 2f)).ToString("0.0") + end;
        } while (rightAns == wrongAns1 || wrongAns1 == wrongAns2 || rightAns == wrongAns2);
        ui.SetActive(true);
    }
}
