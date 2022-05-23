using System;
using System.Collections.Generic;
using System.IO;

namespace Tesseract
{
    enum command
    {
        ADD,
        DEL
    }
    enum recordType
    {
        PATIENT,
        EXAM
    }
 
    class Patient
    {
        private string _name;
        private string _id;
        private HashSet<int> _exams;
        public Patient(string name, string id)
        {
            _name = name;
            _id = id;
            _exams = new HashSet<int>();
        }
        public string GetName()
        {
            return _name;
        }
        public void SetName(string name)
        {
            _name = name;
        }

        public string GetId()
        {
            return _id;
        }
        public void SetId(string id)
        {
            _id = id;
        }
        /*
         * Add new exam
         * return true if success
         * return false if exam already exists
         */
        public Boolean AddExam(int id)
        {
            var before = _exams.Count;
            _exams.Add(id);
            var after = _exams.Count;
            return after > before;
        }
        public void RemoveExam(int id)
        {
            _exams.Remove(id);
        }
        public int GetExamCount()
        {
            return _exams.Count;
        }
    }

    class Program
    {
        static string input = @"..\..\..\files\Input.txt";
        static string output = @"..\..\..\files\Output.txt";
        static Dictionary<string, Patient> records = new Dictionary<string, Patient>();

        static void Main(string[] args)
        {
            ReadFile();
            WriteFile();
        }

        private static void ProcessInput(string[] tokens)
        {
            string ParseName()
            {
                string[] nameArr = new string[tokens.Length - 3];
                for (int i = 3; i < tokens.Length; i++)
                {
                    nameArr[i - 3] = tokens[i];
                }
                return String.Join(' ', nameArr);
            }
            string cmd = tokens[0];
            string rtype = tokens[1];
            string id = tokens[2];
            string name = ParseName();

            command cr;
            if (Enum.TryParse(cmd, out cr))
            {
                switch (cr)
                {
                    case command.ADD:
                        recordType rrt;
                        if (Enum.TryParse(rtype, out rrt))
                        {
                            switch (rrt)
                            {
                                case recordType.PATIENT:
                                    if (!records.ContainsKey(id))
                                    {
                                        records.Add(id, new Patient(name, id));
                                    }
                                    break;
                                case recordType.EXAM:
                                    if (records.ContainsKey(id))
                                    {
                                        records[id].AddExam(Int32.Parse(name));
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case command.DEL:
                        if (Enum.TryParse(rtype, out rrt))
                        {
                            switch (rrt)
                            {
                                case recordType.PATIENT:
                                    records.Remove(id);
                                    break;
                                case recordType.EXAM:
                                    if (records.ContainsKey(id))
                                    {
                                        records[id].RemoveExam(Int32.Parse(name));
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ReadFile()
        {
            string[] lines = File.ReadAllLines(input);
            
            foreach (string line in lines)
            {
                ProcessInput(line.Split(' '));
            }
        }

        private static void WriteFile()
        {
            foreach (var record in records.Values)
            {
                Console.WriteLine("Name: " + record.GetName() + ", ID: " + record.GetId() + ", Exam Count: "  + record.GetExamCount());
            }
        }
    }
}
