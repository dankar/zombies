#ifndef _HEIGHTMAP_H_
#define _HEIGHTMAP_H_

class CPatch
{
	int Grass;
	float *vertArray;
	float *normArray;
	float *texArray;
	float *Heightdata;
	Vec2 m_Samples;
	Vec2 m_Size;
	
	void GetSample(int x, int y, Vec3 *vert);
	void GetNormal(int x, int y, Vec3 *vert);
public:

	CPatch(float* height, float patchesx, float patchesy, float thisx, float thisy);
};

class CHeightMap
{
public:
	std::vector<CPatch*> m_Patches;

	Vec2 m_Size;
	Vec2 m_Samples;
	float m_Scale;
	bool m_UseVBO;
	float* m_Heightdata;

	//dVector3 *m_Vertices;
	//int m_NumVertices;

	//int *m_Indices;
	//int m_NumIndices;

	CHeightMap();
	~CHeightMap();

	int Load(const char* filename, float width, float height);
	int Generate(int x, int y);
	
	void Draw();
	void ChangeHeight();
	void SetHeight(int x, int y, float z);
	void Update();
};

#endif /* _HEIGHTMAP_H_ */
