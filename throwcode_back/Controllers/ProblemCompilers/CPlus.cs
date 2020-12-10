using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using ByteSizeLib;

namespace throwcode_back.Controllers.ProblemCompilers
{
    public class CPlus : Compiler
    {

        private string pathProgramm;
        private string folder;
        Process processTesting = new Process();
        Process processCompiling = new Process();
        public bool Run(string code, string testCases, string runId)
        {
            try
            {
                if (Compile(code, runId))
                {
                    
                    if (Test(testCases))
                    {
                        this.IsCorrect = true;
                        var workingSet = ByteSize.FromBytes(peakWorkingSet);
                        this.LastOutput = "Программа отработала без ошибок\nВремя выполнения: " + (processTesting.ExitTime.Millisecond - processTesting.StartTime.Millisecond) + " МС\nМакс. запрошено памяти: " + workingSet.MegaBytes + " МБ";
                        processTesting.Dispose();
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            Directory.Delete(@"/Programs/" + folder, true);
            return true;
        }

        /// <summary>
        /// Function which compile input code, using in CPlus context!!!
        /// </summary>
        /// <param name="code">Code to compile</param>
        /// <returns>
        /// Returns TRUE if programm compile successfully, FALSE if error while compile
        /// </returns>
        private bool Compile(string code, string runId)
        {
            string output, err;
            folder = runId;
            Directory.CreateDirectory(@"/Programs/" + folder);
            string path = Path.Combine(@"/Programs/" + folder, runId + ".cpp");
            pathProgramm = Path.Combine(@"/Programs/" + folder, "res.exe");
            File.WriteAllText(path, code);

            using (processCompiling)
            {
                processCompiling.StartInfo.FileName = "cmd.exe";
                processCompiling.StartInfo.Arguments = "/c " + @"g++.exe " + path + " -o " + pathProgramm;
                processCompiling.StartInfo.UseShellExecute = false;
                processCompiling.StartInfo.RedirectStandardOutput = true;
                processCompiling.StartInfo.RedirectStandardError = true;
                processCompiling.Start();
                output = processCompiling.StandardOutput.ReadToEnd();
                err = processCompiling.StandardError.ReadToEnd();
                processCompiling.WaitForExit();
            }
            if (err != "")
            {
                this.IsCorrect = false;
                this.LastOutput = err;
                return false;
            }
            else { return true; }
        }
        private bool Test(string testCases)
        {
            try
            {
                string output, err;
                TestData[] testData = JsonConvert.DeserializeObject<TestData[]>(testCases);
                for (int i = 0; i < testData.Length; i++)
                {
                    StringBuilder input = new StringBuilder();
                    for (int y = 0; y < testData[i].input.Length; y++)
                    {
                        input.Append(testData[i].input[y] + " ");
                    }
                    string inputS = input.ToString().Trim();

                    processTesting.StartInfo.FileName = "cmd.exe";
                    processTesting.StartInfo.Arguments = "/c " + pathProgramm + " " + inputS;
                    processTesting.StartInfo.UseShellExecute = false;
                    processTesting.StartInfo.RedirectStandardOutput = true;
                    processTesting.StartInfo.RedirectStandardError = true;
                    processTesting.Start();
                    peakPagedMem = processTesting.PeakPagedMemorySize64;
                    peakVirtualMem = processTesting.PeakVirtualMemorySize64;
                    peakWorkingSet = processTesting.PeakWorkingSet64;
                    output = processTesting.StandardOutput.ReadToEnd();
                    err = processTesting.StandardError.ReadToEnd();
                    


                    if (output != testData[i].output)
                    {

                        this.IsCorrect = false;
                        this.LastOutput = "При входных данных: " + inputS + " \nОжидалось получить: " + testData[i].output + " \nБыло получено: " + output;
                        return false;
                    }
                }
                return true;
            }catch (Exception ex)
            {
                this.IsCorrect = false;
                this.LastOutput = ex.ToString();
            }
            return false;

        }
        private void TimeOut (Process process)
        {
            this.IsCorrect = false;
            this.LastOutput = "Выполнение программы прервано по ограничению времени";
        }
    }
}
