using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace UDPPing
{
    internal static class Program
    {
        static Logic logic;

        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            logic = new Logic();
            try
            {
                try
                {

                    MainAsync(args, cts.Token).Wait();
                }
                catch (System.AggregateException ex)
                {
                    foreach (Exception e in ex.InnerExceptions)
                    {
                        throw e;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
        const string writeMe = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n\0";
        const int  packetSize = 496;
        private static async System.Threading.Tasks.Task MainAsync(string[] args, CancellationToken cancellationToken)
        {
            Console.WriteLine(".");
            await logic.SendMessageAsync("Hello World!\r\n\0", "192.168.100.101");  // Put address of RAK566 here

            var packet = new string(writeMe.Substring(0, packetSize).Reverse().ToArray()) + "\r\n\0";

            while (true)
            {
                await Task.Yield();
                await Task.Delay(100, cancellationToken);  // If you decrease the delay and/or increase the payload below the remote end loses bytes
                await logic.SendMessageAsync(packet);
            }
        }
    }
}
