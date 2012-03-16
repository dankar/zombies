#include <GL/glew.h>
#include <GL/glfw.h>
#include <stdio.h>
#include <stdlib.h>
#include "shader.h"

char* ReadFile(char* filename)
{
    char *s;
   	FILE* file;
   	int size;
   	
   	file = fopen(filename, "rb");
   	if (file == NULL){
        printf("Could not open %s\n", filename);
        return 0;
    }
    fseek(file, 0 , SEEK_END);
    size = ftell(file);
    fseek(file, 0, SEEK_SET); //rewind(file);

    s = new char[size + 1];
    fread(s,1,size,file);
	s[size] = 0;
    fclose(file);
   
    return s;
}

void PrintShaderError(int shader)
{
    char *test = new char[1024];
    int length;
    glGetInfoLogARB(shader, 1024, &length, test);
    printf("%s\n", test);
    delete[] test;
}

int LoadShader(char* filename, int option, int program)
{
	int shader;
    const GLcharARB* shader_source = ReadFile(filename);
    
    if(!shader_source){
        return 0;
    }

	if(!program)
		program = glCreateProgramObjectARB();

	shader = glCreateShaderObjectARB(option);

    glShaderSourceARB(shader, 1, &shader_source, NULL);

    glCompileShaderARB(shader);

    glAttachObjectARB(program, shader);

    glLinkProgramARB(program);

	PrintShaderError(program); 
	
	delete[] shader_source; 
	
	return program;
}

void RunShader(int shader)
{
	glUseProgramObjectARB(shader);
}

void DisableShader()
{
	glUseProgramObjectARB(0);
}

void set_uniform3f(int shader, char* name, float x, float y, float z)
{
    glUniform3fARB(glGetUniformLocationARB(shader, name), x, y, z);
}

void PrintGLError()
{
	int error;
	if((error = glGetError()) != GL_NO_ERROR){
		printf("%s\n", (const char*)gluErrorString(error));
	}else{
		printf("OpenGL:\t\t\t\tok!\n");
	}
}
