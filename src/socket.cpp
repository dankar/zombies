#include <GL/glfw.h>
#include <vector>
#include "socket.h"
#include "log.h"
#include "interface.h"
#include "timer.h"
#include <zlib.h>
#ifndef WIN32
#include <unistd.h>
#include <fcntl.h>
#include <netdb.h>
#include <errno.h>
#endif

int inline CompressPacket(char* packet, int len)
{
	char packed[4096];
	int packlen = 4096;
	compress((Bytef*)packed, (uLongf*)&packlen, (Bytef*)packet+sizeof(int), (uLongf)len - sizeof(int)); 
	memcpy(packet + sizeof(int), packed, packlen);
	return packlen + sizeof(int);
}

int inline UncompressPacket(char* packet, int len)
{
	char unpacked[4096];
	int packlen = 4096;
	uncompress((Bytef*)unpacked, (uLongf*)&packlen, (Bytef*)packet+sizeof(int), (uLongf)len - sizeof(int));
	memcpy(packet + sizeof(int), unpacked, packlen);
	return packlen + sizeof(int);
}

int GetSock()
{
	unsigned long okay = 1;
	int sock = socket(AF_INET, SOCK_DGRAM, 0);
#ifdef WIN32
	ioctlsocket(sock, FIONBIO, &okay);
#else
	fcntl(sock, F_SETFL, O_NONBLOCK);
#endif

	return sock;
}



int inline AddInt(char* buffer, int *position, int i)
{
	int *dst = (int*)(buffer+*position);

	*dst = i;

	*position += sizeof(int);

	return sizeof(int);
}

int inline AddString(char* buffer, const char* string, int *position)
{
	AddInt(buffer, position, int(strlen(string)));

	memcpy(buffer+*position, string, strlen(string));

	*position += strlen(string);

	return strlen(string);
}

int inline AddData(char* buffer, const char* data, int len, int *position)
{
	memcpy(buffer+*position, data, len);

	*position += len;

	return len;
}

int CSocketServer::SendAll(sockaddr_in client, const char* data, int len)
{
	int sent = 0;
	int bytes = 0;

	bytes = sendto(m_Sock, data + sent, len - sent, 0, (sockaddr*)&client, sizeof(sockaddr));

	return bytes;
}

int CSocketServer::StartServer(int p)
{
	char yes = 1;
	struct sockaddr_in listen_addr;


	m_Port = p;

	m_LastPing = miscGetTime();

	m_Sock = GetSock();

	setsockopt(m_Sock, SOL_SOCKET, SO_LINGER, &yes, sizeof(char));

	yes = 1;

	if(m_Sock == -1)
	{
		perror("socket");
		return 0;
	}

	if (setsockopt(m_Sock, SOL_SOCKET, SO_REUSEADDR, &yes, sizeof(int)) == -1) {
		perror("setsockopt");
		return 0;
	}

	listen_addr.sin_family = AF_INET;
	listen_addr.sin_addr.s_addr = INADDR_ANY;
	listen_addr.sin_port = htons(m_Port);
	memset(&(listen_addr.sin_zero), '\0', 8);

	if(bind(m_Sock, (struct sockaddr*)&listen_addr, sizeof(listen_addr)) == -1)
	{
		perror("bind");
		return 0;
	}

	CLog::Get().Write(APP_LOG, LOG_INFO, "Server started listening on UDP port %d\n", m_Port);

	return 1;
}



int CSocketServer::HandleDisconnect(sockaddr_in client) // WILL CHANGE ITERATORS !!!
{
	int cli = m_SockClient[client];

	if(cli)
	{
		sockaddr_in sock = m_Clients[cli]->sock;
		serverClientLeft(m_SockClient[sock]); //m_Script->CallFunction("ServerClientLeft", m_SockClient[sock]);
		delete m_Clients[cli];
		m_Clients.erase(cli);
		m_SockClient.erase(client);
	}
	
	return 1;
}

int CSocketServer::IsConnected(sockaddr_in client)
{
	return m_SockClient.find(client) != m_SockClient.end();
}

int CSocketServer::UnpackInput(char* data, int client)
{
	data += 8;
	netSetPackData(data, 4096);

	serverUnpackInput(client); //m_Script->CallFunction("ServerUnpackInput", client);

	return 1;
}

