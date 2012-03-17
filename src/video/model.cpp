#include "../math/vector.h"
#include <GL/glew.h>
#include <GL/glfw.h>
#include <cstdio>
#include <string>
#include <sstream>
#include <fstream>
#include <vector>
#include "video.h"
#include "../log.h"
#include <cassert>
#include "../interface.h"
#include <cmath>
#include <cstdlib>

using namespace std;


#include "model.h"

vector<string> tokenize(const string& str,const string& delimiters)
{
	vector<string> tokens;
    	
    string::size_type lastPos = str.find_first_not_of(delimiters, 0);
    	
   	string::size_type pos = str.find_first_of(delimiters, lastPos);

    while (string::npos != pos || string::npos != lastPos)
    {
        tokens.push_back(str.substr(lastPos, pos - lastPos));
	
        lastPos = str.find_first_not_of(delimiters, pos);

        pos = str.find_first_of(delimiters, lastPos);
    }

	return tokens;
}

bool CModel::LoadMesh(const char* filename, float scale, float rot, float rx, float ry, float rz)
{
	Destroy();

	char buffer[256];

	fstream fp;
	fp.open(filename, ios_base::in);
	if(!fp.is_open())
		return false;

	while(!fp.eof())
	{
		fp.getline(buffer, 256);
		string str = buffer;

		vector<string> row = tokenize(str, " ");

		if(row.size() > 0)
		if(row[0] == "v")
		{
			float x = atof(row[1].c_str());
			float y = atof(row[2].c_str());
			float z = atof(row[3].c_str());

			Vec3 vertex = (Vec3(x, y, z) * scale);

			vertex.Rotate(rot, rx, ry, rz);

			m_Vertices.push_back(vertex);
		}
		else if(row[0] == "vn")
		{
			float x = atof(row[1].c_str());
			float y = atof(row[2].c_str());
			float z = atof(row[3].c_str());

			Vec3 normal = Vec3(x, y, z);

			normal.Rotate(rot, rx, ry, rz);

			m_Normals.push_back(normal);
		}
		else if(row[0] == "vt")
		{
			float x = atof(row[1].c_str());
			float y = 1.0 - atof(row[2].c_str());
			float z = atof(row[3].c_str());

			m_TexCoords.push_back(Vec3(x, y, z));
		}
		else if(row[0] == "f")
		{
			if(row.size() - 1 != 3)
			{
				printf("polygons are for newbs\n");
				return false;
			}
			
			vector<string> vtn1 = tokenize(row[1], "/");
			vector<string> vtn2 = tokenize(row[2], "/");
			vector<string> vtn3 = tokenize(row[3], "/");

			m_VertIndices.push_back(atoi(vtn1[0].c_str()));
			m_VertIndices.push_back(atoi(vtn2[0].c_str()));
			m_VertIndices.push_back(atoi(vtn3[0].c_str()));

			if(vtn1.size() == 3)
			{
				m_TexIndices.push_back(atoi(vtn1[1].c_str()));
				m_TexIndices.push_back(atoi(vtn2[1].c_str()));
				m_TexIndices.push_back(atoi(vtn3[1].c_str()));

				m_NormIndices.push_back(atoi(vtn1[2].c_str()));
				m_NormIndices.push_back(atoi(vtn2[2].c_str()));
				m_NormIndices.push_back(atoi(vtn3[2].c_str()));
			}
			else
			{
				m_NormIndices.push_back(atoi(vtn1[1].c_str()));
				m_NormIndices.push_back(atoi(vtn2[1].c_str()));
				m_NormIndices.push_back(atoi(vtn3[1].c_str()));
			}

		}


	}

	CLog::Get().Write(APP_LOG, LOG_INFO, "\"%s\": Vertices: %d\tNormals: %d\tTexCoords: %d", filename, m_Vertices.size(), m_Normals.size(), m_TexCoords.size());
	CLog::Get().Write(APP_LOG, LOG_INFO, "\"%s\": VertIn: %d\tNormIn: %d\tTexIn: %d", filename, m_VertIndices.size(), m_NormIndices.size(), m_TexIndices.size());

	fp.close();
	
	GenVBO();

	m_Shadow.Create(*this);

	return true;
}

