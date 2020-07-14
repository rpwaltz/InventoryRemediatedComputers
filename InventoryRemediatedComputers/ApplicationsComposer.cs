using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
/*
 * 
 * Copyright 2020 City of Knoxville, Robert Patrick Waltz and Alexandru-Codrin Panaite

 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * 
 * Code submitted by Alexandru-Codrin Panaite to StackOverflow
 * https://stackoverflow.com/users/10293835/alexandru-codrin-panaite
 * https://stackoverflow.com/a/59804150
 * https://creativecommons.org/licenses/by-sa/4.0/
 * 
 */
namespace InventoryRemediatedComputers
    {
    class ApplicationsComposer
        {

        MachineInventory machineInventory;


        private ExecutableBitness executableBitness = new ExecutableBitness();
        public ApplicationsComposer(MachineInventory machineInventory)
            {
            this.machineInventory = machineInventory;
            }
        public bool composeApplicatons()
            {
            try
                {
                this.composeUninstallList();
                this.composeApplicationsList();
                this.composeAppPathsList();

                }
            catch (Exception ex)
                {

                return false;
                }
            return true;
            }
        private void composeUninstallList()
            {

            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                {
                foreach (string subkey_name in key.GetSubKeyNames())
                    {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                        if (subkey.GetValue("DisplayName") != null)
                            {
                            Application application = new Application
                                {
                                RegistryKey = subkey.Name,
                                Name = (string)subkey.GetValue("DisplayName"),
                                Version = (string)subkey.GetValue("DisplayVersion"),
                                Path = (string)subkey.GetValue("ModifyPath"),
                                Bitness = "NA",
                                MachineName = System.Environment.MachineName
                                };
                            this.machineInventory.Applications.Add(application); ;
                            }
                        }
                    }
                }
            }

        private void composeApplicationsList()
            {

            string registry_key = @"SOFTWARE\Classes\Applications";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                {
                foreach (string subkey_name in key.GetSubKeyNames())
                    {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                        String path = "NA";
                        String bitness = "NA";
                        RegistryKey execPathKey = subkey.OpenSubKey(@"shell\edit\command");
                        if (execPathKey == null)
                            {
                            execPathKey = subkey.OpenSubKey(@"shell\open\command");
                            if (execPathKey == null)
                                {
                                execPathKey = subkey.OpenSubKey(@"shell\read\command");
                                }
                            }
                        if (execPathKey != null)
                            {
                            path = (string)execPathKey?.GetValue("");
                            if (path != null)
                                {
                                if (path.Contains("\""))
                                    {

                                    int doubleQuoteIndex = path.IndexOf("\"");
                                    path = path.Substring(doubleQuoteIndex + 1, path.Length - 1);
                                    doubleQuoteIndex = path.IndexOf("\"");
                                    path = path.Substring(0, doubleQuoteIndex);
                                    }
                                else
                                    {
                                    int spaceIndex = path.IndexOf(" ");
                                    path = path.Substring(0, spaceIndex);
                                    }
                                }
                            if (File.Exists(path))
                                {
                                bitness = this.getBitness(path);
                                }

                            }

                        String displayName = (string)subkey.Name;
                        displayName = displayName.Substring(displayName.LastIndexOf('\\') + 1);
                        Application application = new Application
                            {
                            RegistryKey = subkey.Name,
                            Name = displayName,
                            Version = null,
                            Path = path,
                            Bitness = bitness,
                            MachineName = System.Environment.MachineName
                            };
                        this.machineInventory.Applications.Add(application);
                        }
                    }
                }

            }

        private void composeAppPathsList()
            {

            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                {
                foreach (string subkey_name in key.GetSubKeyNames())
                    {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                        String displayName = (string)subkey.Name;
                        displayName = displayName.Substring(displayName.LastIndexOf('\\') + 1);
                        String path;

                        String bitness = "NA";

                        path = (string)subkey?.GetValue("");
                        if (path != null)
                            {

                            path = Environment.ExpandEnvironmentVariables(path);
                            if (path.Contains("\""))
                                {

                                int doubleQuoteIndex = path.IndexOf("\"");
                                path = path.Substring(doubleQuoteIndex + 1, path.Length - 1);
                                doubleQuoteIndex = path.IndexOf("\"");
                                path = path.Substring(0, doubleQuoteIndex);
                                }
                            if (File.Exists(path))
                                {
                                bitness = this.getBitness(path);
                                }
                            }
                        else
                            {
                            path = "NA";
                            }
                        Application application = new Application
                            {
                            RegistryKey = subkey.Name,
                            Name = displayName,
                            Version = null,
                            Path = path,
                            Bitness = bitness,
                            MachineName = System.Environment.MachineName
                            };
                        this.machineInventory.Applications.Add(application);


                        }
                    }
                }
            }

        private String getInstalledProgramPath(String path, String displayName)
            {

            if (path != null)
                {
                if (Directory.Exists(path))
                    {
                    String execFilepath = path + Path.DirectorySeparatorChar + displayName;
                    if (File.Exists(execFilepath))
                        {
                        return execFilepath;
                        }
                    }
                }
            return "NA";
            }
        private String getBitness(string filePath)
            {

            Boolean isX64 = executableBitness.isInstalledProgramX64(filePath);
            if (isX64)
                {
                return ("x64");
                }
            else
                {
                Boolean isX32 = executableBitness.isInstalledProgramX32(filePath);
                if (isX32)
                    {
                    return "x32";
                    };
                }
            return null;
            }
        }

    }


