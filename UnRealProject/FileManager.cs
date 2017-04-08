using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UnRealProject
{
    public class FileManager
    {
        string _filename;
        List<Student> _students;

        public FileManager(List<Student> students, string filename)
        {
            _filename = filename;
            _students = students;
        }

        public void Save()
        {
            string extension = Path.GetExtension(_filename);

            switch (extension)
            {
                case ".txt":
                    SaveInTxt(_filename);
                    break;
                case ".xml":
                    SaveInXML(_filename);
                    break;
                case ".bin":
                    SaveInBin(_filename);
                    break;
            }
        }

        public void SaveAll()
        {
            string extension = Path.GetExtension(_filename);

            string filename = _filename.Replace(extension, "");

            //может добавить проверку если файл не существует
            SaveInTxt(filename + ".txt");
            SaveInXML(filename + ".xml");
            SaveInBin(filename + ".bin");
        }

        public void Load()
        {
            var extension = Path.GetExtension(_filename);

            switch (extension)
            {
                case ".txt":
                    LoadFromText();
                    break;
                case ".xml":
                    LoadFromXML();
                    break;
                case ".bin":
                    LoadFromBin();
                    break;
            }
        }

        private void SaveInTxt(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var stud in _students)
                {
                    string text = string.Format("{0};{1};{2};{3};{4};{5};", stud.Family, stud.Name, stud.Otch, stud.Course, stud.Faculty, stud.Group);

                    if (stud.Dict.ContainsKey("Математика"))
                    {
                        text += stud.Dict["Математика"] + ";";
                    }
                    else
                    {
                        text += "0;";
                    }

                    if (stud.Dict.ContainsKey("ООП"))
                    {
                        text += stud.Dict["ООП"] + ";";
                    }
                    else
                    {
                        text += "0;";
                    }

                    if (stud.Dict.ContainsKey("Физика"))
                    {
                        text += stud.Dict["Физика"] + ";";
                    }
                    else
                    {
                        text += "0;";
                    }

                    sw.WriteLine(text);
                }
            }
        }

        private void LoadFromText()
        {
            using (StreamReader sr = new StreamReader(_filename))
            {
                while (sr.Peek() != -1)
                {
                    var line = sr.ReadLine().Split(new char[] { ';' });

                    Student stud = new Student();
                    stud.Family = line[0];
                    stud.Name = line[1];
                    stud.Otch = line[2];
                    stud.Course = line[3];
                    stud.Faculty = line[4];
                    stud.Group = int.Parse(line[5]);
                    stud.Dict.Add("Математика", int.Parse(line[6]));
                    stud.Dict.Add("ООП", int.Parse(line[7]));
                    stud.Dict.Add("Физика", int.Parse(line[8]));

                    _students.Add(stud);
                }
            }
        }

        private void SaveInXML(string filename)
        {
            XmlDocument xDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xDoc.DocumentElement;
            xDoc.InsertBefore(xmlDeclaration, root);

            XmlElement elStudents = xDoc.CreateElement("students");
            xDoc.AppendChild(elStudents);

            foreach (var item in _students)
            {
                XmlElement elStudent = xDoc.CreateElement("student");
                elStudents.AppendChild(elStudent);

                var famEl = xDoc.CreateElement("family");
                famEl.InnerText = item.Family;
                elStudent.AppendChild(famEl);

                var nameEl = xDoc.CreateElement("name");
                nameEl.InnerText = item.Name;
                elStudent.AppendChild(nameEl);

                var otchEl = xDoc.CreateElement("otch");
                otchEl.InnerText = item.Otch;
                elStudent.AppendChild(otchEl);

                var courseEl = xDoc.CreateElement("course");
                courseEl.InnerText = item.Course;
                elStudent.AppendChild(courseEl);

                var facultyEl = xDoc.CreateElement("faculty");
                facultyEl.InnerText = item.Faculty;
                elStudent.AppendChild(facultyEl);

                var groupEl = xDoc.CreateElement("group");
                groupEl.InnerText = item.Group.ToString();
                elStudent.AppendChild(groupEl);

                var ratingsEl = xDoc.CreateElement("ratings");
                elStudent.AppendChild(ratingsEl);

                var mathEl = xDoc.CreateElement("math");
                if (item.Dict.ContainsKey("Математика"))
                {
                    mathEl.InnerText = item.Dict["Математика"].ToString();
                }
                else
                {
                    mathEl.InnerText = "0";
                }
                ratingsEl.AppendChild(mathEl);

                var oopEl = xDoc.CreateElement("oop");
                if (item.Dict.ContainsKey("ООП"))
                {
                    oopEl.InnerText = item.Dict["ООП"].ToString();
                }
                else
                {
                    oopEl.InnerText = "0";
                }
                ratingsEl.AppendChild(oopEl);

                var phisEl = xDoc.CreateElement("phis");
                if (item.Dict.ContainsKey("Физика"))
                {
                    phisEl.InnerText = item.Dict["Физика"].ToString();
                }
                else
                {
                    phisEl.InnerText = "0";
                }
                ratingsEl.AppendChild(phisEl);
            }

            xDoc.Save(filename);
        }

        private void LoadFromXML()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_filename);

            var studentsEl = xDoc["students"];

            foreach (XmlNode studEl in studentsEl.ChildNodes)
            {
                Student stud = new Student();

                stud.Family = studEl["family"].InnerText;
                stud.Name = studEl["name"].InnerText;
                stud.Otch = studEl["otch"].InnerText;
                stud.Course = studEl["course"].InnerText;
                stud.Faculty = studEl["faculty"].InnerText;
                stud.Group = int.Parse(studEl["group"].InnerText);

                var ratingsEl = studEl["ratings"];

                stud.Dict.Add("Математика", int.Parse(ratingsEl.ChildNodes[0].InnerText));
                stud.Dict.Add("ООП", int.Parse(ratingsEl.ChildNodes[1].InnerText));
                stud.Dict.Add("Физика", int.Parse(ratingsEl.ChildNodes[2].InnerText));

                _students.Add(stud);
            }
        }

        private void SaveInBin(string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate)))
            {
                foreach (var stud in _students)
                {
                    writer.Write(stud.Family);
                    writer.Write(stud.Name);
                    writer.Write(stud.Otch);
                    writer.Write(stud.Course);
                    writer.Write(stud.Faculty);
                    writer.Write(stud.Group);

                    if (stud.Dict.ContainsKey("Математика"))
                    {
                        writer.Write(stud.Dict["Математика"]);
                    }
                    else
                    {
                        writer.Write(0);
                    }

                    if (stud.Dict.ContainsKey("ООП"))
                    {
                        writer.Write(stud.Dict["ООП"]);
                    }
                    else
                    {
                        writer.Write(0);
                    }

                    if (stud.Dict.ContainsKey("Физика"))
                    {
                        writer.Write(stud.Dict["Физика"]);
                    }
                    else
                    {
                        writer.Write(0);
                    }
                }
            }
        }

        private void LoadFromBin()
        {
            using (BinaryReader reader = new BinaryReader(File.Open(_filename, FileMode.Open)))
            {
                // пока не достигнут конец файла
                // считываем каждое значение из файла
                while (reader.PeekChar() > -1)
                {
                    Student stud = new Student();

                    stud.Family = reader.ReadString();
                    stud.Name = reader.ReadString();
                    stud.Otch = reader.ReadString();
                    stud.Course = reader.ReadString();
                    stud.Faculty = reader.ReadString();
                    stud.Group = reader.ReadInt32();

                    stud.Dict.Add("Математика", reader.ReadInt32());
                    stud.Dict.Add("ООП", reader.ReadInt32());
                    stud.Dict.Add("Физика", reader.ReadInt32());

                    _students.Add(stud);
                }
            }
        }
    }
}
