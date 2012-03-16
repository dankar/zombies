#include <map>
#include <GL/glfw.h>
#include <png.h>
#include <vector>
#include <string>
using namespace std;


#include "math/vector.h"
#include "video/png.h"
#include "video/texture.h"
#include "video/model.h"
#include "level.h"
#include "resourcehandler.h"

std::map<std::string, int> CResourceHandler::m_Textures;
std::map<std::string, CModel*> CResourceHandler::m_Models;
std::map<std::string, CLevel*> CResourceHandler::m_Levels;
std::map<unsigned int, CModel*> CResourceHandler::m_IntModels;
std::map<unsigned int, CLevel*> CResourceHandler::m_IntLevels;

