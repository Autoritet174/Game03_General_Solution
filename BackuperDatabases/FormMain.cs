using System.ComponentModel;
using System.Diagnostics;

namespace BackuperDatabases;

public partial class FormMain : Form
{
    enum Database
    {
        [Description("PostgreSQL - Users database")]
        PostgreSql_Users,

        [Description("PostgreSQL - Game data database")]
        PostgreSql_Data,

        [Description("MongoDB - User data database")]
        MongoDb_UserData
    }

    public FormMain()
    {
        InitializeComponent();
    }

    private void button_Backup_PostgreSql_Users_Click(object sender, EventArgs e)
    {
        Enabled = false;
        Backup(Database.PostgreSql_Users);
        Enabled = true;
    }

    private void button_Backup_PostgreSql_GameData_Click(object sender, EventArgs e)
    {
        Enabled = false;
        Backup(Database.PostgreSql_Data);
        Enabled = true;
    }

    private void button_Backup_MongoDb_UserData_Click(object sender, EventArgs e)
    {
        Enabled = false;
        Backup(Database.MongoDb_UserData);
        Enabled = true;
    }

    private void button_Backup_All_Click(object sender, EventArgs e)
    {
        Enabled = false;
        foreach (Database database in Enum.GetValues<Database>())
        {
            Backup(database);
        }
        Enabled = true;
    }

    private void button_Restore_PostgreSql_Users_Click(object sender, EventArgs e)
    {
        Enabled = false;
        Restore(Database.PostgreSql_Users);
        Enabled = true;
    }

    private void button_Restore_PostgreSql_GameData_Click(object sender, EventArgs e)
    {
        Enabled = false;
        Restore(Database.PostgreSql_Data);
        Enabled = true;
    }

    private void button_Restore_MongoDb_UserData_Click(object sender, EventArgs e)
    {
        Enabled = false;
        Restore(Database.MongoDb_UserData);
        Enabled = true;
    }

    private void button_Restore_All_Click(object sender, EventArgs e)
    {
        Enabled = false;
        foreach (Database database in Enum.GetValues<Database>())
        {
            Restore(database);
        }
        Enabled = true;
    }


    private const string backup_folder = @"c:\Users\Public\Documents\Game03_DatabaseBackups";


    private const string postgre_tools_pathDir = @"C:\Program Files\PostgreSQL\18\bin";
    private const string postgre_server_host = "localhost";
    private const string postgre_DBMS_name = "Postgre";

    private const string mongodb_tools_pathDir = @"c:\Program Files\MongoDB\Tools\100\bin\";
    private const string mongodb_server_host = "localhost";
    private const string mongodb_server_port = "27017";
    private const string mongodb_server_dbName = "userData";
    private const string mongodb_DBMS_name = "MongoDb";


    private const string game03_archive_dirPath = @"c:\UnityProjects\Game03_General_Solution\DataBaseBackupArchives";


    private const string archivator7z_exeFilePath = @"C:\Program Files\7-Zip\7z.exe";


    /*
    должен быть файл
    %APPDATA%\postgresql\pgpass.conf

    содержание
    localhost:5432:*:postgres:мойПароль
    */
    static void Backup(Database database)
    {
        BackupFolderReset();

        string dbFileName, dbFilePath, bat, dbName;

        ////////////////////////////////////////////////////////////////////////////////////////
        // Создание бэкапа базы данных
        if (database is Database.PostgreSql_Users or Database.PostgreSql_Data)
        {
            dbName = database == Database.PostgreSql_Users ? "Game03_Users" : "Game03_Data";
            dbFilePath = Path.Combine(backup_folder, $"{dbName}.sql");
            //--format=plain --format=custom //sql OR binary
            bat = $"""
                "{Path.Combine(postgre_tools_pathDir, "pg_dump.exe")}" -U postgres --host={postgre_server_host} --format=plain --verbose --clean --if-exists --no-owner --no-privileges {dbName} > "{dbFilePath}"
                """;
            ExecuteBatCommand_and_WaitForExit(bat);
        }
        else if (database is Database.MongoDb_UserData)
        {
            dbName = mongodb_server_dbName;
            dbFileName = $"{mongodb_server_dbName}.archiveMongodb";
            dbFilePath = Path.Combine(backup_folder, dbFileName);
            bat = $"""
                "{Path.Combine(mongodb_tools_pathDir, "mongodump.exe")}" --host {mongodb_server_host} --port {mongodb_server_port} --db {mongodb_server_dbName} --archive="{dbFilePath}"
                """;
            ExecuteBatCommand_and_WaitForExit(bat);
        }
        else {
            return;
        }
        ////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////
        // Создание архива
        string now = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss}";
        string archiveFileName = Path.Combine(dbName, $"{now}");
        if (database is Database.PostgreSql_Users or Database.PostgreSql_Data)
        {
            archiveFileName += $".postgre.sql.7z";
            bat = $"""
                "{archivator7z_exeFilePath}" a -t7z "{Path.Combine(game03_archive_dirPath, postgre_DBMS_name, archiveFileName)}" "{dbFilePath}"
                """;
            ExecuteBatCommand_and_WaitForExit(bat);
        }
        else if (database is Database.MongoDb_UserData)
        {
            archiveFileName += $".archiveMongodb.7z";
            bat = $"""
                "{archivator7z_exeFilePath}" a -t7z "{Path.Combine(game03_archive_dirPath, mongodb_DBMS_name, archiveFileName)}" "{dbFilePath}"
                """;
            ExecuteBatCommand_and_WaitForExit(bat);
        }
        else
        {
            return;
        }
        ////////////////////////////////////////////////////////////////////////////////////////

