using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyOrganizer
{
    internal interface IRepository
    {
        List<string> TimeTable();
        string SubjectInfo(string subject);
        float Grade();
        void AddSubject(string subject,string professor, DateTime midterm1, DateTime midterm2, DateTime final, string classroom, string time, int m1p, int m2p, int fp, int partic, int attandance, int proj, float m1g, float m2g, float fg, float participg, float atteng, float projg);
        void UpdateGrade(string subject, int m1, int m2, int f, int p, int a, int proj);

    }
}
