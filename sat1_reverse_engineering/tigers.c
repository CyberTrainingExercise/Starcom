#define _GNU_SOURCE

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <unistd.h>
#include <string.h>

#include <errno.h>

#define ERROR_SYSCALL -1

extern int errno;

char* concat(const char *s1, const char *s2)
{
    char *result = malloc(strlen(s1) + strlen(s2) + 2);
    strcpy(result, s1);
    strcat(result, " ");
    strcat(result, s2);
    return result;
}

int main(int argc, char ** const argv) {
    if (argc <= 1)
    {
        system("python3 tigers.py");
        return 0;
    }
    char* combined_str = argv[1];
    for (int i = 2; i < argc; i++)
    {
        combined_str = concat(combined_str, argv[i]);
    }
    system(concat("python3 tigers.py ", combined_str));
    return 0;
}