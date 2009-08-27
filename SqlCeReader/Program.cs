using System;
using System.Data.SqlServerCe;
using System.Windows.Forms;

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
                var dialog = new OpenFileDialog {DefaultExt = "sdf", Multiselect = false};
                DialogResult result = dialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                fileName = dialog.FileName;
            }
            using (var connection = new SqlCeConnection(string.Format("Data Source={0}", fileName)))
            {
                connection.Open();
                while (true)
                {
                    Console.WriteLine("What?");
                    string commandText = Console.ReadLine();
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
            }
        }
    }
}