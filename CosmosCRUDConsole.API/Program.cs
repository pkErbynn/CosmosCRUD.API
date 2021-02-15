using CosmosCRUDConsole.API.Models;
using CosmosCRUDConsole.API.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

//Using async/await and Task.WhenAll to improve the overall speed of your C# code

//Mostly, a single method or service 
//    which awaits the outputs of numerous methods which are marked as asynchronous.

//If three methods don’t seem to depend on each other in any way, and since they’re all asynchronous methods, 
//    it’s possible to run them in parallel.

namespace CosmosCRUDConsole.API
{
    class Program
    {
        private static readonly string CosmosEndpoint = "https://localhost:8081";
        private static readonly string EmulatorKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly string DatabaseId = "Tourism";
        private static readonly string CastlesCollection = "Castles";

        static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // This method takes about 2.5s to run
            //var complexSum = await SlowAndComplexSumAsync();

            //// The elapsed time will be approximately 2.5s so far
            //Console.WriteLine("Time elapsed when sum completes..." + stopwatch.Elapsed);

            // This method takes about 4s to run
            //var complexWord = await SlowAndComplexWordAsync();

            //// The elapsed time at this point will be about 6.5s
            //Console.WriteLine("Time elapsed when both complete..." + stopwatch.Elapsed);


            //WHEN ALL
            var complexWord = SlowAndComplexWordAsync();
            var complexSum =  SlowAndComplexSumAsync();
            // running them in parallel should take about 4s to complete
            await Task.WhenAll(complexSum, complexWord);
            Console.WriteLine("Time elapsed when both complete..." + stopwatch.Elapsed);



            // These lines are to prove the outputs are as expected,
            // i.e. 300 for the complex sum and "ABC...XYZ" for the complex word
            Console.WriteLine("Result of complex sum = " + complexSum);
            Console.WriteLine("Result of complex letter processing " + complexWord);

            Console.Read();
        }

        // add delays to deliberately slow it down..... 2.5s to run - 25 by 0.1
        private static async Task<int> SlowAndComplexSumAsync()
        {
            int sum = 0;
            foreach (var counter in Enumerable.Range(0, 25))
            {
                sum += counter;
                await Task.Delay(100);
            }

            return sum;
        }

        private static async Task<string> SlowAndComplexWordAsync()
        {
            var word = string.Empty;
            foreach (var counter in Enumerable.Range(65, 26))
            {
                word = string.Concat(word, (char)counter);
                await Task.Delay(150);
            }

            return word;
        }
        
    }
}