        BackupFolderClear();
    }

    static void Restore(Database database)
    {
        BackupFolderReset();
        OpenFileDialog openFileDialog = new();

        ////////////////////////////////////////////////////////////////////////////////////////
        // Распаковка архива
        string DBMS_name, dbName;
        if (database is Database.PostgreSql_Users or Database.PostgreSql_Data) {
            DBMS_name = postgre_DBMS_name;
            dbName = database == Database.PostgreSql_Users ? "Game03_Users" : "Game03_Data";
        }
        else if (database is Database.MongoDb_UserData)
        {
            dbName = "userData";
            DBMS_name = mongodb_DBMS_name;
        } else {
            return;
        }

        openFileDialog.InitialDirectory =  Path.Combine(game03_archive_dirPath, DBMS_name, dbName);
        if (openFileDialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        string bat = $"""
            "{archivator7z_exeFilePath}" x -y "{openFileDialog.FileName}" -o"{Path.Combine(backup_folder)}"
            """;
        ExecuteBatCommand_and_WaitForExit(bat);
    ////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////
        // Восстановление базы данных
        if (database is Database.PostgreSql_Users or Database.PostgreSql_Data)
        {
            string dbFilePath = Path.Combine(backup_folder, $"{dbName}.sql");
            // "{Path.Combine(postgre_tools_pathDir, "pg_restore.exe")}" -U postgres --host={postgre_server_host} --clean --if-exists --verbose --dbname={dbName} "{dbFilePath}"
            bat = $"""
                "{Path.Combine(postgre_tools_pathDir, "psql.exe")}" -U postgres --host={postgre_server_host} --dbname={dbName} -f "{dbFilePath}"
                """;
            ExecuteBatCommand_and_WaitForExit(bat);
        }
        else if (database is Database.MongoDb_UserData)
        {
            string dbFileName = $"{mongodb_server_dbName}.archiveMongodb";
            string dbFilePath = Path.Combine(backup_folder, dbFileName);
            bat = $"""
                "{Path.Combine(mongodb_tools_pathDir, "mongorestore.exe")}" --host {mongodb_server_host} --port {mongodb_server_port} --archive="{dbFilePath}" --drop
                """;
            ExecuteBatCommand_and_WaitForExit(bat);
        }
        ////////////////////////////////////////////////////////////////////////////////////////

        BackupFolderClear();
    }

    /// <summary>
    /// Выполняет команду через временный BAT-файл, ожидает завершения и удаляет BAT.
    /// </summary>
    /// <param name="command">Команда для выполнения.</param>
    static void ExecuteBatCommand_and_WaitForExit(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            throw new ArgumentException("Команда пуста.", nameof(command));

        // создаём временный bat-файл
        string batPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bat");
        File.WriteAllText(batPath, command);

        try
        {
            ProcessStartInfo psi = new()
            {
                FileName = "cmd.exe",
                Arguments = "/c \"" + batPath + "\"",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using Process? process = Process.Start(psi) ?? throw new InvalidOperationException("Не удалось запустить процесс.");
            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new InvalidOperationException($"Процесс завершился с кодом {process.ExitCode}.");
        }
        finally
        {
            // безопасное удаление bat-файла
            try
            {
                if (File.Exists(batPath))
                    File.Delete(batPath);
            }
            catch
            {
                // подавляем ошибку удаления, чтобы не мешала основной логике
            }
        }
    }

    static void BackupFolderReset()
    {
        BackupFolderClear();

        if (!Directory.Exists(backup_folder))
        {
            Directory.CreateDirectory(backup_folder);
        }
    }
    
    static void BackupFolderClear()
    {
        if (Directory.Exists(backup_folder))
        {
            Directory.Delete(backup_folder, true);
        }
    }
}
