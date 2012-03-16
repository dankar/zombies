#ifndef _MATRIX_H_
#define _MATRIX_H_

#include "vector.h"
#include <cmath>

float deg2rad(float deg);

class mat4
{
public:
	float m_Matrix[4][4];

	void LoadNull()
	{
		memset(m_Matrix, 0, sizeof(m_Matrix));
	}

	void LoadIdentity()
	{
		LoadNull();
		m_Matrix[0][0] = m_Matrix[1][1] = m_Matrix[2][2] = m_Matrix[3][3] = 1.0f;
	}

	mat4 GetOrthogonalInverse() const 
	{ 
		mat4 Inverse; 
 
		Inverse.LoadIdentity(); 
 
		for (int i = 0; i < 3; i++) 
			for (int j = 0; j < 3; j++) 
				Inverse.m_Matrix[i][j] = m_Matrix[j][i]; 
 
		Inverse.Translate(-GetTranslation()); 
 
		return Inverse; 
	}

	void SetPerspective(float fov, float aspect, float near, float far)
	{
		float f = 1 / tan(deg2rad(fov)/2);



		LoadNull();

		m_Matrix[0][0] = f / aspect;
		m_Matrix[1][1] = f;

		m_Matrix[2][2] = (far + near) / (near - far);
		
		m_Matrix[3][2] = (2 * far * near) / (near - far);

		m_Matrix[2][3] = -1;
	}

	void SetInfinitePerspective(float fov, float aspect, float near)
	{
		float f = 1 / tan(deg2rad(fov)/2);

		float e = float(1e-22);

		LoadNull();

		m_Matrix[0][0] = f / aspect;
		m_Matrix[1][1] = f;

		m_Matrix[2][2] = e - 1;
		
		m_Matrix[3][2] = (e - 2) * near;

		m_Matrix[2][3] = -1;
	}


	void SetGLProjection()
	{
		float matrix[16];

		glMatrixMode(GL_PROJECTION);
		glLoadIdentity();
		GetOGLMatrix(matrix);
		glMultMatrixf(matrix);
		glMatrixMode(GL_MODELVIEW);
	}

	void GLMultMatrix() const
	{
		float matrix[16];

		GetOGLMatrix(matrix);
		glMultMatrixf(matrix);
	}

	Vec3 GetTranslation() const
	{
		Vec3 trans;

		trans.x = m_Matrix[3][0];
		trans.y = m_Matrix[3][1];
		trans.z = m_Matrix[3][2];

		return trans;
	}

	void Translate(const Vec3& trans)
	{
		m_Matrix[3][0] += trans.x;
		m_Matrix[3][1] += trans.y;
		m_Matrix[3][2] += trans.z;
	}


	void GetOGLMatrix(float matrix[16]) const
	{
		int counter = 0;

		for(int x = 0; x < 4; x++)
			for(int y = 0; y < 4; y++)
				matrix[counter++] = m_Matrix[x][y];
	}

	void FromOGLMatrix(float matrix[16])
	{
		int counter = 0;

		for(int x = 0; x < 4; x++)
			for(int y = 0; y < 4; y++)
				 m_Matrix[x][y] = matrix[counter++];
	}


	void multVec4(float v[4]) const
	{
		GLfloat res[4];
 
		res[0] = m_Matrix[0][ 0] * v[0] + m_Matrix[1][ 0] * v[1] + m_Matrix[2][0] * v[2] + m_Matrix[3][0] * v[3];
		res[1] = m_Matrix[0][ 1] * v[0] + m_Matrix[1][ 1] * v[1] + m_Matrix[2][1] * v[2] + m_Matrix[3][1] * v[3];
		res[2] = m_Matrix[0][ 2] * v[0] + m_Matrix[1][ 2] * v[1] + m_Matrix[2][2] * v[2] + m_Matrix[3][2] * v[3];
		res[3] = m_Matrix[0][ 3] * v[0] + m_Matrix[1][ 3] * v[1] + m_Matrix[2][3] * v[2] + m_Matrix[3][3] * v[3];
	
		v[0]=res[0];
		v[1]=res[1];
		v[2]=res[2];
		v[3]=res[3];
	}

	void multVec4(const float v[4], float result[4]) const
	{
		result[0] = m_Matrix[0][ 0] * v[0] + m_Matrix[1][ 0] * v[1] + m_Matrix[2][0] * v[2] + m_Matrix[3][0] * v[3];
		result[1] = m_Matrix[0][ 1] * v[0] + m_Matrix[1][ 1] * v[1] + m_Matrix[2][1] * v[2] + m_Matrix[3][1] * v[3];
		result[2] = m_Matrix[0][ 2] * v[0] + m_Matrix[1][ 2] * v[1] + m_Matrix[2][2] * v[2] + m_Matrix[3][2] * v[3];
		result[3] = m_Matrix[0][ 3] * v[0] + m_Matrix[1][ 3] * v[1] + m_Matrix[2][3] * v[2] + m_Matrix[3][3] * v[3];
	}


};


#endif /* _MATRIX_H_ */