using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NumericTest : MonoBehaviour
{
    [SerializeField] private GameObject startObject;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject finishObject;
    [SerializeField] private Button finishButton;
    [SerializeField] private Text resText;
    [SerializeField] private Text timeText;
    [SerializeField] private TimeCounter timer;
    [SerializeField] private NumericQuestion[] questions;

    [Header("Fields")] [SerializeField] private GameObject source;
    [SerializeField] private Text questionAsset;
    [SerializeField] private AnswerButton[] answerButtons;

    private int rightAnswers;
    private int questionCounter;

    private List<NumericQuestion> currentTestAssets = new();

    private void Awake()
    {
        foreach (AnswerButton answerButton in answerButtons)
        {
            answerButton.InitAnswer();
        }

        timer.SetCallback(OnTestFinished);

        startButton.onClick.AddListener(StartTest);
        finishButton.onClick.AddListener(FinishTest);
    }

    private void OnEnable()
    {
        startObject.SetActive(true);
        finishObject.SetActive(false);
        source.SetActive(false);
    }

    private void OnDisable()
    {
        timer.Deactivate();
    }

    private void StartTest()
    {
        startObject.SetActive(false);
        timer.Refresh();
        timer.Activate();

        rightAnswers = 0;
        FormTestAssets(10);
        questionCounter = 0;
        source.SetActive(true);
        InitQuestion();
    }

    private void FormTestAssets(int number)
    {
        currentTestAssets.Clear();
        NumericQuestion temp;
        for (int i = 0; i < number; i++)
        {
            do
            {
                temp = questions[Random.Range(0, questions.Length)];
            } 
            while (currentTestAssets.Contains(temp));
            currentTestAssets.Add(temp);
        }
    }

private void OnAnswerPicked(bool right)
    {
        foreach (var but in answerButtons)
        {
            but.ResetAnswer();
        }
        
        if (right)
        {
            rightAnswers++;
        }
        
        currentTestAssets[questionCounter].HideQuestion();
        
        questionCounter++;
        if (questionCounter >= currentTestAssets.Count)
        {
            OnTestFinished();
        }
        else
        {
            InitQuestion();
        }
    }

    private void InitQuestion()
    {
        questionAsset.text = "";
        questionAsset.DOText(currentTestAssets[questionCounter].GetQuestion(), 1f)
            .OnComplete(() =>
            {
                Shuffle(answerButtons);
                currentTestAssets[questionCounter].SetView(out string right, out string ans1, out string ans2);
                answerButtons[0].SetAnswer(right, () => OnAnswerPicked(true));
                answerButtons[1].SetAnswer(ans1, () => OnAnswerPicked(false));
                answerButtons[2].SetAnswer(ans2, () => OnAnswerPicked(false));
            });
    }

    private void OnTestFinished()
    {
        timer.Deactivate();
        foreach (var but in answerButtons)
        {
            but.ResetAnswer();
        }
        
        source.SetActive(false);
        
        resText.text = $"{rightAnswers}/{currentTestAssets.Count}";
        timeText.text = timer.GetTimeCount() + "s";
        finishObject.SetActive(true);
    }

    private void FinishTest()
    {
        finishObject.SetActive(false);
        startObject.SetActive(true);
    }
    
    private void Shuffle<T> (T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = Random.Range(0, n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}
