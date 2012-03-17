#include "level.h"
#include <scew/scew.h>
#include <scew/attribute.h>
#include <GL/glew.h>
#include <GL/glfw.h>
#include "log.h"
#include "interface.h"
#define PI 3.1415926f
#include <math.h>


#define MIN(x, y) (x < y ? x : y)
#define MAX(x, y) (x > y ? x : y)

CLevel::CLevel()
{
}

void CLevel::SetMaterial(material_t mat)
{
}

int CLevel::Collide(Vec3 &position, Vec3 &size, list<Vec3> *Collisions)
{
	float ox, oy, oz;
	block_t blo;
	map<std::string, block_t>::iterator it;
	int collides = 0;

	Vec3 bmin;
	Vec3 bmax;
	Vec3 pmin;
	Vec3 pmax;

	Vec3 sum_extents;
	Vec3 centocen;

	for(it = mBlocks.begin(); it != mBlocks.end(); it++)
	{
		blo = it->second;

		bmin.x = MIN(blo.position.x, blo.position.x+blo.size.x);
		bmax.x = MAX(blo.position.x, blo.position.x+blo.size.x);
		bmin.y = MIN(blo.position.y, blo.position.y+blo.size.y);
		bmax.y = MAX(blo.position.y, blo.position.y+blo.size.y);
		bmin.z = MIN(blo.position.z, blo.position.z+blo.size.z);
		bmax.z = MAX(blo.position.z, blo.position.z+blo.size.z);

		pmin.x = MIN(position.x, position.x+size.x);
		pmax.x = MAX(position.x, position.x+size.x);
		pmin.y = MIN(position.y, position.y+size.y);
		pmax.y = MAX(position.y, position.y+size.y);
		pmin.z = MIN(position.z, position.z+size.z);
		pmax.z = MAX(position.z, position.z+size.z);

		sum_extents.x = (bmax.x - bmin.x) + (pmax.x - pmin.x);
	    sum_extents.y = (bmax.y - bmin.y) + (pmax.y - pmin.y);
		sum_extents.z = (bmax.z - bmin.z) + (pmax.z - pmin.z);

		centocen.x = (pmin.x + pmax.x) - (bmin.x + bmax.x);
		centocen.y = (pmin.y + pmax.y) - (bmin.y + bmax.y);
		centocen.z = (pmin.z + pmax.z) - (bmin.z + bmax.z);

		if(fabsf(centocen.x) < sum_extents.x &&
           fabsf(centocen.y) < sum_extents.y &&
		   fabsf(centocen.z) < sum_extents.z)
		{
			ox = (sum_extents.x - fabsf(centocen.x)) * (centocen.x/fabsf(centocen.x)) / 2;
			oy = (sum_extents.y - fabsf(centocen.y)) * (centocen.y/fabsf(centocen.y)) / 2;
			oz = (sum_extents.z - fabsf(centocen.z)) * (centocen.z/fabsf(centocen.z)) / 2;

			Vec3 collision(ox, oy, oz);

			Collisions->push_back(collision);

			collides = 1;
		}
	}
	
	return collides;
}