void CModel::Draw()
{
	glPushMatrix();

	glColor3f(0.8f, 0.8f, 0.8f);

	int texture = m_TexCoords.size();
	
	
    glEnableClientState(GL_VERTEX_ARRAY);
	glEnableClientState(GL_NORMAL_ARRAY);
	if(texture)
		glEnableClientState(GL_TEXTURE_COORD_ARRAY);

	if(GLEW_ARB_vertex_buffer_object)
	{
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_VertVBO);
		glVertexPointer(3, GL_FLOAT, 0, NULL);

		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_NormVBO);
		glNormalPointer(GL_FLOAT, 0, NULL);

		if(texture)
		{
			glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_TexVBO);
			glTexCoordPointer(3, GL_FLOAT, 0, NULL);
		}
	}
	else
	{
		glVertexPointer(3, GL_FLOAT, 0, vertArray);
		glNormalPointer(GL_FLOAT, 0, normArray);
		if(m_TexCoords.size() > 0)
			glTexCoordPointer(3, GL_FLOAT, 0, texArray);
	}
	
	glDrawArrays(GL_TRIANGLES, 0, m_VertCount);

	glDisableClientState(GL_VERTEX_ARRAY);
	glDisableClientState(GL_NORMAL_ARRAY);
	glDisableClientState(GL_TEXTURE_COORD_ARRAY);

	//m_Shadow.DrawObject();

	glPopMatrix();
}

void CModel::GenVBO()
{
	vector<int>::iterator normIter;
	vector<int>::iterator vertIter;
	vector<int>::iterator texIter;

	int vertCount = m_VertIndices.size();
	int normCount = m_NormIndices.size();
	int texCount = m_TexIndices.size();

	m_VertCount = vertCount;

	vertArray = new float[vertCount * 3];
	normArray = new float[normCount * 3];
	texArray = new float[texCount * 3];

	int counter = 0;
	int texcounter = 0;

	for(texIter = m_TexIndices.begin(), vertIter = m_VertIndices.begin(), normIter = m_NormIndices.begin(); 
			vertIter != m_VertIndices.end() && normIter != m_NormIndices.end() && texIter != m_TexIndices.end(); 
			vertIter++, normIter++, texIter++)
	{
		int normal = (*normIter) - 1;
		int vertex = (*vertIter) - 1;
		int texcoord = (*texIter) - 1;

		normArray[counter + 0] = m_Normals[normal].x;
		normArray[counter + 1] = m_Normals[normal].y;
		normArray[counter + 2] = m_Normals[normal].z;

		vertArray[counter + 0] = m_Vertices[vertex].x;
		vertArray[counter + 1] = m_Vertices[vertex].y;
		vertArray[counter + 2] = m_Vertices[vertex].z;

		texArray[counter + 0] = m_TexCoords[texcoord].x;
		texArray[counter + 1] = m_TexCoords[texcoord].y;
		texArray[counter + 2] = m_TexCoords[texcoord].z;

		counter+=3;
	}

	/*m_Normals.clear();
	m_NormIndices.clear();
	m_Vertices.clear();
	m_VertIndices.clear();
	m_TexCoords.clear();
	m_TexIndices.clear();*/

	if(GLEW_ARB_vertex_buffer_object)
	{
		glGenBuffersARB(1, &m_VertVBO);
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_VertVBO);
	
		glBufferDataARB(GL_ARRAY_BUFFER_ARB, vertCount*3*sizeof(float), vertArray, GL_STATIC_DRAW_ARB);
	
		glGenBuffersARB(1, &m_NormVBO);
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_NormVBO);
	
		glBufferDataARB(GL_ARRAY_BUFFER_ARB, normCount*3*sizeof(float), normArray, GL_STATIC_DRAW_ARB);
	
		if(m_TexCoords.size() > 0)
		{
			glGenBuffersARB(1, &m_TexVBO);
			glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_TexVBO);
	
			glBufferDataARB(GL_ARRAY_BUFFER_ARB, texCount*3*sizeof(float), texArray, GL_STATIC_DRAW_ARB);
		}

		delete[] vertArray;
		delete[] normArray;
		delete[] texArray;
	}
	else
	{
		CLog::Get().Write(APP_LOG, LOG_WARNING, "No support for vertex buffer objects found.");
	}
}

