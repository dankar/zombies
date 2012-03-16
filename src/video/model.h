#ifndef _MODEL_H_
#define _MODEL_H_

#include <vector>
using namespace std;

class CModel;

class CShadowObject
{
	struct Plane
	{
		float a, b, c, d;
	};

	struct Face
	{
		int		vertIndices[3];
		Plane	plane;
		int		neighbourIndices[3];
		bool	visible;
	};

	int		m_NumVertices;
	Vec3*	m_Vertices;

	int		m_NumFaces;
	Face*	m_Faces;

	void SetConnectivity();
	void CalculatePlane(Face &face);
	
	void ShadowPass(GLfloat *light_pos);

	bool Generated;

public:
	void CastShadow(bool firstpass, GLfloat *light_pos);
	void Create(CModel &model);
	void DrawObject();

};

class CModel
{
public:
	vector<Vec3>	m_Vertices;
	vector<int>		m_VertIndices;
	unsigned int	m_VertVBO;

	vector<Vec3>	m_Normals;
	vector<int>		m_NormIndices;
	unsigned int	m_NormVBO;

	vector<Vec3>	m_TexCoords;
	vector<int>		m_TexIndices;
	unsigned int	m_TexVBO;

	int				m_VertCount;

	float*			vertArray;
	float*			normArray;
	float*			texArray;

	CShadowObject	m_Shadow;

	void GenVBO();

public:

	bool LoadMesh(const char* filename, float scale = 1, float rot = 0, float rx = 0, float ry = 0, float rz = 0);

	void Draw();

	void Destroy();

};




#endif /* _MODEL_H_ */
