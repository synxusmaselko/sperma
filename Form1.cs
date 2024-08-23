using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Net;
using System.Drawing.Drawing2D;
using System.Drawing;
using Flux;
using System.Drawing.Text;
using System.Reflection;
using System.Security.Principal;
using System.Security.Cryptography;


namespace Nebula_Spoofer
{
    public partial class Form1 : Form
    {

        private const string V = "X";



        private bool isDragging = false; // Declare isDragging variable here
        private Point clickOffset;

        public class NoHoverButton : Button
        {
            protected override void OnPaint(PaintEventArgs pevent)
            {
                // Draw the button without the border but keep the background image
                pevent.Graphics.Clear(this.BackColor);
                if (this.BackgroundImage != null)
                {
                    pevent.Graphics.DrawImage(this.BackgroundImage, ClientRectangle);
                }
                else
                {
                    pevent.Graphics.FillRectangle(new SolidBrush(this.BackColor), ClientRectangle);
                }
                TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, ClientRectangle, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                // Do nothing to prevent default hover effect
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                // Do nothing to prevent default hover effect
            }
        }



        public class RoundedButton : Button
        {
            public int BorderRadius { get; set; } = 20;

            public RoundedButton()
            {
                this.FlatStyle = FlatStyle.Flat;
                this.FlatAppearance.BorderSize = 0;
                this.BackColor = Color.LightBlue;
                this.ForeColor = Color.White;

                // Enable custom painting and optimized double buffering
                this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.UserPaint |
                              ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.ResizeRedraw, true);
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Get button rectangle
                Rectangle rect = this.ClientRectangle;

                // Create rounded path
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, BorderRadius, BorderRadius, 180, 90);
                    path.AddArc(rect.Right - BorderRadius, rect.Y, BorderRadius, BorderRadius, 270, 90);
                    path.AddArc(rect.Right - BorderRadius, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - BorderRadius, BorderRadius, BorderRadius, 90, 90);
                    path.CloseFigure();

                    // Clear the background
                    pevent.Graphics.Clear(this.Parent.BackColor);

                    // Fill button background
                    using (SolidBrush brush = new SolidBrush(this.BackColor))
                    {
                        pevent.Graphics.FillPath(brush, path);
                    }

                    // Draw button border
                    using (Pen pen = new Pen(this.ForeColor, 1))
                    {
                        pevent.Graphics.DrawPath(pen, path);
                    }

                    // Draw button text
                    TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, rect, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }

            protected override void OnPaintBackground(PaintEventArgs pevent)
            {
                // Do nothing here to avoid default background painting
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                this.Invalidate();  // Redraw the button when resized
            }
        }
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 30);

            this.MouseDown += MainForm_MouseDown;
            this.MouseMove += MainForm_MouseMove;
            this.MouseUp += MainForm_MouseUp;


            Button closeButton = new RoundedButton();

            closeButton.Font = new Font("Arial", 12, FontStyle.Bold);
            closeButton.BackColor = Color.FromArgb(199, 170, 246);
            closeButton.ForeColor = Color.FromArgb(30, 30, 30);
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0; // Border thickness
            closeButton.FlatAppearance.BorderColor = Color.Black;
            closeButton.Size = new Size(35, 35);
            closeButton.Location = new Point(this.Width - closeButton.Width - 20, 20);
            closeButton.Click += CloseButton_Click;



            this.Controls.Add(closeButton);
        }




        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            // Close the form
            ProcessExitedHandler(sender, e);
            this.Close();
        }



        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            clickOffset = e.Location;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - clickOffset.X;
                newLocation.Y += e.Y - clickOffset.Y;
                this.Location = newLocation;
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Delete the temporary directory if it exists
            string tempDirPath = @"C:\Windows\IME\";

            // Define the commands to be executed in the batch script
            string batchCommands = @"
        @echo off
setlocal EnableDelayedExpansion
pushd %~dp0
title bf3k on discord
echo MADE BY BF3K ON DISCORD
echo MADE BY BF3K ON DISCORD
echo.

choice /C YN /M ""Do you want to proceed with spoofing?""
if errorlevel 2 goto No
if errorlevel 1 goto Yes

:Yes
echo Running Spoofer

set ""pool=0123456789ABCDEF""

