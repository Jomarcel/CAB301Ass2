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

            for (var i = 1000; i <= 10000; i = i + 500)
            {

                Console.WriteLine("Analysing data from array size: " + i);

                var arrList = new List<int[]>(); // arrayList to hold all the int[] arrays stored in the .csv file
                var timeList = new List<string>(); // list for tracking execution time
                var opCountList = new List<int>(); // list for tracking operation counts

                var sw = new Stopwatch();
            


                readFile(arrList, i); // this only grabs the 1000.csv file you can put this in a loop to grab all of them in one go and do the process on them


                foreach (var item in arrList)
                {
                    opCount = 0;
                    sw = new Stopwatch();
                    sw.Start();

                    var medVal = Median(item);

                    sw.Stop();
                    timeList.Add(sw.GetTimeString());
                    opCountList.Add(opCount);

                    Console.WriteLine("Median Value: " + medVal + ", Execution Time: " + sw.GetTimeString() + ", OpCount: " + opCount);

                }
                Console.WriteLine("Saving Data collected for array size: " + i);
                writeData(timeList, opCountList, i);
                 // pause to view console   
            }

            Console.WriteLine("Finished All Data Analysis");
            Console.ReadLine();
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
            
            var pivotVal = arr[l];
            var pivotLoc = l;
            for (var j = l + 1; j <= h; j++)
            {
                int temp;
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
            var n = arr.Length;
            for (var i = 0; i < n; i++)
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

            var fileName = @"C:\Users\UnBayleefable\Documents\GitHub\CAB301Ass2V2\testData\" + arrSize + ".csv"; // need to make this a relative path
            try // try/catch is used as if the filestream tries to access a file that doesnt exist
                // it will cause a input/output error to crash your program due to FileMode.Open
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open)) // create tunnel between program and file
                {
                    using (var reader = new StreamReader(fileStream)) // tool for reading each line
                    {

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            var temp = new int[values.Length]; // readline reads in the data as string[] so we need a process to convert it
                            for (var i = 0; i < values.Length - 1; i++)
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
            var fileName = @"C:\Users\UnBayleefable\Documents\GitHub\CAB301Ass2V2\testData\" + arr.Length + ".csv"; // need to make this a relative path
            using (var fileStream = new FileStream(fileName, FileMode.Append))
            {
                using (var outFile = new StreamWriter(fileStream))

                {
                    foreach (var item in arr)
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

        public static void writeTimeData(List<string> timeList, int arraySize)
        {
            var fileName = @"C:\Users\UnBayleefable\Documents\GitHub\CAB301Ass2V2\testData\Output\" + arraySize + "excTimes.csv";    
            using (var fileStream = new FileStream(fileName, FileMode.Append))  
            {
                using (var outFile = new StreamWriter(fileStream))
                {
                    foreach (var item in timeList)
                    {
                        var temp = new string[2];
                        temp = item.Split(',');                         // split the time and its indicator (s, ms, us, ns) 
                        outFile.WriteLine(temp[0] + "," + temp[1]);     // for easier use of data
                    }
                    outFile.WriteLine(); // move to next line in file
                }
            }
        }

        public static void writeOPData(List<int> opCountList, int arraySize)
        {
            var fileName = @"C:\Users\UnBayleefable\Documents\GitHub\CAB301Ass2V2\testData\Output\" + arraySize + "opCount.csv"; // relative paths      
            using (var fileStream = new FileStream(fileName, FileMode.Append))
            {
                using (var outFile = new StreamWriter(fileStream))
                {
                    foreach (var item in opCountList)
                    {                                              
                        outFile.WriteLine(item);     // for easier use of data
                    }                    
                }
            }
        }

        #endregion Data management

    }
}