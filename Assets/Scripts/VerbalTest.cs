using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class VerbalTest : MonoBehaviour
{
    [SerializeField] private GameObject startObject;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject finishObject;
    [SerializeField] private Button finishButton;
    [SerializeField] private Text resText;
    [SerializeField] private Text timeText;
    [SerializeField] private TimeCounter timer;
    [SerializeField] private StringProvider stringProvider;
    
    [Header("Fields")]
    [SerializeField] private GameObject source;
    [SerializeField] private Text textAsset;
    [SerializeField] private Text questionAsset;
    [SerializeField] private AnswerButton[] answerButtons;

    private int rightAnswers;
    private List<TestAsset> currentTestAssets;
    private int textCounter;
    private int questionCounter;
    
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
        currentTestAssets = stringProvider.GetVerbalTestAssets(Random.Range(6, 9));
        textCounter = questionCounter = 0;
        source.SetActive(true);
        InitQuestion();
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
        
        questionCounter++;
        if (questionCounter >= currentTestAssets[textCounter].questions.Count)
        {
            textCounter++;
            questionCounter = 0;
            if (textCounter >= currentTestAssets.Count)
            {
                OnTestFinished();
                return;
            }
            InitQuestion(false);
        }
        else
        {
            InitQuestion();
        }
    }

    private void InitQuestion(bool animateText = true)
    {
        if (animateText)
        {
            textAsset.text = "";
            textAsset.DOText(currentTestAssets[textCounter].text, 2f);
        }

        questionAsset.text = "";
        questionAsset.DOText(currentTestAssets[textCounter].questions[questionCounter].question, 1f)
            .OnComplete(() =>
            {
                Shuffle(answerButtons);
                answerButtons[0].SetAnswer(currentTestAssets[textCounter].questions[questionCounter].answers[0], 
                    () => OnAnswerPicked(true));
                answerButtons[1].SetAnswer(currentTestAssets[textCounter].questions[questionCounter].answers[1], 
                    () => OnAnswerPicked(false));
                answerButtons[2].SetAnswer(currentTestAssets[textCounter].questions[questionCounter].answers[2], 
                    () => OnAnswerPicked(false));
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
        
        resText.text = $"{rightAnswers}/{currentTestAssets.Count * 2}";
        timeText.text = timer.GetTimeCount() + "s";
        finishObject.SetActive(true);
    }

    private void FinishTest()
    {
        finishObject.SetActive(false);
        startObject.SetActive(true);
    }
    
    public static void Shuffle<T> (T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = Random.Range(0, n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}