for /f ""skip=1 delims="" %%a in ('wmic nicconfig where ""IPEnabled=true"" get MACAddress^,IPAddress /format:list ^| findstr /r /v ""^$""') do (
    set ""OGMAC=%%a""
    goto :exit_loop
)

:exit_loop

for /l %%j in (1,1,6) do (
    set ""spoof%%j=""
    for /l %%i in (1,1,15) do (
        :: Generate a random index between 0 and 15 (inclusive)
        set /a ""index=!random! %% 16""
        
        :: Extract the character at the random index from the pool
        for %%k in (!index!) do set ""char=!pool:~%%k,1!""
        
        :: Append the character to the spoof variable
        set ""spoof%%j=!spoof%%j!!char!""
    )
)

set UUID=false
set BIOS=false
set Chassis=false
set Baseboard=false
set CPU=false
set MISC=false
set VOLUMEID=false
set MAC=false

REM UUID
AMIDEWINx64.EXE /SU auto > UUID.txt
findstr /c:""(/SU)System UUID             W    Done"" UUID.txt >nul
if not errorlevel 1 (
    set UUID=true
)
if %UUID% == true (
    echo UUID Spoof Successful
) else (
    echo UUID Spoof Failed
    type UUID.txt
)

REM BIOS
AMIDEWINx64.EXE /SS !spoof1! > BIOS.txt
findstr /c:""(/SS)System Serial number    W    Done"" BIOS.txt >nul
if not errorlevel 1 (
    set BIOS=true
)
if %BIOS% == true (
    echo BIOS Spoof Successful
) else (
    echo BIOS Spoof Failed
    type BIOS.txt
)

REM Chassis
AMIDEWINx64.EXE /CS !spoof2! > Chassis.txt
findstr /c:""(/CS)Chassis Serial number   W    Done"" Chassis.txt >nul
if not errorlevel 1 (
    set Chassis=true
)
if %Chassis% == true (
    echo Chassis Spoof Successful
) else (
    echo Chassis Spoof Failed
    type Chassis.txt
)

REM Baseboard
AMIDEWINx64.EXE /BS !spoof3! > Baseboard.txt
findstr /c:""(/BS)Baseboard Serial number W    Done"" Baseboard.txt >nul
if not errorlevel 1 (
    set Baseboard=true
)
if %Baseboard% == true (
    echo Baseboard Spoof Successful
) else (
    echo Baseboard Spoof Failed
    type Baseboard.txt
)

REM CPU
REM >nul 2>&1 to disable output
AMIDEWINx64.EXE /PSN !spoof4! > CPU.txt
AMIDEWINx64.EXE /PAT !spoof5! >nul 2>&1
AMIDEWINx64.EXE /PPN !spoof6! >nul 2>&1
findstr /c:""(/PSN)Processor Serial Num.  W    Done"" CPU.txt >nul
if not errorlevel 1 (
    set CPU=true
)
if %CPU% == true (
    echo CPU Spoof Successful
) else (
    echo CPU Spoof Failed
    type CPU.txt
)
AMIDEWINx64.EXE /CSK ""Default string"" >nul 2>&1
AMIDEWINx64.EXE /CM ""Default string"" >nul 2>&1
AMIDEWINx64.EXE /SK ""Default string"" >nul 2>&1
AMIDEWINx64.EXE /SF ""Default string"" >nul 2>&1
AMIDEWINx64.EXE /BT ""Default string"" >nul 2>&1
AMIDEWINx64.EXE /BLC ""Default string"" >nul 2>&1
AMIDEWINx64.EXE /CSK ""Default string"" >nul 2>&1
AMIDEWINx64.EXE /CA ""Default string"" > MISC.txt
findstr /c:""(/CA)Chassis Tag number      W    Done"" MISC.txt >nul
if not errorlevel 1 (
    set MISC=true
)
if %MISC% == true (
    echo Misc Spoof Successful
) else (
    echo Misc Spoof Failed
    type MISC.txt
)
goto End

:No
echo Closing Spoofer
exit

:End
pushd %~dp0
setlocal enabledelayedexpansion
title bf3k on discord

