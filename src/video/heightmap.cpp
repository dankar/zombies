#include <GL/glew.h>
#include <GL/glfw.h>
#include <png.h>
#include <ode/ode.h>
#include <vector>
#include <map>
#include <string>
using namespace std;

#include "png.h"
#include "texture.h"
#include "../math/vector.h"
#include "model.h"
#include "heightmap.h"
#include "../resourcehandler.h"
#include "../log.h"
#include "../interface.h"
#include "../input.h"

void CPatch::GetSample(int x, int y, Vec3 *vert)
{
	vert->x = float(x) / (m_Samples.x-1)  * m_Size.x - m_Size.x/2;
	vert->z = float(y) / (m_Samples.y-1)  * m_Size.y - m_Size.y/2;
	vert->y = Heightdata[int(y * m_Samples.x + x)];
}

void CPatch::GetNormal(int x, int y, Vec3 *vert)
{
	Vec3 point, n1, n2, n3, n4, norm1, norm2, norm3, norm4, normal;

	GetSample(x, y, &point);

	if(y > 0)
		GetSample(x, y-1, &n1);
	else
		n1 = Vec3(0.0f, 0.0f, 0.0f);

	if(x < m_Samples.x)
        GetSample(x+1, y, &n2);
	else
		n2 = Vec3(0.0f, 0.0f, 0.0f);

	if(y < m_Samples.y)
        GetSample(x, y+1, &n3);
	else
		n3 = Vec3(0.0f, 0.0f, 0.0f);

	if(x > 0)
        GetSample(x-1, y, &n4);
	else
		n4 = Vec3(0.0f, 0.0f, 0.0f);

	norm1 = (n2 - point).cross(n1 - point);
	norm2 = (n3 - point).cross(n2 - point);
	norm3 = (n4 - point).cross(n3 - point);
	norm4 = (n1 - point).cross(n4 - point);

	norm1.normalize();
	norm2.normalize();
	norm3.normalize();
	norm4.normalize();

	normal = (norm1 + norm2 + norm3 + norm4);

	normal.normalize();

    *vert = normal;
}

CPatch::CPatch(float* height, float patchesx, float patchesy, float thisx, float thisy)
{
	float sizex = patchesx * 32;
	float sizey = patchesy * 32;
	int texcounter = 0, counter = 0;

	thisx = thisx * 32;
	thisy = thisy * 32;

	Heightdata = height;

	for(int y = (int)thisy; y < (int)thisy+31; y++)
	{
		for(int x = (int)thisx; x < (int)thisx+31; x++)
		{
			Vec3 tri1v1, tri1v2, tri1v3, tri2v1, tri2v2, tri2v3;
			Vec3 tri1v1n, tri1v2n, tri1v3n, tri2v1n, tri2v2n, tri2v3n;

			GetNormal(x, y, &tri1v1n);
			GetSample(x, y, &tri1v1);

			GetNormal(x+1, y, &tri1v2n);
			GetSample(x+1, y, &tri1v2);

			GetNormal(x, y+1, &tri1v3n);
			GetSample(x, y+1, &tri1v3);

			GetNormal(x+1, y, &tri2v1n);
			GetSample(x+1, y, &tri2v1);

			GetNormal(x+1, y+1, &tri2v2n);
			GetSample(x+1, y+1, &tri2v2);

			GetNormal(x, y+1, &tri2v3n);
			GetSample(x, y+1, &tri2v3);

			Vec2 t1(0.0f, 0.0f);
			Vec2 t2(1.0f, 0.0f);
			Vec2 t3(1.0f, 1.0f);
			Vec2 t4(0.0f, 1.0f);

			//t1, t2, t4, t2, t3, t4

			texArray[texcounter++] = t1.x;
			texArray[texcounter++] = t1.y;

			texArray[texcounter++] = t2.x;
			texArray[texcounter++] = t2.y;

			texArray[texcounter++] = t4.x;
			texArray[texcounter++] = t4.y;

			texArray[texcounter++] = t2.x;
			texArray[texcounter++] = t2.y;

			texArray[texcounter++] = t3.x;
			texArray[texcounter++] = t3.y;

			texArray[texcounter++] = t4.x;
			texArray[texcounter++] = t4.y;

			

			// vert1

			normArray[counter] = tri1v1n.x;
			vertArray[counter++] = tri1v1.x;
			
			normArray[counter] = tri1v1n.y;
			vertArray[counter++] = tri1v1.y;

			normArray[counter] = tri1v1n.z;
			vertArray[counter++] = tri1v1.z;

			// vert2
			normArray[counter] = tri1v2n.x;
			vertArray[counter++] = tri1v2.x;

			normArray[counter] = tri1v2n.y;
			vertArray[counter++] = tri1v2.y;

			normArray[counter] = tri1v2n.z;
			vertArray[counter++] = tri1v2.z;


			// vert3
			normArray[counter] = tri1v3n.x;
			vertArray[counter++] = tri1v3.x;

			normArray[counter] = tri1v3n.y;
			vertArray[counter++] = tri1v3.y;

			normArray[counter] = tri1v3n.z;
			vertArray[counter++] = tri1v3.z;

			// vert1
			normArray[counter] = tri2v1n.x;
			vertArray[counter++] = tri2v1.x;

			normArray[counter] = tri2v1n.y;
			vertArray[counter++] = tri2v1.y;

			normArray[counter] = tri2v1n.z;
			vertArray[counter++] = tri2v1.z;

			// vert2
			normArray[counter] = tri2v2n.x;
			vertArray[counter++] = tri2v2.x;

			normArray[counter] = tri2v2n.y;
			vertArray[counter++] = tri2v2.y;

			normArray[counter] = tri2v2n.z;
			vertArray[counter++] = tri2v2.z;

			// vert3
			normArray[counter] = tri2v3n.x;
			vertArray[counter++] = tri2v3.x;

			normArray[counter] = tri2v3n.y;
			vertArray[counter++] = tri2v3.y;

			normArray[counter] = tri2v3n.z;
			vertArray[counter++] = tri2v3.z;

		}
	}
}