void CModel::Destroy()
{
	m_Normals.clear();
	m_NormIndices.clear();
	m_Vertices.clear();
	m_VertIndices.clear();
	m_TexCoords.clear();
	m_TexIndices.clear();
}

#define INFINITY 500

void CShadowObject::Create(CModel &model)
{
	int counter;

	m_NumVertices = model.m_Vertices.size();

	if(m_NumVertices > 10000)
	{
		Generated = false;
		return;
	}

	m_Vertices = new Vec3[m_NumVertices];

	counter = 0;
	for(vector<Vec3>::iterator it = model.m_Vertices.begin(); it != model.m_Vertices.end(); it++)
	{
		m_Vertices[counter++] = (*it);
	}

	m_NumFaces = model.m_VertIndices.size() / 3;
	
	assert(model.m_VertIndices.size() % 3 == 0);

	m_Faces = new Face[m_NumFaces];

	for(int i = 0; i < m_NumFaces; i++)
	{
		for(int j = 0; j < 3; j++)
		{
			m_Faces[i].neighbourIndices[j] = -1;
		}

		m_Faces[i].vertIndices[0] = model.m_VertIndices[i*3+0]-1;
		m_Faces[i].vertIndices[1] = model.m_VertIndices[i*3+1]-1;
		m_Faces[i].vertIndices[2] = model.m_VertIndices[i*3+2]-1;

		//m_Faces[i].normals[0] = model.m_Vertices[model.m_NormIndices[i*3+0]];
		//m_Faces[i].normals[1] = model.m_Vertices[model.m_NormIndices[i*3+1]];
		//m_Faces[i].normals[2] = model.m_Vertices[model.m_NormIndices[i*3+2]];
	}

	SetConnectivity();

	for(int i = 0; i < m_NumFaces; i++)
	{
		CalculatePlane(m_Faces[i]);
	}

	Generated = true;
}

void CShadowObject::SetConnectivity()
{
	unsigned int p1i, p2i, p1j, p2j;
	unsigned int P1i, P2i, P1j, P2j;
	unsigned int i,j,ki,kj;

	int neighbours = 0;
	int edge = 0;

	for(i = 0; i < m_NumFaces - 1; i++)
	{
		for(j = i + 1; j < m_NumFaces; j++)
		{
			for(ki = 0; ki < 3; ki++)
			{
				if(m_Faces[i].neighbourIndices[ki] == -1)
				{
					for(kj = 0; kj < 3; kj++)
					{
						p1i = ki;
						p1j = kj;
						p2i = (ki + 1) % 3;
						p2j = (kj + 1) % 3;

						p1i = m_Faces[i].vertIndices[p1i];
						p2i = m_Faces[i].vertIndices[p2i];
						p1j = m_Faces[j].vertIndices[p1j];
						p2j = m_Faces[j].vertIndices[p2j];

						P1i = ((p1i + p2i) - abs(int(p1i-p2i))) / 2;
						P2i = ((p1i + p2i) + abs(int(p1i-p2i))) / 2;
						P1j = ((p1j + p2j) - abs(int(p1j-p2j))) / 2 ;
						P2j = ((p1j + p2j) + abs(int(p1j-p2j))) / 2;

						if((P1i == P1j) && (P2i == P2j)){  //they are neighbours
							m_Faces[i].neighbourIndices[ki] = j;	  
							m_Faces[j].neighbourIndices[kj] = i;	  
						}
					}
				}
			}
		}
	}
}

