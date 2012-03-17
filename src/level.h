#ifndef _LEVEL_H_
#define _LEVEL_H_

#include <list>
#include <string>
#include <map>
#include <scew/scew.h>
#include "math/vector.h"
#include <vector>
using std::list;
using std::map;
using std::string;
using std::vector;

class CLevel
{
private:
	struct material_t
	{
		string name;
		string type;
		float ambient[4];
		float specular[4];
		float diffuse[4];
		float emissive[4];
		float shininess;

		int texture;
	};

	struct block_t
	{
		string name;
		material_t *material;

		Vec3 position;
		Vec3 size;
	};

	struct item_t
	{
		string name;
		string type;

		Vec3 position;
	};

	struct light_t
	{
		string name;
		string type;

		Vec3 position;
		float intensity;
		float radius;
	};

	struct flashlight_t
	{
		float *x;
		float *y;
		float *z;
		float *nx;
		float *ny;
	};

	map<int, flashlight_t> mFlashlights;
	map<string, material_t> mMaterials;
	map<string, block_t> mBlocks;
	map<string, item_t> mItems;
	map<string, light_t> mLights;


	int ReadBlock(scew_element* block);
	int ReadLevel(scew_element* level);
	int ReadMaterial(scew_element* material);
	int ReadRGBA(scew_element* el, float rgba[4]);
	int ReadXYZ(scew_element* el, Vec3* vec);
	int ReadItem(scew_element* item);
	void RenderFlashlightsAlpha(flashlight_t light);
	void DrawBlock(block_t block);
public:
	CLevel();
	int Collide(Vec3 &position, Vec3 &size, list<Vec3> *Collisions);
	void SetMaterial(material_t mat);
	void Render();
	void ClearAlphaBuffer();
	void RenderLightsAlpha(light_t light);
	void RenderShadows(light_t light);
	void DrawShadow(Vec3 p1, Vec3 p2, Vec3 p3, Vec3 p4, Vec3 l);
	int ReadXML(const char* filename);

	int AddFlashlight(float *x, float *y, float *z, float *nx, float *ny, int id);
	int RemoveFlashlight(int id);
	vector<Vec3> FindItemByName(const char* name);
	~CLevel();
};

#endif