void CLevel::DrawBlock(block_t blo)
{
	// Draw top
	float px = blo.position.x;
	float py = blo.position.y;
	float pz = blo.position.z;

	float sx = blo.size.x;
	float sy = blo.size.y;
	float sz = blo.size.z;

    glBegin(GL_QUADS);
        // Bottom
        if (sy > 0)
            { glNormal3f(0.0f, -1.0f, 0.0f); }
        else
            { glNormal3f(0.0f, 1.0f, 0.0f); }

        glMaterialfv(GL_FRONT, GL_DIFFUSE, blo.material->diffuse);
        glMaterialfv(GL_FRONT, GL_SPECULAR, blo.material->ambient);
        glMaterialfv(GL_FRONT, GL_AMBIENT, blo.material->specular);
		glMaterialf(GL_FRONT, GL_SHININESS, blo.material->shininess);
        glVertex3f(px, py, pz);
        glVertex3f(px+sx, py, pz);
        glVertex3f(px+sx, py, pz+sz);
        glVertex3f(px, py, pz+sz);
        // Left
        if (sx > 0)
            { glNormal3f(-1.0f, 0.0f, 0.0f); }
        else
            { glNormal3f(1.0f, 0.0f, 0.0f); }
        glVertex3f(px, py, pz);
        glVertex3f(px, py+sy, pz);
        glVertex3f(px, py+sy, pz + sz);
        glVertex3f(px, py, pz + sz);
        // Top
        if (sy > 0)
            {glNormal3f(0.0f, 1.0f, 0.0f);}
        else
            {glNormal3f(0.0f, -1.0f, 0.0f);}
        glVertex3f(px, py+sy, pz);
        glVertex3f(px + sx, py + sy, pz);
        glVertex3f(px + sx, py + sy, pz + sz);
        glVertex3f(px, py + sy, pz + sz);
        // Right
        if (sx > 0)
            { glNormal3f(1.0f, 0.0f, 0.0f); }
        else
            { glNormal3f(-1.0f, 0.0f, 0.0f); }
        glVertex3f(px + sx, py, pz);
        glVertex3f(px + sx, py + sy, pz);
        glVertex3f(px + sx, py + sy, pz + sz);
        glVertex3f(px + sx, py, pz + sz);
        // Back
        if (sz > 0)
            { glNormal3f(0.0f, 0.0f, -1.0f); }
        else
            { glNormal3f(0.0f, 0.0f, 1.0f); }	
        glVertex3f(px, py, pz);
        glVertex3f(px, py + sy, pz);
        glVertex3f(px + sx, py + sy, pz);
        glVertex3f(px + sx, py, pz);
        // Front
        if (sz > 0)
            { glNormal3f(0.0f, 0.0f, 1.0f); }
        else
            { glNormal3f(0.0f, 0.0f, -1.0f); }		
        glVertex3f(px, py, pz+sz);
        glVertex3f(px, py + sy, pz + sz);
        glVertex3f(px + sx, py + sy, pz + sz);
        glVertex3f(px + sx, py, pz + sz);
    glEnd();
}

void CLevel::RenderFlashlightsAlpha(flashlight_t light)
{

	map<string, block_t>::iterator bit;

	float radius = 50.0f;
	float flashradius = 300.0f;

	glDepthMask(false); // But don't write depth...
	glEnable(GL_BLEND);
	glBlendFunc(GL_ONE, GL_ONE);

	Vec3 position(*light.x+10, *light.y+10, *light.z+10);

	float aim_ang = atan2(*light.ny, *light.nx);

	glColorMask(false, false, false, true);

	glBegin(GL_TRIANGLE_FAN);

	//Center of light
	glColor4f(0.0f, 0.0f, 0.0f, 1.0f);
	glVertex3f(position.x, position.y, position.z);

	// Rest of them
	glColor4f(0.0f, 0.0f, 0.0f, 0.0f);
	

	for(float angle=0; angle<=PI*2; angle+=((PI*2)/50) )
	{
		glVertex3f( radius*(float)cos(angle) + position.x, 
			radius*(float)sin(angle) + position.y, 
			position.z);  
	}

	glVertex3f(position.x+radius, position.y, position.z);

	glEnd();

	

	glBegin(GL_TRIANGLE_FAN);

	glColor4f(0.0f, 0.0f, 0.0f, 1.0f);
	glVertex3f(position.x, position.y, position.z);

	glColor4f(0.0f, 0.0f, 0.0f, 0.0f);

	for(float angle = aim_ang - 0.6f; angle <= (aim_ang + 0.6f); angle+=((PI*2)/24) )
	{
		glVertex3f( flashradius*(float)cos(angle) + position.x, 
			flashradius*(float)sin(angle) + position.y, 
			position.z);  
	}

	glEnd();

	
	glDisable(GL_BLEND);

	light_t li;

	li.position = position;

	RenderShadows(li);
	
	glDepthMask(true);
}