void CShadowObject::CalculatePlane(Face &face)
{
	// Get Shortened Names For The Vertices Of The Face

	const Vec3& v1 = m_Vertices[face.vertIndices[0]];
	const Vec3& v2 = m_Vertices[face.vertIndices[1]];
	const Vec3& v3 = m_Vertices[face.vertIndices[2]];

	face.plane.a = v1.y*(v2.z-v3.z) + v2.y*(v3.z-v1.z) + v3.y*(v1.z-v2.z);
	face.plane.b = v1.z*(v2.x-v3.x) + v2.z*(v3.x-v1.x) + v3.z*(v1.x-v2.x);
	face.plane.c = v1.x*(v2.y-v3.y) + v2.x*(v3.y-v1.y) + v3.x*(v1.y-v2.y);
	face.plane.d = -( v1.x*( v2.y*v3.z - v3.y*v2.z ) +
				v2.x*(v3.y*v1.z - v1.y*v3.z) +
				v3.x*(v1.y*v2.z - v2.y*v1.z) );
}

void CShadowObject::CastShadow(bool firstpass, GLfloat *light_pos)
{
	if(!Generated)
		return;

	for ( int i = 0; i < m_NumFaces; i++ )
	{
		const Plane& plane = m_Faces[i].plane;

		GLfloat side = plane.a * light_pos[0] + plane.b * light_pos[1] +
					   plane.c * light_pos[2] + plane.d;

		if ( side > 0 )
			m_Faces[i].visible = true;
		else
			m_Faces[i].visible = false;
	}

	

	//glFrontFace(GL_CW);

	glPushAttrib( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_ENABLE_BIT | GL_POLYGON_BIT | GL_STENCIL_BUFFER_BIT );
	glEnable(GL_CULL_FACE);
	glDisable( GL_LIGHTING );					// Turn Off Lighting
	glDepthMask( GL_FALSE );					// Turn Off Writing To The Depth-Buffer
	glDepthFunc( GL_LEQUAL );
	glEnable( GL_STENCIL_TEST );					// Turn On Stencil Buffer Testing
	glColorMask( GL_FALSE, GL_FALSE, GL_FALSE, GL_FALSE );		// Don't Draw Into The Colour Buffer
	glStencilFunc( GL_ALWAYS, 1, 0xffffffff );

	gfxBlendNormal();
	glColor4f( 0.0f, 1.0f, 0.0f, 0.1f );

	//CVideo::SetInfinitePerspective();

	if(firstpass)
	{
		glCullFace(GL_FRONT);
		glStencilOp( GL_KEEP, GL_INCR, GL_KEEP);
		ShadowPass(light_pos);
	}
	else
	{	
		// Second Pass. Decrease Stencil Value In The Shadow
		glCullFace(GL_BACK);
		glStencilOp( GL_KEEP, GL_DECR, GL_KEEP);
		ShadowPass(light_pos);
	}

	//CVideo::SetPerspective();

	glColorMask( GL_TRUE, GL_TRUE, GL_TRUE, GL_TRUE );	// Enable Rendering To Colour Buffer For All Components

	/*glBlendFunc( GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA );
	glColor4f( 0.2f, 0.2f, 0.2f, 0.1f );

	ShadowPass(light_pos);

	glCullFace(GL_BACK);*/

	glPopAttrib();
}

