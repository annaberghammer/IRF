using IRF_09.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRF_09
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        Random rng = new Random(1234);

        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Windows\Temp\nép.csv");
            BirthProbabilities = GetBirthProbability(@"C:\Windows\Temp\születés.csv");
            DeathProbabilities = GetDeathProbability(@"C:\Windows\Temp\halál.csv");

            for (int i = 2005; i <= 2024; i++)
            {
                for (int j = 0; j < Population.Count; j++)
                {

                }

                int NbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive == true
                                  select x).Count();

                int NbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive == true
                                    select x).Count();
            }
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                var line = sr.ReadLine().Split(';');
                Person p = new Person();
                p.BirthYear = int.Parse(line[0]);
                p.Gender = (Gender)int.Parse(line[1]);
                p.NbrOfChildren = int.Parse(line[2]);
                population.Add(p);
            }

            return population;
        }

        public List<BirthProbability> GetBirthProbability(string csvpath)
        {
            List<BirthProbability> birthprobaility = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                var line = sr.ReadLine().Split(';');
                BirthProbability b = new BirthProbability();
                b.Age = int.Parse(line[0]);
                b.NbrOfChildren = int.Parse(line[1]);
                b.P = double.Parse(line[2]);
                birthprobaility.Add(b);
            }

            return birthprobaility;
        }

        public List<DeathProbability> GetDeathProbability(string csvpath)
        {
            List<DeathProbability> deathprobability = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                var line = sr.ReadLine().Split(';');
                DeathProbability d = new DeathProbability();
                d.Gender = (Gender)int.Parse(line[0]);
                d.Age = int.Parse(line[1]);
                d.P = double.Parse(line[2]);
                deathprobability.Add(d);
            }

            return deathprobability;
        }
    }
}
