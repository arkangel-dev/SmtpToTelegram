using Microsoft.VisualBasic;
using MimeKit;
using Newtonsoft.Json.Linq;
using SmtpForwarder;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public static class Program {
    public static void Main(string[] args) {
        var forwarder = new Forwarder();
        forwarder.Start();
        Console.WriteLine("Started...");
        Console.ReadLine();
    }
}