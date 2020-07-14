using System;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using CommandLine;


namespace InventoryRemediatedComputers
    {

    class Options
        {
        [Option('p', "pwd", Required = true,
          HelpText = "Password for DB.")]
        public string pwd { get; set; }

        [Option('u', "userid", Required = true,
          HelpText = "userId for DB.")]
        public string userid { get; set; }

        }

    class Program
        {
        private const int SUCCESS = 0;
        private const int ERROR = -1;

        private static string password;
        private static string userid;

        static void Main(string[] args)
            {
            Options options = new Options();
            ParserResult<Options> parserResult = CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsed(RunOptions);

            Program run = new Program();
            if (run.Start())
                {
                Environment.ExitCode = SUCCESS;
                }
            else
                {
                Environment.ExitCode = ERROR;
                }

            }
        static void RunOptions(Options options)
            {
            System.Diagnostics.Debug.WriteLine(options.pwd);
            System.Diagnostics.Debug.WriteLine(options.userid);
            Program.password = options.pwd;
            Program.userid = options.userid;
            }
        public Boolean Start()
            {
            try
                {
                System.Diagnostics.Debug.WriteLine(Program.userid);
                System.Diagnostics.Debug.WriteLine(Program.password);

                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

                string strConn = connectionStringsSection.ConnectionStrings["MachineInventory"].ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(strConn);

                builder.Password = Program.password;
                builder.UserID = Program.userid;
                connectionStringsSection.ConnectionStrings["MachineInventory"].ConnectionString = builder.ConnectionString;
                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
                System.Diagnostics.Debug.WriteLine(builder.ConnectionString);

                MachineInventory machineInventory = new MachineInventory();
                MachineComposer machineComposer = new MachineComposer(machineInventory);
                bool complete = machineComposer.ComposeMachine();
                ApplicationsComposer applicationsComposer = new ApplicationsComposer(machineInventory);
                complete = applicationsComposer.composeApplicatons();
                machineInventory.SaveChanges();
                }
            catch (Exception ex)
                {
                return false;
                }
            return true;
            }

        }
    }
