using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class HighScoreViewController : ViewController
    {
        private readonly HighScoreView _highScoreView;

        public HighScoreViewController(MainWindow mainWindow) : base(mainWindow)
        {
            _highScoreView = new HighScoreView();
            _highScoreView.Loaded += HighScoreCanvas_Loaded;
            SetContentOfMain(mainWindow, _highScoreView);
            SetButtonEvents(_highScoreView.BtnBack, BtnBack_Click);
        }

        public List<KeyValuePair<string, string>> GetListOfHighScoresDescending()
        {
            string line;
            var highScoreList = new List<KeyValuePair<string, string>>();

            // Read the file and add it line by line to List.
            var file = new StreamReader($"{Environment.CurrentDirectory}\\Highscores.txt");
            while ((line = file.ReadLine()) != null)
            {
                highScoreList.Add(new KeyValuePair<string, string>(line.Substring(0, line.IndexOf(' ')), line.Substring(line.IndexOf(' '))));
            }
            file.Close();

            return highScoreList.OrderByDescending(kvp => kvp.Value).ToList();
        }

        public FlowDocument CreateFlowDocument()
        {
            // Create the parent FlowDocument...
            FlowDocument flowDoc = new FlowDocument {Foreground = Brushes.Yellow, Background = Brushes.Black};

            // Create the Table
            Table table = new Table();

            // set fontfamlily
            table.FontFamily =  new FontFamily(new Uri("pack://application:,,,/"),"./Assets/Fonts/#CrackMan");
            // ...and add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(table);

            // Set some global formatting properties for the table.
            table.CellSpacing = 10;
            table.Background = Brushes.Black;

            // Create 3 columns and add them to the table's Columns collection.
            int numberOfColumns = 3;
            for (int x = 0; x < numberOfColumns; x++)
            {
                table.Columns.Add(new TableColumn());
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            table.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRow.FontSize = 55;
            currentRow.FontWeight = FontWeights.Bold;

            // Add the header row with content, 
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("High Score Table"))));
            // and set the row to span all 3 columns.
            currentRow.Cells[0].ColumnSpan = 3;

            // Add the second (header) row.
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 36;
            currentRow.FontWeight = FontWeights.Bold;

            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Rank"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Score"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("User name"))));

            int rankCounter = 1;
            int rowCounter = 2;
            int red = 1;
            int pink = 2;
            int blue = 3;
            int orange = 4;
            int yellow = 5;
            foreach (var highScore in GetListOfHighScoresDescending())
            {
                // Add the row.
                table.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table.RowGroups[0].Rows[rowCounter];

                // Global formatting for the row.
                currentRow.FontSize = 30;

                if (rankCounter == red)
                {
                    currentRow.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#d03e19"));
                    red += 5;
                }
                else if (rankCounter == pink)
                {
                    currentRow.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#ea82e5"));
                    pink += 5;
                }
                else if (rankCounter == blue)
                {
                    currentRow.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#46bfee"));
                    blue += 5;
                }
                else if (rankCounter == orange)
                {
                    currentRow.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#d03e19"));
                    orange += 5;
                }
                else if (rankCounter == yellow)
                {
                    currentRow.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#fdff00"));
                    yellow += 5;
                }

                currentRow.FontWeight = FontWeights.Normal;

                // Add cells with content to the row.
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"{rankCounter}e"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"{highScore.Value}"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"{highScore.Key}"))));

                // Bold the first cell.
                currentRow.Cells[0].FontWeight = FontWeights.Bold;

                rankCounter++;
                rowCounter++;
            }

            return flowDoc;
        }

        private void HighScoreCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            _highScoreView.FlowDocumentScrollViewer.Document = CreateFlowDocument();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            new StartWindowViewController(_mainWindow);
        }

    }
}
