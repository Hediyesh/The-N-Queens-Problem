using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projeNvazir
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //make the list
                List<NVazir> nVazirs = new List<NVazir>();
                List<int> indexFits = new List<int>();
                Random r = new Random();
                string exit = "n";
                do
                {
                    //Getting inputs
                    int n = 1;
                    Console.WriteLine("Press e for Standard and j for permutation");
                    string eOrj = Console.ReadLine();
                    Console.WriteLine("For the selection method Tornoment press t and for Roulette press r:");
                    string fEntekhabi = Console.ReadLine();
                    int k = 1;
                    if (fEntekhabi == "t")
                    {
                        Console.WriteLine("Please enter number of k for tornoment:");
                        k = Convert.ToInt32(Console.ReadLine());
                    }
                    Console.WriteLine("For the crossover method SinglePoint press t and for Consistent press y:");
                    string cEntekhabi = Console.ReadLine();
                    Console.WriteLine("For the mutation method Standard press e and for Exchange press t:");
                    string mEntekhabi = Console.ReadLine();
                    do
                    {
                        //The number of members in each generation is 500 and that is equal to 200.
                        //If the standard method is considered, collisions no longer matter.
                        List<int[]> halatha = new List<int[]>();
                        if (eOrj == "e")
                        {
                            for (int i = 0; i < 500; i++)
                            {
                                int[] halat = new int[200];
                                for (int j = 0; j < 200; j++)
                                {
                                    halat[j] = r.Next(0, 200);
                                }
                                halatha.Add(halat);
                            }
                        }
                        //If the method is permutation, we should not have row and column collisions.
                        else if (eOrj == "j")
                        {
                            for (int i = 0; i < 500; i++)
                            {
                                int[] halat = new int[200];
                                List<int> li = new List<int>();
                                int x = 0;
                                do
                                {
                                    x = r.Next(0, 200);
                                    if (li.Contains(x) == false)
                                    {
                                        li.Add(x);
                                    }
                                } while (li.Count < 200);
                                for (int l = 0; l < 200; l++)
                                {
                                    halat[l] = li[l];
                                }
                                halatha.Add(halat);
                            }
                        }
                        NVazir vazir = new NVazir(halatha, eOrj, fEntekhabi, cEntekhabi, mEntekhabi, k);
                        int index = vazir.Genetic();
                        Console.WriteLine("__________________________________");
                        Console.WriteLine("The best after 100 jenerations With the fitness: " + vazir.fitsJadid[index] + "\n" + "And the genotype : " + string.Join(",", vazir.halatJadid[index].Select(s => s.ToString()).ToArray()));
                        nVazirs.Add(vazir);
                        Console.WriteLine("__________________________________");
                        Console.WriteLine("Average of fitness:" + vazir.fitsJadid.Average());
                        indexFits.Add(index);
                        Console.WriteLine("__________________________________");
                        n++;
                    } while (n <= 10);
                    //After the loop above is complete, we will display the best of 10.
                    Console.WriteLine("Number of rounds:" + nVazirs.Count);
                    Console.WriteLine("**************************");
                    int best = nVazirs[0].fitsJadid[indexFits[0]];
                    double bestAvr = nVazirs[0].fitsJadid.Average();
                    for (int i = 0; i < nVazirs.Count; i++)
                    {
                        if (best <= nVazirs[i].fitsJadid[indexFits[i]])
                        {
                            best = nVazirs[i].fitsJadid[indexFits[i]];
                        }
                        if (bestAvr <= nVazirs[i].fitsJadid.Average())
                        {
                            bestAvr = nVazirs[i].fitsJadid.Average();
                        }
                    }
                    Console.WriteLine("The best fitness=>" + best + "\n" + "And the best average=>" + bestAvr);
                    Console.WriteLine("**************************");
                    nVazirs.Clear();
                    indexFits.Clear();
                    Console.WriteLine("Press y if you want to exit otherwise press n:");
                    exit = Console.ReadLine();
                    if (exit == "n")
                    {
                        Console.Clear();
                    }
                } while (exit == "n");
                Console.WriteLine("Exited.");
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
    //N-Queens class
    class NVazir
    {
        //Variable definition
        public List<int[]> halatha = new List<int[]>();
        public List<int[]> halatJadid = new List<int[]>();
        public int n = 200,tedadnasl = 100,ozvnasl = 500,k;
        public string eOrj, fEntekhabi, cEntekhabi, mEntekhabi;
        public List<int> fits = new List<int>();
        public List<int> fitsJadid = new List<int>();
        Random rand = new Random();
        //The following method is used to obtain fitness by calculating the number of collisions and returning its negative.
        public int Fitness(int[] halat)
        {
            int k = 0;
            int sumfit = 0;
            do
            {
                int hits = 0;
                for (int i = 0; i < n; i++)
                {
                    if (i != k)
                    {
                        if (halat[k] == halat[i] || halat[k] - halat[i] == k - i || halat[i] - halat[k] == k - i)
                        {
                            hits++;
                        }
                    }
                }
                sumfit += hits;
                k++;
            } while (k < n);
            return (-1) * sumfit;
        }
        //Standard mutation is performed for the standard state and for permutations in case of no collision.
        public void JaheshEstandard(int[] halat, string eOrj)
        {
            int index = rand.Next(0, n);
            int next = rand.Next(0, n);
            if (eOrj == "e")
            {
                halat[index] = next;
            }
            else
            {
                int hit = 0;
                for (int i = 0; i < halat.Length; i++)
                {
                    if (i != index && next == halat[i])
                    {
                        hit++;
                    }
                }
                if (hit == 0)
                {
                    halat[index] = next;
                }
            }

        }
        //The substitution mutation is performed for both cases and the two genes are swapped.
        public void JaheshTaviz(int[] halat)
        {
            int next1 = rand.Next(0, n);
            int next2 = rand.Next(0, n);
            int x = halat[next1];
            halat[next1] = halat[next2];
            halat[next2] = x;
        }
        //In single-point crossover, the interval is swapped for the standard mode, and for permutations, the number of collisions is checked before swapping.
        public void CrossOverTakNoghte(int[] halat1, int[] halat2, string eOrj)
        {
            int index = rand.Next(0, n);
            int[] hal1 = new int[halat1.Length];
            int[] hal2 = new int[halat2.Length];
            if (eOrj == "j")
            {
                for (int i = 0; i < halat1.Length; i++)
                {
                    if (i < index)
                    {
                        int x = halat1[i];
                        hal1[i] = halat2[i];
                        hal2[i] = x;
                    }
                    else
                    {
                        hal1[i] = halat1[i];
                        hal2[i] = halat2[i];
                    }
                }
                int hit = 0;
                for (int i = 0; i < halat1.Length; i++)
                {
                    for (int j = i + 1; j < halat1.Length; j++)
                    {
                        if (hal1[i] == hal1[j] || hal2[i] == hal2[j])
                        {
                            hit++;
                        }
                    }
                }
                if (hit == 0)
                {
                    for (int i = 0; i < halat2.Length; i++)
                    {
                        halat1[i] = hal1[i];
                        halat2[i] = hal2[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < index; i++)
                {
                    int x = halat1[i];
                    halat1[i] = halat2[i];
                    halat2[i] = x;
                }
            }
        }
        //Consistent crossover for the standard case occurs if the probability is less than or equal to Px.
        public void CrossOverYekvakht(int[] halat1, int[] halat2, double px)
        {
            for (int i = 0; i < n; i++)
            {
                double p = rand.NextDouble();
                if (p <= px)
                {
                    int x = halat1[i];
                    halat1[i] = halat2[i];
                    halat2[i] = x;
                }
            }
        }
        //Order-based consistent crossover for permutation mode
        public void CrossOverYekvakhtTartib(int[] halat1, int[] halat2, double px)
        {
            //First, a list of numbers that need to be replaced is made, then they are sorted and placed in order.
            List<int> t1 = new List<int>();
            List<int> t2 = new List<int>();
            int[] ehtemal = new int[halat1.Length];
            List<int> t1moratab = new List<int>();
            List<int> t2moratab = new List<int>();
            int n = 0;
            for (int i = 0; i < halat1.Length; i++)
            {
                double p = rand.NextDouble();
                if (p <= px)
                {
                    ehtemal[i] = 1;
                    t1.Add(halat1[i]);
                    t2.Add(halat2[i]);
                    n++;
                }
                else { ehtemal[i] = 0; }
            }
            for (int i = 0; i < halat1.Length; i++)
            {
                if (t1.Contains(halat2[i]))
                {
                    t1moratab.Add(t1.Where(w => w == halat2[i]).Single());
                }
                if (t2.Contains(halat1[i]))
                {
                    t2moratab.Add(t2.Where(w => w == halat1[i]).Single());
                }
            }
            int c = 0;
            for (int i = 0; i < halat1.Length; i++)
            {
                if (ehtemal[i] == 1)
                {
                    halat1[i] = t1moratab[c];
                    halat2[i] = t2moratab[c];
                    c++;
                }
            }
        }
        //A roulette wheel that initially receives a prioritized list and returns the selected index based on a random number.
        public int Roulette(List<KeyValuePair<int, double>> folaviat)
        {
            var val = rand.Next(0, (int)folaviat[499].Value);
            int key = 0;
            for (int k = 1; k < folaviat.Count; k++)
            {
                if (val >= folaviat[k - 1].Value && val <= folaviat[k].Value)
                {
                    key = folaviat[k].Key;
                    break;
                }
            }
            return key;
        }
        //A tournament where initially a certain number of members are selected, then the best one is selected.
        public int Tornoment(int k, List<int> fits)
        {
            List<KeyValuePair<int, int>> fs = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < k; i++)
            {
                int index = rand.Next(0, ozvnasl);
                KeyValuePair<int, int> f = new KeyValuePair<int, int>(index, fits[index]);
                fs.Add(f);
            }
            int max = fs[0].Value;
            int maxkey = fs[0].Key;
            for (int i = 1; i < k; i++)
            {
                if (fs[i].Value >= max)
                {
                    max = fs[i].Value;
                    maxkey = fs[i].Key;
                }
            }
            return maxkey;
        }
        //Genetic 
        public int Genetic()
        {
            Console.WriteLine("Algorithm began:");
            double Px = 0.5, Pm = 0.01;
            int t = 0;
            List<int[]> vazira = new List<int[]>(halatha);
            List<int> fitnessha = new List<int>(fits);
            do
            {
                //First, it enters the next generation and the probability of each fitness is calculated.
                t = t + 1;
                List<int[]> jadid = new List<int[]>();
                List<KeyValuePair<int, double>> frels = new List<KeyValuePair<int, double>>();
                int sum = 0;
                for (int i = 0; i < ozvnasl; i++)
                {
                    sum += fitnessha[i];
                }
                for (int i = 0; i < ozvnasl; i++)
                {
                    double d = fitnessha[i] / (double)sum;
                    KeyValuePair<int, double> f = new KeyValuePair<int, double>(i, d);
                    frels.Add(f);
                }
                //Sorted ascendingly
                for (int i = 0; i < ozvnasl; i++)
                {
                    double min1 = frels[i].Value;
                    int j = i;
                    for (int l = i + 1; l < ozvnasl; l++)
                    {
                        if (min1 >= frels[l].Value)
                        {
                            min1 = frels[l].Value;
                            j = l;
                        }
                    }
                    KeyValuePair<int, double> f1 =new KeyValuePair<int, double>(frels[i].Key, frels[i].Value);
                    frels[i] = frels[j];
                    frels[j] = f1;
                }
                //If the roulette wheel is selected
                if (fEntekhabi == "r")
                {
                    List<KeyValuePair<int, double>> folaviat = new List<KeyValuePair<int, double>>(frels);
                    for (int i = 0; i < folaviat.Count; i++)
                    {
                        if (i != 0)
                        {
                            folaviat[i] = new KeyValuePair<int, double>(folaviat[i].Key, (i + (i - 1)));
                        }
                        else
                        {
                            folaviat[i] = new KeyValuePair<int, double>(folaviat[i].Key, 1);
                        }
                    }
                    for (int i = 0; i < ozvnasl; i++)
                    {
                        var indexChosen = Roulette(folaviat);
                        var halat = vazira[indexChosen];
                        jadid.Add(halat);
                    }
                }
                //If the tournament is selected
                else
                {
                    for (int i = 0; i < ozvnasl; i++)
                    {
                        int max = Tornoment(k, fitnessha);
                        jadid.Add(vazira[max]);
                    }
                }
                //Erasing the old generation
                vazira.Clear();
                fitnessha.Clear();
                Random r = new Random();
                for (int i = 1; i < ozvnasl; i = i + 2)
                {
                    //Crossover based on selection
                    double u = r.NextDouble();
                    if (u <= Px)
                    {
                        //Standard
                        if (eOrj == "e")
                        {
                            if (cEntekhabi == "t")
                            {
                                CrossOverTakNoghte(jadid[i - 1], jadid[i], "e");
                            }
                            else
                            {
                                CrossOverYekvakht(jadid[i - 1], jadid[i], Px);
                            }
                        }
                        //Permutation
                        else
                        {
                            if (cEntekhabi == "t")
                            {
                                CrossOverTakNoghte(jadid[i - 1], jadid[i], "j");
                            }
                            else
                            {
                                CrossOverYekvakhtTartib(jadid[i - 1], jadid[i], Px);

                            }
                        }
                    }
                    //Mutation also occurs based on selection.
                    double u1 = r.NextDouble();
                    if (u1 <= Pm)
                    {
                        //Standard
                        if (eOrj == "e")
                        {
                            if (mEntekhabi == "e")
                            {
                                JaheshEstandard(jadid[i - 1], "e");
                                JaheshEstandard(jadid[i], "e");
                            }
                            else
                            {
                                JaheshTaviz(jadid[i - 1]);
                                JaheshTaviz(jadid[i]);
                            }
                        }
                        //Permutation
                        else
                        {
                            if (mEntekhabi == "e")
                            {
                                JaheshEstandard(jadid[i - 1], "j");
                                JaheshEstandard(jadid[i], "j");
                            }
                            else
                            {
                                JaheshTaviz(jadid[i - 1]);
                                JaheshTaviz(jadid[i]);
                            }
                        }
                    }
                    //Adding new members to the new generation
                    vazira.Add(jadid[i - 1]);
                    vazira.Add(jadid[i]);
                }
                //Calculating fitness for new members
                for (int i = 0; i < vazira.Count(); i++)
                {
                    fitnessha.Add(Fitness(vazira[i]));
                }
            } while (t <= tedadnasl);
            halatJadid = vazira;
            fitsJadid = fitnessha;
            //The index of the best member is selected and returned.
            int maxfit = fitnessha[0], index = 0;
            for (int i = 1; i < fitnessha.Count; i++)
            {
                if (fitnessha[i] >= maxfit)
                {
                    maxfit = fitnessha[i];
                    index = i;
                }
            }
            return index;
        }
        //Constructor
        public NVazir(List<int[]> halatha, string eOrj, string fEntekhabi, string cEntekhabi, string mEntekhabi, int k)
        {
            this.halatha = halatha;
            for (int i = 0; i < halatha.Count; i++)
            {
                fits.Add(Fitness(halatha[i]));
            }
            this.eOrj = eOrj;
            this.k = k;
            this.fEntekhabi = fEntekhabi;
            this.cEntekhabi = cEntekhabi;
            this.mEntekhabi = mEntekhabi;
        }
    }
}
