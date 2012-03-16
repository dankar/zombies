#ifndef _RESOURCEHANDLER_H_
#define _RESOURCEHANDLER_H_

#include "level.h"
#include "types.h"
#include <expat.h>

#pragma warning(disable : 4311)

class CResourceHandler
{
	static std::map<std::string, int> m_Textures;
	static std::map<std::string, CModel*> m_Models;
	static std::map<std::string, CLevel*> m_Levels;
	static std::map<unsigned int, CModel*> m_IntModels;
	static std::map<unsigned int, CLevel*> m_IntLevels;

public:

	static unsigned int checksum(const char* string)
	{
		unsigned int result = 0;
		for(int i = 0; i < strlen(string); i++)
		{
			result *= string[i];
		}

		return result;
	}

	
	static int LoadTexture(const char* filename, bool mipmap, bool repeat, bool linear = true, int invert = 0)
	{
		int index;

		if(m_Textures.find(std::string(filename)) == m_Textures.end())
		{
			;
			string sfilename = "netroids3d/textures/";
			sfilename += filename;
			if(!(m_Textures[filename] = LoadTexturePNG(sfilename.c_str(), mipmap, repeat, linear, invert)))
			{
				m_Textures[filename] = 0;
				return 0;
			}
			else
			{
				index = m_Textures[filename];
			}

			return index;
		}
		else
		{
			return m_Textures[filename];
		}

        		
	}

	static int LoadModel(const char* filename, float scale, float rot, float rx, float ry, float rz)
	{
		int index;

		if(m_Models.find(std::string(filename)) == m_Models.end())
		{
			m_Models[filename] = new CModel();
			string sfilename = "netroids3d/models/";
			sfilename += filename;
			if(!m_Models[filename]->LoadMesh(sfilename.c_str(), scale, rot, rx, ry, rz))
			{
				printf("Could not open '%s'\n", sfilename.c_str());
				delete m_Models[filename];
				return 0;
			}
			else
			{
				index = (int)m_Models[filename];
				m_IntModels[index] = m_Models[filename];
			}

			return index;
		}
		else
		{
			return m_Models[filename]->m_VertVBO;
		}
	}

	static int LoadLevel(const char* filename)
	{
		int index;

		if(m_Levels.find(std::string(filename)) == m_Levels.end())
		{
			m_Levels[filename] = new CLevel();
			string sfilename = "netroids3d/levels/";
			sfilename += filename;
			if(!m_Levels[filename]->ReadXML(sfilename.c_str()))
			{
				printf("Could not open '%s'\n", sfilename.c_str());
				delete m_Levels[filename];
				return 0;
			}
			else
			{
				index = (int)m_Levels[filename];
				m_IntLevels[index] = m_Levels[filename];
			}

			return index;
		}
		else
		{
			return (int)m_Levels[filename];
		}
	}

	static CModel* GetModel(int model)
	{
		return m_IntModels[model];
	}
};


#endif /* _RESOURCEHANDLER_H_ */