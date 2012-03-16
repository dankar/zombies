#ifndef _LOG_H_
#define _LOG_H_

#include <fstream>

#define LOGG(x) CLog::Get().Write(APP_LOG, LOG_INFO, x);

const int APP_LOG = 0x01;
const int PROFILE_LOG = 0x08;

enum 
{
	LOG_WARNING=0,
	LOG_ERROR=1,
	LOG_INFO=2
};

class CLog
{
protected:
	CLog();

	std::ofstream m_AppLog;
	//std::ofstream m_ProfileLog;

public:
	static CLog& Get();

	bool Init();

	void Write(int _log, int warn, const char *_msg, ...);
	void Write(int _log, int warn, const char *_msg, int bleh, va_list args);
};

#endif /* _LOG_H_ */
