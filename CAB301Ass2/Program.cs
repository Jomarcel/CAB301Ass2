using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace CAB301Ass2
{
    class Program
    {
        static Random _random = new Random();
        static int opCount;

        public static void Main(string[] args)
        {
            #region Test Data Creation

            /* loop for creating test data
            for (int i = 1000; i <= 10000; i = i + 500) // array size from 1000 to 10,000 in 500 increments
            {
                arr = new int[i];
                Console.WriteLine("array size: " + arr.Length + " to begin writing to file.");
                for (int j = 0; j < 500; j++) // create 500 unique test cases for each array length
                {

                    for (int m = 0; m < arr.Length; m++) // fill array with numbers equal to number of elements
                    {
                        arr[m] = m + 1;
                    }
                    shuffleArr(arr);

                    createTestData(arr);


                }
                Console.WriteLine("array size: " + arr.Length + " finished writing to file.");
            }
            */

            #endregion

            List<int[]> arrList = new List<int[]>(); // arrayList to hold all the int[] arrays stored in the .csv file
            List<string> timeList = new List<string>(); // list for tracking execution time
            List<int> opCountList = new List<int>(); // list for tracking operation counts

            Stopwatch sw = new Stopwatch();

            readFile(arrList, 1000); // this only grabs the 1000.csv file you can put this in a loop to grab all of them in one go and do the process on them

            
            foreach (int[] item in arrList)
            {
                opCount = 1;
                opCountList = new List<int>();
                timeList = new List<string>();
                sw = new Stopwatch();
                sw.Start();

                int medVal = Median(item);

                sw.Stop();
                timeList.Add(sw.GetTimeString());
                opCountList.Add(opCount);

                Console.WriteLine("Median Value: " + medVal + ", Execution Time: "+ sw.GetTimeString() + ", OpCount: " + opCount);   
                
            }
             writeData(timeList,opCountList, 1000);            
            Console.ReadLine(); // pause to view console            
        }

        #region Algorithm

        public static int Median(int[] arr)
        {
            
            if (arr.Length == 1)
            {
                return arr[0];
            }
            else
            {
                
                return Select(arr, 0, arr.Length / 2, arr.Length - 1);
            }
        }

        public static int Select(int[] arr, int l, int m, int h)
        {
            
            var pos = Partition(arr, l, h);

            if (pos == m)
            {
                return arr[pos];
            }
            if (pos > m)
            {
                return Select(arr, l, m, pos - 1);
            }
            if (pos < m)
            {
                return Select(arr, pos + 1, m, h);
            }
            else
            {
                return -1; // if no other criteria met (error)
            }
        }

        public static int Partition(int[] arr, int l, int h)
        {
            
            int pivotVal = arr[l];
            int pivotLoc = l;
            int temp;
            for (int j = l + 1; j <= h; j++)
            {
               
                if (arr[j] < pivotVal)
                {
                    pivotLoc++;
                    temp = arr[pivotLoc]; // swap
                    arr[pivotLoc] = arr[j];
                    arr[j] = temp;
                    
                }

                opCount++; // the second swap is the basic operation
                temp = arr[l]; // swap
                arr[l] = arr[pivotLoc];
                arr[pivotLoc] = temp;
                
            }
            return pivotLoc;
        }

        #endregion Algorithm

        #region Data Management

        public static void shuffleArr(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n; i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                var r = i + _random.Next(n - i);
                var t = arr[r];
                arr[r] = arr[i];
                arr[i] = t;
            }
        }

        public static void readFile(List<int[]> arrList, int arrSize)
        {

            string fileName = @"D:\testData\" + arrSize + ".csv"; // need to make this a relative path
            try // try/catch is used as if the filestream tries to access a file that doesnt exist
                // it will cause a input/output error to crash your program due to FileMode.Open
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open)) // create tunnel between program and file
                {
                    using (var reader = new StreamReader(fileStream)) // tool for reading each line
                    {

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            int[] temp = new int[values.Length]; // readline reads in the data as string[] so we need a process to convert it
                            for (int i = 0; i < values.Length - 1; i++)
                            {
                                temp[i] = Convert.ToInt32(values[i]);
                            }
                            arrList.Add(temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Read File Not found."); // error handling
            }

        }

        public  void createTestData(int[] arr)
        {
            string fileName = @"D:\\testData\\" + arr.Length + ".csv"; // need to make this a relative path
            using (FileStream fileStream = new FileStream(fileName, FileMode.Append))
            {
                using (StreamWriter outFile = new StreamWriter(fileStream))

                {
                    foreach (int item in arr)
                    {
                        outFile.Write(item + ",");

                    }
                    outFile.WriteLine();
                }
            }
        }
        public static void writeData(List<string> timeList, List<int> opCountList, int arraySize)
        {
            writeTimeData(timeList, arraySize);
            writeOPData(opCountList, arraySize);
        }

        public static void writeTimeData(List<string> timeList, int arraySize) // relative paths
        {
            string fileName = @"D:\testData\Output\" + arraySize + "excTimes.csv";      // currently only setup for time list 
            using (FileStream fileStream = new FileStream(fileName, FileMode.Append))   // if passed the opcount list it will output but the filename will be wrong
            {
                using (StreamWriter outFile = new StreamWriter(fileStream))
                {
                    foreach (string item in timeList)
                    {
                        string[] temp = new string[2];
                        temp = item.Split(',');                         // split the time and its indicator (s, ms, us, ns) 
                        outFile.WriteLine(temp[0] + "," + temp[1]);     // for easier use of data
                    }
                    outFile.WriteLine(); // move to next line in file
                }
            }
        }

        public static void writeOPData(List<int> opCountList, int arraySize)
        {
            string fileName = @"D:\testData\Output\" + arraySize + "opCount.csv"; // relative paths      
            using (FileStream fileStream = new FileStream(fileName, FileMode.Append))
            {
                using (StreamWriter outFile = new StreamWriter(fileStream))
                {
                    foreach (int item in opCountList)
                    {                                              
                        outFile.WriteLine(item);     // for easier use of data
                    }                    
                }
            }
        }

        #endregion Data management

    }
}