CHeightMap::CHeightMap()
{
}

int CHeightMap::Generate(int x, int y)
{
	m_Samples.x = (float)x;
	m_Samples.y = (float)y;
	m_Size.x = (float)x;
	m_Size.y = (float)y;
	m_Scale = 1.0f;

	m_Heightdata = new float[int(x * y)];

	for(int i = 0; i < x*y; i++)
		m_Heightdata[i] = 0.0f;

	m_UseVBO = 0;

//	GenVBO();

	return 1;
}

int CHeightMap::Load(const char* filename, float width, float height)
{
	/*m_Model = CResourceHandler::LoadModel(filename, 500, -90, 1, 0, 0);

	m_NumVertices = m_Model->m_Vertices.size();
	m_NumIndices = m_Model->m_VertIndices.size();

	m_Vertices = new dVector3[m_NumVertices];
	m_Indices = new int[m_NumIndices];

	for(int i = 0; i < m_NumVertices; i++)
	{
		m_Vertices[i][0] = m_Model->m_Vertices[i].x;
		m_Vertices[i][1] = m_Model->m_Vertices[i].y;
		m_Vertices[i][2] = m_Model->m_Vertices[i].z;
	}

	for(int i = 0; i < m_NumIndices; i++)
	{
		m_Indices[i] = m_Model->m_VertIndices[i] - 1;
	}

	SetCollisionHeightmap(m_Vertices, m_Indices, m_NumVertices, m_NumIndices);

	SetPosition(0.0f, 0.0f, 0.0f);*/

	PNG heightmap;
	int counter = 0;

	if(!heightmap.open(filename))
	{
		return 0;
	}

	m_Samples.x = (float)heightmap.width;
	m_Samples.y = (float)heightmap.height;
	m_Size.x = width;
	m_Size.y = height;
	m_Scale = 1.0f;

	m_Heightdata = new float[int(m_Samples.x * m_Samples.y)];

	for(int i = 0; i < m_Samples.y; i++)
	{
		for(int j = 0; j < m_Samples.x; j++)
		{
			m_Heightdata[counter] = heightmap.row_pointers[i][j*3];
			counter++;
		}
	}

//	SetCollisionHeightmap(m_Heightdata, m_Size.x, m_Size.y, m_Samples.x, m_Samples.y, m_Scale, 10.0f);

//	SetPosition(0.0f, 0.0f, 0.0f);

	m_UseVBO = 1.0f;

//	GenVBO();

	return 1;
}



