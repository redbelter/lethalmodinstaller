using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTotalNet.Results;
using VirusTotalNet;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Objects;
using System.IO;

namespace LethalRed
{

    //virus total api: 40d7fca521c042e1ca37a4cce62e2efcc7a4f9aecd05367e5387c928de205f40
    public class CheckForVirus
    {

        public static async Task<bool> CheckFile(string path, bool printResult = false, int percentThreshold = 50)
        {
            VirusTotal virusTotal = new VirusTotal("40d7fca521c042e1ca37a4cce62e2efcc7a4f9aecd05367e5387c928de205f40");

            //Use HTTPS instead of HTTP
            virusTotal.UseTLS = true;

            byte[] fileToScan = File.ReadAllBytes(path);

            //Check if the file has been scanned before.
            FileReport report = await virusTotal.GetFileReportAsync(fileToScan);

            Console.WriteLine("Seen before: " + (report.ResponseCode == FileReportResponseCode.Present ? "Yes" : "No"));


            //Check if the file has been scanned before.
            FileReport fileReport = await virusTotal.GetFileReportAsync(fileToScan);

            bool hasFileBeenScannedBefore = fileReport.ResponseCode == FileReportResponseCode.Present;

            Console.WriteLine("File has been scanned before: " + (hasFileBeenScannedBefore ? "Yes" : "No"));


            //If the file has been scanned before, the results are embedded inside the report.
            ScanResult fileResult = null;
            if (hasFileBeenScannedBefore)
            {
                if (printResult)
                {
                    PrintScanDetailed(fileReport);
                }
            }
            else
            {

                fileResult = await virusTotal.ScanFileAsync(fileToScan, Path.GetFileName(path));
                if (printResult)
                {
                    PrintScan(fileResult);
                }
            }
            return CheckResult(percentThreshold, fileReport, fileResult);
        }


        public static async void CheckFileTest() { 
            VirusTotal virusTotal = new VirusTotal("40d7fca521c042e1ca37a4cce62e2efcc7a4f9aecd05367e5387c928de205f40");

            //Use HTTPS instead of HTTP
            virusTotal.UseTLS = true;

            //Create the EICAR test virus. See http://www.eicar.org/86-0-Intended-use.html
            byte[] eicar = Encoding.ASCII.GetBytes(@"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");

            //Check if the file has been scanned before.
            FileReport report = await virusTotal.GetFileReportAsync(eicar);

            Console.WriteLine("Seen before: " + (report.ResponseCode == FileReportResponseCode.Present? "Yes" : "No"));


            //Check if the file has been scanned before.
            FileReport fileReport = await virusTotal.GetFileReportAsync(eicar);

            bool hasFileBeenScannedBefore = fileReport.ResponseCode == FileReportResponseCode.Present;

            Console.WriteLine("File has been scanned before: " + (hasFileBeenScannedBefore ? "Yes" : "No"));

            //If the file has been scanned before, the results are embedded inside the report.
            if (hasFileBeenScannedBefore)
            {
                PrintScanShort(fileReport);
            }
            else
            {
                ScanResult fileResult = await virusTotal.ScanFileAsync(eicar, "EICAR.txt");
                PrintScan(fileResult);
            }

        }

        private static bool CheckResult(int percentThreshold, FileReport file, ScanResult scanresult)
        {
            if(file != null)
            {
                int fail = 0;
                int total = 0;
                if (file.ResponseCode == FileReportResponseCode.Present)
                {
                    foreach (KeyValuePair<string, ScanEngine> scan in file.Scans)
                    {
                        //   Console.WriteLine("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                        total++;
                        if (scan.Value.Detected)
                        {
                            fail++;
                        }
                    }
                    if(total/2 > fail)
                    {
                        return true;
                    }
                    return false;
                }
                throw new Exception("dont know");


            } else {
                PrintScan(scanresult);
                return true;
            }
        }

        private static void PrintScanShort(FileReport fileReport)
        {
            int fail = 0;
            int total = 0;
            if (fileReport.ResponseCode == FileReportResponseCode.Present)
            {
                foreach (KeyValuePair<string, ScanEngine> scan in fileReport.Scans)
                {
                    //   Console.WriteLine("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                    total++;
                    if (scan.Value.Detected)
                    {
                        fail++;
                    }
                }
            }
            Console.WriteLine(fail + " out of " + total + " AVs detected it");

        }

        private static void PrintScanDetailed(FileReport fileReport)
        {
            Console.WriteLine("Scan ID: " + fileReport.ScanId);
            Console.WriteLine("Message: " + fileReport.VerboseMsg);

            if (fileReport.ResponseCode == FileReportResponseCode.Present)
            {
                foreach (KeyValuePair<string, ScanEngine> scan in fileReport.Scans)
                {
                    Console.WriteLine("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                }
            }

            Console.WriteLine();
        }

        private static void PrintScan(ScanResult scanResult)
        {
            Console.WriteLine("Scan ID: " + scanResult.ScanId);
            Console.WriteLine("Message: " + scanResult.VerboseMsg);
            Console.WriteLine();
        }

    }
}
