using System;
using System.Collections.Generic;

[Serializable]
public class QuestionData
{
    public string questionText;

    public QuestionType questionType;

    public int duration;

    public ScoreType scoreMultiplier;

    public List<string> answers;

    public List<int> correctAnswers;

    public float minimumValue;
    public float maximumValue;
    public float correctValue;
}
