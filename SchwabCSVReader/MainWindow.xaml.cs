using System.Globalization;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Path = System.IO.Path;
using CsvHelper;


namespace SchwabCSVReader;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnSelectDirectoryClick(object sender, RoutedEventArgs e)
    {
        OpenFolderDialog folderDialog = new OpenFolderDialog
        {
            Title = "Please pick a folder that contains csv files"
        };
        bool? success = folderDialog.ShowDialog();
        if (success == true)
        {
            string path = folderDialog.FolderName;
            DirectoryTextBox.Text = path;
            string outputPath = Path.Combine(path, "combined.csv");

            try
            {
                CombineCsvFiles(path, outputPath);
                MessageBox.Show($"CSV files successfully combined into {outputPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        
    }

    private void CombineCsvFiles(string folderPath, string outputPath)
    {
        string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
        
        // Filter out the "combined.csv" file to avoid processing it
        csvFiles = csvFiles.Where(file => !string.Equals(Path.GetFileName(file), "combined.csv", StringComparison.OrdinalIgnoreCase)).ToArray();
        
        if (csvFiles.Length == 0)
        {
            throw new Exception("No csv files found");
        }

        var combinedRows = new List<CombinedRow>();

        foreach (var file in csvFiles)
        {
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.Read();
                string? watchlist = csv.GetField<string>(0);
                
                
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var row = new CombinedRow
                    {
                        Symbol = csv.GetField(0),
                        Last = csv.GetField(1),
                        ADX_D = csv.GetField(2),
                        Description = csv.GetField(3),
                        WatchList = watchlist
                    };
                    combinedRows.Add(row);
                }
            }
        }
        
        using (var writer = new StreamWriter(outputPath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<CombinedRow>();
            csv.NextRecord();
            foreach (var row in combinedRows)
            {
                csv.WriteRecord(row);
                csv.NextRecord();
            }
        }
    }
    
    public class CombinedRow
    {
        public string? Symbol { get; set; }
        public string? Last { get; set; }
        public string? ADX_D { get; set; }
        public string? Description { get; set; }
        public string? WatchList { get; set; }
    }
}



