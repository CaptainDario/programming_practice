/********************************
 * Author: Danny Puhan			*
 * Email puhan@uni-potsdam.de	*
 * Universtitaet Potsdam		*
 * Institut fuer Informatik		*
 ********************************/
#include "./lib_km_seq.c"
#include "../io/io.c"

bool is_digit(char* in,int size){
    for(int i=0;i<size;i++){
        if(!isdigit(in[i])){return false;}    
    }
    return true;
}

int main(int argc, char** argv) 
{
    int size = 0;
    int dim = 0;
    int num_centroids = 0;
    double* values = NULL;
    int mode = -1;
    int max_iteration = 0; //no limit if <0
    CHECK_ARGS();     

    if(mode==GENERATE){
        /* value generation */  
        values = (double *) malloc(sizeof(double) * size * dim);     
        for(long i=0;i<size*dim;i++){
            values[i] = (((i+34)*dim*145)%7337)*(double)(0.81);
        } 
    }else if(mode==FILE_INPUT){
        /* read values from file */  
        char* filepath = "../data/";
        if(is_digit(argv[FILE_ARG],strlen(argv[FILE_ARG]))){
            ASSERT(false)
        }
        char* filename = argv[FILE_ARG];
        char* file = malloc(strlen(filepath)+strlen(filename)*sizeof(char));

        strcpy(file, filepath);    
        strcat(file,filename);
        ASSERT_FILE(file_exist(file));
        read_size_dim(file,&size,&dim); 
        ASSERT_MSG(!(size == 0 || dim == 0),"\nNo Data in the File.\n");
        ASSERT_MSG( size > num_centroids, "Number of data have to be bigger than number of centroids.\n" );
        values = (double *) malloc(sizeof(double) * size * dim);
        read_kmeans_data(file, size, dim, values);
    }else{
        /*if undef mode return*/
        printf("\nNo known mode defined.\n"); 
        return -1;
    }

    /* All Input was ok */
    double* centroids = (double *) malloc(sizeof(double) * num_centroids * dim);
    int* result = (int *) malloc(sizeof(int) * size); //number of cluster for each data

    /*  Random pick centroids out of data.
        Must not init with unique values. Maybe there are less than k centroids 
        TODO: init centroids with k unique values
    */
	for (int k = 0; k < num_centroids; k++) {
		for (int i = 0; i < dim; i++) {
			centroids[k * dim + i] = values[k*dim*(size/(k+1))+i];
        }
	}
    
    printf("mode: %d; dim: %d; size: %d; k: %d; max_iteration: %d;\n",mode,dim,size,num_centroids,max_iteration);
	
    if(values!=NULL){
        kmeans( dim, values, size, centroids, num_centroids, result, max_iteration);
    }
    if(values!=NULL)
        free(values);
    if(result!=NULL)
        free(result);
    if(result!=NULL)
        free(centroids);
    return 0;
}
