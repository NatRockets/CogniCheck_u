using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TableQuestion : NumericQuestion
{
    [Header("UI")]
    [SerializeField] private Text[] nameFields;
    [SerializeField] private Text[] date1Fields;
    [SerializeField] private Text[] date2Fields;

    [Header("Params")]
    [SerializeField] private string[] names;

    [SerializeField] private bool termNotAge;
    
    private DateTime startDate = new DateTime(1985, 1, 1);
    private DateTime endDate = new DateTime(2000, 1, 1);
    private int daysMin = 6570;
    private int daysMax = 9125;

    private string CalculateTermAnswer(List<DateTime> dates)
    {
        int minIndex = 0;

        DateTime minDate = dates[0];
        for (int i = 1; i < dates.Count; i++)
        {
            if (dates[i] < minDate)
            {
                minIndex = i;
                minDate = dates[i];
            }
        }
        
        return names[minIndex];
    }

    private string CalculateAgeAnswer(List<DateTime> dates1, List<DateTime> dates2)
    {
        int maxIndex = 0;

        int maxDays = 0;
        int currentDays;
        for (int i = 1; i < dates1.Count; i++)
        {
            currentDays = (int)(dates2[i] - dates1[i]).TotalDays;
            if (currentDays > maxDays)
            {
                maxIndex = i;
                maxDays = currentDays;
            }
        }
        
        return names[maxIndex];
    }
    
    private DateTime RandomDay()
    {
        int range = (endDate - startDate).Days;           
        return startDate.AddDays(Random.Range(0, range));
    }
    
    public override void SetView(out string rightAns, out string wrongAns1, out string wrongAns2)
    {
        List<DateTime> dates1 = new List<DateTime>();
        List<DateTime> dates2 = new List<DateTime>();
        
        VerbalTest.Shuffle(names);

        for (int i = 0; i < nameFields.Length; i++)
        {
            nameFields[i].text = names[i];
            
            dates1.Add(RandomDay());
            dates2.Add(dates1[i].AddDays(Random.Range(daysMin, daysMax)));
            
            date1Fields[i].text = dates1[i].ToString("yyyy.MM.dd");
            date2Fields[i].text = dates2[i].ToString("yyyy.MM.dd");
        }
        
        rightAns = termNotAge ? CalculateTermAnswer(dates2) : CalculateAgeAnswer(dates1, dates2);
        do
        {
            wrongAns1 = names[Random.Range(0, names.Length)];
            wrongAns2 = names[Random.Range(0, names.Length)];
        } while (rightAns == wrongAns1 || wrongAns1 == wrongAns2 || rightAns == wrongAns2);
        ui.SetActive(true);
    }
}
