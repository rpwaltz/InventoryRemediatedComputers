using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
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
        private const int ERROR = 1;

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
            Program.password = options.pwd;
            Program.userid = options.userid;
            }

        public Boolean Start()
            {
            bool complete = false;
            try
                {

                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
                
                string strConn = connectionStringsSection.ConnectionStrings["MachineInventory"].ConnectionString;

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(strConn);
                
                builder.Password = Program.password;
                builder.UserID = Program.userid;
                
                connectionStringsSection.ConnectionStrings["MachineInventory"].ConnectionString = builder.ConnectionString;
                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
                
                MachineInventory machineInventory = new MachineInventory();
                MachineComposer machineComposer = new MachineComposer(machineInventory);
                complete = machineComposer.ComposeMachine();
                if (complete)
                    {
                    ApplicationsComposer applicationsComposer = new ApplicationsComposer(machineInventory);
                    complete = applicationsComposer.composeApplicatons();
                    if (complete) 
                        {
                        try
                            {
                            machineInventory.SaveChanges();
                            }
                        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                            {
                            Exception raise = dbEx;

                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                                {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                    {
                                    string message = string.Format("{0}:{1}",
                                        validationErrors.Entry.Entity.ToString(),
                                        validationError.ErrorMessage);
                                    // raise a new exception nesting
                                    // the current instance as InnerException
                                    raise = new InvalidOperationException(message, raise);
                                    }
                                }
                            throw raise;
                            }
                        
                        }
                    }
                }
            catch (Exception ex)
                {

                return false;
                }
            return complete;
            }

        }
    }
