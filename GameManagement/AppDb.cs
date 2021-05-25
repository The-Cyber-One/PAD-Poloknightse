using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

public class Highscore
{
    public string PlayerName { get; set; }
    public int Score { get; set; }
    public int Level { get; set; }
    public DateTime DateTime { get; set; }

    /**
     * Every object has the ToString() function that can be overridden. If you do, you return a string and whenever an object is written down
     * (for example when debugging or when writing to the console), it uses that string function. Very handy!
     */
    public override string ToString()
    {
        /**
         * string.Format is a function that accepts two or more parameters: a string, with a specific placeholder syntax and the values that will be used
         * to replace said placeholders.
         */
        return string.Format("Highscore: [playerName: {0}, score: {1}, level: {2}, dateTime: {3}]", PlayerName, Score, Level, DateTime);
    }
}

public class Table
{
    List<List<object>> table;
    int columnCount;

    /// <summary>
    /// Constructor for a Table
    /// </summary>
    /// <param name="columnsCount">Amount of columns in the table</param>
    public Table(int columnsCount)
    {
        this.columnCount = columnsCount;
        table = new List<List<object>>();
    }

    public int ColumnCount { get { return columnCount; } }
    public int RowCount { get { return table.Count; } }

    public List<object> GetRow(int index)
    {
        return table[index];
    }

    public void AddRow(List<object> newRow)
    {
        table.Add(new List<object>(ColumnCount));
        for (int i = 0; i < newRow.Count; i++)
        {
            table[RowCount - 1].Add(newRow[i]);
        }
    }

    public override string ToString()
    {
        string theString = "";
        for (int i = 0; i < RowCount; i++)
        {
            theString += i + 1 + "] ";
            for (int j = 0; j < ColumnCount; j++)
            {
                theString += table[i][j].ToString() + " | ";
            }
            theString += "\n";
        }
        return theString;
    }
}

public class AppDb : IDisposable
{
    private readonly MySqlConnection Connection;

    public AppDb()
    {
        Connection = new MySqlConnection("host=oege.ie.hva.nl; port=3306; user id=dekkerm51; password=nC8uQl1Muz#Oa7K#; database=zdekkerm51;");
    }

    /// <summary>
    /// Get a table from the database with a SELECT statement
    /// </summary>
    /// <param name="SQLCode">SQL SELECT statement</param>
    /// <returns>SQL statement result</returns>
    public async Task<Table> GetTable(string SQLCode)
    {
        //Wait for the connection to open...
        await Connection.OpenAsync();

        using MySqlCommand command = new MySqlCommand(SQLCode, Connection);
        
        //Wait for the query to run...
        using MySqlDataReader reader = await command.ExecuteReaderAsync();

        //Create an empty table that will hold our data.
        Table table = new Table(reader.GetColumnSchema().Count);

        while (await reader.ReadAsync())
        {
            //get the fields based on the column index
            List<object> row = new List<object>();
            for (int i = 0; i < reader.GetColumnSchema().Count; i++)
            {
                row.Add(reader.GetFieldValue<object>(i));
            }

            table.AddRow(row);
        }

        //Close the connection.
        Dispose();

        //Return the table
        return table;
    }

    /// <summary>
    /// Add a highscore object to the database
    /// </summary>
    /// <param name="highscore">Highscore object with values for the database</param>
    public async void AddHighscore(Highscore highscore)
    {
        if (!await PlayerNameExists(highscore.PlayerName))
        {
            await AddPlayer(highscore.PlayerName);
        }

        //Wait for the connection to open...
        await Connection.OpenAsync();

        //Insert highscore
        using MySqlCommand command = new MySqlCommand($"INSERT INTO Highscore (playerName, dateTime, score, level) value ('{highscore.PlayerName}', '{highscore.DateTime.ToString("yyyy-MM-dd HH:mm:ss")}', {highscore.Score}, {highscore.Level});", Connection);

        //Wait for query...
        await command.ExecuteReaderAsync();

        //Close the connection
        Dispose();
    }

    /// <summary>
    /// Checks if <paramref name="playerName"/> exists in the database
    /// </summary>
    /// <param name="playerName">Player name to check</param>
    /// <returns></returns>
    public async Task<bool> PlayerNameExists(string playerName)
    {
        //Wait for the connection to open...
        await Connection.OpenAsync();

        //Check if player name exists
        using MySqlCommand command = new MySqlCommand($"SELECT playerName FROM Player WHERE playerName = '{playerName}';", Connection);
        
        //Wait for teh query...
        MySqlDataReader reader = await command.ExecuteReaderAsync();

        bool succes = reader.HasRows;

        //Close the connection
        Dispose();

        //Return if the command found any rows
        return succes;
    }

    /// <summary>
    /// Add a player to the database
    /// </summary>
    /// <param name="playerName">Player name to add</param>
    /// <returns></returns>
    public async Task AddPlayer(string playerName)
    {
        //Wait for the connection to open...
        await Connection.OpenAsync();

        //Insert new player
        using MySqlCommand command = new MySqlCommand($"INSERT INTO Player (playerName) VALUE ('{playerName}');", Connection);

        //Wait for query...
        await command.ExecuteReaderAsync();

        //Close the connection
        Dispose();
    }

    public void Dispose()
    {
        Connection.Close();
    }
}