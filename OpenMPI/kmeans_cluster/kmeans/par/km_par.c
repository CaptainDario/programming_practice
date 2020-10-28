
#include "./lib_km_par.c"
#include "../io/io.c"
//MPI
#include <mpi.h>
#include "../include/common.h"
int rank, num_procs;

//AVX


bool is_digit(char* in,int size){
    for(int i=0;i<size;i++){
        if(!isdigit(in[i])){return false;}    
    }
    return true;
}

int main(int argc, char** argv) 
{
    //initialize MPI
    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &num_procs);
    //printf("rank: %d, num procs: %d\n", rank, num_procs);
    
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
        //multi process
        if(num_procs > 1){
            //iteration size for this process 
            int it_size  = floor((size * dim) / num_procs);     //per process calculated element count
            int sendcounts[num_procs], reccounts[num_procs];    //receive and send element-count
            int sdispls [num_procs], recdispls [num_procs];     //receive and send displacement
            for (int i = 0; i < num_procs; i++){
                
                sendcounts[i] = reccounts[i] = it_size;
                sdispls [i]   = rank * it_size;
                recdispls[i]  = i * it_size;
                //received messages can be longer
                if(i+1 == num_procs)
                    reccounts[i] = it_size + ((size*dim)%num_procs);
                //send messages from the processes with highest rank can be longer
                if(rank == num_procs-1)
                    sendcounts[i] = it_size + ((size*dim)%num_procs);
            }

            if(rank+1 != num_procs){
                for(long i = rank * it_size; i < (rank+1)*it_size; i++){
                    values[i] = (((i+34)*dim*145)%7337)*(double)(0.81);
                }
            }
            else{
                for(long i = rank * it_size; i < (rank+1)*it_size + ((size*dim)%num_procs); i++){
                    values[i] = (((i+34)*dim*145)%7337)*(double)(0.81);
                }
            }
            //collect and send generated values
            MPI_Alltoallv(values, sendcounts, sdispls, MPI_DOUBLE,
                        values, reccounts, recdispls, MPI_DOUBLE,
                        MPI_COMM_WORLD);
        }
        //single process
        else{
            for(long i = 0; i < size*dim; i++){
                values[i] = (((i+34)*dim*145)%7337)*(double)(0.81);
            }
        }

    }
    else if(mode==FILE_INPUT){
        //only rank 0 reads the file and sends it to the other processes
        //if(rank == 0){
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

            //
        //}
    }else{
        /*if undef mode return*/
        if(rank == 0) printf("\nNo known mode defined.\n"); 
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
    
    if(rank == 0)
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

    MPI_Finalize();

    return 0;
}