int CSocketServer::HandleData(sockaddr_in client, char *buffer, int messagelen)
{
	if(messagelen < 4)
	{
		CLog::Get().Write(APP_LOG, LOG_WARNING, "Recieved malformed package from %s:%d", inet_ntoa(client.sin_addr), client.sin_port);
		return 0;
	}

	int messagetype = *(int*)buffer;

	if(IsConnected(client))
	{
		m_Clients[m_SockClient[client]]->last_response = miscGetTime();
		
		if(messagetype == PONG_PACKET)
		{
			m_Clients[m_SockClient[client]]->ping = (miscGetTime() - *(int*)(buffer+sizeof(int))) - 16;
			if(m_Clients[m_SockClient[client]]->ping < 0) m_Clients[m_SockClient[client]]->ping = 0;
		}
		else if(messagetype == INPUT_PACKET)
		{
			int clientint = m_SockClient[client];
			
			UnpackInput(buffer, clientint);
		}
		else if(messagetype == DISCONNECT_PACKET)
		{
			HandleDisconnect(client);

			CLog::Get().Write(APP_LOG, LOG_INFO, "Recieved disconnect from %s:%d", inet_ntoa(client.sin_addr), client.sin_port);

		}
		else
		{
			
			CLog::Get().Write(APP_LOG, LOG_WARNING, "Recieved malformed package from %s:%d", inet_ntoa(client.sin_addr), client.sin_port);
			return 0;
		}
	}
	else
	{
		if(messagetype == CONNECT_PACKET)
		{
			HandleConnection(client);
		}
		else
		{
			CLog::Get().Write(APP_LOG, LOG_WARNING, "Unknown client sending garbage from %s:%d", inet_ntoa(client.sin_addr), client.sin_port);
			return 0;
		}

	}



	return 1;
}

int CSocketServer::HandleConnection(sockaddr_in client)
{
	Client *cli = new Client;
	cli->last_response = miscGetTime();
	cli->ping = 999;
	cli->sock = client;
	cli->id = netGetUniqueID();

	m_SockClient[client] = cli->id;
	m_Clients[cli->id] = cli;

	CLog::Get().Write(APP_LOG, LOG_INFO, "Accepted new connection from %s:%d", inet_ntoa(client.sin_addr), client.sin_port);
	serverClientJoined(m_SockClient[client]); //m_Script->CallFunction("ServerClientJoined", m_SockClient[client]);

	return 1;
}


int CSocketServer::Pump()
{
	sockaddr_in from;
	char buffer[4096];
	int fromlen = sizeof(sockaddr);
	int messagelen = 1;

    
	while(messagelen != 0)
	{
		if((messagelen = recvfrom(m_Sock, buffer, 4096, 0, (sockaddr*)&from, (socklen_t*)&fromlen)) <= 0)
		{

#ifdef WIN32
			int last = WSAGetLastError();
			if(last == WSAECONNRESET)
			{
				CLog::Get().Write(APP_LOG, LOG_INFO, "Connection lost to %s:%d", inet_ntoa(from.sin_addr), from.sin_port);
				HandleDisconnect(from);
			}
			else if(last == WSAEWOULDBLOCK)
			{
				break;
			}
			else
			{
				CLog::Get().Write(APP_LOG, LOG_WARNING, "Something is very wrong with the network code :(");
			}
#else
			if(errno == EWOULDBLOCK)
			{
				break;
			}
			CLog::Get().Write(APP_LOG, LOG_INFO, "Connection lost to %s:%d", inet_ntoa(from.sin_addr), from.sin_port);
			HandleDisconnect(from);
#endif
		}
		else
		{
			HandleData(from, buffer, messagelen);
		}
	}

	std::map<sockaddr_in, int, sockless>::iterator it;

	for(it = m_SockClient.begin(); it != m_SockClient.end();)
	{
		std::map<sockaddr_in, int, sockless>::iterator it2 = it;
		it++;

		if(m_Clients[it2->second])
		{
			if(miscGetTime() - m_Clients[it2->second]->last_response > 10000) // should be 10 000
			{
				CLog::Get().Write(APP_LOG, LOG_INFO, "Dropped connection to %s:%d dropped due to timeout", inet_ntoa(it2->first.sin_addr), it2->first.sin_port);
				HandleDisconnect(it2->first);
			}
		}
	}

	if(miscGetTime() - m_LastPing > 1000)
	{
		m_LastPing = miscGetTime();

		PingClients();
	}


    return 1;
}

