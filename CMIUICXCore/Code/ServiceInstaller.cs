using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CMIUICXCore.Code
{
    /// <summary>
    /// Defines the ServiceInstallOptions.
    /// </summary>
    public enum ServiceInstallOptions
    {
        /// <summary>
        /// Defines the Automatic.
        /// </summary>
        Automatic = ServiceBootFlag.Automatic,

        /// <summary>
        /// Defines the Manual.
        /// </summary>
        Manual = ServiceBootFlag.Manual,

        /// <summary>
        /// Defines the Disabled.
        /// </summary>
        Disabled = ServiceBootFlag.Disabled,

        /// <summary>
        /// Defines the Default.
        /// </summary>
        Default = Automatic
    }

    /// <summary>
    /// Инструменты для работы со службами Windows.
    /// </summary>
    public static class ServiceInstaller
    {
        //private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        /// <summary>
        /// Defines the SERVICE_WIN32_OWN_PROCESS.
        /// </summary>
        private const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;

        /// <summary>
        /// Defines the SERVICE_CONFIG_DESCRIPTION.
        /// </summary>
        private const int SERVICE_CONFIG_DESCRIPTION = 1;

        /// <summary>
        /// Defines the SERVICE_CONFIG_FAILURE_ACTIONS.
        /// </summary>
        private const int SERVICE_CONFIG_FAILURE_ACTIONS = 2;

        /// <summary>
        /// Defines the ERROR_ACCESS_DENIED.
        /// </summary>
        private const int ERROR_ACCESS_DENIED = 5;

        /// <summary>
        /// The OpenSCManager.
        /// </summary>
        /// <param name="machineName">The machineName<see cref="string"/>.</param>
        /// <param name="databaseName">The databaseName<see cref="string"/>.</param>
        /// <param name="dwDesiredAccess">The dwDesiredAccess<see cref="ScmAccessRights"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenSCManager(string machineName, string databaseName, ScmAccessRights dwDesiredAccess);

        /// <summary>
        /// The OpenService.
        /// </summary>
        /// <param name="hSCManager">The hSCManager<see cref="IntPtr"/>.</param>
        /// <param name="lpServiceName">The lpServiceName<see cref="string"/>.</param>
        /// <param name="dwDesiredAccess">The dwDesiredAccess<see cref="ServiceAccessRights"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "OpenServiceW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, ServiceAccessRights dwDesiredAccess);

        /// <summary>
        /// The CreateService.
        /// </summary>
        /// <param name="hSCManager">The hSCManager<see cref="IntPtr"/>.</param>
        /// <param name="lpServiceName">The lpServiceName<see cref="string"/>.</param>
        /// <param name="lpDisplayName">The lpDisplayName<see cref="string"/>.</param>
        /// <param name="dwDesiredAccess">The dwDesiredAccess<see cref="ServiceAccessRights"/>.</param>
        /// <param name="dwServiceType">The dwServiceType<see cref="int"/>.</param>
        /// <param name="dwStartType">The dwStartType<see cref="ServiceBootFlag"/>.</param>
        /// <param name="dwErrorControl">The dwErrorControl<see cref="ServiceError"/>.</param>
        /// <param name="lpBinaryPathName">The lpBinaryPathName<see cref="string"/>.</param>
        /// <param name="lpLoadOrderGroup">The lpLoadOrderGroup<see cref="string"/>.</param>
        /// <param name="lpdwTagId">The lpdwTagId<see cref="IntPtr"/>.</param>
        /// <param name="lpDependencies">The lpDependencies<see cref="string"/>.</param>
        /// <param name="lp">The lp<see cref="string"/>.</param>
        /// <param name="lpPassword">The lpPassword<see cref="string"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "CreateServiceW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName, ServiceAccessRights dwDesiredAccess, int dwServiceType, ServiceBootFlag dwStartType, ServiceError dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lp, string lpPassword);

        /// <summary>
        /// The CloseServiceHandle.
        /// </summary>
        /// <param name="hSCObject">The hSCObject<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "CloseServiceHandle", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseServiceHandle(IntPtr hSCObject);

        /// <summary>
        /// The QueryServiceStatus.
        /// </summary>
        /// <param name="hService">The hService<see cref="IntPtr"/>.</param>
        /// <param name="lpServiceStatus">The lpServiceStatus<see cref="SERVICE_STATUS"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "QueryServiceStatus", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int QueryServiceStatus(IntPtr hService, SERVICE_STATUS lpServiceStatus);

        /// <summary>
        /// The DeleteService.
        /// </summary>
        /// <param name="hService">The hService<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "DeleteService", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteService(IntPtr hService);

        /// <summary>
        /// The ControlService.
        /// </summary>
        /// <param name="hService">The hService<see cref="IntPtr"/>.</param>
        /// <param name="dwControl">The dwControl<see cref="ServiceControl"/>.</param>
        /// <param name="lpServiceStatus">The lpServiceStatus<see cref="SERVICE_STATUS"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "ControlService", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int ControlService(IntPtr hService, ServiceControl dwControl, SERVICE_STATUS lpServiceStatus);

        /// <summary>
        /// The StartService.
        /// </summary>
        /// <param name="hService">The hService<see cref="IntPtr"/>.</param>
        /// <param name="dwNumServiceArgs">The dwNumServiceArgs<see cref="int"/>.</param>
        /// <param name="lpServiceArgVectors">The lpServiceArgVectors<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "StartServiceW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);

        /// <summary>
        /// The ChangeServiceDescription.
        /// </summary>
        /// <param name="hService">The hService<see cref="IntPtr"/>.</param>
        /// <param name="dwInfoLevel">The dwInfoLevel<see cref="int"/>.</param>
        /// <param name="lpInfo">The lpInfo<see cref="SERVICE_DESCRIPTION"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2W", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeServiceDescription(IntPtr hService, int dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_DESCRIPTION lpInfo);

        /// <summary>
        /// The ChangeServiceFailureActions.
        /// </summary>
        /// <param name="hService">The hService<see cref="IntPtr"/>.</param>
        /// <param name="dwInfoLevel">The dwInfoLevel<see cref="int"/>.</param>
        /// <param name="lpInfo">The lpInfo<see cref="SERVICE_FAILURE_ACTIONS"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        [DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2W", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeServiceFailureActions(IntPtr hService, int dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref SERVICE_FAILURE_ACTIONS lpInfo);

        /// <summary>
        /// The Install.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        /// <param name="displayName">The displayName<see cref="string"/>.</param>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        /// <param name="bootFlag">The bootFlag<see cref="ServiceBootFlag"/>.</param>
        public static void Install(string serviceName, string displayName, string fileName, ServiceBootFlag bootFlag = ServiceBootFlag.Automatic)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);

                if (service == IntPtr.Zero)
                    service = CreateService(scm, serviceName, displayName, ServiceAccessRights.AllAccess, SERVICE_WIN32_OWN_PROCESS, bootFlag, ServiceError.Normal, fileName, null, IntPtr.Zero, null, null, null);

                if (service == IntPtr.Zero)
                {
                    if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                        throw new ApplicationException("Access denied while installing service");
                    else
                        throw new ApplicationException("Failed to install service " + Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// The Uninstall.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        public static void Uninstall(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Service not installed.");

                try
                {
                    StopService(service);
                    if (!DeleteService(service))
                    {
                        if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                            throw new ApplicationException("Access denied while uninstalling service");
                        else
                            throw new ApplicationException("Could not delete service " + Marshal.GetLastWin32Error());
                    }
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// The ServiceIsInstalled.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool ServiceIsInstalled(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);

                if (service == IntPtr.Zero)
                    return false;

                CloseServiceHandle(service);
                return true;
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// The InstallAndStart.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        /// <param name="displayName">The displayName<see cref="string"/>.</param>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        public static void InstallAndStart(string serviceName, string displayName, string fileName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);

                if (service == IntPtr.Zero)
                    service = CreateService(scm, serviceName, displayName, ServiceAccessRights.AllAccess, SERVICE_WIN32_OWN_PROCESS, ServiceBootFlag.Automatic, ServiceError.Normal, fileName, null, IntPtr.Zero, null, null, null);

                if (service == IntPtr.Zero)
                {
                    if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                        throw new ApplicationException("Access denied while installing service");
                    else
                        throw new ApplicationException("Failed to install service " + Marshal.GetLastWin32Error());
                }

                try
                {
                    StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// The StartService.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        public static void StartService(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
                if (service == IntPtr.Zero)
                {
                    if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                        throw new ApplicationException("Access denied while opening service");
                    else
                        throw new ApplicationException("Failed to open service " + Marshal.GetLastWin32Error());
                }

                try
                {
                    StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// The StopService.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        public static void StopService(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Stop);
                if (service == IntPtr.Zero)
                {
                    if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                        throw new ApplicationException("Access denied while opening service");
                    else
                        throw new ApplicationException("Failed to open service " + Marshal.GetLastWin32Error());
                }

                try
                {
                    StopService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// The StartService.
        /// </summary>
        /// <param name="service">The service<see cref="IntPtr"/>.</param>
        private static void StartService(IntPtr service)
        {
            //SERVICE_STATUS status = new SERVICE_STATUS();
            _ = StartService(service, 0, 0);
            var changedStatus = WaitForServiceStatus(service, ServiceState.StartPending, ServiceState.Running);
            if (!changedStatus)
                throw new ApplicationException("Unable to start service");
        }

        /// <summary>
        /// The StopService.
        /// </summary>
        /// <param name="service">The service<see cref="IntPtr"/>.</param>
        private static void StopService(IntPtr service)
        {
            SERVICE_STATUS status = new();
            _ = ControlService(service, ServiceControl.Stop, status);
            var changedStatus = WaitForServiceStatus(service, ServiceState.StopPending, ServiceState.Stopped);
            if (!changedStatus)
                throw new ApplicationException("Unable to stop service");
        }

        /// <summary>
        /// The GetServiceStatus.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        /// <returns>The <see cref="ServiceState"/>.</returns>
        public static ServiceState GetServiceStatus(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    return ServiceState.NotFound;

                try
                {
                    return GetServiceStatus(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// The SetServiceDescription.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        /// <param name="description">The description<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool SetServiceDescription(string serviceName, string description)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);

            var pinfo = new SERVICE_DESCRIPTION
            {
                lpDescription = description
            };

            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.ChangeConfig);
                if (service == IntPtr.Zero)
                    return false;
                try
                {
                    return ChangeServiceDescription(service, SERVICE_CONFIG_DESCRIPTION, ref pinfo);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        /// <summary>
        /// The GetServiceStatus.
        /// </summary>
        /// <param name="service">The service<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="ServiceState"/>.</returns>
        private static ServiceState GetServiceStatus(IntPtr service)
        {
            SERVICE_STATUS status = new();

            if (QueryServiceStatus(service, status) == 0)
                throw new ApplicationException("Failed to query service status.");

            return status.dwCurrentState;
        }

        /// <summary>
        /// The WaitForServiceStatus.
        /// </summary>
        /// <param name="service">The service<see cref="IntPtr"/>.</param>
        /// <param name="waitStatus">The waitStatus<see cref="ServiceState"/>.</param>
        /// <param name="desiredStatus">The desiredStatus<see cref="ServiceState"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool WaitForServiceStatus(IntPtr service, ServiceState waitStatus, ServiceState desiredStatus)
        {
            SERVICE_STATUS status = new();

            _ = QueryServiceStatus(service, status);
            if (status.dwCurrentState == desiredStatus) return true;

            int dwStartTickCount = Environment.TickCount;
            int dwOldCheckPoint = status.dwCheckPoint;

            while (status.dwCurrentState == waitStatus)
            {
                // Do not wait longer than the wait hint. A good interval is
                // one tenth the wait hint, but no less than 1 second and no
                // more than 10 seconds.

                int dwWaitTime = status.dwWaitHint / 10;

                if (dwWaitTime < 1000) dwWaitTime = 1000;
                else if (dwWaitTime > 10000) dwWaitTime = 10000;

                Thread.Sleep(dwWaitTime);

                // Check the status again.

                if (QueryServiceStatus(service, status) == 0) break;

                if (status.dwCheckPoint > dwOldCheckPoint)
                {
                    // The service is making progress.
                    dwStartTickCount = Environment.TickCount;
                    dwOldCheckPoint = status.dwCheckPoint;
                }
                else
                {
                    if (Environment.TickCount - dwStartTickCount > status.dwWaitHint)
                    {
                        // No progress made within the wait hint
                        break;
                    }
                }
            }
            return (status.dwCurrentState == desiredStatus);
        }

        /// <summary>
        /// The OpenSCManager.
        /// </summary>
        /// <param name="rights">The rights<see cref="ScmAccessRights"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        private static IntPtr OpenSCManager(ScmAccessRights rights)
        {
            IntPtr scm = OpenSCManager(null, null, rights);
            if (scm == IntPtr.Zero)
            {
                if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                    throw new ApplicationException("Access denied while connecting to service control manager");
                else
                    throw new ApplicationException("Failed to connect to service control manager " + Marshal.GetLastWin32Error());
            }

            return scm;
        }

        /// <summary>
        /// The SetRecoveryOptions.
        /// </summary>
        /// <param name="serviceName">The serviceName<see cref="string"/>.</param>
        /// <param name="pFirstFailure">The pFirstFailure<see cref="SC_ACTION"/>.</param>
        /// <param name="pSecondFailure">The pSecondFailure<see cref="SC_ACTION"/>.</param>
        /// <param name="pSubsequentFailures">The pSubsequentFailures<see cref="SC_ACTION"/>.</param>
        /// <param name="pDaysToResetFailureCount">The pDaysToResetFailureCount<see cref="int"/>.</param>
        public static void SetRecoveryOptions(string serviceName, SC_ACTION pFirstFailure, SC_ACTION pSecondFailure, SC_ACTION pSubsequentFailures, int pDaysToResetFailureCount = 0)
        {
            int NUM_ACTIONS = 3;
            int[] arrActions = new int[NUM_ACTIONS * 2];
            int index = 0;
            arrActions[index++] = (int)pFirstFailure.Type;
            arrActions[index++] = pFirstFailure.Delay;
            arrActions[index++] = (int)pSecondFailure.Type;
            arrActions[index++] = pSecondFailure.Delay;
            arrActions[index++] = (int)pSubsequentFailures.Type;
            arrActions[index++] = pSubsequentFailures.Delay;

            IntPtr tmpBuff = Marshal.AllocHGlobal(NUM_ACTIONS * 8);

            try
            {
                Marshal.Copy(arrActions, 0, tmpBuff, NUM_ACTIONS * 2);
                SERVICE_FAILURE_ACTIONS sfa = new()
                {
                    cActions = 3,
                    dwResetPeriod = pDaysToResetFailureCount,
                    lpCommand = null,
                    lpRebootMsg = null,
                    lpsaActions = new IntPtr(tmpBuff.ToInt64())
                };

                IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);

                if (service == IntPtr.Zero)
                {
                    if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                        throw new ApplicationException("Access denied while opening service");
                    else
                        throw new ApplicationException("Unknown error while opening service");
                }
                bool success = ChangeServiceFailureActions(service, SERVICE_CONFIG_FAILURE_ACTIONS, ref sfa);
                if (!success)
                {
                    if (Marshal.GetLastWin32Error() == ERROR_ACCESS_DENIED)
                        throw new ApplicationException("Access denied while setting failure actions");
                    else
                        throw new ApplicationException("Unknown error while setting failure actions");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(tmpBuff);
            }
        }
    }

    /// <summary>
    /// Defines the ServiceState.
    /// </summary>
    public enum ServiceState
    {
        /// <summary>
        /// Defines the Unknown.
        /// </summary>
        Unknown = -1, // The state cannot be (has not been) retrieved.

        /// <summary>
        /// Defines the NotFound.
        /// </summary>
        NotFound = 0, // The service is not known on the host server.

        /// <summary>
        /// Defines the Stopped.
        /// </summary>
        Stopped = 1,

        /// <summary>
        /// Defines the StartPending.
        /// </summary>
        StartPending = 2,

        /// <summary>
        /// Defines the StopPending.
        /// </summary>
        StopPending = 3,

        /// <summary>
        /// Defines the Running.
        /// </summary>
        Running = 4,

        /// <summary>
        /// Defines the ContinuePending.
        /// </summary>
        ContinuePending = 5,

        /// <summary>
        /// Defines the PausePending.
        /// </summary>
        PausePending = 6,

        /// <summary>
        /// Defines the Paused.
        /// </summary>
        Paused = 7
    }

    /// <summary>
    /// Defines the ScmAccessRights.
    /// </summary>
    public enum ScmAccessRights
    {
        /// <summary>
        /// Defines the Connect.
        /// </summary>
        Connect = 0x0001,

        /// <summary>
        /// Defines the CreateService.
        /// </summary>
        CreateService = 0x0002,

        /// <summary>
        /// Defines the EnumerateService.
        /// </summary>
        EnumerateService = 0x0004,

        /// <summary>
        /// Defines the Lock.
        /// </summary>
        Lock = 0x0008,

        /// <summary>
        /// Defines the QueryLockStatus.
        /// </summary>
        QueryLockStatus = 0x0010,

        /// <summary>
        /// Defines the ModifyBootConfig.
        /// </summary>
        ModifyBootConfig = 0x0020,

        /// <summary>
        /// Defines the StandardRightsRequired.
        /// </summary>
        StandardRightsRequired = 0xF0000,

        /// <summary>
        /// Defines the AllAccess.
        /// </summary>
        AllAccess = (StandardRightsRequired | Connect | CreateService |
                     EnumerateService | Lock | QueryLockStatus | ModifyBootConfig)
    }

    /// <summary>
    /// Defines the ServiceAccessRights.
    /// </summary>
    public enum ServiceAccessRights
    {
        /// <summary>
        /// Defines the QueryConfig.
        /// </summary>
        QueryConfig = 0x1,

        /// <summary>
        /// Defines the ChangeConfig.
        /// </summary>
        ChangeConfig = 0x2,

        /// <summary>
        /// Defines the QueryStatus.
        /// </summary>
        QueryStatus = 0x4,

        /// <summary>
        /// Defines the EnumerateDependants.
        /// </summary>
        EnumerateDependants = 0x8,

        /// <summary>
        /// Defines the Start.
        /// </summary>
        Start = 0x10,

        /// <summary>
        /// Defines the Stop.
        /// </summary>
        Stop = 0x20,

        /// <summary>
        /// Defines the PauseContinue.
        /// </summary>
        PauseContinue = 0x40,

        /// <summary>
        /// Defines the Interrogate.
        /// </summary>
        Interrogate = 0x80,

        /// <summary>
        /// Defines the UserDefinedControl.
        /// </summary>
        UserDefinedControl = 0x100,

        /// <summary>
        /// Defines the Delete.
        /// </summary>
        Delete = 0x00010000,

        /// <summary>
        /// Defines the StandardRightsRequired.
        /// </summary>
        StandardRightsRequired = 0xF0000,

        /// <summary>
        /// Defines the AllAccess.
        /// </summary>
        AllAccess = (StandardRightsRequired | QueryConfig | ChangeConfig |
                     QueryStatus | EnumerateDependants | Start | Stop | PauseContinue |
                     Interrogate | UserDefinedControl)
    }

    /// <summary>
    /// Defines the ServiceBootFlag.
    /// </summary>
    [Flags]
    public enum ServiceBootFlag
    {
        /// <summary>
        /// Defines the Start.
        /// </summary>
        Start = 0x00000000,

        /// <summary>
        /// Defines the SystemStart.
        /// </summary>
        SystemStart = 0x00000001,
        /// <summary>
        /// Defines the Automatic.
        /// </summary>
        Automatic = 0x00000002,
        /// <summary>
        /// Defines the Manual.
        /// </summary>
        Manual = 0x00000003,

        /// <summary>
        /// Defines the Disabled.
        /// </summary>
        Disabled = 0x00000004
    }

    /// <summary>
    /// Defines the ServiceControl.
    /// </summary>
    public enum ServiceControl
    {
        /// <summary>
        /// Defines the Stop.
        /// </summary>
        Stop = 0x00000001,

        /// <summary>
        /// Defines the Pause.
        /// </summary>
        Pause = 0x00000002,

        /// <summary>
        /// Defines the Continue.
        /// </summary>
        Continue = 0x00000003,

        /// <summary>
        /// Defines the Interrogate.
        /// </summary>
        Interrogate = 0x00000004,

        /// <summary>
        /// Defines the Shutdown.
        /// </summary>
        Shutdown = 0x00000005,

        /// <summary>
        /// Defines the ParamChange.
        /// </summary>
        ParamChange = 0x00000006,

        /// <summary>
        /// Defines the NetBindAdd.
        /// </summary>
        NetBindAdd = 0x00000007,

        /// <summary>
        /// Defines the NetBindRemove.
        /// </summary>
        NetBindRemove = 0x00000008,

        /// <summary>
        /// Defines the NetBindEnable.
        /// </summary>
        NetBindEnable = 0x00000009,

        /// <summary>
        /// Defines the NetBindDisable.
        /// </summary>
        NetBindDisable = 0x0000000A
    }

    /// <summary>
    /// Defines the ServiceError.
    /// </summary>
    public enum ServiceError
    {
        /// <summary>
        /// Defines the Ignore.
        /// </summary>
        Ignore = 0x00000000,

        /// <summary>
        /// Defines the Normal.
        /// </summary>
        Normal = 0x00000001,

        /// <summary>
        /// Defines the Severe.
        /// </summary>
        Severe = 0x00000002,

        /// <summary>
        /// Defines the Critical.
        /// </summary>
        Critical = 0x00000003
    }

    /// <summary>
    /// Defines the ScActionType.
    /// </summary>
    public enum ScActionType
    {
        /// <summary>
        /// Defines the SC_ACTION_NONE.
        /// </summary>
        SC_ACTION_NONE = 0,         // No action.

        /// <summary>
        /// Defines the SC_ACTION_RESTART.
        /// </summary>
        SC_ACTION_RESTART = 1,      // Restart the service.

        /// <summary>
        /// Defines the SC_ACTION_REBOOT.
        /// </summary>
        SC_ACTION_REBOOT = 2,       // Reboot the computer.

        /// <summary>
        /// Defines the SC_ACTION_RUN_COMMAND.
        /// </summary>
        SC_ACTION_RUN_COMMAND = 3// Run a command.  
    }

    /// <summary>
    /// Defines the <see cref="SERVICE_DESCRIPTION" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVICE_DESCRIPTION
    {
        /// <summary>
        /// Defines the lpDescription.
        /// </summary>
        public string lpDescription;

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            return string.Equals(lpDescription, (string)obj);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(lpDescription);
        }

        /// <summary>
        /// Equal operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SERVICE_DESCRIPTION left, SERVICE_DESCRIPTION right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Notequal operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SERVICE_DESCRIPTION left, SERVICE_DESCRIPTION right)
        {
            return !(left == right);
        }
    }

    /// <summary>
    /// Defines the <see cref="SERVICE_FAILURE_ACTIONS" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SERVICE_FAILURE_ACTIONS
    {
        /// <summary>
        /// Defines the dwResetPeriod.
        /// </summary>
        public int dwResetPeriod;

        /// <summary>
        /// Defines the lpRebootMsg.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpRebootMsg;

        /// <summary>
        /// Defines the lpCommand.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpCommand;

        /// <summary>
        /// Defines the cActions.
        /// </summary>
        public int cActions;

        /// <summary>
        /// Defines the lpsaActions.
        /// </summary>
        public IntPtr lpsaActions;

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            return dwResetPeriod == ((SERVICE_FAILURE_ACTIONS)obj).dwResetPeriod &&
                lpRebootMsg == ((SERVICE_FAILURE_ACTIONS)obj).lpRebootMsg &&
                lpCommand == ((SERVICE_FAILURE_ACTIONS)obj).lpCommand &&
                cActions == ((SERVICE_FAILURE_ACTIONS)obj).cActions &&
                lpsaActions == ((SERVICE_FAILURE_ACTIONS)obj).lpsaActions;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(dwResetPeriod, lpRebootMsg, lpCommand, cActions, lpsaActions);
        }

        /// <summary>
        /// Equal operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SERVICE_FAILURE_ACTIONS left, SERVICE_FAILURE_ACTIONS right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Notequal operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SERVICE_FAILURE_ACTIONS left, SERVICE_FAILURE_ACTIONS right)
        {
            return !(left == right);
        }
    }

    /// <summary>
    /// Defines the <see cref="SERVICE_STATUS" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class SERVICE_STATUS
    {
        /// <summary>
        /// Defines the dwServiceType.
        /// </summary>
        public int dwServiceType;

        /// <summary>
        /// Defines the dwCurrentState.
        /// </summary>
        public ServiceState dwCurrentState;

        /// <summary>
        /// Defines the dwControlsAccepted.
        /// </summary>
        public int dwControlsAccepted;

        /// <summary>
        /// Defines the dwWin32ExitCode.
        /// </summary>
        public int dwWin32ExitCode;

        /// <summary>
        /// Defines the dwServiceSpecificExitCode.
        /// </summary>
        public int dwServiceSpecificExitCode;

        /// <summary>
        /// Defines the dwCheckPoint.
        /// </summary>
        public int dwCheckPoint;

        /// <summary>
        /// Defines the dwWaitHint.
        /// </summary>
        public int dwWaitHint;
    }

    /// <summary>
    /// Defines the <see cref="SC_ACTION" />.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SC_ACTION
    {
        /// <summary>
        /// Defines the Type.
        /// </summary>
        public ScActionType Type;

        /// <summary>
        /// Defines the Delay.
        /// </summary>
        public int Delay;

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object obj)
        {
            return Type == ((SC_ACTION)obj).Type &&
                Delay == ((SC_ACTION)obj).Delay;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Delay);
        }

        /// <summary>
        /// Equal operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SC_ACTION left, SC_ACTION right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Notequal operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SC_ACTION left, SC_ACTION right)
        {
            return !(left == right);
        }
    }
}