void CLevel::RenderLightsAlpha(light_t light)
{
	map<string, block_t>::iterator bit;

	

	glDepthMask(false); // But don't write depth...
	glEnable(GL_BLEND);
	glBlendFunc(GL_ONE, GL_ONE);

	Vec3 position = light.position;

	glColorMask(false, false, false, true);

	glBegin(GL_TRIANGLE_FAN);

	//Center of light
	glColor4f(0.0f, 0.0f, 0.0f, 1.0f);
	glVertex3f(position.x, position.y, position.z);

	// Rest of them
	glColor4f(0.0f, 0.0f, 0.0f, 0.0f);

	for(float angle=0; angle<=PI*2; angle+=((PI*2)/24) )
	{
		glVertex3f( light.radius*(float)cos(angle) + position.x, 
			light.radius*(float)sin(angle) + position.y, 
			position.z);  
	}

	glVertex3f(position.x+light.radius, position.y, position.z);

	glEnd();
	glDisable(GL_BLEND);

	RenderShadows(light);
	
	glDepthMask(true);
}

void CLevel::DrawShadow(Vec3 p1, Vec3 p2, Vec3 p3, Vec3 p4, Vec3 l)
{
	Vec3 p1p = p1 + (p1-l).normal() * 5000;
	Vec3 p2p = p2 + (p2-l).normal() * 5000;
	Vec3 p3p = p3 + (p3-l).normal() * 5000;
	Vec3 p4p = p4 + (p4-l).normal() * 5000;


	glVertex3fv(p1);
	glVertex3fv(p1p);
	glVertex3fv(p2p);
	glVertex3fv(p2);

	glVertex3fv(p2);
	glVertex3fv(p2p);
	glVertex3fv(p3p);
	glVertex3fv(p3);

	glVertex3fv(p3);
	glVertex3fv(p3p);
	glVertex3fv(p4p);
	glVertex3fv(p4);

	glVertex3fv(p4);
	glVertex3fv(p4p);
	glVertex3fv(p1p);
	glVertex3fv(p1);

}


