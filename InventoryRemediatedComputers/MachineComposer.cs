using System;
using Microsoft.Win32;


namespace InventoryRemediatedComputers
    {
    class MachineComposer
        {
        private MachineInventory machineInventory;
        public MachineComposer(MachineInventory machineInventory)
            {
            this.machineInventory = machineInventory;
            }
        public bool ComposeMachine()
            {

            /* check out legal notices
* 
*/
            string systemsPolicyString = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
            RegistryKey systemsPolicyKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(systemsPolicyString);
            String legalNoticeCaption = (String)systemsPolicyKey.GetValue("legalnoticecaption");
            String legalNoticeText = (String)systemsPolicyKey.GetValue("legalnoticetext");
            bool hasHackedLegalNotice = false;
            if (!(String.IsNullOrWhiteSpace(legalNoticeCaption) || String.IsNullOrWhiteSpace(legalNoticeText)))
                {
                
                hasHackedLegalNotice = true;
                }
            string ntCurrentVersionString = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            RegistryKey ntCurrentVersionKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(ntCurrentVersionString);
            String productName = (String)ntCurrentVersionKey.GetValue("ProductName");
            String releaseId;
            if (productName.Contains("10"))
                {
                releaseId = (String)ntCurrentVersionKey.GetValue("ReleaseId");
                }
            else
                {
                string csdVerion = (String)ntCurrentVersionKey.GetValue("CSDVersion");
                string csdBuildNumber = (String)ntCurrentVersionKey.GetValue("CSDBuildNumber");
                releaseId = csdVerion + " " + csdBuildNumber;
                }

            if (String.IsNullOrWhiteSpace(releaseId))
                {
                releaseId = "NA";
                }
            Machine machine = new Machine { MachineName = System.Environment.MachineName, isOSx64 = System.Environment.Is64BitOperatingSystem, ProductName = productName, Version = releaseId, hasRegistryHack = hasHackedLegalNotice };


            this.machineInventory.Machines.Add(machine);
            return true;
            }
        }
    }
