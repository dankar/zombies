#ifndef _SOCKET_H_
#define _SOCKET_H_

#include <winsock.h>
#include <map>
#include "interface.h"


class CScript;

enum
{
	CONNECT_PACKET = 0, // no data
	SNAPSHOT_PACKET = 1, // snapshot struct
	INPUT_PACKET = 2, // data_len, data
	DISCONNECT_PACKET = 3, // no data
	PING_PACKET = 4, // send_tick, last_ping
	PONG_PACKET = 5 // send_tick
};

struct sockless
{
  bool operator()(sockaddr_in s1, sockaddr_in s2) const
  {
    return memcmp(&s1, &s2, sizeof(sockaddr)) < 0;
  }
};

struct Packet
{
	int len;
	char* data;
	int client;
};

struct Client
{
	int id;
	int ping;
	int ping_sent;
	int last_response;
	sockaddr_in sock;
};

int SendAll(int sock, const char* data, int len);

class CSocketServer
{
	std::map<int,Client*>				 m_Clients;
	std::map<sockaddr_in, int, sockless> m_SockClient;
    SOCKET								 m_Sock;
	int									 m_LastPing;
    int									 m_Port;
	int									 m_SnapshotTimer;
	int									 m_Tick;

	int HandleDisconnect(sockaddr_in client);
	int HandleNew();
	int HandleData(sockaddr_in client, char* buffer, int messagelen);
	int HandleConnection(sockaddr_in client);
	int SendToAll(const char* message, int len);
	int Pump();
	int GetClients(Client *clients, int len);
 	int SendSnapshot(sockaddr_in client);
	int SendAll(sockaddr_in client, const char* data, int len);
	int PingClients();
	int UnpackInput(char* data, int client);
	int Ping();
	
public:
	int StartServer(int listen);
	int IsConnected(sockaddr_in client);
	int Listen(int port);
	int Tick();
	int Disconnect();
	
};

class CSocketClient
{
	SOCKET			m_Sock;
	sockaddr_in		m_Server;
	char*			m_CurSnap;
	char*			m_PrevSnap;
	int				m_SnapshotTime;
	int				m_NumSnapObjects;
	int				m_SnapshotSize;
		
	int Connected;

	int SendPingResponse(int ping);
	int SendInputData();
	int Send(const char* data, int len);
	void NewSnapshot(char* buffer, int len);

public:
	int				m_Ping;
	CSocketClient(){ Connected = 0; }
	int Connect(const char* host, int port);
	int Tick();
	float GetInterpolation();
	const char* GetCurrentSnap();
	const char* GetPreviousSnap();
	void Render();
	int IsConnected();
	int Disconnect();
	int GetSnapSize(){ return m_SnapshotSize; }
	int GetSnapNumObjects(){ return m_NumSnapObjects; }
	~CSocketClient(){ Disconnect(); }
};

#endif