using System;
using System.Data.SqlServerCe;

namespace SqlCeReader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string fileName;
            if (args.Length > 0) fileName = args[0];
            else
            {
                Console.WriteLine("You need to specify the path to the sdf file as a command-line argument!!");
                return;
            }
            using (var connection = new SqlCeConnection(string.Format("Data Source={0}", fileName)))
            {
                connection.Open();
                while (true)
                {
                    Console.WriteLine("What?");
                    string commandText = Console.ReadLine().Trim();
                    if (commandText.Equals("exit") || commandText.Equals("quit")) return;
                    try
                    {
                        if (!commandText.ToLower().StartsWith("select"))
                        {
                            int count = new SqlCeCommand(commandText, connection).ExecuteNonQuery();
                            Console.WriteLine("Number of rows affected:\t{0}", count);
                            continue;
                        }
                        SqlCeDataReader reader = new SqlCeCommand(commandText, connection).ExecuteReader();
                        int columnCount;
                        for (int i = 0;; i++)
                        {
                            try
                            {
                                Console.Write("{0}\t", reader.GetName(i));
                            }
                            catch
                            {
                                columnCount = i;
                                break;
                            }
                        }
                        Console.WriteLine();
                        while (reader.Read())
                        {
                            for (int i = 0; i < columnCount; i++)
                            {
                                Console.Write("{0}\t", reader.GetValue(i));
                            }
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("There was an error!!! What did you do?");
                        Console.WriteLine("The error details are:");
                        Console.WriteLine(e);
                        Console.WriteLine("Let's try to not repeat it shall we!!");
                    }
                }
            }
        }
    }
}