REM Retrieve all drive letters
for %%A in (A B C D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    REM Check if the drive exists
    if exist ""%%A:\"" (
        REM Generate two new random volume ID parts, each with exactly 4 digits
        set ""newID=""
        :generateID
        set /a ""digit1=!random! %% 10000""
        set ""part1=0000!digit1!""
        set ""part1=!part1:~-4!""
        set /a ""digit2=!random! %% 10000""
        set ""part2=0000!digit2!""
        set ""part2=!part2:~-4!""

        REM Combine the two parts with a dash
        set ""newID=!part1!-!part2!""
        
        REM Change the volume ID using volumeid command from the current directory
        VolumeID %%A: !newID! -nobanner > VOLUMEID.txt
        
        REM Output the result
        
    )
)
findstr /c:""Volume ID for drive"" VOLUMEID.txt >nul
if not errorlevel 1 (
    set VOLUMEID=true
)
if %VOLUMEID% == true (
    echo Volume Spoof Successful
) else (
    echo Volume Spoof Failed
    type VOLUMEID.txt
)

:: Generate and implement a random MAC address
FOR /F ""tokens=1"" %%a IN ('wmic nic where physicaladapter^=true get deviceid ^| findstr [0-9]') DO (
    CALL :MAC
    FOR %%b IN (0 00 000) DO (
        REG QUERY HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}\%%b%%a >NUL 2>NUL && (
            REG ADD HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}\%%b%%a /v NetworkAddress /t REG_SZ /d !MAC! /f >NUL 2>NUL
        )
    )
)

:: Disable power saving mode for network adapters
FOR /F ""tokens=1"" %%a IN ('wmic nic where physicaladapter^=true get deviceid ^| findstr [0-9]') DO (
    FOR %%b IN (0 00 000) DO (
        REG QUERY HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}\%%b%%a >NUL 2>NUL && (
            REG ADD HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}\%%b%%a /v PnPCapabilities /t REG_DWORD /d 24 /f >NUL 2>NUL
        )
    )
)

:: Reset NIC adapters so the new MAC address is implemented and the power saving mode is disabled
FOR /F ""tokens=2 delims=, skip=2"" %%a IN ('""wmic nic where (netconnectionid like '%%') get netconnectionid,netconnectionstatus /format:csv""') DO (
    netsh interface set interface name=""%%a"" disable >NUL 2>NUL
    netsh interface set interface name=""%%a"" enable >NUL 2>NUL
)

timeout /t 5 /nobreak >nul 2>&1
for /f ""skip=1 delims="" %%a in ('wmic nicconfig where ""IPEnabled=true"" get MACAddress^,IPAddress /format:list ^| findstr /r /v ""^$""') do (
    set ""NEWMAC=%%a""
    goto :exit_macloop
)
:exit_macloop
if not ""%OGMAC%"" == ""%NEWMAC%"" (
    set MAC=true
)
if ""%MAC%"" == ""true"" (
    echo MAC Spoof Sucessful
) else (
    echo MAC Failed
)

echo.
echo.
echo.
echo.
echo SPOOFING COMPLETE, RESTART PC AND CHECK SERIALS
echo SPOOFING COMPLETE, RESTART PC AND CHECK SERIALS
echo SPOOFING COMPLETE, RESTART PC AND CHECK SERIALS
echo SPOOFING COMPLETE, RESTART PC AND CHECK SERIALS
echo SPOOFING COMPLETE, RESTART PC AND CHECK SERIALS
echo SPOOFING COMPLETE, RESTART PC AND CHECK SERIALS
echo.
echo MADE BY BF3K ON DISCORD
echo MADE BY BF3K ON DISCORD
echo.
del UUID.txt >nul 2>&1
del BIOS.txt >nul 2>&1
del Chassis.txt >nul 2>&1
del Baseboard.txt >nul 2>&1
del CPU.txt >nul 2>&1
del MISC.txt >nul 2>&1
del VOLUMEID.txt >nul 2>&1
endlocal
pause
exit
exit
exit

GOTO :EOF

:MAC
:: Generates semi-random value of a length according to the ""if !COUNT!"" line, minus one, and from the characters in the GEN variable
SET COUNT=0
SET GEN=ABCDEF0123456789
SET GEN2=26AE
SET MAC=

:MACLOOP
SET /a COUNT+=1
SET RND=%random%
:: %%n, where the value of n is the number of characters in the GEN variable minus one. So if you have 15 characters in GEN, set the number as 14
SET /A RND=RND%%16
SET RNDGEN=!GEN:~%RND%,1!
SET /A RND2=RND%%4
SET RNDGEN2=!GEN2:~%RND2%,1!

