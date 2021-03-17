using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


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

public class AppDb : IDisposable
{
    private readonly MySqlConnection Connection;

    public AppDb()
    {
        Connection = new MySqlConnection("host=oege.ie.hva.nl; port=3306; user id=dekkerm51; password=nC8uQl1Muz#Oa7K#; database=zdekkerm51;");
    }

    /**
     * Async functions are a bit tricky. They work in conjunction with the await statement, which basically waits for an operation to finish 
     * before continueing to the next statement. In this case, we have to wait for the connection to be establised, for the query to run and 
     * for all the different records to be run. Once this is completed, we can return from the function, with the List of achievements. 
     * Be careful that an sync function can only return either a Task (System.Threading.Tasks) or be a void.
     */
    public async Task<List<Highscore>> GetHighscore()
    {
        //Wait for the connection to open...
        await Connection.OpenAsync();

        using MySqlCommand command = new MySqlCommand("SELECT * FROM Highscore;", Connection);
        //Wait for the query to run...
        using MySqlDataReader reader = await command.ExecuteReaderAsync();

        //Create an empty achievements list that will hold our achievements.
        List<Highscore> achievements = new List<Highscore>();
        while (await reader.ReadAsync())
        {
            //get the fields based on the columnnames.
            string playerName = reader.GetString("playerName");
            int score = reader.GetInt32("score");
            int level = reader.GetInt32("level");
            DateTime dateTime = reader.GetDateTime("dateTime");

            //Add achievement to the list.
            achievements.Add(new Highscore { PlayerName = playerName, Score = score, Level = level, DateTime = dateTime });
        }

        //Close the connection.
        Dispose();

        //Return the now full list.
        return achievements;
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
    private async Task<bool> PlayerNameExists(string playerName)
    {
        //Wait for the connection to open...
        await Connection.OpenAsync();

        //Check if player name exists
        using MySqlCommand command = new MySqlCommand($"SELECT playerName FROM Player WHERE playerName = '{playerName}';", Connection);
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