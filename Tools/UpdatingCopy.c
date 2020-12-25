#include "C:\Factory\Common\all.h"

static void RemoveOldZip(char *wDir)
{
	autoList_t *files = lsFiles(wDir);
	char *file;
	uint index;

	foreach(files, file, index)
		if(!_stricmp(getExt(file), "zip"))
			removeFile(file);

	releaseDim(files, 1);
}
static char *GetFirstZipFile(char *rDir)
{
	autoList_t *files = lsFiles(rDir);
	char *file;
	uint index;

	foreach(files, file, index)
		if(!_stricmp(getExt(file), "zip"))
			break;

	errorCase_m(!file, ".zip �t�@�C����������܂���B");

	file = strx(file);
	releaseDim(files, 1);
	return file;
}
static void CopyZip(char *rDir, char *wDir)
{
	char *rFile = GetFirstZipFile(rDir);
	char *wFile;

	wFile = combine(wDir, getLocal(rFile));

	copyFile(rFile, wFile);

	memFree(rFile);
	memFree(wFile);
}
int main(int argc, char **argv)
{
	char *rDir;
	char *wDir;
	char *oldPrefix;

	rDir = nextArg();
	wDir = nextArg();

	errorCase(!existDir(rDir));
	errorCase(!existDir(wDir));

	LOGPOS();
	RemoveOldZip(wDir);
	LOGPOS();
	CopyZip(rDir, wDir);
	LOGPOS();
}
