#include "../include/kmeans.h"
#include "../include/debug.h"
#include "../include/common.h"
#include <mpi.h>

#include <unistd.h>

#define sqr(x) ((x)*(x))
#define pos(x) (x)<0? -(x):x
/**********************************************
 * standard functions to calculate kmean
 *   euklid_distance
 *   manhatten_distance
 ***********************************************/

double euklid_distance(int dim, const double* data_point, double* centroid) {
	double dist = 0;
	bool isValid = data_point != NULL && centroid != NULL;
	isValid &= sizeof(&data_point) == sizeof(&centroid);
	if (!isValid) {
		return -1;
	}

	for (int i_dim = 0; i_dim < dim; i_dim++) {
		dist += sqr(data_point[i_dim] - centroid[i_dim]);
	}
	return dist;

}

double manhatten_distance(int dim, const double* data_point, double* centroid) {
	double dist = 0;
	bool isValid = data_point != NULL && centroid != NULL;
	isValid &= sizeof(&data_point) == sizeof(&centroid);
	if (!isValid) {
		return -1;
	}
	for (int i_dim = 0; i_dim < dim; i_dim++) {
		dist += pos(data_point[i_dim] - centroid[i_dim]);
	}
	return dist;
}

//1st impact
//calc each distance between a single data and all centroids. result to distance
void calc_data_cluster_distance(int dim, const double* data_point,
		double* centroid_points, int num_centroid,
		Distance dist_fnc , double* distance) {


   if(dist_fnc == EUKLID){
	    for (int i_centroid = 0; i_centroid < num_centroid; i_centroid++) {
		    distance[i_centroid] = euklid_distance(dim, data_point,
				    &centroid_points[i_centroid * dim]);
	    }
    }else if(dist_fnc == MANHATTEN){
       for (int i_centroid = 0; i_centroid < num_centroid; i_centroid++) {
		    distance[i_centroid] = manhatten_distance(dim, data_point,
				    &centroid_points[i_centroid * dim]);
	    }
    }
}
//calc each distance between data and centroids. result to distance
void calc_distances(int dim, const double* data_points, int num_data,
		double* centroid_points, int num_centroid,
		Distance dist_fnc, double* distance) {

	//multi process
	if(num_procs > 1){
		int it_size = floor(num_data/num_procs);					//per process loop length
		int data_size = it_size * num_centroid; 
		int sendcounts[num_procs], reccounts[num_procs];			//receive and send element-count
		int sdispls [num_procs], recdispls [num_procs]; 			//receive and send displacement
		for (int i = 0; i < num_procs; i++){
			
			sendcounts[i] = reccounts[i] = data_size;
			sdispls [i]   = rank * data_size;
			recdispls[i]  = i * data_size;
			//received message can be longer from process with heighest rankcd
			if(i+1 == num_procs)
				reccounts[i] += (num_data % num_procs) * num_centroid;
			//send messages from the processes with highest rank can be longer
			if(rank+1 == num_procs)
				sendcounts[i] += (num_data % num_procs) * num_centroid;
		}

		//the last process maybe needs to calculate more than the others
		if(rank+1 != num_procs){
			for (int i = rank*it_size; i < (rank+1)*it_size; i++) {
				calc_data_cluster_distance(dim, &data_points[i * dim], centroid_points,
						num_centroid, dist_fnc, &distance[i * num_centroid]);
			}
		}else{
			// (num_data % num_procs) * num_centroid
			for (int i = rank*it_size; i < (rank+1)*it_size + (num_data % num_procs); i++) {
				calc_data_cluster_distance(dim, &data_points[i * dim], centroid_points,
						num_centroid, dist_fnc, &distance[i * num_centroid]);
			}
		}

		//collect and send values
		MPI_Alltoallv(distance, sendcounts, sdispls, MPI_DOUBLE,
					distance, reccounts, recdispls, MPI_DOUBLE,
					MPI_COMM_WORLD);
	}
	//single process
	else{
		for (int i = 0; i < num_data; i++) {
			calc_data_cluster_distance(dim, &data_points[i * dim], centroid_points,
					num_centroid, dist_fnc, &distance[i * num_centroid]);
		}
	}
}

//search nearest centroid and assign to it. result to cluster_assignment
void assign_data_to_cluster(int num_data, int num_centroid,
		double *distances, int *cluster_assignment) {
	double min_dist;
	int closest_centroid;
	double tmp_distance;
	for (int i_data = 0; i_data < num_data; i_data++) {
		min_dist = DOUBLE_MAX;
		closest_centroid = -1;

		for (int i_centroid = 0; i_centroid < num_centroid; i_centroid++) {
			tmp_distance = distances[i_data * num_centroid + i_centroid];
			if (tmp_distance < min_dist) {

				closest_centroid = i_centroid;
				min_dist = tmp_distance;
			}
		}
		cluster_assignment[i_data] = closest_centroid;
		IF_DB {
			if(rank == 0) printf("Data[%d] added to centroid: %d\n", i_data, closest_centroid);
		}
	}
}

void copy_array_dbl(int num_data, double *src, double *dst) {
	for (int i_data = 0; i_data < num_data; i_data++)
		dst[i_data] = src[i_data];
}

void copy_array_int(int num_data, int *src, int *dst) {
	for (int i_data = 0; i_data < num_data; i_data++)
		dst[i_data] = src[i_data];
}

void print_array_int(int num_data, int *array) {
	for (int i_data = 0; i_data < num_data; i_data++){
		if(rank == 0) printf("%d\t", array[i_data]);
	}
	if(rank == 0) printf("\n");
}

//*

//calc new centroids (average of cluster members). result to centroids
void calc_centroids(int dim, int num_data, int num_centroid, const double *data,
		int *cluster_assignment, double *centroids) {

	int cluster_size[num_centroid];

	
	for (int i_centroid = 0; i_centroid < num_centroid; i_centroid++) {
		cluster_size[i_centroid] = 0;

		for (int i_dim = 0; i_dim < dim; i_dim++) {
			centroids[i_centroid * dim + i_dim] = 0;
		}
	}

		for (int i_data = 0; i_data < num_data; i_data++) {
			int cluster = cluster_assignment[i_data];

			cluster_size[cluster]++;

			for (int i_dim = 0; i_dim < dim; i_dim++)
				centroids[cluster * dim + i_dim] += data[i_data * dim + i_dim];
		}

	for (int i_centroid = 0; i_centroid < num_centroid; i_centroid++) {
		if (cluster_size[i_centroid] != 0) {
			for (int i_dim = 0; i_dim < dim; i_dim++) {
				centroids[i_centroid * dim + i_dim] = (double) centroids[i_centroid
						* dim + i_dim] / cluster_size[i_centroid];

			}
			continue;
		}
        IF_DB {
			if(rank == 0) printf("WARNING: Empty cluster %d! \n", i_centroid);
        }	
    }
}

// return sum of distance(data,centroid(assigned)) 
double sum_total_distance(int num_data, double *distances, int num_centroid, int *cluster_assignment) {
	double total_distance = 0;
	int cluster;
	for (int i_data = 0; i_data < num_data; i_data++) {
		cluster = cluster_assignment[i_data];
		if (cluster != -1) {
			total_distance += distances[i_data * num_centroid + cluster];
		}
	}

	return total_distance;
}
/**********************************************
 *  Validation
 **********************************************/
bool is_equal_array_int(int num_data, int *a, int *b) {
	bool isEqual = true;
	while (num_data--)
		isEqual &= (b[num_data] == a[num_data]);
	return isEqual;
}

//*/

