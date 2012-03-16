#ifndef _CLIENT_H_
#define _CLIENT_H_

class CClient
{
public:
	virtual int Init();
	virtual int PackInput() = 0;
	int Render(const char* prevsnap, const char* cursnap, float interpolation);
};

#endif 