/*void CHeightMap::GenVBO()
{
	int counter = 0;
	int texcounter = 0;

	m_NumVerts = int((m_Samples.x) * (m_Samples.y) * 6);
	vertArray = new float[m_NumVerts * 3];
	normArray = new float[m_NumVerts * 3];
	texArray = new float[m_NumVerts * 2];
	

	for(int y = 0; y < m_Samples.y - 1; y++)
	{
		for(int x = 0; x < m_Samples.x - 1; x++)
		{
			Vec3 tri1v1, tri1v2, tri1v3, tri2v1, tri2v2, tri2v3;
			Vec3 tri1v1n, tri1v2n, tri1v3n, tri2v1n, tri2v2n, tri2v3n;

			GetNormal(x, y, &tri1v1n);
			GetSample(x, y, &tri1v1);

			GetNormal(x+1, y, &tri1v2n);
			GetSample(x+1, y, &tri1v2);

			GetNormal(x, y+1, &tri1v3n);
			GetSample(x, y+1, &tri1v3);

			GetNormal(x+1, y, &tri2v1n);
			GetSample(x+1, y, &tri2v1);

			GetNormal(x+1, y+1, &tri2v2n);
			GetSample(x+1, y+1, &tri2v2);

			GetNormal(x, y+1, &tri2v3n);
			GetSample(x, y+1, &tri2v3);

			Vec2 t1(0.0f, 0.0f);
			Vec2 t2(1.0f, 0.0f);
			Vec2 t3(1.0f, 1.0f);
			Vec2 t4(0.0f, 1.0f);

			//t1, t2, t4, t2, t3, t4

			texArray[texcounter++] = t1.x;
			texArray[texcounter++] = t1.y;

			texArray[texcounter++] = t2.x;
			texArray[texcounter++] = t2.y;

			texArray[texcounter++] = t4.x;
			texArray[texcounter++] = t4.y;

			texArray[texcounter++] = t2.x;
			texArray[texcounter++] = t2.y;

			texArray[texcounter++] = t3.x;
			texArray[texcounter++] = t3.y;

			texArray[texcounter++] = t4.x;
			texArray[texcounter++] = t4.y;

			

			// vert1

			normArray[counter] = tri1v1n.x;
			vertArray[counter++] = tri1v1.x;
			
			normArray[counter] = tri1v1n.y;
			vertArray[counter++] = tri1v1.y;

			normArray[counter] = tri1v1n.z;
			vertArray[counter++] = tri1v1.z;

			// vert2
			normArray[counter] = tri1v2n.x;
			vertArray[counter++] = tri1v2.x;

			normArray[counter] = tri1v2n.y;
			vertArray[counter++] = tri1v2.y;

			normArray[counter] = tri1v2n.z;
			vertArray[counter++] = tri1v2.z;


			// vert3
			normArray[counter] = tri1v3n.x;
			vertArray[counter++] = tri1v3.x;

			normArray[counter] = tri1v3n.y;
			vertArray[counter++] = tri1v3.y;

			normArray[counter] = tri1v3n.z;
			vertArray[counter++] = tri1v3.z;

			// vert1
			normArray[counter] = tri2v1n.x;
			vertArray[counter++] = tri2v1.x;

			normArray[counter] = tri2v1n.y;
			vertArray[counter++] = tri2v1.y;

			normArray[counter] = tri2v1n.z;
			vertArray[counter++] = tri2v1.z;

			// vert2
			normArray[counter] = tri2v2n.x;
			vertArray[counter++] = tri2v2.x;

			normArray[counter] = tri2v2n.y;
			vertArray[counter++] = tri2v2.y;

			normArray[counter] = tri2v2n.z;
			vertArray[counter++] = tri2v2.z;

			// vert3
			normArray[counter] = tri2v3n.x;
			vertArray[counter++] = tri2v3.x;

			normArray[counter] = tri2v3n.y;
			vertArray[counter++] = tri2v3.y;

			normArray[counter] = tri2v3n.z;
			vertArray[counter++] = tri2v3.z;
		}
	}

	if(m_UseVBO && GLEW_ARB_vertex_buffer_object)
	{
		glGenBuffersARB(1, &m_VertVBO);
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_VertVBO);
	
		glBufferDataARB(GL_ARRAY_BUFFER_ARB, m_NumVerts * 3 * sizeof(float), vertArray, GL_STATIC_DRAW_ARB);
	
		glGenBuffersARB(1, &m_NormVBO);
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_NormVBO);
	
		glBufferDataARB(GL_ARRAY_BUFFER_ARB, m_NumVerts * 3 * sizeof(float), normArray, GL_STATIC_DRAW_ARB);
	
		glGenBuffersARB(1, &m_TexVBO);
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_TexVBO);
	
		glBufferDataARB(GL_ARRAY_BUFFER_ARB, m_NumVerts*2*sizeof(float), texArray, GL_STATIC_DRAW_ARB);
	
		delete[] vertArray;
		delete[] normArray;
		delete[] texArray;
	}
	else
	{
		CLog::Get().Write(APP_LOG, LOG_WARNING, "No VBOs, might be slow");
	}
}*/
	