int CSocketServer::PingClients()
{
	char buffer[100];
	int pos = 0;
	int socklen = sizeof(sockaddr);

	int tick = miscGetTime();

	AddInt(buffer, &pos, PING_PACKET);
	AddInt(buffer, &pos, tick);
	AddInt(buffer, &pos, 0);

	std::map<sockaddr_in, int, sockless>::iterator it;

	for(it = m_SockClient.begin(); it != m_SockClient.end(); it++)
	{
		m_Clients[it->second]->ping_sent = tick;
		pos -= 4;
		AddInt(buffer, &pos, m_Clients[it->second]->ping);
		sendto(m_Sock, buffer, pos, 0, (sockaddr*)&it->first, socklen);
	}

	return 1;
}

int CSocketServer::GetClients(Client *clients, int len)
{
	int counter = 0;

	std::map<int, Client*>::iterator it;

	for(it = m_Clients.begin(); it != m_Clients.end(); it++)
	{
		if(counter == len) break;

		clients[counter] = *(it->second);
		counter++;
	}


	return counter;
}



int CSocketServer::SendSnapshot(sockaddr_in client)
{
	char buffer[4096];
	int pos = 0;
	int numobjects;

	AddInt(buffer, &pos, SNAPSHOT_PACKET);
	AddInt(buffer, &pos, netGetSnapshotTick());
	numobjects = netGetSnapshotNumObjects();
	AddInt(buffer, &pos, numobjects);

	for(int i = 0; i < numobjects; i++)
	{
		_snapobject *object = netGetSnapshotObject(i);

		AddInt(buffer, &pos, object->id);
		AddString(buffer, object->classname, &pos);
		AddInt(buffer, &pos, object->data_len);
		AddData(buffer, object->data, object->data_len, &pos);
	}

	int newlen = CompressPacket(buffer, pos);

	//CLog::Get().Write(APP_LOG, LOG_INFO, "Original length = %d. Compressed length = %d.", pos, newlen);

	SendAll(client, buffer, newlen);

	return newlen;
}

int CSocketServer::Listen(int port)
{
	serverInit(); // m_Script->CallFunction("ServerInit");

	if(!StartServer(port))
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Server could not listen on port %d", port);
		return 0;
	}

	m_SnapshotTimer = 0;
	return 1;
}

int CSocketServer::Tick()
{
	int snapshotSize;
	int snapshotObjects;
	int clients[20];
	int numclients;

	m_SnapshotTimer += timerGetDiff();
	m_Tick = timerGetTick();

	Pump();

	if(m_SnapshotTimer > 1000.0/20.0)
	{
		Client clients[20];
		m_SnapshotTimer = 0;

		numclients = GetClients(clients, 20);

		_snapshot *snap;

		serverTick(m_Tick);

		for(int i = 0; i < numclients; i++)
		{
			netBeginSnapshot();
			//m_Script->CallFunction("ServerSnapshot", clients[i].id);
			serverSnapshot(clients[i].id);
			netEndSnapshot();

			snapshotSize = SendSnapshot(clients[i].sock);

			//CLog::Get().Write(APP_LOG, LOG_INFO, "Sent snapshot to %s:%d", inet_ntoa(clients[i].sock.sin_addr), clients[i].sock.sin_port);
		}
		

		netNextSnapshot();
	}

	return 1;
}

int CSocketServer::Disconnect()
{

	return 1;
}

/*
 *	Client class
 */



int CSocketClient::Connect(const char* host, int port)
{
	hostent *he;
	int result;

	m_Ping = 0;
	m_CurSnap = 0;
	m_PrevSnap = 0;

	he = gethostbyname(host);
	int connect = CONNECT_PACKET;

	if(he == NULL)
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not look up hostname %s", host);
		return 0;
	}

	m_Sock = GetSock();

	if(m_Sock == -1)
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not get socket");
		return 0;
	}

	m_Server.sin_family = AF_INET;
	m_Server.sin_port = htons(port);
	m_Server.sin_addr = *((in_addr*)he->h_addr);
	memset(&m_Server.sin_zero, 0, 8);

	if((result = Send((char*)&connect, sizeof(int)) != sizeof(int)))
	{

		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not connect to %s:%d", host, port);
		return 0;

	}

	Connected = 1;

	return 1;
}

int CSocketClient::SendPingResponse(int ping)
{
	char buffer[8];
	int pos = 0;
	
	AddInt(buffer, &pos, PONG_PACKET);
	AddInt(buffer, &pos, ping);

	Send(buffer, sizeof(int)*2);

	return 1;
}

