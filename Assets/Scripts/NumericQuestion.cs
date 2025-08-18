using System;
using UnityEngine;

public abstract class NumericQuestion : MonoBehaviour
{
    [SerializeField] protected GameObject ui;
    [SerializeField] protected string question;

    public abstract void SetView(out string rightAns, out string wrongAns1, out string wrongAns2);

    public string GetQuestion()
    {
        return question;
    }

    public void HideQuestion()
    {
        ui.SetActive(false);
    }
}
