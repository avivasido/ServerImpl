using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Constants;
using System.Threading;
using QALogic;

namespace Server
{
    public class ServerImpl : IServer
    {
        private const string GENERAL_INPUT_ERROR = "There is a null, the string \"null\" or an empty string as an input.";
        private const string INVALID_EMAIL = "Invalid eMail address.";
        private const string ILLEGAL_PASSWORD = "Illegal password. Password must be 5 to 15 characters long and consist of only letters and numbers.";
        private const string INVALID_TEMPORAL_PASSWORD = "Bad cookie. Could not identify user.";
        private const string NOT_LOGGED_IN = "User is not logged in.";
        private const string EMAIL_IN_USE = "This eMail address is already in use.";
        private const int USERS_CACHE_LIMIT = 1000;
        private const int HOURS_TO_LOGOUT = 1;
        private const int MILLISECONDS_TO_SLEEP = HOURS_TO_LOGOUT * 60 * 60 * 1000;

        private List<User> _usersCache; // each action will move the user to the last position of the cache, removing old users from the beginning
        private Dictionary<User, DateTime> _loggedUsers;
        private Random _random;
        private int _userUniqueInt;
        private int _questionID;
        private int _groupID;
        private int _testID;

        private ILogic _logic;
        private MedTrainDBContext _db;

        public ServerImpl()
        {
            _usersCache = new List<User>();
            _loggedUsers = new Dictionary<User, DateTime>();
            _random = new Random();
            _db = new MedTrainDBContext();
            _logic = new LogicImpl(_db);
            _userUniqueInt = 100000;
            _questionID = 1;
            _groupID = 1;
            _testID = 1;
            Thread oThread = new Thread(new ThreadStart(removeNonActiveUsers));
            oThread.Start();
            //_db.addUser(new User { UserId = "a@a.a", userPassword = "p", userMedicalTraining = "mt", userFirstName = "fn", userLastName = "ln" });
        }

        private void removeNonActiveUsers()
        {
            Thread.Sleep(MILLISECONDS_TO_SLEEP);
            List<User> toBeRemoved = new List<User>();
            foreach (User u in _loggedUsers.Keys)
            {
                if (DateTime.Now.Subtract(_loggedUsers[u]).Hours >= HOURS_TO_LOGOUT)
                {
                    toBeRemoved.Add(u);
                }
            }
            foreach (User u in toBeRemoved)
            {
                _loggedUsers.Remove(u);
            }
        }

        private List<String> getEmails()
        {
            List<string> eMails = new List<string>();
            foreach (User u in _loggedUsers.Keys)
            {
                eMails.Add(u.UserId);
            }
            return eMails;
        }

        public Tuple<string, int> register(string eMail, string password, string medicalTraining, string firstName, string lastName)
        {
            // check for illegal input values
            if (!InputTester.isValidInput(new List<string>() { medicalTraining, firstName, lastName }))
            {
                return new Tuple<string,int>(GENERAL_INPUT_ERROR, -1);
            }
            if (!InputTester.isLegalEmail(eMail))
            {
                return new Tuple<string, int>(INVALID_EMAIL, -1);
            }
            if (!InputTester.isLegalPassword(password))
            {
                return new Tuple<string, int>(ILLEGAL_PASSWORD, -1);
            }
            // search user in cache
            List<User> matches = _usersCache.Where(u => u.UserId == eMail).ToList();
            if (matches.Count != 0)
            {
                return new Tuple<string, int>(EMAIL_IN_USE, -1);
            }
            // search DB
            if (_db.getUser(eMail) != null)
            {
                return new Tuple<string, int>(EMAIL_IN_USE, -1);
            }
            // if DB contains user with that eMail return error message
            User user = new User { UserId = eMail, userPassword = password, userMedicalTraining = medicalTraining, userFirstName = firstName, userLastName = lastName, uniqueInt = _userUniqueInt++ };
            // add to DB
            _db.addUser(user);
            if (_db.getUser(eMail) == null)
            {
                return new Tuple<string, int>("Error - could not register user to the system.", -1);
            }
            if (_usersCache.Count == USERS_CACHE_LIMIT)
            {
                _usersCache.RemoveAt(0);
            }
            // add to chache
            _usersCache.Add(user);
            return new Tuple<string, int>(Replies.SUCCESS, _userUniqueInt - 1);
        }

        public Tuple<string, int> login(string eMail, string password)
        {
            // check for illegal input
            if (!InputTester.isLegalEmail(eMail))
            {
                return new Tuple<string,int>(INVALID_EMAIL, -1);
            }
            if (!InputTester.isLegalPassword(password))
            {
                return new Tuple<string, int>(ILLEGAL_PASSWORD, -1);
            }
            // search user in cache
            List<User> matches = _usersCache.Where(u => u.UserId == eMail).ToList();
            if (matches.Count == 1)
            {
                return verifyLogin(matches.ElementAt(0), password);
            }
            // search DB
            User user = _db.getUser(eMail);
            if (user == null)
            {
                return new Tuple<string, int>("Wrong eMail or password.", -1);
            }
            // if found add to cache and return relevant message as shown above
            updateUserLastActionTime(user);
            return verifyLogin(user, password);
        }

        private Tuple<string, int> verifyLogin(User u, string password)
        {
            if (!u.userPassword.Equals(password))
            {
                return new Tuple<string,int>("Wrong password", -1);
            }
            // place user at the end of the cache (to be the last one to be removed)
            _usersCache.Remove(u);
            _usersCache.Add(u);
            // addd user to logged users list
            updateUserLastActionTime(u);
            return new Tuple<string,int>(Replies.SUCCESS, u.uniqueInt);
        }

