using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnRealProject
{
    
    public class Student
    {
        //поля
        private string _family;
        private string _name;
        private string _otch;

        private string _course;
        private string _faculty;
        private int _group;

        //словарь предмет - оценка
        private Dictionary<string, int> _dict = new Dictionary<string, int>();

        //свойства
        public string Family
        {
            get
            {
                return _family;
            }
            set
            {
                if (value != "")
                    _family = value;
            }
        }

        public string Course
        {
            get
            {
                return _course;
            }
            set
            {
                _course = value;
            }
        }

        public string Faculty
        {
            get
            {
                return _faculty;
            }

            set
            {
                _faculty = value;
            }
        }

        public int Group
        {
            get
            {
                return _group;
            }

            set
            {
                _group = value;
            }
        }

        public Dictionary<string, int> Dict
        {
            get
            {
                return _dict;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string Otch
        {
            get
            {
                return _otch;
            }

            set
            {
                _otch = value;
            }
        }

        //конструкторы
        public Student(string name, string fam, string otch)
        {
            _family = fam;
            Name = name;
            Otch = otch;
            Group = 1;
        }

        public Student()
        {
            Group = 1;
        }
        //методы
    }
}
