#include <map>
#include "interface.h"
using std::map;
#include "global.h"
#include "server.h"
#include "factory.h"


CServer::CServer()
{
	m_ClientClass = 0;
}

int CServer::Init()
{
	return 1;
}

int CServer::Tick(int tick)
{
	gWorld->Tick();
	return 1;
}

int CServer::Snapshot(int client)
{
	gWorld->Snapshot(client);
	return 1;
}

int CServer::ClientJoined(int client)
{
	if(m_ClientClass != 0)
	{
		CEntity *cli = (CEntity*)m_ClientClass();
		gWorld->AddEntity(cli);
		m_Clients[client] = cli;
	}
	return 1;
}

int CServer::ClientLeft(int client)
{
	if(m_ClientClass != 0)
	{
		CEntity *cli = m_Clients[client];
		gWorld->RemoveEntity(cli);
		m_Clients.erase(client);
	}
	return 1;
}

void CServer::SetClientClass(void* (*classtype)())
{
	m_ClientClass = classtype;
}

CEntity* CServer::GetClientEntity(int client)
{
	return m_Clients[client];
}

int CServer::UnpackInput(int client)
{

	CEntity *ent = GetClientEntity(client);

	if(ent)
	{
		ent->UnpackInput();
	}

	return 1;
}

CServer::~CServer()
{

}
