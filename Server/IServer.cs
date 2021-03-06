﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IServer
    {
        Tuple<string, int> register(string eMail, string password, string medicalTraining, string firstName, string lastName);

        Tuple<string, int> login(string eMail, string password);

        string restorePassword(string eMail);

        string AnswerAQuestion(int userUniqueInt, int questionID, bool isNormal, int normalityCertainty, List<string> diagnoses, List<int> diagnosisCertainties);

        Tuple<string, Question> getAutoGeneratedQuesstion(int userUniqueInt, string subject, string topic);

        Tuple<string, List<Question>> getAutoGeneratedTest(int userUniqueInt, string subject, string topic, int numOfQuestions, bool answerEveryTime);
    }
}