int CSocketClient::Send(const char* data, int len)
{
	int result;
	if((result = sendto(m_Sock, data, len, 0, (sockaddr*)&m_Server, sizeof(sockaddr))) <= 0)
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not send data to server, maybe it's not there");
		Disconnect();
	}

	return result;
}

int CSocketClient::SendInputData()
{
	char data[512];
	char sendbuffer[512];
	int datalen = 0;
	int pos = 0;
	int type = INPUT_PACKET;

	netSetPackData(data, 512);

	clientPackInput(); //m_Script->CallFunction("ClientPackInput");

	datalen = netGetPackPosition();

	AddInt(sendbuffer, &pos, INPUT_PACKET);
	AddInt(sendbuffer, &pos, datalen);
	memcpy(sendbuffer+pos, data, datalen);
	pos += datalen;

	Send(sendbuffer, pos);

	return 1;
}

int CSocketClient::IsConnected()
{
	return Connected;
}

int CSocketClient::Tick()
{
	sockaddr_in from;
	int fromlen = sizeof(sockaddr);
	char buffer[4096];
	int messagelen;
	

	if(Connected)
	{
		SendInputData();

		gfxBlendNormal();

		while(1)
		{
			if((messagelen = recvfrom(m_Sock, buffer, 4096, 0, (sockaddr*)&from, (socklen_t*)&fromlen)) <= 0)
			{
#ifdef WIN32
				int error = WSAGetLastError();

				if(error != WSAEWOULDBLOCK)
				{
					Disconnect();
				}
#else
				if(errno != EWOULDBLOCK)
				{
					Disconnect();
				}
#endif
				break;

			}
			else
			{
				if(messagelen < 4)
				{
					CLog::Get().Write(APP_LOG, LOG_WARNING, "Recieved malformed package from %s:%d", inet_ntoa(from.sin_addr), from.sin_port);
					return 1;
				}

				if(memcmp(&from, &m_Server, sizeof(sockaddr_in)) == 0)
				{
					// message from the server
					int messagetype = *(int*)buffer;

					if(messagetype == PING_PACKET)
					{
						if(messagelen != sizeof(int)*3)
						{
							CLog::Get().Write(APP_LOG, LOG_WARNING, "Malformed ping package from server");
							return 1;
						}
						else
						{
							m_Ping = *(int*)(buffer + sizeof(int)*2);
							SendPingResponse(*(int*)(buffer + sizeof(int)));
						}
					}
					else if(messagetype == SNAPSHOT_PACKET)
					{
						NewSnapshot(buffer, messagelen);
					}
				}
				else
				{
					// someone else sent it
					CLog::Get().Write(APP_LOG, LOG_WARNING, "Got message from other host than server: %s:%d", inet_ntoa(from.sin_addr), from.sin_port);
					return 1;
				}
			}
		}
	}

	return 1;
}

void CSocketClient::NewSnapshot(char *buffer, int len)
{
	int newlen = UncompressPacket(buffer, len);

	int snapTick = *(int*)(buffer + sizeof(int));
	int curTick = 0;

	m_SnapshotSize = len;
	m_NumSnapObjects = *(int*)(buffer + sizeof(int)*2);

	if(m_CurSnap)
		curTick = *(int*)m_CurSnap;
	
	if(snapTick > curTick)
	{
		if(m_PrevSnap) delete[] m_PrevSnap;
		m_PrevSnap = m_CurSnap;
		m_CurSnap = new char[newlen];

		memcpy(m_CurSnap, buffer, newlen);

		m_SnapshotTime = miscGetTime();

		//if(m_PrevSnap && m_CurSnap)
			//m_Script->SetSnapshot(m_PrevSnap, m_CurSnap);

		//CLog::Get().Write(APP_LOG, LOG_INFO, "Recieved new snapshot packet");
	}
}

float CSocketClient::GetInterpolation()
{
	int time = miscGetTime();
	int timediff = time - m_SnapshotTime;
	float result = timediff * 20.0 / 1000.0f;

	return result;
}

const char* CSocketClient::GetCurrentSnap()
{
	return m_CurSnap;
}

const char* CSocketClient::GetPreviousSnap()
{
	return m_PrevSnap;
}

int CSocketClient::Disconnect()
{
	int disconnect = DISCONNECT_PACKET;

	sendto(m_Sock, (char*)&disconnect, sizeof(int), 0, (sockaddr*)&m_Server, sizeof(sockaddr));

	Connected = 0;

	close(m_Sock);

	CLog::Get().Write(APP_LOG, LOG_ERROR, "Lost connection to server");

	return 1;
}
