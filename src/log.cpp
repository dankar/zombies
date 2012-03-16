#include <fstream>
using namespace std;

#include <cstdarg>
#include <vector>
#include <string>
#include "video/font.h"
#include "log.h"
#include "video/console.h"

CLog::CLog()
{

}

CLog &CLog::Get()
{
	static CLog log;
	return log;
}

bool CLog::Init()
{
	m_AppLog.open("log.txt");
	//m_ProfileLog.open("profilelog.txt");

	return true;
}

void CLog::Write(int _log, int warn, const char *_msg, ...)
{
	va_list args;
	va_start(args, _msg);
	char buff[1024];
	char buff2[1024];
	vsprintf(buff2, _msg, args);

	switch(warn)
	{
	case LOG_WARNING:
        sprintf(buff, "[WARNING] %s", buff2);
		break;
	case LOG_ERROR:
        sprintf(buff, "[ERROR] %s", buff2);
		break;
	case LOG_INFO:
        sprintf(buff, "[INFO] %s", buff2);
		break;
	}

	if(_log & APP_LOG)
	{
		m_AppLog << buff << "\n";
		m_AppLog.flush();

		printf("%s\n", buff);
		CConsole::AddLine(buff);
	}

	

	va_end(args);
}

void CLog::Write(int _log, int warn, const char *_msg, int bleh, va_list args)
{
	char buff[1024];
	char buff2[1024];
	vsprintf(buff2, _msg, args);

	switch(warn)
	{
	case LOG_WARNING:
        sprintf(buff, "[WARNING] %s", buff2);
		break;
	case LOG_ERROR:
        sprintf(buff, "[ERROR] %s", buff2);
		break;
	case LOG_INFO:
        sprintf(buff, "[INFO] %s", buff2);
		break;
	}

	if(_log & APP_LOG)
	{
		m_AppLog << buff << "\n";
		m_AppLog.flush();

		printf("%s\n", buff);
		CConsole::AddLine(buff);
	}
}