void CHeightMap::Draw()
{
/*	const dReal *pos, *R;

//	gfxBindTexture(m_Grass);

	float mat_specular[] = { 0.0f, 0.0f, 0.0f, 1.0f };
	glLightfv(GL_LIGHT0, GL_SPECULAR, mat_specular);

	glPushMatrix();

    glColor3f(0.8f, 0.8f, 0.8f);
	
	
    glEnableClientState(GL_VERTEX_ARRAY);
	glEnableClientState(GL_NORMAL_ARRAY);
	glEnableClientState(GL_TEXTURE_COORD_ARRAY);

	if(m_UseVBO && GLEW_ARB_vertex_buffer_object)
	{
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_VertVBO);
		glVertexPointer(3, GL_FLOAT, 0, NULL);
	
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_NormVBO);
		glNormalPointer(GL_FLOAT, 0, NULL);
	
		glBindBufferARB(GL_ARRAY_BUFFER_ARB, m_TexVBO);
		glTexCoordPointer(2, GL_FLOAT, 0, NULL);
	}
	else
	{
		glVertexPointer(3, GL_FLOAT, 0, vertArray);
		glNormalPointer(GL_FLOAT, 0, normArray);
		glTexCoordPointer(2, GL_FLOAT, 0, texArray);
	}

	/*if(!GLEW_EXT_vertex_array)
	{
		printf("You're fucked\n");
	}*/
	
/*	glDrawArrays(GL_TRIANGLES, 0, m_NumVerts);
	
	glDisableClientState(GL_VERTEX_ARRAY);
	glDisableClientState(GL_NORMAL_ARRAY);
	glDisableClientState(GL_TEXTURE_COORD_ARRAY);

	glBindTexture(GL_TEXTURE_2D, 0);

	glPopMatrix();

	//glDisable(GL_TEXTURE_2D);*/
}

void CHeightMap::Update()
{

}

void CHeightMap::SetHeight(int x, int y, float z)
{
}

    

void CHeightMap::ChangeHeight()
{
	GLfloat winx, winy, winz;
	GLdouble posx, posy, posz;
	GLint viewport[4];
	GLdouble modelview[16];
	GLdouble projection[16];

	glGetDoublev(GL_MODELVIEW_MATRIX, modelview);
	glGetDoublev(GL_PROJECTION_MATRIX, projection);
	glGetIntegerv(GL_VIEWPORT, viewport);

	winx = (float)inpGetMouseX();
	winy = (float)viewport[3] - inpGetMouseY();

	glReadPixels((int)winx, (int)winy, 1, 1, GL_DEPTH_COMPONENT, GL_FLOAT, &winz);

	gluUnProject(winx, winy, winz, modelview, projection, viewport, &posx, &posy, &posz);

	glPointSize(15.0f);
	glBegin(GL_POINTS);
	glVertex3f(posx, posy, posz);
	glEnd();

	if(inpMouse(2))
	{
	}
}

CHeightMap::~CHeightMap()
{
//	delete[] m_Vertices;
//	delete[] m_Indices;
}
