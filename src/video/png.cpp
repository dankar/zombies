#include <png.h>
#include <cstdlib>
#include "png.h"

int PNG::open(const char* _pFilename)
{
	png_byte header[8];
	int is_png;
	
	FILE *fp = fopen(_pFilename, "rb");
	if(!fp) return 0;
	
	fread(header, 1, 8, fp);
	
	is_png = !png_sig_cmp(header, 0, 8);
	if(!is_png) return 0;
	
	png_ptr = png_create_read_struct(PNG_LIBPNG_VER_STRING, NULL, NULL, NULL);

	if(!png_ptr) return 0;
	
	info_ptr = png_create_info_struct(png_ptr);
	if(!info_ptr){
		png_destroy_read_struct(&png_ptr,
				(png_infopp)NULL, (png_infopp)NULL);
		return 0;
	}
	
	end_info = png_create_info_struct(png_ptr);
	if(!end_info)
	{
		png_destroy_read_struct(&png_ptr, &info_ptr, 
				(png_infopp)NULL);
		return 0;
	}
	
	if(setjmp(png_jmpbuf(png_ptr)))
	{
		png_destroy_read_struct(&png_ptr, &info_ptr,
				&end_info);
		fclose(fp);
		return 0;
	}
	
	png_init_io(png_ptr, fp);
	png_set_sig_bytes(png_ptr, 8);

	png_read_info(png_ptr, info_ptr);

	width = info_ptr->width;
	height = info_ptr->height;

	png_read_update_info(png_ptr, info_ptr);

	row_pointers = (png_bytep*) malloc(sizeof(png_bytep) * height); // TODO: Asserts här?

	for (int y=0; y<height; y++)
		row_pointers[y] = (png_byte*) malloc(info_ptr->rowbytes); // TODO: Asserts här?
	
	//png_read_png(png_ptr, info_ptr, /*PNG_TRANSFORM_STRIP_ALPHA*/ 0, NULL);
	
	png_read_image(png_ptr, row_pointers);

	png_read_end(png_ptr, end_info);

	int color_type = info_ptr->color_type;

	png_destroy_read_struct(&png_ptr, &info_ptr, &end_info);

	return 1 + (color_type == PNG_COLOR_TYPE_RGB_ALPHA?1:0);
}

void PNG::Destroy()
{
	if(row_pointers){
		for(int y=0; y < height; y++)
			free(row_pointers[y]);
		free(row_pointers);
	}
	row_pointers = 0;

	// TODO: maybe this is not safe?
	//png_destroy_read_struct(&png_ptr, &info_ptr,&end_info);
}

PNG::~PNG()
{
	Destroy();
}

PNG::PNG()
{
	row_pointers = 0;
}