        private string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public string restorePassword(string eMail)
        {
            // check for illegal input values
            if (!InputTester.isLegalEmail(eMail))
            {
                return INVALID_EMAIL;
            }
            // search user in cache
            List<User> matches = _usersCache.Where(u => u.UserId == eMail).ToList();
            User user = null;
            if (matches.Count == 1)
            {
                // place user at the end of the cache (to be the last one to be removed)
                _usersCache.Remove(matches.ElementAt(0));
                _usersCache.Add(matches.ElementAt(0));
                user = matches.ElementAt(0);
            }
            // search user in DB
            if (user == null)
            {
                user = _db.getUser(eMail);
            }
            // if doesn't exist return error message
            if (user == null)
            {
                return "eMail address does not exist in the system.";
            }
            // send eMail
            StringBuilder sb = new StringBuilder();
            sb.Append("Hello " + user.userFirstName + " " + user.userLastName + "," + Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("Your password for our system is: " + user.userPassword + Environment.NewLine);
            EmailSender.sendMail(eMail, "Medical Training System Password Restoration", sb.ToString());
            return Replies.SUCCESS;
        }

        public string AnswerAQuestion(int userUniqueInt, int questionID, bool isNormal, int normalityCertainty, List<string> diagnoses, List<int> diagnosisCertainties)
        {
            if (diagnoses.Count != diagnosisCertainties.Count)
            {
                return "Error - cannot have a different number of diagnoses and diagnosis certainties.";
            }
            // check for illegal input values
            List<string> input = new List<string>();
            foreach (string s in diagnoses)
            {
                input.Add(s);
            }
            if (!InputTester.isValidInput(input))
            {
                return GENERAL_INPUT_ERROR;
            }
            User user = null;
            List<User> matches = _usersCache.Where(u => u.uniqueInt.Equals(userUniqueInt)).ToList();
            if (matches.Count == 1)
            {
                user = matches.ElementAt(0);
            }
            else
            {
                user = _db.getUser(userUniqueInt);
            }
            // verify user is logged in
            if (!_loggedUsers.ContainsKey(user))
            {
                return NOT_LOGGED_IN;
            }
            // get data from DB
            Question q = _db.getQuestion(questionID);
            if (q == null)
            {
                return "Wrong data accepted. Incorrect question ID was recieved.";
            }
            UserLevel userLevel = _db.getUserLevel(user.UserId, q.subjectName, q.topicName);
            if (userLevel == null)
            {
                // return error
            }
            return _logic.answerAQuestion(user.UserId, q, userLevel, isNormal, normalityCertainty, diagnoses, diagnosisCertainties);
        }

        public Tuple<string, Question> getAutoGeneratedQuesstion(int userUniqueInt, string subject, string topic)
        {
            Tuple<string, List<Question>> questions = getAutoGeneratedTest(userUniqueInt, subject, topic, 1, true);
            if (!questions.Item1.Equals(Replies.SUCCESS))
            {
                return new Tuple<string, Question>(questions.Item1, null);
            }
            return new Tuple<string, Question>(questions.Item1, questions.Item2.ElementAt(0));
        }

        public Tuple<string, List<Question>> getAutoGeneratedTest(int userUniqueInt, string subject, string topic, int numOfQuestions, bool answerEveryTime)
        {
            bool subjectProvided = !subject.Equals("");
            bool topicProvided = !topic.Equals("");
            // check for illegal input values
            List<string> input = new List<string>();
            if (!subject.Equals(""))
            {
                input.Add(subject);
                if (topicProvided)
                {
                    input.Add(topic);
                }
            }
            else if (!topic.Equals(""))
            {
                return new Tuple<string, List<Question>>("Error  - cannot provide a topic without providing a subject.", null);
            }
            if (input.Count > 0 && !InputTester.isValidInput(input))
            {
                return new Tuple<string, List<Question>>(GENERAL_INPUT_ERROR, null);
            }
            // verify user is logged in
            User user = getUserByInt(userUniqueInt);
            updateUserLastActionTime(user);
            if (!_loggedUsers.ContainsKey(user))
            {
                return new Tuple<string, List<Question>>(GENERAL_INPUT_ERROR, null);
            }
            // if subject is not in DB return error
            if (!subject.Equals(""))
            {
                List<Subject> subjects = _db.getSubjects().Where(s => s.SubjectId.Equals(subject)).ToList();
                if (subjects.Count != 0)
                {
                    return new Tuple<string, List<Question>>("Error - provided subject is invalid.", null);
                }
                if (!topic.Equals(""))
                {
                    List<Topic> topics = _db.getTopics(subject).Where(t => t.TopicId.Equals(topic)).ToList();
                    if (topics.Count != 0)
                    {
                        return new Tuple<string, List<Question>>("Error - provided topic is invalid.", null);
                    }
                }
            }
            // if topic is not in the subject's topic select a random matching topic
            int userLvl = _db.getUserLevel(user.UserId, subject, topic).level;
            return _logic.getAutoGeneratedTest(subject, topic, !subject.Equals(""), !topic.Equals(""), userLvl, numOfQuestions);
        }

        private User getUserByInt(int userUniqueInt)
        {
            List<User> matches = _usersCache.Where(u => u.uniqueInt.Equals(userUniqueInt)).ToList();
            if (matches.Count == 1)
            {
                return matches.ElementAt(0);
            }
            else
            {
                return _db.getUser(userUniqueInt);
            }
        }

        private void updateUserLastActionTime(User u)
        {
            _loggedUsers[u] = DateTime.Now;
            _usersCache.Remove(u);
            _usersCache.Add(u);
            if (_usersCache.Count == USERS_CACHE_LIMIT)
            {
                _usersCache.RemoveAt(0);
            }
        }
    }
}