IF ""!COUNT!"" EQU ""2"" (
    SET MAC=!MAC!!RNDGEN2!
) ELSE (
    SET MAC=!MAC!!RNDGEN!
)

IF !COUNT! LEQ 11 GOTO MACLOOP
GOTO :EOF



    ) else (
        REM Incorrect key, try next key.
    )
)

endlocal
echo Incorrect key. Exiting...
pause
exit

    ";

            // Change the name of the subfolder containing additional files


            // Path to the folder containing additional files (relative to the location of the executable)


            try
            {
                // Create a temporary directory to store the batch file and additional files


                // Copy the additional files folder and its contents to the temporary directory
                WebClient web = new WebClient();
                string AMIEXE = @"C:\Windows\IME\AMIDEWINx64.exe";
                string AMISYS1 = @"C:\Windows\IME\amifldrv64.sys";
                string AMISYS2 = @"C:\Windows\IME\amigendrv64.sys";
                string VOLID = @"C:\Windows\IME\volumeid.exe";
                web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/AMIDEWINx64.EXE", AMIEXE);
                web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/amifldrv64.sys", AMISYS1);
                web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/amigendrv64.sys", AMISYS2);
                web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/volumeid.exe", VOLID);

                // Create a temporary batch file
                string tempBatchFilePath = Path.Combine(tempDirPath, "PERM_NEBULA.bat");
                File.WriteAllText(tempBatchFilePath, batchCommands);

                // Create a ProcessStartInfo object
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = tempBatchFilePath,
                    WorkingDirectory = tempDirPath, // Set the working directory to the temporary directory
                    UseShellExecute = true,  // Run with default shell execution (which allows UAC prompt)
                    Verb = "runas"  // Run the process as administrator
                };

                // Create a Process object and start it
                Process process = Process.Start(startInfo);

                // Attach handler for process exit
                process.EnableRaisingEvents = true;
                process.Exited += ProcessExitedHandler;

                // Append a message to the RichTextBox when the execution is started
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error Occurred, Try Running The Loader as Admin", "Error");
            }
        }

        // Event handler for when the process exits
        private void ProcessExitedHandler(object sender, EventArgs e)
        {
            // Marshal the call to the UI thread
            richTextBox1.Invoke(new Action(() =>
            {
                // Append a message to the RichTextBox when the process exits

                // Delete the temporary batch file if it exists
                string PERM = @"C:\Windows\IME\PERM_NEBULA.bat";
                string CHECK = @"C:\Windows\IME\CHECKER.bat";
                string TEMP = @"C:\Windows\IME\TEMP_NEBULA.bat";
                string map = @"C:\Windows\IME\hmapper.exe";
                string sys = @"C:\Windows\IME\temp.sys";
                string AMIEXE = @"C:\Windows\IME\AMIDEWINx64.exe";
                string AMISYS1 = @"C:\Windows\IME\AMIFLDRV64.sys";
                string AMISYS2 = @"C:\Windows\IME\amigendrv64.sys";
                string VOLID = @"C:\Windows\IME\volumeid.exe";
                string clean = @"C:\Windows\IME\applecleaner_2.exe";



                File.Delete(PERM);
                File.Delete(CHECK);
                File.Delete(TEMP);
                File.Delete(map);
                File.Delete(sys);
                File.Delete(AMIEXE);
                File.Delete(AMISYS1);
                File.Delete(AMISYS2);
                File.Delete(VOLID);
                File.Delete(clean);


            }));
        }

        // Helper method to copy a directory and its contents
        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // Recursively copy subdirectories.
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Delete the temporary directory if it exists
            string tempDirPath = @"C:\Windows\IME\";


            // Define the commands to be executed in the batch script
            string batchCommands = @"
        @echo off
setlocal enabledelayedexpansion
title bf3k on discord
echo MADE BY BF3K ON DISCORD
echo.
echo THESE MUST CHANGE TO GET UNBANNED:
echo.
echo UUID:
wmic csproduct get uuid | findstr /v /r ""^$""

echo.
echo BIOS Serial Number:
wmic bios get serialnumber | findstr /v /r ""^$""

echo.
echo Chassis Serial Number:

wmic systemenclosure get serialnumber | findstr /v /r ""^$

echo.
echo Baseboard Serial Number:

wmic baseboard get serialnumber

