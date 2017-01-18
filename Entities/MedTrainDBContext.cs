using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class MedTrainDBContext : DbContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<Admin> Admins { get; set; }
        public IDbSet<Subject> Subjects { get; set; }
        public IDbSet<Topic> Topics { get; set; }
        public IDbSet<Question> Questions { get; set; }
        public IDbSet<Test> Tests { get; set; }
        public IDbSet<Group> Groups { get; set; }
        public IDbSet<Answer> Answers { get; set; }
        public IDbSet<UserLevel> UsersLevels { get; set; }
        public IDbSet<UserGroupTest> UsersGroupsTests { get; set; }
        public IDbSet<GroupTestAnswer> GroupsTestsQuestionsAnswers { get; set; }

        public MedTrainDBContext()
            : base("MedTrainDB")
        {
            //Database.SetInitializer<MedTrainDBContext>(new CreateDatabaseIfNotExists<MedTrainDBContext>());
            Database.SetInitializer<MedTrainDBContext>(new DropCreateDatabaseIfModelChanges<MedTrainDBContext>());
            //Database.SetInitializer<MedTrainDBContext>(new DropCreateDatabaseAlways<MedTrainDBContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public void addUser(User u)
        {
            if (Users.Find(new object[1] { u.UserId }) != null)
            {
                return;
            }
            Users.Add(u);
            SaveChanges();
        }

        public User getUser(string eMail)
        {
            return Users.Find(eMail);
        }

        public User getUser(int uniqueInt)
        {
            var query = from u in Users
                        where (u.uniqueInt == uniqueInt)
                        select u;
            if (query.Count() == 0)
            {
                return null;
            }
            return query.ToList().ElementAt(0);
        }

        public bool addAdmin(string eMail)
        {
            if (Admins.Find(new object[1] { eMail }) != null)
            {
                return false;
            }
            Admins.Add(new Admin { AdminId = eMail });
            SaveChanges();
            return true;
        }

        public Admin getAdmin(string eMail)
        {
            return Admins.Find(new object[1] { eMail });
        }

        public Question getQuestion(int id)
        {
            return Questions.Find(new object[1] { id });
        }

        public List<Question> getQuestions(string subject, string topic)
        {
            var query = from q in Questions
                        where (q.subjectName.Equals(subject) && q.topicName.Equals(topic))
                        select q;
            return query.ToList();
        }

        public UserLevel getUserLevel(string eMail, string subject, string topic)
        {
            return UsersLevels.Find(eMail, subject, topic);
        }

        public bool addAnswer(Answer a)
        {
            if (Answers.Find(a.questionId, a.userId, a.timeAdded) != null)
            {
                return false;
            }
            Answers.Add(a);
            SaveChanges();
            return true;
        }

        public List<Topic> getTopics(string subject)
        {
            var query = from t in Topics
                        where t.subjectName.Equals(subject)
                        select t;
            return query.ToList();
        }

        public List<Subject> getSubjects()
        {
            var query = from s in Subjects
                        select s;
            return query.ToList();
        }
    }
}
