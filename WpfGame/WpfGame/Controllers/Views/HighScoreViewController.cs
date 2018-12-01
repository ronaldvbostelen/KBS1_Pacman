using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            // TODO set button events
        }

        public ArrayList GetListOfHighScores()
        {
            int counter = 0;
            string line;
            ArrayList arrayList = new ArrayList();

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(@"C:\Users\sel01\source\repos\KBS1-CSharp game\WpfGame\WpfGame\bin\Debug\Highscore.txt");
            while ((line = file.ReadLine()) != null)
            {
                arrayList.Add(line);
                counter++;
            }
            file.Close();
            return arrayList;
        }

        public FlowDocument CreateFlowDocument()
        {
            // Create the parent FlowDocument...
            FlowDocument flowDoc = new FlowDocument();

            // Create the Table
            Table table = new Table();
            // ...and add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(table);

            // Set some global formatting properties for the table.
            table.CellSpacing = 10;
            table.Background = Brushes.White;

            // Create 3 columns and add them to the table's Columns collection.
            int numberOfColumns = 6;
            for (int x = 0; x < numberOfColumns; x++)
            {
                table.Columns.Add(new TableColumn());

                // Set alternating background colors for the middle colums.
                table.Columns[x].Background = x % 2 == 0 ? Brushes.Beige : Brushes.LightSteelBlue;
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            table.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRow.Background = Brushes.Silver;
            currentRow.FontSize = 40;
            currentRow.FontWeight = FontWeights.Bold;

            // Add the header row with content, 
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("High Score Table"))));
            // and set the row to span all 6 columns.
            currentRow.Cells[0].ColumnSpan = 3;

            // Add the second (header) row.
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Bold;

            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Rank"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Score"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Gebruikersnaam"))));

            // Add the third row.
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[2];

            // Global formatting for the row.
            currentRow.FontSize = 12;
            currentRow.FontWeight = FontWeights.Normal;

            // Add cells with content to the third row.
            // TODO add highscores with the GetListOfHighScores method
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("1"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("280"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sake"))));

            // Bold the first cell.
            currentRow.Cells[0].FontWeight = FontWeights.Bold;

            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[3];

            // Global formatting for the footer row.
            currentRow.Background = Brushes.LightGray;
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Normal;

            // Add the header row with content, 
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Footer text"))));
            // and set the row to span all 6 columns.
            currentRow.Cells[0].ColumnSpan = 3;

            return flowDoc;
        }

        private void HighScoreCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO add table when loaded
        }

    }
}