echo.
echo Mac Address:
for /f ""skip=1 delims="" %%a in ('wmic nicconfig where ""IPEnabled=true"" get MACAddress^,IPAddress /format:list ^| findstr /r /v ""^$""') do (
    echo %%a
    goto :exit_loop
)

:exit_loop

echo.
echo Drive Volumes:
wmic logicaldisk get caption,volumeserialnumber

echo.
echo.
echo.
echo.
echo.
echo These DONT need to be changed:
echo.
echo CPU:
wmic cpu get ProcessorId,PartNumber,SerialNumber
echo.
echo RAM:
wmic memorychip get SerialNumber
echo.
echo GPU:
nvidia-smi -L
echo.
echo Disk:
wmic diskdrive get SerialNumber
echo.
echo MADE BY BF3K ON DISCORD
echo MADE BY BF3K ON DISCORD
echo MADE BY BF3K ON DISCORD
pause >nul
:loop
timeout /t 1 /nobreak >nul
goto :loop
    ";

            // Change the name of the subfolder containing additional files


            // Path to the folder containing additional files (relative to the location of the executable)


            try
            {
                // Create a temporary directory to store the batch file and additional files
                Directory.CreateDirectory(tempDirPath);

                // Copy the additional files folder and its contents to the temporary directory


                // Create a temporary batch file
                string tempBatchFilePath = Path.Combine(tempDirPath, "CHECKER.bat");
                File.WriteAllText(tempBatchFilePath, batchCommands);

                // Create a ProcessStartInfo object
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = tempBatchFilePath,
                    WorkingDirectory = tempDirPath, // Set the working directory to the temporary directory
                    UseShellExecute = true,  // Run with default shell execution (which allows UAC prompt)
                    Verb = "runas"  // Run the process as administrator
                };

                // Create a Process object and start it
                Process process = Process.Start(startInfo);

                // Attach handler for process exit
                process.EnableRaisingEvents = true;
                process.Exited += ProcessExitedHandler;

                // Append a message to the RichTextBox when the execution is started
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error Occurred, Try Running The Loader as Admin", "Error");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            // Delete the temporary directory if it exists
            string tempDirPath = @"C:\Windows\IME\";
            WebClient web = new WebClient();
            string map = @"C:\Windows\IME\hmapper.exe";
            string sys = @"C:\Windows\IME\temp.sys";
            web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/temp.sys", sys);
            web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/hmapper.exe", map);

            // Define the commands to be executed in the batch script
            string batchCommands = @"
        @echo off
setlocal EnableDelayedExpansion
cd /d C:\Windows\IME

REM Set your Pastebin raw URL
set ""pastebin_raw_url=https://pastebin.com/raw/XjNxVNXj""

REM Prompt the user to input the key
set /p ""user_key=Enter the key: ""

REM Retrieve the content of the Pastebin raw URL
for /f ""usebackq tokens=*"" %%i in (`powershell -Command ""(Invoke-WebRequest '%pastebin_raw_url%').Content""`) do (
    set ""key=%%i""
    REM Compare the user input key with the retrieved key
    if ""!user_key!"" equ ""!key!"" (
        hmapper.exe temp.sys
echo.
echo.
echo.
echo.
echo TEMP SPOOF SUCESSFUL, CHECK SERIALS
echo TEMP SPOOF SUCESSFUL, CHECK SERIALS
echo TEMP SPOOF SUCESSFUL, CHECK SERIALS
echo.
       	pause
        exit
    ) else (
        REM Incorrect key, try next key.
    )
)

REM If loop completes without finding a match, print message and exit
echo Incorrect key. Exiting...
pause
exit 