void CLevel::RenderShadows(light_t light)
{
	map<string, block_t>::iterator b_it;

	float normal;

	glColorMask(false, false, false, true);
	glDepthMask(false);

	glColor4f(0.0f, 0.0f, 0.0f, 0.0f);

	// For each light, go through each block and project shadows.
	for(b_it = mBlocks.begin(); b_it != mBlocks.end(); b_it++)
	{
		Vec3 p = b_it->second.position; // position
		Vec3 s = b_it->second.size; // size
		Vec3 l = light.position; // light

		float direction;
		
		glBegin(GL_QUADS);
			// Bottom
			if (s.y > 0)
				normal = -1.0f;
			else
				normal = 1.0f;

			// Get direction to light

			direction = p.y - l.y;

			direction /= fabs(direction);

			if(direction == normal)
			{
				glNormal3f(0.0f, normal, 0.0f);
				DrawShadow(Vec3(p.x, p.y, p.z), 
					Vec3(p.x+s.x, p.y, p.z),
					Vec3(p.x+s.x, p.y, p.z+s.z), 
					Vec3(p.x, p.y, p.z+s.z), l);
				
			}
			// Left
			if (s.x > 0)
				normal = -1.0f;
			else
				normal = 1.0f;

			direction = p.x - l.x;

			direction /= fabs(direction);

			if(direction == normal)
			{
				glNormal3f(normal, 0.0f, 0.0f);
				DrawShadow(Vec3(p.x, p.y, p.z),
				Vec3(p.x, p.y+s.y, p.z),
				Vec3(p.x, p.y+s.y, p.z + s.z),
				Vec3(p.x, p.y, p.z + s.z), l);
			}
			// Top
			if (s.y > 0)
				normal = 1.0f;
			else
				normal = -1.0f;

			direction = (p.y + s.y) - l.y;

			direction /= fabs(direction);

			if(direction == normal)
			{
				glNormal3f(0.0f, normal, 0.0f);
				DrawShadow(Vec3(p.x, p.y+s.y, p.z),
				Vec3(p.x + s.x, p.y + s.y, p.z),
				Vec3(p.x + s.x, p.y + s.y, p.z + s.z),
				Vec3(p.x, p.y + s.y, p.z + s.z),l);
			}

			// Right
			if (s.x > 0)
				normal = 1.0f;
			else
				normal = -1.0f;

			direction = (p.x + s.x) - l.x;

			direction /= fabs(direction);

			if(direction == normal)
			{
				glNormal3f(normal, 0.0f, 0.0f);
				DrawShadow(Vec3(p.x + s.x, p.y, p.z),
				Vec3(p.x + s.x, p.y + s.y, p.z),
				Vec3(p.x + s.x, p.y + s.y, p.z + s.z),
				Vec3(p.x + s.x, p.y, p.z + s.z),l);
			}
			// Back
			if (s.z > 0)
				normal = -1.0f;
			else
				normal = 1.0f;

			direction = p.z - l.z;

			direction /= fabs(direction);

			if(direction == normal)
			{

				glNormal3f(0.0f, 0.0f, normal);
				DrawShadow(Vec3(p.x, p.y, p.z),
				Vec3(p.x, p.y + s.y, p.z),
				Vec3(p.x + s.x, p.y + s.y, p.z),
				Vec3(p.x + s.x, p.y, p.z),l);
			}
			// Front
			if (s.z > 0)
				normal = 1.0f;
			else
				normal = -1.0f;

			direction = (p.z + s.z) - l.z;

			direction /= fabs(direction);

			if(direction == normal)
			{
				glNormal3f(0.0f, 0.0f, normal);
				DrawShadow(Vec3(p.x, p.y, p.z+s.z),
				Vec3(p.x, p.y + s.y, p.z + s.z),
				Vec3(p.x + s.x, p.y + s.y, p.z + s.z),
				Vec3(p.x + s.x, p.y, p.z + s.z),l);
			}
		glEnd();
	}


	glDepthMask(true);
}


void CLevel::Render()
{
	glEnable(GL_LIGHTING);
	glDisable(GL_TEXTURE_2D);
	gfxBindTexture(0);
	glShadeModel(GL_SMOOTH);
	glDepthFunc(GL_LEQUAL);

	//float global_ambient[] = { 0.5f, 0.5f, 0.5f, 1.0f };
	//glLightModelfv(GL_LIGHT_MODEL_AMBIENT, global_ambient);

	glLightModelf(GL_LIGHT_MODEL_LOCAL_VIEWER,1.0f);
    float lightIntensity[4] = { 0.3f, 0.3f, 0.3f, 1.0f };
    float lightSpecular[4] = { 0.3f, 0.3f, 0.3f, 1.0f };
    float lightAmbient[4] = { 0.80f, 0.80f, 0.80f, 1.0f };
	float pos[4] = { 0.0f, 500.0f, 0.0f, 1.0f };

	

	glLightfv(GL_LIGHT0, GL_AMBIENT, lightAmbient);
	glLightfv(GL_LIGHT0, GL_DIFFUSE, lightSpecular);
	glLightfv(GL_LIGHT0, GL_SPECULAR, lightSpecular);
	glLightfv(GL_LIGHT0, GL_POSITION, pos);
	//glLightf(GL_LIGHT0, GL_SHININESS, 64.0f);

	glEnable(GL_LIGHT0);

	glDisable(GL_LIGHTING);

	glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
	glClear(GL_COLOR_BUFFER_BIT);
	glDisable(GL_BLEND);


	// Write Z-values
	glColorMask(false, false, false, false);

	map<std::string, block_t>::iterator it;
	map<string, light_t>::iterator lit;
	map<int, flashlight_t>::iterator flit;

	for(it = mBlocks.begin(); it != mBlocks.end(); it++)
	{
		DrawBlock(it->second);
	}

	glPolygonOffset(1.0f, 0.0001f);
	glEnable(GL_POLYGON_OFFSET_FILL);

	for(lit = mLights.begin(); lit != mLights.end(); lit++)
	{

		ClearAlphaBuffer();
		// Render current lighting to alpha buffer
		RenderLightsAlpha(lit->second);

		// Render geometry
		glEnable(GL_LIGHTING);
		glColorMask(true, true, true, true);

		glEnable(GL_BLEND);
		glBlendFunc(GL_DST_ALPHA, GL_ONE);

		for(it = mBlocks.begin(); it != mBlocks.end(); it++)
		{
			DrawBlock(it->second);
		}

		glDisable(GL_BLEND);
		glDisable(GL_LIGHTING);
	}

	for(flit = mFlashlights.begin(); flit != mFlashlights.end(); flit++)
	{
		ClearAlphaBuffer();
		// Render current lighting to alpha buffer
		RenderFlashlightsAlpha(flit->second);

		// Render geometry
		glEnable(GL_LIGHTING);
		glColorMask(true, true, true, true);

		glEnable(GL_BLEND);
		glBlendFunc(GL_DST_ALPHA, GL_ONE);

		for(it = mBlocks.begin(); it != mBlocks.end(); it++)
		{
			DrawBlock(it->second);
		}

		glDisable(GL_BLEND);
		glDisable(GL_LIGHTING);
	}

	glDisable(GL_POLYGON_OFFSET_FILL);
}