void CShadowObject::ShadowPass(GLfloat *light_pos)
{
	//glColor3f(1.0f, 1.0f, 1.0f);

	vector<Face> backface;
	vector<Face> frontface;

	for ( int i = 0; i < m_NumFaces; i++ )
	{
		const Face& face = m_Faces[i];

		if ( face.visible )
		{
			glEnable(GL_POLYGON_OFFSET_FILL);
			glPolygonOffset(1.0f, 1.0f);

			backface.push_back(face);
			glBegin(GL_TRIANGLES);
			glVertex3fv(m_Vertices[face.vertIndices[0]]);
			glVertex3fv(m_Vertices[face.vertIndices[1]]);
			glVertex3fv(m_Vertices[face.vertIndices[2]]);
			glEnd();

			glDisable(GL_POLYGON_OFFSET_FILL);

			for ( int j = 0; j < 3; j++ )
			{
				int neighbourIndex = face.neighbourIndices[j];
				
				if ( neighbourIndex == -1 || m_Faces[neighbourIndex].visible == false )
				{
					Vec3 v1 = m_Vertices[face.vertIndices[j]];
					Vec3 v2 = m_Vertices[face.vertIndices[(j + 1) % 3]];

					/*glColor3f(1.0f, 1.0f, 1.0f);
					glLineWidth(10.0f);
					glBegin(GL_LINES);
						glVertex3f(v1.x, v1.y, v1.z);
						glVertex3f(v2.x, v2.y, v2.z);
					glEnd();*/

					Vec3 v3, v4;

					v3 = v1.minus(light_pos);

					v4 = v2.minus(light_pos);

					v3.normalize();
					v4.normalize();

					v3 = v3 * INFINITY;
					v4 = v4 * INFINITY;

					/*v3.x = ( v1.x-light_pos[0] )*INFINITY;
					v3.y = ( v1.y-light_pos[1] )*INFINITY;
					v3.z = ( v1.z-light_pos[2] )*INFINITY;*/

					/*v4.x = ( v2.x-light_pos[0] )*INFINITY;
					v4.y = ( v2.y-light_pos[1] )*INFINITY;
					v4.z = ( v2.z-light_pos[2] )*INFINITY;*/


					glBegin( GL_TRIANGLE_STRIP );
						glVertex3f( v1.x, v1.y, v1.z );
						glVertex4f	( v1.x+v3.x, v1.y+v3.y, v1.z+v3.z, 1.0f );
						glVertex3f( v2.x, v2.y, v2.z );
						glVertex4f( v2.x+v4.x, v2.y+v4.y, v2.z+v4.z, 1.0f );
					glEnd();

				}
			}
		}
	}

	/*glColor4f(1.0f, 1.0f, 1.0f, 1.0f);
	glPointSize(15.0f);*/
	glBegin(GL_TRIANGLES);
	for(int i = 0; i < backface.size(); i++)
	{
		const Face& face = backface[i];

		Vec3 v1 = m_Vertices[face.vertIndices[0]].minus(light_pos);
		Vec3 v2 = m_Vertices[face.vertIndices[1]].minus(light_pos);
		Vec3 v3 = m_Vertices[face.vertIndices[2]].minus(light_pos);

		v1.normalize();
		v2.normalize();
		v3.normalize();

		v1 = v1 * INFINITY;
		v2 = v2 * INFINITY;
		v3 = v3 * INFINITY;

		glVertex3fv(v3 + m_Vertices[face.vertIndices[2]]);
		glVertex3fv(v2 + m_Vertices[face.vertIndices[1]]);
		glVertex3fv(v1 + m_Vertices[face.vertIndices[0]]);
		
	}
	glEnd();

}

void CShadowObject::DrawObject()
{
	glBegin(GL_TRIANGLES);
	for ( int i = 0; i < m_NumFaces; i++ )
	{
		const Face& face = m_Faces[i];

		for ( int j = 0; j < 3; j++ )
		{
			if(face.visible)
				glColor3f(1.0f, 0.0f, 0.0f);
			else
				glColor3f(1.0f, 1.0f, 1.0f);
			const Vec3& vertex = m_Vertices[face.vertIndices[j]];

//			glNormal3f( face.normals[j].x, face.normals[j].y, face.normals[j].z );
			glVertex3f( vertex.x, vertex.y, vertex.z );
		}
	}
	glEnd();
}
