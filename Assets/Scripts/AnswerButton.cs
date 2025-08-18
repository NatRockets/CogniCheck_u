using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    private Button target;
    private Text lavel;

    public void InitAnswer()
    {
        target = GetComponent<Button>();
        lavel = transform.GetChild(0).GetComponent<Text>();
    }
    
    public void ResetAnswer()
    {
        target.onClick.RemoveAllListeners();
        target.interactable = false;
        lavel.text = "";
    }

    public void SetAnswer(string labelT, Action callback)
    {
        target.onClick.AddListener(() => callback());
        lavel.DOText(labelT, 1f)
            .OnComplete(() => target.interactable = true);
    }
}
