#ifndef _SHADER_H_
#define _SHADER_H_

void DisableShader();
void RunShader(int shader);
int LoadShader(char* filename, int option, int program);
void PrintShaderError(int shader);
void PrintGLError();

#endif /* _SHADER_H_ */