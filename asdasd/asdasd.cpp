#include <iostream>
#include <Windows.h>
#include <strsafe.h>
#include <vector>
#include <errno.h>

using namespace std;

#define BUF_SIZE 255

vector<vector<int>> dataMas{ vector<int> {4, 5}, vector<int> {7, 2} };
vector<string> infoMas = {"Thread_1", "Thread_2" };

int counter = 0;
int GlobalData = 0;

void DisplayMessage(HANDLE hScreen, char* ThreadName, int Data, int Count)
{
    TCHAR buf[BUF_SIZE];
    size_t stringSize;
    DWORD dwChars;

    StringCchPrintf(buf, BUF_SIZE, TEXT("Executing iteration % 02d of % S with data = % 02d(global = % d)\n"), Count, ThreadName, Data, GlobalData);
    StringCchLength(buf, BUF_SIZE, &stringSize);

    WriteConsole(hScreen, buf, stringSize, &dwChars, NULL);
    Sleep(1000);
}

DWORD WINAPI Thread(LPVOID lpParam)
{
    int iterationNum = dataMas[counter][0], increment = dataMas[counter][1];
    char* message = const_cast<char*>(infoMas[counter].c_str());
    ++counter;
    int Data = 0, count = 0;
    HANDLE hStdout = NULL;

    hStdout = GetStdHandle(STD_OUTPUT_HANDLE);
    if (hStdout == INVALID_HANDLE_VALUE) return 1;


    Data = *((int*)lpParam);

    for (count = 0; count <= iterationNum; count++)
    {
        DisplayMessage(hStdout, message, Data, count);
    }
    GlobalData += increment;

    return 0;
}

int main()
{
    int Data_Of_Thread_1 = 1, Data_Of_Thread_2 = 2;

    HANDLE Handle_Of_Thread_1 = 0, Handle_Of_Thread_2 = 0, Threads_Handles_Array[2];

    Handle_Of_Thread_1 = CreateThread(NULL, 0, Thread, &Data_Of_Thread_1, 0, NULL);
    if (Handle_Of_Thread_1 == NULL) ExitProcess(Data_Of_Thread_1);
    
    Handle_Of_Thread_2 = CreateThread(NULL, 0, Thread, &Data_Of_Thread_2, 0, NULL);
    if (Handle_Of_Thread_2 == NULL) ExitProcess(Data_Of_Thread_2);

    Threads_Handles_Array[0] = Handle_Of_Thread_1;
    Threads_Handles_Array[1] = Handle_Of_Thread_2;

    WaitForMultipleObjects(2, Threads_Handles_Array, TRUE, INFINITE);
    printf("\nALL threads have been successfully executed\n");

    CloseHandle(Handle_Of_Thread_1);
    CloseHandle(Handle_Of_Thread_2);

    printf("\nGlobal variable = %2d\n", GlobalData);
    system("pause");
}