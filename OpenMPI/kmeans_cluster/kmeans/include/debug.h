/********************************
 * Author: Danny Puhan			*
 * Email puhan@uni-potsdam.de	*
 * Universtitaet Potsdam		*
 * Institut fuer Informatik		*
 ********************************/
#include "../include/common.h"

#define DEBUG false
#define IF_DB if(DEBUG)

#define PRINTASSERT if(rank==0){printf("For generating data use: ./%s <max_iteration> <k> 0 <size> <dim> \n For reading data from file use: ./%s <max_iteration> <k> 1 <filename>\n ",argv[0],argv[0] );}
#define ASSERT(_Expression) if(!(_Expression)){PRINTASSERT;return -1;}
#define ASSERT_MSG(_Expression, _Msg) if(rank==0){if(!(_Expression)){printf(_Msg);return -1;}}
#define ASSERT_FILE(_Expression) if(rank==0){if(!(_Expression)){printf("File %s not found!\n", argv[FILE_ARG]);return -1;}}


#define CHECK_ARGS() if(argc>=MIN_ARGS){\
        if( is_digit(argv[MAX_ITER_ARG],strlen(argv[MAX_ITER_ARG]))&&atoi(argv[MAX_ITER_ARG])>=0 ){\
            max_iteration = atoi(argv[MAX_ITER_ARG]);\
        }else{ASSERT_MSG(false, "max_iteration must be a digit or >= 0\n");}\
        if( is_digit(argv[K_ARG],strlen(argv[K_ARG]) ) && atoi(argv[K_ARG])>0){\
            num_centroids = atoi(argv[K_ARG]);\
        }else{ASSERT_MSG(false, "Number of centroids must be >0 or a digit.\n");}\
        if( is_digit(argv[MODE_ARG],strlen(argv[MODE_ARG])) ){\
            mode = atoi(argv[MODE_ARG]);\
        }else{ASSERT_MSG(false, "Mode have to be a digit: 0 - Generate; 1 - Read file");}\
    }\
    if(argc == 6){\
        ASSERT_MSG(is_digit( argv[SIZE_ARG], strlen(argv[SIZE_ARG]) )&&is_digit( argv[DIM_ARG], strlen(argv[DIM_ARG]) ), "Size and dim have to be a number.\n")\
    }\
    if( mode == GENERATE ){\
        size = atoi( argv[SIZE_ARG] );\
        dim = atoi( argv[DIM_ARG] );\
         ASSERT_MSG(!(size == 0 || dim == 0),"\nNo valid data Configuration. Check size and dim.\n");\
        ASSERT_MSG( size > num_centroids, "Number of data have to be bigger than number of centroids.\n" );\
    }\
    bool gen_assert = (mode == GENERATE);\
    bool file_assert = (argc == 5 && mode==FILE_INPUT);\
    ASSERT( gen_assert || file_assert )

