#ifndef _SERVER_H_
#define _SERVER_H_

class CServer
{
	void* (*m_ClientClass)();
	map<int, CEntity*> m_Clients;
public:
	CServer();
	virtual int Init();
	int Tick(int tick);
	int Snapshot(int client);
	int ClientJoined(int client);
	int ClientLeft(int client);
	int UnpackInput(int client);
	void SetClientClass(void* (*classtype)());
	CEntity* GetClientEntity(int client);
	
	virtual ~CServer();
};

#endif