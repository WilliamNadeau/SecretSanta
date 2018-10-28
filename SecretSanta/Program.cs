using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace SecretSanta
{
    class Program
    {
        static void Main(string[] args)
        {
            var rng = new Random();

            var names = new List<string>( new [] {
                "insert email recipients here"
            } );

            var randomizedNames = new List<string>();

            while (names.Count > 0)
            {
                var pick = names[rng.Next(names.Count)];
                names.Remove(pick);
                randomizedNames.Add(pick);
            }

            var namePairs = new List<(string sendingFrom, string sendingTo)>();

            for (var i = 0; i < randomizedNames.Count; i++)
            {
                if (i == 0)
                {
                    namePairs.Add((randomizedNames.Last(), randomizedNames[i]));
                }
                else
                {
                    namePairs.Add((randomizedNames[i - 1], randomizedNames[i]));
                }
            }

            var print = false;
            var test = false;

            if (print)
            {
                var len = namePairs.Max(f => f.sendingFrom.Length);

                foreach (var pair in namePairs)
                {
                    Console.WriteLine($"{pair.sendingFrom.PadRight(len)} to send a gift to {pair.sendingTo}");
                }
            }
            else
            {
                var emailAccount = "insert emailAccount here";

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                        emailAccount,
                        "emailpassword")
                };

                foreach (var pair in namePairs)
                {
                    Console.WriteLine($"Sending to {pair.sendingFrom}...");
                    client.Send(new MailMessage(
                        emailAccount,
                        test ? emailAccount : pair.sendingFrom,
                        //pair.sendingFrom, 
                        "Your secret santa assignment",
                        $"You, {pair.sendingFrom}, your secret santa recipient is {pair.sendingTo}! Execute your duty with conviction and glory!"));
                }
            }

            Console.WriteLine("Complete!");

            Console.ReadLine();
        }
    }
}