void CLevel::ClearAlphaBuffer()
{
	glColorMask(false, false, false, true);
	glColor4f(0.0f, 0.0f, 0.0f, 0.0f);
	glDisable(GL_DEPTH_TEST);
	glDisable(GL_BLEND);
	glBegin(GL_QUADS);
	glVertex3f(-10000.0f, -10000.0f, 0.0f);
	glVertex3f(10000.0f, -10000.0f, 0.0f);
	glVertex3f(10000.0f, 10000.0f, 0.0f);
	glVertex3f(-10000.0f, 10000.0f, 0.0f);
	glEnd();
	glEnable(GL_DEPTH_TEST);

}

int CLevel::ReadItem(scew_element* item)
{
	item_t it;
	
	it.name = scew_attribute_value(scew_element_attribute_by_name(item, "name"));
	it.type = scew_attribute_value(scew_element_attribute_by_name(item, "type"));

	ReadXYZ(scew_element_by_name(item, "position"), &it.position);

	mItems[it.name] = it;

	return 1;
}

int CLevel::ReadBlock(scew_element* block)
{

	scew_element *el;
	scew_attribute *attr;
	string mat;
	string texture;

	block_t blo;

	blo.name = scew_attribute_value(scew_element_attribute_by_name(block, "name"));
	mat = scew_attribute_value(scew_element_attribute_by_name(block, "material"));
	blo.material = &mMaterials[mat];

	ReadXYZ(scew_element_by_name(block, "position"), &blo.position);
	ReadXYZ(scew_element_by_name(block, "size"), &blo.size);

	mBlocks[blo.name] = blo;

	CLog::Get().Write(APP_LOG, LOG_INFO, "Loaded block %s.", blo.name.c_str());
	
	return 1;
	

	return 1;
}

int CLevel::ReadXYZ(scew_element* el, Vec3* vec)
{
	vec->x = 10.0f*atof(scew_attribute_value(scew_element_attribute_by_name(el, "x")));
	vec->y = 10.0f*atof(scew_attribute_value(scew_element_attribute_by_name(el, "z")));
	vec->z = 10.0f*atof(scew_attribute_value(scew_element_attribute_by_name(el, "y")));
	return 1;
}

vector<Vec3> CLevel::FindItemByName(const char* name)
{
	vector<Vec3> items;

	map<string, item_t>::iterator it;

	for(it = mItems.begin(); it != mItems.end(); it++)
	{
		if(it->second.type == name)
		{
			items.push_back(it->second.position);
		}
	}
	
	return items;
}

