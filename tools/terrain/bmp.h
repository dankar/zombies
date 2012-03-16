//#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef unsigned short WORD;
typedef unsigned long DWORD;
typedef long LONG;

#pragma warning (disable : 4103)

#pragma pack(1)
typedef struct tagBITMAPFILEHEADER {
        WORD    bfType;
        DWORD   bfSize;
        WORD    bfReserved1;
        WORD    bfReserved2;
        DWORD   bfOffBits;
} BITMAPFILEHEADER;

typedef struct tagBITMAPINFOHEADER{
        DWORD      biSize;
        LONG       biWidth;
        LONG       biHeight;
        WORD       biPlanes;
        WORD       biBitCount;
        DWORD      biCompression;
        DWORD      biSizeImage;
        LONG       biXPelsPerMeter;
        LONG       biYPelsPerMeter;
        DWORD      biClrUsed;
        DWORD      biClrImportant;
} BITMAPINFOHEADER;

char* ConvertRGBToBMPBuffer ( char* Buffer, int width, int height, long* newsize )
{
	int padding = 0;
	int scanlinebytes = width * 3;
	int psw = scanlinebytes + padding;
	int x, y;
	char* newbuf;
	long bufpos = 0;   
	long newpos = 0;
	
	if ( ( NULL == Buffer ) || ( width == 0 ) || ( height == 0 ) )
		return NULL;

	
	while ( ( scanlinebytes + padding ) % 4 != 0 )
		padding++;

	*newsize = height * psw;

	newbuf = (char*)malloc(*newsize);

	memset ( newbuf, 0, *newsize );

	for (y = 0; y < height; y++ )
		for (x = 0; x < 3 * width; x+=3 )
		{
			bufpos = y * 3 * width + x;     // position in original buffer
			newpos = ( height - y - 1 ) * psw + x;           // position in padded buffer

			newbuf[newpos] = Buffer[bufpos+2];       // swap r and b
			newbuf[newpos + 1] = Buffer[bufpos + 1]; // g stays
			newbuf[newpos + 2] = Buffer[bufpos];     // swap b and r
		}

	return newbuf;
}

int SaveBMP ( char* Buffer, int width, int height, long paddedsize, char* bmpfile )
{
	// declare bmp structures 
	BITMAPFILEHEADER bmfh;
	BITMAPINFOHEADER info;
	FILE* fp;
	unsigned long bwritten;
	
	memset ( &bmfh, 0, sizeof (BITMAPFILEHEADER ) );
	memset ( &info, 0, sizeof (BITMAPINFOHEADER ) );
	
	bmfh.bfType = 0x4d42;
	bmfh.bfReserved1 = 0;
	bmfh.bfReserved2 = 0;
	bmfh.bfSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + paddedsize;
	bmfh.bfOffBits = 0x36;
	
	info.biSize = sizeof(BITMAPINFOHEADER);
	info.biWidth = width;
	info.biHeight = height;
	info.biPlanes = 1;
	info.biBitCount = 24;
	info.biCompression = 0;//BI_RGB;	
	info.biSizeImage = 0;
	info.biXPelsPerMeter = 0x0ec4;
	info.biYPelsPerMeter = 0x0ec4;     
	info.biClrUsed = 0;
	info.biClrImportant = 0;

	fp = fopen(bmpfile, "wb");
	
	if(!fp)
		return 0;
	
	
	fwrite((char*)&bmfh, 1, sizeof ( BITMAPFILEHEADER ), fp);

	fwrite((char*)&info, 1, sizeof( BITMAPINFOHEADER ), fp);
	
	fwrite(Buffer, 1, paddedsize, fp);

	fclose(fp);

	return 1;
}

