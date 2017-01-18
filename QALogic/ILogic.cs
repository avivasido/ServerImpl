﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QALogic
{
    public interface ILogic
    {
        Question getAutoGeneratedQuestion(string subject, string topic, bool subjectProvided, bool topicProvided, int userLvl);

        Tuple<string, List<Question>> getAutoGeneratedTest(string subject, string topic, bool subjectProvided, bool topicProvided, int userLvl, int numOfQuestions);

        string answerAQuestion(string eMail, Question q, UserLevel userLVL, bool isNormal, int normalityCertainty, List<string> diagnoses, List<int> diagnosisCertainties);
    }
}
