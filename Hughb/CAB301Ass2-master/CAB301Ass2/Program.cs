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
            /*
            //loop for creating test data
            int numOfArrays = 100;
            int[] arr;
            for (int i = 10; i <= 100; i = i + 10) // array size from 1000 to 10,000 in 500 increments
            {
                arr = new int[i];
                Console.WriteLine("array size: " + arr.Length + " to begin writing to file.");
                for (int j = 1; j <= numOfArrays; j++) // create 500 unique test cases for each array length
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


            #region Test cases 
            
            int[] ascendingArray = {4,1,10,9,7,12,8,2,15 };
            Console.WriteLine(" 15, 2, 8, 12, 7, 9, 10, 1, 4 \n ");
            int median = Median(ascendingArray);
            Console.WriteLine("median = " + median);

            int[] ascendingArray2 = {1,2,3,4,5,6,7,8,9};
            Console.WriteLine("1,2,3,4,5,6,7,8,9");
            int median2 = Median(ascendingArray2);
            Console.WriteLine("median = " + median2);

            int[] ascendingArray3 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 , 10};
            Console.WriteLine("1,2,3,4,5,6,7,8,9, 10");
            int median3 = Median(ascendingArray3);
            Console.WriteLine("median = " + median3);

            #endregion Test Cases
            
            for(var i = 10; i <= 100; i = i + 10) {

                Console.WriteLine("Analysing data from array size: " + i);

                var arrList = new List<int[]>( ); // arrayList to hold all the int[] arrays stored in the .csv file
                var timeList = new List<string>( ); // list for tracking execution time
                var opCountList = new List<int>( ); // list for tracking operation counts

                var sw = new Stopwatch( );
         
                readFile(arrList, i); // this only grabs the 1000.csv file you can put this in a loop to grab all of them in one go and do the process on them

 
                foreach(var item in arrList) {

                    //Count Basic Operations
                    opCount = 0;
                    var m = BasicOperationBruteForceMedian(item); //Just need to change Algorithms
                    opCountList.Add(opCount);

                    //Measure Execution Time 
                    sw = new Stopwatch( );
                    sw.Start( );
                    int medVal = BruteForceMedian(item); //Just need to change Algorithms
                    sw.Stop( );
                    timeList.Add(sw.GetTimeString( ));


                    Console.WriteLine("Median Value: " + medVal + ", Execution Time: " + sw.GetTimeString( ) + ", OpCount: " + opCount);

                }
                Console.WriteLine("Saving Data collected for array size: " + i);
                writeData(timeList, opCountList, i);
                // pause to view console   
            }

            Console.WriteLine("Finished All Data Analysis");
            
            Console.ReadLine( );
        }

        #region Basic Operatiopn Selection Median 

        public static int BasicOperationMedian(int[] arr) {

            if(arr.Length == 1) {
                return arr[0];
            }
            else {

                return BasicOperationSelect(arr, 0, arr.Length / 2, arr.Length - 1);
            }
        }

        public static int BasicOperationSelect(int[] arr, int l, int m, int h) {

            var pos = Partition(arr, l, h);

            if(pos == m) {
                return arr[pos];
            }
            if(pos > m) {
                return Select(arr, l, m, pos - 1);
            }
            if(pos < m) {
                return Select(arr, pos + 1, m, h);
            }
            else {
                return -1; // if no other criteria met (error)
            }
        }

        public static int BasicOperationPartition(int[] arr, int l, int h) {

            var pivotVal = arr[l];
            var pivotLoc = l;
            for(var j = l + 1; j <= h; j++) {
                int temp;
                if(arr[j] < pivotVal) {
                    pivotLoc++;
                    temp = arr[pivotLoc]; // swap
                    arr[pivotLoc] = arr[j];
                    arr[j] = temp;

                }
                temp = arr[l]; // swap
                arr[l] = arr[pivotLoc];
                arr[pivotLoc] = temp;

            }
            return pivotLoc;
        }





        #endregion Basic Operation Selection Median

        #region Selection Median Algorithm

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

        #region Basic Operation Brute Force Median

        static int BasicOperationBruteForceMedian(int[] Array) {
            //int basicOperation = 0;
            int length = Array.Length; //minus one because test data has an extra;
            int k = ((length + 1) / 2);
            int median = 0;

            for(int i = 0; i < length - 1; i++) {
                int numsmaller = 0; //How many elements are smaller than Array[i]
                int numequal = 0; // How many elemenrs are equal to Array[i]

                for(int j = 0; j < length - 1; j++) {
                    opCount++;

                    if(Array[j] < Array[i]) {
                        numsmaller = numsmaller + 1;
                    }
                    else if(Array[j] == Array[i]) {
                        numequal = numequal + 1;
                    }
                }//end inner for loop [j]
                if((numsmaller < k) && (k <= (numsmaller + numequal))) {
                    median = Array[i];
                }
            }//end outer for loop [i]
            // Console.WriteLine("BO : " + basicOperation);
            return opCount;
        } // end BruteForceMedian
        #endregion Basic Operation Brute Force Median

        #region Brute Force Algorithm
          
        static int BruteForceMedian(int[] Array) {
            int length = Array.Length; //minus one because test data has an extra;
            int k = ((length+1) / 2);
            //k = Math.Ceiling(k);
            int median = 0;

            for(int i = 0; i < length - 1; i++) {
                int numsmaller = 0; //How many elements are smaller than Array[i]
                int numequal = 0; // How many elemenrs are equal to Array[i]

                for(int j = 0; j < length - 1; j++) {
                    if(Array[j] < Array[i]) {
                        numsmaller = numsmaller + 1;
                    }
                    else if(Array[j] == Array[i]) {
                        numequal = numequal + 1;
                    }
                }//end inner for loop [j]

                if((numsmaller < k) && (k <= (numsmaller + numequal))) {
                    median = Array[i];
                }
            }//end outer for loop [i]
            return median;
        } // end BruteForceMedian
        #endregion Brute Force Algorithm

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

            var fileName = @"C:\Users\Hugh\Documents\QUT\Year-4\CAB301-Algorithms-and-Complexity\Assignment-2\CAB301Ass2-master\newTestData\" + arrSize + ".csv"; // need to make this a relative path
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

        public static void createTestData(int[] arr)
        {
            var fileName = @"C:\Users\Hugh\Documents\QUT\Year-4\CAB301-Algorithms-and-Complexity\Assignment-2\CAB301Ass2-master\newTestData\" + arr.Length + ".csv"; // need to make this a relative path
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
            var fileName = @"C:\Users\Hugh\Documents\QUT\Year-4\CAB301-Algorithms-and-Complexity\Assignment-2\CAB301Ass2-master\testData\Output\" + arraySize + "excTimes.csv";    
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
            var fileName = @"C:\Users\Hugh\Documents\QUT\Year-4\CAB301-Algorithms-and-Complexity\Assignment-2\CAB301Ass2-master\testData\Output\" + arraySize + "opCount.csv"; // relative paths      
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