int CLevel::AddFlashlight(float *x, float *y, float *z, float *nx, float *ny, int id)
{
	flashlight_t flash;

	flash.x = x;
	flash.y = y;
	flash.z = z;
	flash.nx = nx;
	flash.ny = ny;

	if(id == 3)
	{
		printf("tjo!");
	}

	mFlashlights[id] = flash;

	return 1;
}

int CLevel::RemoveFlashlight(int id)
{
	mFlashlights.erase(id);

	return 1;
}

int CLevel::ReadLevel(scew_element* level)
{
	scew_element* el;
	scew_list *child_list = NULL;
	if(strcmp(scew_element_name(level), "level") != 0)
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "XML should contain level-element.");
		return 0;
	}

	// Get first child
	child_list = scew_element_children(level);

	do
	{
			el = (scew_element*)scew_list_data(child_list);
		if(strcmp(scew_element_name(el), "material") == 0)
		{
			if(!ReadMaterial(el))
				return 0;
		}
		else if(strcmp(scew_element_name(el), "block") == 0)
		{
			if(!ReadBlock(el))
				return 0;
		}
		else if(strcmp(scew_element_name(el), "item") == 0)
		{
			if(!ReadItem(el))
				return 0;
		}
		else
		{
			CLog::Get().Write(APP_LOG, LOG_ERROR, "Unknown XML element: %s", scew_element_name(el));
			return 0;
		}
	}while(child_list = scew_list_next(child_list));

	light_t light1;
	light_t light2;
	light_t light3;

	light1.name = "tjo";
	light1.intensity = 1.0f;
	light1.position.x = 100.0f;
	light1.position.y = 0.0f;
	light1.position.z = 70.0f;
	light1.radius = 200.0f;

	light3 = light2 = light1;

	light2.name = "tjo2";
	light2.position.x = -200.0f;

	light3.name = "tjo3";
	light3.position.y = -200.0f;
	
	mLights[light1.name] = light1;
	mLights[light2.name] = light2;
	mLights[light3.name] = light3;

	return 1;
}

int CLevel::ReadRGBA(scew_element* el, float rgba[4])
{
	rgba[0] = atof(scew_attribute_value(scew_element_attribute_by_name(el, "r")));
	rgba[1] = atof(scew_attribute_value(scew_element_attribute_by_name(el, "g")));
	rgba[2] = atof(scew_attribute_value(scew_element_attribute_by_name(el, "b")));
	rgba[3] = atof(scew_attribute_value(scew_element_attribute_by_name(el, "a")));

	return 1;
}


int CLevel::ReadMaterial(scew_element* material)
{
	scew_element *el;
	scew_attribute *attr;

	material_t mat;

	ReadRGBA(scew_element_by_name(material, "ambient"), mat.ambient);
	ReadRGBA(scew_element_by_name(material, "diffuse"), mat.diffuse);
	ReadRGBA(scew_element_by_name(material, "emissive"), mat.emissive);
	ReadRGBA(scew_element_by_name(material, "specular"), mat.specular);

	mat.name = scew_attribute_value(scew_element_attribute_by_name(material, "name"));
	mat.type = scew_attribute_value(scew_element_attribute_by_name(material, "type"));

	mat.shininess = atof(scew_attribute_value(scew_element_attribute_by_name(scew_element_by_name(material, "shininess"), "value")));

	mMaterials[mat.name] = mat;

	CLog::Get().Write(APP_LOG, LOG_INFO, "Loaded material %s.", mat.name.c_str());
	
	return 1;
}

int CLevel::ReadXML(const char* filename)
{
	scew_parser *parser;
	scew_tree *tree;
	int result = 0;

	parser = scew_parser_create();

	/*if(!scew_parser_load_file(parser, filename))
	{	
		scew_parser_free(parser);
		return 0;
	}

	tree = scew_parser_tree(parser);


	if(!ReadLevel(scew_tree_root(tree)))
	{
		result = 0;
	}

	scew_tree_free(tree);*/

	scew_parser_free(parser);

	result = 1;
	return result;
}
CLevel::~CLevel()
{

}
