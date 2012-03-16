#ifndef _PNG_H_
#define _PNG_H_

class PNG{

private:
	png_structp png_ptr;
	png_infop info_ptr;
	png_infop end_info;
public:
	int height;
	int width;
	png_bytep *row_pointers;
	int open(const char* _pFilename);
	void Destroy();
	PNG();
	~PNG();

};

#endif /* _PNG_H_ */