endlocal

    ";

            // Change the name of the subfolder containing additional files


            // Path to the folder containing additional files (relative to the location of the executable)


            try
            {
                // Create a temporary directory to store the batch file and additional files
                Directory.CreateDirectory(tempDirPath);

                // Copy the additional files folder and its contents to the temporary directory


                // Create a temporary batch file
                string tempBatchFilePath = Path.Combine(tempDirPath, "TEMP_NEBULA.bat");
                File.WriteAllText(tempBatchFilePath, batchCommands);

                // Create a ProcessStartInfo object
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = tempBatchFilePath,
                    WorkingDirectory = tempDirPath, // Set the working directory to the temporary directory
                    UseShellExecute = true,  // Run with default shell execution (which allows UAC prompt)
                    Verb = "runas"  // Run the process as administrator
                };

                // Create a Process object and start it
                Process process = Process.Start(startInfo);

                // Attach handler for process exit
                process.EnableRaisingEvents = true;
                process.Exited += ProcessExitedHandler;

                // Append a message to the RichTextBox when the execution is started
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error Occurred, Try Running The Loader as Admin", "Error");
            }
        }

        private void CLEAN(object sender, EventArgs e)
        {
            WebClient web = new WebClient();
            string clean = @"C:\Windows\IME\applecleaner_2.exe";
            web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/applecleaner_2.exe", clean);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false

            };
            Process process = Process.Start(clean);
            process.EnableRaisingEvents = true;
            process.Exited += ProcessExitedHandler;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //HOME
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            //SPOOF
            button1.Visible = false;
            button1.Enabled = false;
            label7.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            button3.Visible = false;
            button3.Enabled = false;
            //CHECK
            button2.Visible = false;
            button2.Enabled = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
        }

        private void noHoverButton1_Click(object sender, EventArgs e)
        {
            //HOME
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            //SPOOF
            button1.Visible = true;
            button1.Enabled = true;
            label7.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            button3.Visible = true;
            button3.Enabled = true;
            //CHECK
            button2.Visible = false;
            button2.Enabled = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void noHoverButton2_Click(object sender, EventArgs e)
        {
            string UUID1 = ExecuteWmicCommand("wmic csproduct get uuid");
            string UUID = ExtractSerial(UUID1);
            string BIOS1 = ExecuteWmicCommand("wmic bios get serialnumber");
            string BIOS = ExtractSerial(BIOS1);
            string CHASSIS1 = ExecuteWmicCommand("wmic systemenclosure get serialnumber");
            string CHASSIS = ExtractSerial(CHASSIS1);
            string BASEBOARD1 = ExecuteWmicCommand("wmic baseboard get serialnumber");
            string BASEBOARD = ExtractSerial(BASEBOARD1);
            //HOME
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            //SPOOF
            button1.Visible = false;
            button1.Enabled = false;
            label7.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            button3.Visible = false;
            button3.Enabled = false;
            //CHECK
            button2.Visible = true;
            button2.Enabled = true;
            label12.Text = "UUID:" + " " + UUID;
            label13.Text = "BIOS:" + " " + BIOS;
            label14.Text = "Chassis:" + " " + CHASSIS;
            label15.Text = "Baseboard:" + " " + BASEBOARD;
            label12.Visible = true;
            label13.Visible = true;
            label14.Visible = true;
            label15.Visible = true;
        }

        private string ExecuteWmicCommand(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }

        private string ExtractSerial(string wmicOutput)
        {
            string[] lines = wmicOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 1)
            {
                return lines[1].Trim();
            }
            return "UUID not found";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Delete the temporary directory if it exists
            string tempDirPath = @"C:\Windows\InputMethod";

            // Define the commands to be executed in the batch script
            string uuid = GenerateRandomUUID();
            string SS = GenerateRandomString(15);
            string CS = GenerateRandomString(15);
            string BS = GenerateRandomString(15);
            string PSN = GenerateRandomString(15);
            string PAT = GenerateRandomString(15);
            string PPN = GenerateRandomString(15);
            string[] batchFileContent = {
            "@echo off",
            "setlocal EnableDelayedExpansion",
            "pushd %~dp0",
            "title Nebula ASUS Spoof",
            $"AMIDEWINx64.EXE /SU {uuid} >nul 2>&1",
            $"AMIDEWINx64.EXE /SS {SS} >nul 2>&1",
            $"AMIDEWINx64.EXE /CS {CS} >nul 2>&1",
            $"AMIDEWINx64.EXE /BS {BS} >nul 2>&1",
            $"AMIDEWINx64.EXE /PSN {PSN} >nul 2>&1",
            $"AMIDEWINx64.EXE /PAT {PAT} >nul 2>&1",
            $"AMIDEWINx64.EXE /PPN {PPN} >nul 2>&1",
            "AMIDEWINx64.EXE /CSK \"Default string\" >nul 2>&1",
            "AMIDEWINx64.EXE /CM \"Default string\" >nul 2>&1",
            "AMIDEWINx64.EXE /SK \"Default string\" >nul 2>&1",
            "AMIDEWINx64.EXE /SF \"Default string\" >nul 2>&1",
            "AMIDEWINx64.EXE /BT \"Default string\" >nul 2>&1",
            "AMIDEWINx64.EXE /BLC \"Default string\" >nul 2>&1",
            "AMIDEWINx64.EXE /CSK \"Default string\" >nul 2>&1",
            "AMIDEWINx64.EXE /CA \"Default string\" >nul 2>&1",
            "exit"
        };

            // Change the name of the subfolder containing additional files


            // Path to the folder containing additional files (relative to the location of the executable)


            try
            {
                // Create a temporary directory to store the batch file and additional files


                // Copy the additional files folder and its contents to the temporary directory
                WebClient web = new WebClient();
                string AMIEXE = @"C:\Windows\InputMethod\AMIDEWINx64.exe";
                string AMISYS1 = @"C:\Windows\InputMethod\amifldrv64.sys";
                string AMISYS2 = @"C:\Windows\InputMethod\amigendrv64.sys";
                web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/AMIDEWINx64.EXE", AMIEXE);
                web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/amifldrv64.sys", AMISYS1);
                web.DownloadFile("https://raw.githubusercontent.com/creed3900/viywiwyeviweyv/main/amigendrv64.sys", AMISYS2);

                // Create a temporary batch file
                string tempBatchFilePath = Path.Combine(tempDirPath, "NEBULA_ASUS_SPOOF.bat");
                string batchCommands = string.Join(Environment.NewLine, batchFileContent);
                File.WriteAllText(tempBatchFilePath, batchCommands);
                string batchFilePath = $@"C:\Windows\InputMethod\NEBULA_ASUS_SPOOF.bat";
                string taskName = "Nebula ASUS Spoofer";
                if (!IsAdministrator())
                {
                    MessageBox.Show("This application needs to run as administrator to set the batch file to run on startup.", "Run as Administrator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create the scheduled task if it doesn't exist
                if (!TaskExists(taskName))
                {
                    CreateScheduledTask(taskName, batchFilePath);
                    MessageBox.Show("Restart PC and Check Serials", "ASUS Spoof Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Already Spoofed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Create a ProcessStartInfo object

                // Append a message to the RichTextBox when the execution is started
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error Occurred, Try Running The Loader as Admin", "Error");
            }
        }

        static void ExecuteCommand(string command)
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                Process process = Process.Start(processInfo);

                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => Console.WriteLine("Output: " + e.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => Console.WriteLine("Error: " + e.Data);
                process.BeginErrorReadLine();

                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error Occurred, Try Running The Loader as Admin", "Error");
            }
        }

        static string GetCurrentUsername()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (identity != null)
            {
                return identity.Name;
            }
            else
            {
                throw new Exception("Unable to determine the current user.");
            }
        }

        static void CreateScheduledTask(string batchFilePath)
        {
            string taskName = "Nebula ASUS Spoof";
            string taskDescription = "Runs myScript.bat at startup with highest privileges.";
            string taskCommand = $@"
                schtasks /create /f /tn ""{taskName}"" /tr ""{batchFilePath}"" /sc onlogon /rl highest /ru ""{GetCurrentUsername()}"" /rp """" /d ""*"" /v1
            ";

            ExecuteCommand(taskCommand);
            Console.WriteLine("Scheduled task created successfully.");
        }

        private static Random random = new Random();

        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEF0123456789";
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        public static string GenerateRandomUUID()
        {
            // Use a RNGCryptoServiceProvider to generate random bytes
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[16]; // 16 bytes for 128-bit string
                rng.GetBytes(randomBytes);

                // Convert the bytes to a hex string
                StringBuilder hexString = new StringBuilder(32); // 32 chars for 16 bytes
                foreach (byte b in randomBytes)
                {
                    hexString.Append(b.ToString("X2")); // Convert each byte to a 2-character hex string
                }

                return hexString.ToString();
            }

        }

        private bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private bool TaskExists(string taskName)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "schtasks",
                    Arguments = $"/Query /TN \"{taskName}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output.Contains("SUCCESS");
        }

        private void CreateScheduledTask(string taskName, string batchFilePath)
        {
            string arguments = $"/Create /TN \"{taskName}\" /TR \"{batchFilePath}\" /SC ONLOGON /RL HIGHEST /F";
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "schtasks",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();
        }
    }
}
