using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
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

            System.Diagnostics.Debug.WriteLine(System.Environment.MachineName);
            if (System.Environment.Is64BitOperatingSystem)
                {
                System.Diagnostics.Debug.WriteLine("64 bit OS");
                }
            else
                {
                System.Diagnostics.Debug.WriteLine("32 bit OS");
                }

            /* check out legal notices
* 
*/
            string systemsPolicyString = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
            RegistryKey systemsPolicyKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(systemsPolicyString);
            String legalNoticeCaption = (String)systemsPolicyKey.GetValue("legalnoticecaption");
            String legalNoticeText = (String)systemsPolicyKey.GetValue("legalnoticetext");
            bool hasHackedLegalNotice = false;
            if (legalNoticeCaption.Length > 0 || legalNoticeText.Length > 0)
                {
                System.Diagnostics.Debug.WriteLine("found legalNotice: " + legalNoticeCaption + " " + legalNoticeText);
                hasHackedLegalNotice = true;
                }
            string ntCurrentVersionString = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            RegistryKey ntCurrentVersionKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(ntCurrentVersionString);
            String productName = (String)ntCurrentVersionKey.GetValue("ProductName");
            String releaseId = (String)ntCurrentVersionKey.GetValue("ReleaseId");
            Machine machine = new Machine { MachineName = System.Environment.MachineName, isOSx64 = System.Environment.Is64BitOperatingSystem, ProductName = productName, Version = releaseId, hasRegistryHack = hasHackedLegalNotice };

            System.Diagnostics.Debug.WriteLine("ProductName " + productName + " ReleaseId " + releaseId);

            this.machineInventory.Machines.Add(machine);
            return true;
            }
        }
    }
