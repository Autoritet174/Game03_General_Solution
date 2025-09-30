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
        if (!YesNo())
        {
            return;
        }
        Backup(Database.PostgreSql_Users);
    }

    private void button_Backup_PostgreSql_GameData_Click(object sender, EventArgs e)
    {
        if (!YesNo())
        {
            return;
        }
        Backup(Database.PostgreSql_Data);
    }

    private void button_Backup_MongoDb_UserData_Click(object sender, EventArgs e)
    {
        if (!YesNo())
        {
            return;
        }
        Backup(Database.MongoDb_UserData);
    }

    private void button_Backup_All_Click(object sender, EventArgs e)
    {
        if (!YesNo())
        {
            return;
        }
        foreach (Database database in Enum.GetValues<Database>())
        {
            Backup(database);
        }
    }

    private void button_Restore_PostgreSql_Users_Click(object sender, EventArgs e)
    {
        if (!YesNo())
        {
            return;
        }
        Restore(Database.PostgreSql_Users);
    }

    private void button_Restore_PostgreSql_GameData_Click(object sender, EventArgs e)
    {
        if (!YesNo())
        {
            return;
        }
        Restore(Database.PostgreSql_Data);
    }

    private void button_Restore_MongoDb_UserData_Click(object sender, EventArgs e)
    {
        if (!YesNo())
        {
            return;
        }
        Restore(Database.MongoDb_UserData);
    }

    private void button_Restore_All_Click(object sender, EventArgs e)
    {
        if (!YesNo())
        {
            return;
        }
        foreach (Database database in Enum.GetValues<Database>())
        {
            Restore(database);
        }
    }

    static bool YesNo() {
        return MessageBox.Show("Go?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes;
    }
    
    private const string backup_folder = @"C:\Game03_DataBasesRestore";

    const string SERVER_POSTGRES_HOST = "127.127.126.5";
    const string SERVER_MONGODB_HOST = "127.127.126.5";
    const string Path_7zExe = @"C:\Program Files\7-Zip\7z.exe";
    const string PathDir_MongoDbTools = @"C:\OSPanel\addons\MDBTools-100.12";

    static void Backup(Database database)
    {
        if (Directory.Exists(backup_folder))
        {
            Directory.Delete(backup_folder, true);
        }

        if (!Directory.Exists(backup_folder))
        {
            Directory.CreateDirectory(backup_folder);
        }


        // Создание бэкапа базы данных
        switch (database)
        {
            case Database.PostgreSql_Users:
            case Database.PostgreSql_Data:
                ExecuteBatCommand_and_WaitForExit($"""
                    pg_dump -U postgres --host={SERVER_POSTGRES_HOST} --format=custom --verbose --no-owner --no-privileges --dbname={(database==Database.PostgreSql_Users ? "Game03_Users": "Game03_Data")} > "{backup_folder}\{database}.backup"
                    """);
                break;
            case Database.MongoDb_UserData:
                ExecuteBatCommand_and_WaitForExit($"""
                    "{PathDir_MongoDbTools}\mongodump.exe" --host {SERVER_MONGODB_HOST} --port 27017 --db userData --out "{backup_folder}"
                """);
                break;
            default:
                break;
        }


        // Создание архива
        string archive_name = $@"c:\UnityProjects\Game03_General_Solution\DataBaseBackupArchives\{database}.7z";

        ExecuteBatCommand_and_WaitForExit($"""
            "{Path_7zExe}" a -t7z "{archive_name}" "{backup_folder}\*"
            """);


        // Очистка временных файлов
        if (Directory.Exists(backup_folder))
        {
            Directory.Delete(backup_folder, true);
        }
    }

    static void Restore(Database database)
    {
        if (Directory.Exists(backup_folder))
        {
            Directory.Delete(backup_folder, true);
        }

        if (!Directory.Exists(backup_folder))
        {
            Directory.CreateDirectory(backup_folder);
        }


        // Распаковка архива
        string archive_name = $@"c:\UnityProjects\Game03_General_Solution\DataBaseBackupArchives\{database}.7z";
        ExecuteBatCommand_and_WaitForExit($"""
            "{Path_7zExe}" x -y "{archive_name}" -o"{backup_folder}\*"
            """);



        // Восстановление базы данных
        switch (database)
        {
            case Database.PostgreSql_Users:
            case Database.PostgreSql_Data:
                ExecuteBatCommand_and_WaitForExit($"""
                    pg_restore -U postgres --host={SERVER_POSTGRES_HOST} --clean --if-exists --verbose --dbname={(database == Database.PostgreSql_Users ? "Game03_Users" : "Game03_Data")} > "{backup_folder}\{database}.backup"
                    """);
                break;
            case Database.MongoDb_UserData:
                ExecuteBatCommand_and_WaitForExit($"""
                    "{PathDir_MongoDbTools}\mongorestore.exe" --host {SERVER_MONGODB_HOST} --port 27017 --drop --db userData "{backup_folder}\userData"
                    """);
                break;
            default:
                break;
        }


        // Очистка временных файлов
        if (Directory.Exists(backup_folder))
        {
            Directory.Delete(backup_folder, true);
        }
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
}
