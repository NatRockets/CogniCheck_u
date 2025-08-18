using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class TestAsset
{
    public string text;
    public List<TextQuestion> questions;
}

[Serializable]
public class TextQuestion
{
    public string question;
    public List<string> answers;//1st answer is right
}
public class StringProvider : MonoBehaviour
{
    [SerializeField] private TestAsset[] textVerbalAssets;

    public List<TestAsset> GetVerbalTestAssets(int number)
    {
        List<TestAsset> assets = new List<TestAsset>();
        TestAsset temp;
        for (int i = 0; i < number; i++)
        {
            do
            {
                temp = textVerbalAssets[Random.Range(0, textVerbalAssets.Length)];
            } 
            while (assets.Contains(temp));
            assets.Add(temp);
        }
        return assets;
    }
}
