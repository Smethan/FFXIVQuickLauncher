using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace XIVLauncher.Common.Util;

public static class TotpHelpers {
    public static string GetTotpToken(string key, string algorithm = "sha1", int digits = 6, int period = 30)
            {
                var processStartInfo = new ProcessStartInfo(".\\Resources\\otp-cli.exe", $"\"{key}\" -a {algorithm.ToLower()} -d {digits} -p {period}")
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                };
                var process = Process.Start(processStartInfo) ?? throw new Exception("Failed to get Token");
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    var error = process.StandardError.ReadToEnd();
                    throw new Exception($"Token Generation Failed: {error}");
                }
                return process.StandardOutput.ReadToEnd().Trim();
            }
}