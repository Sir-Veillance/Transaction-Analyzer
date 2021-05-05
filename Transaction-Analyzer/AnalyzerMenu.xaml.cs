using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Transaction_Analyzer
{
    /// <summary>
    /// Interaction logic for AnalyzerMenu.xaml
    /// </summary>
    public partial class AnalyzerMenu : Window
    {
        private static MySqlConnection conn;
        public static MySqlConnection Conn
        {
            get { return conn; }
            set { conn = value; }
        }
        public AnalyzerMenu()
        {
            InitializeComponent();
        }

        private void ImportButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new();
            fileDialog.DefaultExt = ".csv";
            fileDialog.Filter = "CSV Files (*.csv)|*.csv";

            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                string csvFile = fileDialog.FileName;
                StreamReader fileReader = new(File.OpenRead(csvFile));
                // parse out header
                fileReader.ReadLine();

                string query;

                while (!fileReader.EndOfStream)
                {
                    string fileLine = fileReader.ReadLine();
                    string[] columns = fileLine.Split(",");

                    if (columns[0] == "***END OF FILE***")
                    {
                        continue;
                    }

                    string type;

                    string transactionDate = columns[0];
                    string[] splitDate = transactionDate.Split("/");
                    string transactionMonth = splitDate[0];
                    if (transactionMonth.Length < 2)
                    {
                        transactionMonth = "0" + transactionMonth;
                    }
                    string transactionDay = splitDate[1];
                    if (transactionDay.Length < 2)
                    {
                        transactionDay = "0" + transactionDay;
                    }
                    string transactionYear = splitDate[2];
                    transactionDate = transactionYear + "-" + transactionMonth + "-" + transactionDay;

                    string transactionID = columns[1];
                    string description = columns[2];
                    string quantity = columns[3];

                    string symbol = columns[4];
                    // extract symbol from option transactions
                    if (symbol.Length > 5)
                    {
                        symbol = symbol.Split(" ")[0];
                    }

                    string price = columns[5];
                    string amount = columns[7];

                    string transactionType;
                    if (float.Parse(amount) > 0)
                    {
                        transactionType = "s";
                    }
                    else
                    {
                        transactionType = "b";
                    }

                    string[] splitDescription = description.Split(" ");

                    // determining line type
                    if (description.Length < 2)
                    {
                        type = "endoffile";
                    } else if (splitDescription.Length < 5)
                    {
                        if (splitDescription[1] == "DIVIDEND")
                        {
                            type = "dividend";
                        } else
                        {
                            type = "notimplemented";
                        }
                    } else if (splitDescription[3] == "@")
                    {
                        type = "stock";
                    } else if (splitDescription[1] == "REMOVAL")
                    {
                        type = "optionremoval";
                    } else if (splitDescription.Length >= 9)
                    {
                        if (splitDescription[8] == "@")
                        {
                            type = "option";
                        } else
                        {
                            type = "notimplemented";
                        }
                    } else
                    {
                        type = "notimplemented";
                    }

                    // handling line
                    if (type == "stock")
                    {
                        query = "INSERT INTO Stock_Transaction " +
                                "VALUES (" +
                                transactionID + ",'" +
                                symbol + "','" +
                                transactionDate + "'," +
                                quantity + "," +
                                price + ",'" +
                                transactionType + "'" +
                                ");";

                        MySqlCommand insertCommand = new(query, conn);
                        insertCommand.ExecuteNonQuery();
                    } else if (type == "option")
                    {
                        string optionType;
                        string strike = splitDescription[6];
                        if (splitDescription[7] == "Call")
                        {
                            optionType = "c";
                        } else if (splitDescription[7] == "Put")
                        {
                            optionType = "p";
                        } else
                        {
                            optionType = "u";
                        }

                        // generating expiration date
                        string month = splitDescription[3];
                        switch (month)
                        {
                            case "Jan":
                                month = "01";
                                break;
                            case "Feb":
                                month = "02";
                                break;
                            case "Mar":
                                month = "03";
                                break;
                            case "Apr":
                                month = "04";
                                break;
                            case "May":
                                month = "05";
                                break;
                            case "Jun":
                                month = "06";
                                break;
                            case "Jul":
                                month = "07";
                                break;
                            case "Aug":
                                month = "08";
                                break;
                            case "Sep":
                                month = "09";
                                break;
                            case "Oct":
                                month = "10";
                                break;
                            case "Nov":
                                month = "11";
                                break;
                            case "Dec":
                                month = "12";
                                break;
                            default:
                                month = "01";
                                break;
                        }

                        string day = splitDescription[4];
                        if (day.Length < 2)
                        {
                            day = "0" + day;
                        }
                        string year = splitDescription[5];
                        

                        query = "INSERT INTO Option_Transaction " +
                                "VALUES (" +
                                transactionID + ",'" +
                                symbol + "','" +
                                transactionDate + "'," +
                                quantity + "," +
                                price + ",'" +
                                transactionType + "','" +
                                optionType + "'," +
                                strike + ",'" +
                                year + "-" + month + "-" + day + "'" +
                                ");";

                        MySqlCommand insertCommand = new(query, conn);
                        insertCommand.ExecuteNonQuery();
                    } else
                    {
                        
                    }
                }
            } else
            {
                MessageBox.Show("Invalid file selection.",
                                "Confirmation",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
            }
            
        }

        private void ManualButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ModButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void AnalysisButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ReturnButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
